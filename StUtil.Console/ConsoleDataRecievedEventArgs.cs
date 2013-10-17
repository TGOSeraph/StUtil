using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StUtil.Console
{
    /// <summary>
    /// Event args passed when data is recieved from the console
    /// </summary>
    /// <remarks>
    /// 2013-10-17  - Initial version
    /// </remarks>
    public class ConsoleDataRecievedEventArgs : EventArgs
    {
        /// <summary>
        /// If the message was sent to STDERROR
        /// </summary>
        public bool IsError { get; private set; }
        /// <summary>
        /// The message that was sent
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Create a new console data recieved arg
        /// </summary>
        /// <param name="message">The message that was sent</param>
        /// <param name="isError">If it was an error</param>
        public ConsoleDataRecievedEventArgs(string message, bool isError)
        {
            IsError = isError;
            Message = message;
        }
    }
}
