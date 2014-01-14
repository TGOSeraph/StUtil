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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }





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
