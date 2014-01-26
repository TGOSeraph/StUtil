using System;

namespace StUtil.Native.Monitor
{
    public class ProcessActivityEventArgs : EventArgs
    {
        public ProcessActivityData Data { get; set; }
        public ProcessActivityEventArgs(ProcessActivityData data)
        {
            this.Data = data;
        }
    }
}
