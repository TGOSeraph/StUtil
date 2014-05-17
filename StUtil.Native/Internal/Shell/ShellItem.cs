using StUtil.Internal.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Internal.Shell
{
    public class ShellItem
    {
        private static ShellItem desktopItem;

        static NativeInterfaces.IShellFolder m_shRootShell = null;

        IntPtr m_pIDL = IntPtr.Zero;

        NativeInterfaces.IShellFolder m_shShellFolder = null;

        /// <summary>
        /// Gets or set the display name for this shell item.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets a boolean indicating whether this shell item has any sub-folders.
        /// </summary>
        public bool HasSubFolder { get; set; }

        /// <summary>
        /// Gets or sets the system image list icon index for this shell item.
        /// </summary>
        public Int32 IconIndex { get; set; }

        /// <summary>
        /// Gets or sets a boolean indicating whether this shell item is a folder.
        /// </summary>
        public bool IsFolder { get; set; }

        /// <summary>
        /// Gets or sets the system path for this shell item.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets the fully qualified PIDL for this shell item.
        /// </summary>
        public IntPtr PIDL
        {
            get { return m_pIDL; }
        }

        /// <summary>
        /// Gets the IShellFolder interface of the Desktop.
        /// </summary>
        public NativeInterfaces.IShellFolder RootShellFolder
        {
            get { return m_shRootShell; }
        }

        /// <summary>
        /// Gets the IShellFolder interface of this shell item.
        /// </summary>
        public NativeInterfaces.IShellFolder ShellFolder
        {
            get { return m_shShellFolder; }
        }

        /// <summary>
        /// Constructor. Create a sub-item shell item object.
        /// </summary>
        /// <param name="shDesktop">IShellFolder interface of the Desktop</param>
        /// <param name="pIDL">The fully qualified PIDL for this shell item</param>
        /// <param name="shParent">The ShellItem object for this item's parent</param>
        public ShellItem(NativeInterfaces.IShellFolder shDesktop, IntPtr pIDL, ShellItem shParent)
        {
            // We need the Desktop shell item to exist first.
            if (desktopItem == null)
                throw new Exception("The root shell item must be created before creating a sub-item");

            // Create the FQ PIDL for this new item.
            m_pIDL = NativeMethods.ILCombine(shParent.PIDL, pIDL);

            // Get the properties of this item.
            NativeEnums.SFGAOF uFlags = NativeEnums.SFGAOF.SFGAO_FOLDER | NativeEnums.SFGAOF.SFGAO_HASSUBFOLDER;

            // Here we get some basic attributes.
            shDesktop.GetAttributesOf(1, out m_pIDL, out uFlags);
            IsFolder = Convert.ToBoolean(uFlags & NativeEnums.SFGAOF.SFGAO_FOLDER);
            HasSubFolder = Convert.ToBoolean(uFlags & NativeEnums.SFGAOF.SFGAO_HASSUBFOLDER);

            // Now we want to get extended attributes such as the icon index etc.
            NativeStructs.SHFILEINFO shInfo = new NativeStructs.SHFILEINFO();
            NativeEnums.SHGFI vFlags =
                NativeEnums.SHGFI.SHGFI_SMALLICON |
                NativeEnums.SHGFI.SHGFI_SYSICONINDEX |
                NativeEnums.SHGFI.SHGFI_PIDL |
                NativeEnums.SHGFI.SHGFI_DISPLAYNAME;
            NativeMethods.SHGetFileInfo(m_pIDL, 0, out shInfo, (uint)Marshal.SizeOf(shInfo), vFlags);
            DisplayName = shInfo.szDisplayName;
            IconIndex = shInfo.iIcon;
            Path = GetPath();

            // Create the IShellFolder interface for this item.
            if (IsFolder)
            {
                uint hRes = shParent.m_shShellFolder.BindToObject(pIDL, IntPtr.Zero, ref NativeConsts.IID_IShellFolder, out m_shShellFolder);
                if (hRes != 0)
                    Marshal.ThrowExceptionForHR((int)hRes);
            }
        }

        /// <summary>
        /// Constructor. Creates the ShellItem object for the Desktop.
        /// </summary>
        protected ShellItem()
        {
            // Obtain the root IShellFolder interface.
            int hRes = NativeMethods.SHGetDesktopFolder(ref m_shRootShell);
            if (hRes != 0)
                Marshal.ThrowExceptionForHR(hRes);

            // Now get the PIDL for the Desktop shell item.
            hRes = NativeMethods.SHGetSpecialFolderLocation(IntPtr.Zero, NativeEnums.CSIDL.CSIDL_DESKTOP, ref m_pIDL);
            if (hRes != 0)
                Marshal.ThrowExceptionForHR(hRes);

            // Now retrieve some attributes for the root shell item.
            NativeStructs.SHFILEINFO shInfo = new NativeStructs.SHFILEINFO();
            NativeMethods.SHGetFileInfo(m_pIDL, 0, out shInfo, (uint)Marshal.SizeOf(shInfo),
                NativeEnums.SHGFI.SHGFI_DISPLAYNAME |
                NativeEnums.SHGFI.SHGFI_PIDL |
                NativeEnums.SHGFI.SHGFI_SMALLICON |
                NativeEnums.SHGFI.SHGFI_SYSICONINDEX
            );

            // Set the arributes to object properties.
            DisplayName = shInfo.szDisplayName;
            IconIndex = shInfo.iIcon;
            IsFolder = true;
            HasSubFolder = true;
            Path = GetPath();

            // Internal with no set{} mutator.
            m_shShellFolder = RootShellFolder;
        }

        ~ShellItem()
        {
            // Release the IShellFolder interface of this shell item.
            if (m_shShellFolder != null)
                Marshal.ReleaseComObject(m_shShellFolder);

            // Free the PIDL too.
            if (!m_pIDL.Equals(IntPtr.Zero))
                Marshal.FreeCoTaskMem(m_pIDL);

            GC.SuppressFinalize(this);
        }

        public static ShellItem GetRoot()
        {
            if (desktopItem == null)
            {
                desktopItem = new ShellItem();
            }
            return desktopItem;
        }

        /// <summary>
        /// Retrieves an array of ShellItem objects for sub-folders of this shell item.
        /// </summary>
        /// <returns>List of ShellItem objects.</returns>
        public List<ShellItem> GetDirectories()
        {
            return GetItems(NativeEnums.SHCONTF.SHCONTF_FOLDERS);
        }

        public List<ShellItem> GetFiles()
        {
            return GetItems(NativeEnums.SHCONTF.SHCONTF_NONFOLDERS);
        }

        /// <summary>
        /// Gets the system path for this shell item.
        /// </summary>
        /// <returns>A path string.</returns>
        public string GetPath()
        {
            StringBuilder strBuffer = new StringBuilder(256);
            NativeMethods.SHGetPathFromIDList(m_pIDL, strBuffer);
            return strBuffer.ToString();
        }

        public bool HasDirectories()
        {
            return HasItems(NativeEnums.SHCONTF.SHCONTF_FOLDERS);
        }

        private bool HasItems(NativeEnums.SHCONTF type)
        {
            // Make sure we have a folder.
            if (IsFolder == false)
                throw new Exception("Unable to retrieve items for a non-folder.");

            NativeInterfaces.IEnumIDList pEnum = null;
            uint hRes = ShellFolder.EnumObjects(IntPtr.Zero, type, out pEnum);
            if (hRes != 0)
                Marshal.ThrowExceptionForHR((int)hRes);
            if (pEnum == null)
            {
                return false;
            }
            IntPtr pIDL = IntPtr.Zero;
            Int32 iGot = 0;

            // Grab the first enumeration.
            pEnum.Next(1, out pIDL, out iGot);

            return !pIDL.Equals(IntPtr.Zero) && iGot == 1;
        }

        private List<ShellItem> GetItems(NativeEnums.SHCONTF type)
        {
            // Make sure we have a folder.
            if (IsFolder == false)
                throw new Exception("Unable to retrieve items for a non-folder.");

            List<ShellItem> arrChildren = new List<ShellItem>();
            try
            {
                // Get the IEnumIDList interface pointer.
                NativeInterfaces.IEnumIDList pEnum = null;
                uint hRes = ShellFolder.EnumObjects(IntPtr.Zero, type, out pEnum);
                if (hRes != 0)
                    Marshal.ThrowExceptionForHR((int)hRes);

                IntPtr pIDL = IntPtr.Zero;
                Int32 iGot = 0;

                // Grab the first enumeration.
                pEnum.Next(1, out pIDL, out iGot);

                // Then continue with all the rest.
                while (!pIDL.Equals(IntPtr.Zero) && iGot == 1)
                {
                    // Create the new ShellItem object.
                    arrChildren.Add(new ShellItem(m_shRootShell, pIDL, this));

                    // Free the PIDL and reset counters.
                    Marshal.FreeCoTaskMem(pIDL);
                    pIDL = IntPtr.Zero;
                    iGot = 0;

                    // Grab the next item.
                    pEnum.Next(1, out pIDL, out iGot);
                }

                // Free the interface pointer.
                if (pEnum != null)
                    Marshal.ReleaseComObject(pEnum);
            }
            catch (Exception)
            {
            }

            return arrChildren;
        }
    }
}
