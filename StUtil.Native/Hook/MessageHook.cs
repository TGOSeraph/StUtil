using StUtil.Native.Internal;
using StUtil.Native.Hook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StUtil.Extensions;
using System.Runtime.InteropServices;
using StUtil.Generic;

namespace StUtil.Native.Hook
{
    public class MessageHook : WindowsHook
    {
        public event EventHandler<EventArgs<Message>> MessageReceived;

        public MessageHook(HookMethod hooker)
            :base(hooker, HookType.Message)
        {
        }

        protected override bool ProcessEvent(IntPtr wParam, IntPtr lParam)
        {
            try
            {
                NativeStructs.CWPSTRUCT msg = (NativeStructs.CWPSTRUCT)Marshal.PtrToStructure(lParam, typeof(NativeStructs.CWPSTRUCT));
                MessageReceived.RaiseEvent(this, new Message
                {
                    HWnd = msg.hwnd,
                    LParam = msg.lParam,
                    Msg = msg.message,
                    WParam = msg.wParam
                });
            }
            catch (Exception)
            {
            }
            return false;
        }
    }
}
