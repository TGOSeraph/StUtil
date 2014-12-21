using StUtil.Native.Windows;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace StUtil.Native.Hook
{
    public abstract class ApplicationMessageHook
    {
        public Process Process { get; private set; }

        public ApplicationMessageHook(Process targetProcess)
        {
            this.Process = targetProcess;
        }

        protected abstract void MessageReceived(object sender, Generic.EventArgs<Message> e);

        public void Hook()
        {
            if (this.Process.Id == System.Diagnostics.Process.GetCurrentProcess().Id)
            {
                MessageHook hook = null;
                Subclasser subclass = null;
                subclass = new Subclasser(System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle, delegate(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam)
                {
                    subclass.Restore();
                    if (hook == null)
                    {
                        hook = new MessageHook(new LocalHook());
                        hook.SetHook();
                        hook.MessageReceived += MessageReceived;
                    }
                    return false;
                });
                subclass.Hook();
            }
            else
            {
                //Injector.Inject(Process, ApplicationMessageHook.Implant, this.GetType().Assembly.Location, this.GetType().FullName);
            }
        }

        public static int Implant(string args)
        {
            try
            {
                string[] split = args.Split(new char[] { ';' }, 2);
                Assembly asm = Assembly.LoadFrom(split[0]);

                Type t = asm.GetType(split[1]);

                ApplicationMessageHook hook = (ApplicationMessageHook)Activator.CreateInstance(t, Process.GetCurrentProcess());

                hook.Hook();
            }
            catch (Exception ex)
            {
                byte[] message = System.Text.Encoding.Unicode.GetBytes(ex.ToString());
                byte[] length = BitConverter.GetBytes(message.Length);
                IntPtr ptr = Marshal.AllocHGlobal(message.Length + length.Length);
                Marshal.Copy(length, 0, ptr, length.Length);
                Marshal.Copy(message, 0, new IntPtr(ptr.ToInt64() + length.Length), message.Length);

                return -ptr.ToInt32();
            }

            return 1;
        }
    }
}