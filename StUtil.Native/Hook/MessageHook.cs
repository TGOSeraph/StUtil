using StUtil.Extensions;
using StUtil.Generic;
using StUtil.Native.Internal;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace StUtil.Native.Hook
{
    public class MessageHook : WindowsHook
    {
        public event EventHandler<EventArgs<Message>> MessageReceived;

        public MessageHook(HookMethod hooker)
            : base(hooker, HookType.Message)
        {
        }

        protected override bool ProcessEvent(IntPtr wParam, IntPtr lParam)
        {
            NativeStructs.CWPSTRUCT msg = (NativeStructs.CWPSTRUCT)Marshal.PtrToStructure(lParam, typeof(NativeStructs.CWPSTRUCT));
            MessageReceived.RaiseEvent(this, new Message
            {
                HWnd = msg.hwnd,
                LParam = msg.lParam,
                Msg = msg.message,
                WParam = msg.wParam
            });
            return false;
        }
    }
}