using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;

namespace StUtil.Native.Windows.Taskbar.SysTray
{
    public class TaskbarNotifyIcon
    {
        public Process Owner { get; set; }
        public IntPtr WindowHandle { get; set; }
        public string WindowCaption { get; set; }
        public string NotifyIconCaption { get; set; }
        public Icon Icon { get; set; }
        public bool UserVisible { get; set; }
    }
}
