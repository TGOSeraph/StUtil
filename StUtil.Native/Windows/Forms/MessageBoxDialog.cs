using StUtil.Native.Windows.Controls;
using System;
using System.Linq;

namespace StUtil.Native.Windows.Forms
{
    public class MessageBoxDialog : NativeForm
    {
        public NativeButton Button1 { get; private set; }

        public NativeButton Button2 { get; private set; }

        public NativeButton Button3 { get; private set; }

        public NativeStatic Message { get; set; }

        public NativeStatic Image { get; set; }

        public MessageBoxDialog(IntPtr handle)
            : base(handle)
        {
            var children = Children;
            var buttons = children.OfType<NativeButton>().ToList();
            switch (buttons.Count)
            {
                case 1:
                    Button1 = buttons[0];
                    break;

                case 2:
                    Button1 = buttons[0];
                    Button2 = buttons[1];
                    break;

                case 3:
                    Button1 = buttons[0];
                    Button2 = buttons[1];
                    Button3 = buttons[2];
                    break;

                default:
                    throw new Exception("Could not find messagebox buttons");
            }

            var statics = children.OfType<NativeStatic>().ToList();
            if (statics.Count == 1)
            {
                Message = statics[0];
            }
            else
            {
                Image = statics[0];
                Message = statics[1];
            }
        }
    }
}