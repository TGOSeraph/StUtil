using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace StUtil.Native.PE
{
    public class WindowsPE
    {
        /// <summary>
        /// Identifiers for the NT magic field
        /// </summary>
        private enum MagicType : ushort
        {
            IMAGE_NT_OPTIONAL_HDR32_MAGIC = 0x10b,
            IMAGE_NT_OPTIONAL_HDR64_MAGIC = 0x20b
        }
        /// <summary>
        /// Identifiers for each of the data directories
        /// </summary>
        private enum DataDirectories : ushort
        {
            Export = 0,
            Import = 1,
            Resource = 2,
            Exception = 3,
            Security = 4,
            Relocation = 5,
            Debug = 6,
            Architecture = 7,
            Reserved = 8,
            TLS = 9,
            LoadConfig = 10,
            BoundImport = 11,
            ImportAddressTable = 12,
            DelayImport = 13,
            CLR = 14
        }
        /// <summary>
        /// Identifier for the type of assembly that is being analysed
        /// </summary>
        public enum AssemblyCodeType
        {
            None,
            Managed,
            Native
        }
        /// <summary>
        /// The target CPU architecture
        /// </summary>
        public enum TargetPlatformArchitecture
        {
            None,
            x86,
            x64,
            AnyCPU
        }

        /// <summary>
        /// Marker for a valid DOS PE
        /// </summary>
        private const ushort IMAGE_DOS_SIGNATURE = 0x00005A4D;
        /// <summary>
        /// Marker for a valid NT PE
        /// </summary>
        private const uint IMAGE_NT_SIGNATURE = 0x00004550;

        /// <summary>
        /// The path to the PE being inspected
        /// </summary>
        public string FilePath { get; private set; }

        /// <summary>
        /// The file loaded into memory
        /// </summary>
        private byte[] buffer;
        private byte[] Buffer
        {
            get
            {
                //If the buffer is null, then the file and store it in the buffer
                if (buffer == null)
                {
                    using (FileStream s = System.IO.File.OpenRead(FilePath))
                    {
                        buffer = new byte[s.Length];
                        s.Read(buffer, 0, buffer.Length);
                    }

                }
                return buffer;
            }
        }

        private bool? isValidPE = null;
        /// <summary>
        /// If the file being inspected is a valid PE
        /// </summary>
        public bool IsValidPE
        {
            get
            {
                //If we havnt computed if we have a valid PE yet, then check for the DOS and NT signatures
                if (!isValidPE.HasValue)
                {
                    isValidPE = GetValue<ushort>(0x0) == IMAGE_DOS_SIGNATURE //DosHeader.e_magic
                        && GetValue<uint>(NtHeaderPointer) == IMAGE_NT_SIGNATURE; //NtHeaders.Signature
                }
                return isValidPE.Value;
            }
        }

        private TargetPlatformArchitecture targetArchitecture = TargetPlatformArchitecture.None;
        /// <summary>
        /// The target CPU architecture 
        /// </summary>
        public TargetPlatformArchitecture TargetArchitecture
        {
            get
            {
                //If we havn't got a target yet then compute it
                if (targetArchitecture == TargetPlatformArchitecture.None)
                {
                    ComputePETargets();
                }
                return targetArchitecture;
            }
        }

        private AssemblyCodeType assemblyType = AssemblyCodeType.None;
        /// <summary>
        /// The type of the file, CLR managed or native code
        /// </summary>
        public AssemblyCodeType AssemblyType
        {
            get
            {
                //If we havn't got a target yet then compute it
                if (assemblyType == AssemblyCodeType.None)
                {
                    ComputePETargets();
                }
                return assemblyType;
            }
        }

        private ushort clrMajorRuntimeVersion = ushort.MaxValue;
        /// <summary>
        /// The major version of the CLR the assembly is built against
        /// </summary>
        public ushort ClrMajorRuntimeVersion
        {
            get
            {
                //If we havn't got a target yet then compute it
                if (clrMajorRuntimeVersion == ushort.MaxValue)
                {
                    ComputePETargets();
                }
                return clrMajorRuntimeVersion;
            }
        }

        private ushort clrMinorRuntimeVersion = ushort.MaxValue;
        /// <summary>
        /// The minor version of the CLR the assembly is built against
        /// </summary>
        public ushort ClrMinorRuntimeVersion
        {
            get
            {
                //If we havn't got a target yet then compute it
                if (clrMinorRuntimeVersion == ushort.MaxValue)
                {
                    ComputePETargets();
                }
                return clrMinorRuntimeVersion;
            }
        }

        private uint? ntHeaderOffset = null;
        /// <summary>
        /// The pointer to the NT header
        /// </summary>
        private uint NtHeaderPointer
        {
            get
            {
                //If we havnt got the NT header offset yet then load it from the DOS header, then compute targets
                if (!ntHeaderOffset.HasValue)
                {
                    ntHeaderOffset = GetValue<uint>(0x3C); //DosHeader.e_lfanew
                    ComputePETargets();
                }
                return ntHeaderOffset.Value;
            }
        }

        private uint? exportsDirectoryRVA = null;
        /// <summary>
        /// The relative virtual address to the exports directory
        /// </summary>
        private uint ExportsDirectoryRVA
        {
            get
            {
                //If we havn't computed the exports RVA yet then compute targets
                if (!exportsDirectoryRVA.HasValue)
                {
                    ComputePETargets();
                }
                return exportsDirectoryRVA.Value;
            }
        }

        private uint? clrDirectoryPtr = null;
        /// <summary>
        /// The pointer to the CLR data directory
        /// </summary>
        private uint CLRDirectoryPointer
        {
            get
            {
                //If we havn't computed the CLR Pointer yet then compute targets
                if (!clrDirectoryPtr.HasValue)
                {
                    clrDirectoryPtr = GetDirectoryDataPtr(DataDirectories.CLR);
                }
                return clrDirectoryPtr.Value;
            }
        }

        private List<ExportedFunction> exports;
        /// <summary>
        /// The names of the functions that are exported from the module
        /// </summary>
        public List<ExportedFunction> Exports
        {
            get
            {
                //If we havn't loaded the exports yet, we need to traverse the exports directory
                if (exports == null)
                {
                    exports = new List<ExportedFunction>();
                    uint exportPtr = GetDirectoryDataPtr(DataDirectories.Export);
                    if (exportPtr != 0)
                    {
                        uint numberOfFunctions = GetValue<uint>(exportPtr + 0x18); //ExportDirectory.NumberOfFunctions

                        uint addressOfFunctions = GetDataPtr(GetValue<uint>(exportPtr + 0x1C)); //ExportDirectory.AddressOfFunctions
                        uint addressOfNames = GetDataPtr(GetValue<uint>(exportPtr + 0x20)); //ExportDirectory.AddressOfNames
                        uint addressOfNameOrdinals = GetDataPtr(GetValue<uint>(exportPtr + 0x24)); //ExportDirectory.AddressOfNameOrdinals

                        //Iterate over each function and load its name
                        for (int i = 0; i < numberOfFunctions; i++)
                        {
                            uint offset = (uint)(i * 4);
                            uint offset2 = (uint)(i * 2);
                            ExportedFunction fn = new ExportedFunction();

                            uint nameRVA = GetValue<uint>(addressOfNames + offset); //Export.Function.NameRVA
                            string name = GetValueString(GetDataPtr(nameRVA), 0x255);

                            fn.Ordinal = i + 1;
                            fn.RVA = GetValue<uint>(addressOfFunctions + offset); //Export.Function.RVA
                            fn.Hint = GetValue<ushort>(addressOfNameOrdinals + offset2); //Export.Function.NameOrdinal
                            fn.Name = name.Substring(0, name.IndexOf('\0'));

                            exports.Add(fn);
                        }
                    }
                }
                return exports;
            }
        }

        private List<ImportedModule> imports;
        /// <summary>
        /// The list of imported modules and their imported functions
        /// </summary>
        public List<ImportedModule> Imports
        {
            get
            {
                if (imports == null)
                {
                    imports = new List<ImportedModule>();
                    //Get the pointer to the imports directory
                    uint importsPtr = GetDirectoryDataPtr(DataDirectories.Import);
                    if (importsPtr != 0)
                    {
                        //Iterate over each imported module
                        while (true)
                        {
                            uint originalFirstThunkRVA = GetValue<uint>(importsPtr); //ImportDirectory[i].OriginalFirstThunk
                            uint firstThunkRVA = GetValue<uint>(importsPtr + 16); //ImportDirectory[i].FirstThunk

                            //If the RVAs are 0, then we are at the end of the array
                            if (originalFirstThunkRVA == 0 && firstThunkRVA == 0)
                            {
                                break;
                            }

                            //Convert RVAs to Ptrs
                            uint originalFirstThunkPtr = GetDataPtr(originalFirstThunkRVA);
                            uint firstThunkPtr = GetDataPtr(firstThunkRVA);

                            ImportedModule mod = new ImportedModule();

                            //Get the name of the module
                            uint nameRVA = GetValue<uint>(importsPtr + 12); //ImportDirectory[i].Name
                            string name = GetValueString(GetDataPtr(nameRVA), 255); //ImportDirectory[i].NameData
                            mod.Name = name.Substring(0, name.IndexOf('\0'));
                            mod.Functions = new List<ImportedFunction>();

                            //Iterate over each imported function
                            while (true)
                            {
                                uint oftRVA = GetValue<uint>(firstThunkPtr); //ImportDirectory[i].Functions[i].OFTs
                                //If the RVA is 0 then we are at the end of the array
                                if (oftRVA == 0)
                                {
                                    break;
                                }
                                uint oftPtr = GetDataPtr(oftRVA);

                                ushort hint = GetValue<ushort>(oftPtr); //ImportDirectory[i].Functions[i].Hint
                                string fnName = GetValueString(oftPtr + 0x2, 255); //ImportDirectory[i].Functions[i].Name

                                mod.Functions.Add(new ImportedFunction
                                {
                                    Hint = hint,
                                    Name = fnName.Substring(0, fnName.IndexOf('\0'))
                                });

                                firstThunkPtr += 4; //sizeof(thunkPtr)
                            }

                            imports.Add(mod);
                            importsPtr += 20; //sizeof(IMAGE_IMPORT_DESCRIPTOR)
                        }
                    }
                }
                return imports;
            }
        }

        /// <summary>
        /// Create a new PE analyser
        /// </summary>
        /// <param name="filePath">The path to the file to inspect</param>
        public WindowsPE(string filePath)
        {
            this.FilePath = filePath;
        }

        /// <summary>
        /// Compute the assembly type and target architecture
        /// </summary>
        private void ComputePETargets()
        {
            if (IsValidPE) //DosHeader.e_magic
            {
                ushort magic = GetValue<ushort>(NtHeaderPointer + 0x18); //OptionalHeader.Magic

                if (magic == (ushort)MagicType.IMAGE_NT_OPTIONAL_HDR32_MAGIC)
                {
                    //x86 or AnyCPU
                    exportsDirectoryRVA = NtHeaderPointer + 0x78;
                    targetArchitecture = TargetPlatformArchitecture.x86;
                }
                else
                {
                    //x64
                    exportsDirectoryRVA = NtHeaderPointer + 0x88;
                    targetArchitecture = TargetPlatformArchitecture.x64;
                }

                //Load the CLR Pointer
                if (CLRDirectoryPointer > 0)
                {
                    assemblyType = AssemblyCodeType.Managed;
                    if (targetArchitecture != TargetPlatformArchitecture.x64)
                    {
                        if (GetValue<uint>(CLRDirectoryPointer + 0x10) == 0x1) //CLRDirectory.Flags
                        {
                            targetArchitecture = TargetPlatformArchitecture.AnyCPU;
                        }
                        else
                        {
                            targetArchitecture = TargetPlatformArchitecture.x86;
                        }
                    }

                    clrMajorRuntimeVersion = GetValue<ushort>(CLRDirectoryPointer + 0x4);
                    clrMinorRuntimeVersion = GetValue<ushort>(CLRDirectoryPointer + 0x6);
                }
                else
                {
                    assemblyType = AssemblyCodeType.Native;
                }
            }
        }

        /// <summary>
        /// Convert a RVA into a static offset for usage when module statically loaded
        /// </summary>
        /// <param name="rva">The relative virtual address to convert</param>
        /// <returns>The static pointer to use when RVAs have not been mapped</returns>
        private uint GetDataPtr(uint rva)
        {
            ushort numberOfSections = GetValue<ushort>(NtHeaderPointer + 0x6); //FileHeader.NumberOfSections
            ushort sizeOfOptionalHeader = GetValue<ushort>(NtHeaderPointer + 0x14); //FileHeader.SizeOfOptionalHeader

            uint section = NtHeaderPointer + 0x18 + sizeOfOptionalHeader;
            for (int i = 0; i < numberOfSections; i++)
            {
                var rawSize = GetValue<uint>(section + 0x10); //SectionHeaders[i].SizeOfRawData
                var virtualAddr = GetValue<uint>(section + 0xC); //SectionHeaders[i].VirtualAddress
                if (rva >= virtualAddr && rva < virtualAddr + rawSize)
                {
                    var rawDataPtr = GetValue<uint>(section + 0x14); //SectionHeaders[i].PointerToRawData
                    return rawDataPtr + (rva - virtualAddr);
                }
                section += 0x28; //Add 0x28 to move to the next section as sizeof(SectionHeader)
            }

            return 0;
        }

        /// <summary>
        /// Get the pointer to the specified data directory
        /// </summary>
        /// <param name="directory">The data directiory to get the pointer to</param>
        /// <returns>The pointer to the specified data directory</returns>
        private uint GetDirectoryDataPtr(DataDirectories directory)
        {
            uint rvaPtr = (uint)(ExportsDirectoryRVA + ((ushort)directory * 8)); //sizeof(dd.RVA) + sizeof(dd.Size) == 8
            uint rva = GetValue<uint>(rvaPtr); //OptionalHeader.DataDirectory[dirId].RVA
            uint size = GetDirectoryDataSize(rvaPtr);

            if (size == 0)
            {
                return 0;
            }

            return GetDataPtr(rva);
        }
        private uint GetDirectoryDataSize(uint rvaPtr)
        {
            return GetValue<uint>(rvaPtr + 0x4); //OptionalHeader.DataDirectory[dirId].Size
        }
        private uint GetDirectoryDataSize(DataDirectories directory)
        {
            uint rvaPtr = (uint)(ExportsDirectoryRVA + ((ushort)directory * 8)); //sizeof(dd.RVA) + sizeof(dd.Size) == 8
            return GetDirectoryDataSize(rvaPtr);
        }

        /// <summary>
        /// Get the value at the specified offset as a string
        /// </summary>
        /// <param name="offset">The offset in the buffer to begin the string at</param>
        /// <param name="length">The length of the string</param>
        /// <returns>The string located at the specified position</returns>
        private string GetValueString(uint offset, int length)
        {
            return GetValueString((int)offset, length);
        }
        /// <summary>
        /// Get the value at the specified offset as a string
        /// </summary>
        /// <param name="offset">The offset in the buffer to begin the string at</param>
        /// <param name="length">The length of the string</param>
        /// <returns>The string located at the specified position</returns>
        private string GetValueString(int offset, int length)
        {
            return Encoding.ASCII.GetString(Buffer, offset, length);
        }

        /// <summary>
        /// Get the value at the specified offset converted to the type T
        /// </summary>
        /// <typeparam name="T">A struct type to convert the data to</typeparam>
        /// <param name="offset">The offset to load the structure from</param>
        /// <returns>The structure located at the specified offset</returns>
        private T GetValue<T>(int offset) where T : struct
        {
            return GetValue<T>((uint)offset);
        }
        /// <summary>
        /// Get the value at the specified offset converted to the type T
        /// </summary>
        /// <typeparam name="T">A struct type to convert the data to</typeparam>
        /// <param name="offset">The offset to load the structure from</param>
        /// <returns>The structure located at the specified offset</returns>
        private T GetValue<T>(uint offset) where T : struct
        {
            int length = Marshal.SizeOf(typeof(T));
            byte[] data = new byte[length];
            Array.Copy(Buffer, offset, data, 0, length);
            IntPtr mem = Marshal.AllocHGlobal(length);
            Marshal.Copy(data, 0, mem, length);
            T obj = (T)Marshal.PtrToStructure(mem, typeof(T));
            Marshal.FreeHGlobal(mem);
            return obj;
        }
    }

}
