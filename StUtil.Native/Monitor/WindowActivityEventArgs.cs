using System;

namespace StUtil.Native.Monitor
{
    public class WindowActivityEventArgs : EventArgs
    {
        public WindowActivityData Data { get; set; }
        public WindowActivityEventArgs(WindowActivityData data)
        {
            this.Data = data;
        }
    }
}
