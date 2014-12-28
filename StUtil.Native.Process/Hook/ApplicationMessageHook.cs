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
    public abstract class ApplicationMessageHook : ApplicationHook
    {
        public ApplicationMessageHook(System.Diagnostics.Process targetProcess)
            : base(targetProcess)
        {
        }

        protected abstract void MessageReceived(object sender, Generic.EventArgs<Message> e);

        protected override void ApplyHook(string args)
        {
            MessageHook hook = null;
            Subclasser subclass = null;
            /* We will have been injected into the remote process using CreateRemoteThread meaning we will
             * not be on the UI thread. Therefore we need to subclass the main window of the process and
             * then trigger our message hook after the first message has been received as we will be on
             * the UI thread.
             */
            subclass = new Subclasser(System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle, delegate(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam)
            {
                //Unsubclass the window as we are performing a hook
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
    }
}
