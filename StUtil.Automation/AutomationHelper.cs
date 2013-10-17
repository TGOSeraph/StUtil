using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using System.Windows.Automation;

namespace StUtil.Automation
{
    /// <summary>
    /// Wrapper for automation elements to provide easier traversal
    /// </summary>
    /// <remarks>
    /// 2013-06-28  - Initial version
    /// </remarks>
    public class AutomationHelper : BaseAutomationHelper<AutomationHelper>
    {
        /// <summary>
        /// Create a new basic automation helper
        /// </summary>
        /// <param name="element">The element to wrap</param>
        public AutomationHelper(AutomationElement element)
        {
            base.element = element;
        }
        /// <summary>
        /// Create an instance of the class to wrap an element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override AutomationHelper CreateInstance(AutomationElement element)
        {
            return new AutomationHelper(element);
        }

        /// <summary>
        /// Create an automation helper from a specified process
        /// </summary>
        /// <param name="proc">The process to get the main window from to use as the element</param>
        /// <returns>A wrapper around the processes main window</returns>
        public static AutomationHelper FromProcess(Process proc)
        {
            return FromHandle(proc.MainWindowHandle);
        }
        /// <summary>
        /// Create an automation helper from a specified process
        /// </summary>
        /// <param name="name">The name of the process to search for</param>
        /// <param name="useFirst">If true, the first process with the specified name will be used, else if multiple processes are found, and error will be thrown</param>
        /// <returns>A wrapper around the processes main window</returns>
        public static AutomationHelper FromProcess(string name, bool useFirst = false)
        {
            Process[] proc = Process.GetProcessesByName(name);
            if (proc.Length > 1 && !useFirst)
            {
                throw new Exception("Multiple processes found matching " + name);
            }
            return FromHandle(proc[0].MainWindowHandle);
        }

        /// <summary>
        /// Create an automation helper from a specific window handle
        /// </summary>
        /// <param name="handle">The window handle to wrap</param>
        /// <returns>A wrapper around the specified handle</returns>
        public static AutomationHelper FromHandle(IntPtr handle)
        {
            return new AutomationHelper(AutomationElement.FromHandle(handle));
        }

        /// <summary>
        /// Wait for a process to create a main window
        /// </summary>
        /// <param name="proc">The process to wait for and get a helper for</param>
        /// <param name="timeout">The amount of time to wait</param>
        /// <returns>A helper for the main window of the specified process</returns>
        public static AutomationHelper WaitForProcessHandle(Process proc, int timeout)
        {
             DateTime dt = DateTime.Now.Add(TimeSpan.FromMilliseconds(timeout));
             while (DateTime.Now < dt)
             {
                 if (proc.MainWindowHandle != IntPtr.Zero)
                 {
                     return FromHandle(proc.MainWindowHandle);
                 }
             }
             throw new TimeoutException();
        }
    }
}
