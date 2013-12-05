using StUtil.Native.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace StUtil.Native.Windows.Taskbar.SysTray
{
    public class Win7TaskbarNotifyIconInfo : ITaskbarNotifyIconInfo
    {
        private IntPtr OverflowHandle;
        private IntPtr SystemPromotedHandle;
        private IntPtr UserPromotedHandle;

        public Win7TaskbarNotifyIconInfo()
        {
            WindowFinder trayNotifyWnd = WindowFinder
                .Find(default(string), "Shell_TrayWnd")
                .ChildByClass("TrayNotifyWnd");

            this.UserPromotedHandle = trayNotifyWnd
                .ChildByClass("SysPager")
                .Child("User Promoted Notification Area", "ToolbarWindow32")
                .Handle;

            this.SystemPromotedHandle = trayNotifyWnd
                .Child("System Promoted Notification Area", "ToolbarWindow32")
                .Handle;

            this.OverflowHandle = WindowFinder
                 .Find(default(string), "NotifyIconOverflowWindow")
                 .Child("Overflow Notification Area", "ToolbarWindow32")
                 .Handle;
        }

        public IEnumerable<TaskbarNotifyIcon> GetIcons()
        {
            List<TaskbarNotifyIcon> icons = new List<TaskbarNotifyIcon>();

            using (ExternalProcess proc = new ExternalProcess(this.OverflowHandle))
            {
                proc.Open();
                LoadFromToolbar(proc, icons, this.UserPromotedHandle, true);
                LoadFromToolbar(proc, icons, this.SystemPromotedHandle, true);
                LoadFromToolbar(proc, icons, this.OverflowHandle, false);
            }

            return icons;
        }

        private void LoadFromToolbar(ExternalProcess proc, List<TaskbarNotifyIcon> list, IntPtr hWnd, bool userVisible)
        {
            uint itemCount = (uint)Internal.Native.NativeMethods.SendMessage(hWnd, Internal.Native.NativeConsts.TB_BUTTONCOUNT, IntPtr.Zero, IntPtr.Zero);
            using (ExternalMemory buttonMem = proc.Allocate<Internal.Native.NativeStructs.TBBUTTON>(new Internal.Native.NativeStructs.TBBUTTON()))
            {
                for (int i = 0; i < itemCount; i++)
                {
                    Internal.Native.NativeMethods.SendMessage(hWnd, Internal.Native.NativeConsts.TB_GETBUTTON, new IntPtr(i), buttonMem.Address);
                    Internal.Native.NativeStructs.TBBUTTON btn = buttonMem.Read<Internal.Native.NativeStructs.TBBUTTON>();
                    TaskbarNotifyIcon icon = new TaskbarNotifyIcon();
                    using (ExternalMemory dataMem = proc.Get(new IntPtr((int)btn.dwData), (uint)Marshal.SizeOf(typeof(TRAYDATA))))
                    {
                        TRAYDATA data = dataMem.Read<TRAYDATA>();
                        System.Drawing.Icon ico = System.Drawing.Icon.FromHandle(data.hIcon);
                        if (ico.Width > 0 && ico.Height > 0)
                        {
                            icon.Icon = ico;
                        }
                        icon.WindowHandle = data.hWnd;
                        icon.WindowCaption = Utilities.GetWindowText(data.hWnd);
                        icon.Owner = StUtil.Native.Utilities.GetProcessFromHWND(data.hWnd);
                    }
                    using (ExternalMemory stringMem = proc.Get(btn.iString, 256))
                    {
                        icon.NotifyIconCaption = stringMem.Read(Encoding.Unicode);
                        icon.NotifyIconCaption = icon.NotifyIconCaption.Substring(0, icon.NotifyIconCaption.IndexOf('\0'));
                    }
                    icon.UserVisible = userVisible;
                    list.Add(icon);
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TRAYDATA
        {
            public IntPtr hWnd;
            public uint uID;
            public uint uCallbackMessage;
            private uint reserved0;
            private uint reserved1;
            public IntPtr hIcon;
        }
    }
}
