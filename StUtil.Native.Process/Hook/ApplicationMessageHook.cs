using StUtil.Native.Hook;
using StUtil.Native.Process;
using StUtil.Native.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.Native.Hook
{
    public abstract class ApplicationMessageHook
    {
        public System.Diagnostics.Process Process { get; private set; }

        public ApplicationMessageHook(System.Diagnostics.Process targetProcess)
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
                RemoteProcess p = new RemoteProcess(this.Process);
                p.Open();
                p.LoadDotNetModule(Assembly.GetExecutingAssembly().Location, typeof(ApplicationMessageHook).FullName, "Implant", this.GetType().Assembly.Location + ";" + this.GetType().FullName);
            }
        }

        public static int Implant(string args)
        {
            string[] split = args.Split(new char[] { ';' }, 2);
            Assembly asm = Assembly.LoadFrom(split[0]);

            Type t = asm.GetType(split[1]);

            ApplicationMessageHook hook = (ApplicationMessageHook)Activator.CreateInstance(t, System.Diagnostics.Process.GetCurrentProcess());

            hook.Hook();

            return 1;
        }
    }
}
