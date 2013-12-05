using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StUtil.Native.Windows.Taskbar.SysTray
{
    public interface ITaskbarNotifyIconInfo
    {
        IEnumerable<TaskbarNotifyIcon> GetIcons();
    }
}
