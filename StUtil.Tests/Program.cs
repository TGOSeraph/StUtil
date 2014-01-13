using System;
using System.Collections.Generic;
using System.Linq;
using StUtil.CodeGen;
using System.Windows.Forms;
using StUtil.Extensions;

namespace StUtil.Tests
{
    using StUtil.CodeGen.CodeObjects.Misc;
    using StUtil.CodeGen.CodeObjects.CodeStructures;
    using StUtil.CodeGen.CodeObjects.Generic;
    using StUtil.CodeGen.CodeObjects.Data;
    using StUtil.CodeGen.CodeObjects.Attributes;
    using StUtil.CodeGen.CodeObjects.Syntax;
    using System.Runtime.InteropServices;

    static class Program
    {
        private const uint WM_DROPFILES = 0x233;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            IntPtr hWnd = StUtil.Native.WindowFinder.Find("Untitled - Notepad", null).Handle;


            DROPFILES drop = new DROPFILES
            {
                pt = new Internal.Native.NativeStructs.POINT(10, 10),
                fNc = false,
                fWide = true,
                pFiles = Marshal.SizeOf(typeof(DROPFILES))
            };

            int sz = 0;
            string[] files = { @"E:\Development\Programs\AntTrak\AntTrak.sln" };
            for (int i = 0; i < files.Length; i++)
            {
                sz += System.Text.Encoding.Default.GetByteCount(files[i]) + 2;
            }

            sz += 2;

            IntPtr lngHDROP = GlobalAlloc(GHND, new UIntPtr((uint)(drop.pFiles + sz)));
            IntPtr lngPDROP = GlobalLock(lngHDROP);

            IntPtr hMem = Marshal.AllocHGlobal(drop.pFiles);
            Marshal.StructureToPtr(drop, hMem, true);
            CopyMemory(lngPDROP, hMem, drop.pFiles);
            Marshal.FreeHGlobal(hMem);

            lngPDROP = lngPDROP + drop.pFiles;


            for (int i = 0; i < files.Length; i++)
            {
                hMem = Marshal.StringToHGlobalUni(files[i]);
                int ssz = System.Text.Encoding.Unicode.GetByteCount(files[i]);
                CopyMemory(lngPDROP, hMem,ssz );
                Marshal.FreeHGlobal(hMem);
                lngPDROP = lngPDROP + ssz + 2;
            }

            GlobalUnlock(lngHDROP);
            StUtil.Internal.Native.NativeMethods.SendMessage(hWnd, WM_DROPFILES, lngHDROP, IntPtr.Zero);

            //            For i = 0 To lngCnt - 1
            //bytD = strFile (i)
            //CopyMemory ByVal lngPDROP, bytD (0), UBound (bytD) + 1 + 2
            //lngPDROP = lngPDROP + UBound (bytD) + 1 + 2
            //Next
            //GlobalUnlock lngHDROP

            //IntPtr hMem = GlobalAlloc(GHND | GMEM_SHARE, new UIntPtr(1500));
            //IntPtr hLock = GlobalLock(hMem);

            //IntPtr ptrAddr = hLock + Marshal.SizeOf(typeof(DropFiles));

            //DropFiles df = new DropFiles();
            //df.pFiles = ptrAddr.ToInt32();
            //df.pt = new Internal.Native.NativeStructs.POINT(0, 0);
            //df.fNc = false;
            //df.fWide = false;
            //byte[] b = df.ToBytes();
            //Marshal.Copy(b, 0, hLock, b.Length);
            //b = System.Text.Encoding.ASCII.GetBytes("C:\\"); ;
            //Marshal.Copy(b, 0, ptrAddr, b.Length);

            //StUtil.Internal.Native.NativeMethods.SendMessage(hWnd, WM_DROPFILES, ptrAddr, IntPtr.Zero);

            //GlobalUnlock(hLock);
            //GlobalFree(hMem);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }




        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory")]
        static extern void CopyMemory(IntPtr dest, IntPtr src, int size);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GlobalUnlock(IntPtr hMem);
        [DllImport("kernel32.dll")]
        static extern IntPtr GlobalAlloc(uint uFlags, UIntPtr dwBytes);

        public struct DROPFILES
        {
            public int pFiles;
            public StUtil.Internal.Native.NativeStructs.POINT pt;
            public bool fNc;
            public bool fWide;
        }

        public const uint GMEM_SHARE = 0x2000;
        public const uint GMEM_MOVEABLE = 0x2;
        public const uint GMEM_ZEROINIT = 0x40;
        public const uint GHND = (GMEM_MOVEABLE | GMEM_ZEROINIT);

        [DllImport("kernel32.dll")]
        static extern IntPtr GlobalLock(IntPtr hMem);


        [DllImport("kernel32.dll")]
        static extern IntPtr GlobalFree(IntPtr hMem);






        //        procedure DoDropFiles(Wnd: HWND; Files: TStringList);

        //var
        //  Size: Cardinal;
        //  DropFiles: PDropFiles;
        //  Run: PChar;
        //  MemHandle: THandle;
        //  I: Integer;

        //begin
        //  // first determine size of string buffer we have to allocate
        //  Size := 0;
        //  for I := 0 to Files.Count - 1 do
        //  begin
        //    // number of characters per string (as ANSI) plus one #0 terminator
        //    Inc(Size, Length(Files[I]) + 1);
        //  end;
        //  if Size > 0 then
        //  begin
        //    // entire string list is terminated by another #0, add drop files structure size too
        //    Inc(Size, 1 + SizeOf(TDropFiles));
        //    // allocate globally accessible memory
        //    MemHandle := GlobalAlloc(GHND or GMEM_SHARE, Size);
        //    DropFiles := GlobalLock(MemHandle);
        //    // fill the header
        //    with DropFiles^ do
        //    begin
        //      pFiles := SizeOf(TDropFiles); // offset of file list, it follows immediately the structure
        //      pt := Point(0, 0);            // drop point (client coords), not important here
        //      fNC := False;                 // is it on NonClient area }, not important here
        //      fWide := False;               // WIDE character switch, we pass ANSI string in this routine
        //    end;
        //    // and finally the file names
        //    Run := Pointer(DropFiles);
        //    Inc(Run, SizeOf(TDropFiles));
        //    for I := 0 to Files.Count - 1 do
        //    begin
        //      StrPCopy(Run, Files[I]);
        //      Inc(Run, Length(Files[I]));
        //    end;
        //    // put a final #0 character at the end
        //    Run^ := #0;
        //    // release the lock we have to the memory,...
        //    GlobalUnlock(MemHandle);
        //    // ...do the message...
        //    SendMessage(Wnd, WM_DROPFILES, MemHandle, 0);
        //    // ... and finally release the memory
        //    GlobalFree(MemHandle);
        //  end;
        //end;
    }
}
