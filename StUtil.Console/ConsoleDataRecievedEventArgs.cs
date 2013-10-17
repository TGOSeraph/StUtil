using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace StUtil.Console
{
    public class ConsoleDataRecievedEventArgs : EventArgs
    {
        public bool IsError { get; private set; }
        public string Message { get; private set; }

        public ConsoleDataRecievedEventArgs(string message, bool isError)
        {
            IsError = isError;
            Message = message;
        }
    }
}
