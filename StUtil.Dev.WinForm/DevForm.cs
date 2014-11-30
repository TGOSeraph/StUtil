using System;
using System.Threading;
using System.Windows.Forms;

namespace StUtil.Dev.WinForm
{
    public partial class DevForm : Form
    {
        public DevForm()
        {
            InitializeComponent();

            this.Shown += DevForm_Shown;
        }

        private void DevForm_Shown(object sender, EventArgs e)
        {
        }

        private void Anim(Panel p)
        {
            StUtil.UI.Animation.Animate(UI.Animation.Easing.EaseInOutQuad, (this.Width / 2), TimeSpan.FromSeconds(2), p, "Left").Animated += (s, ev) =>
            {
                StUtil.UI.Animation.Animate(UI.Animation.Easing.EaseInOutQuad, this.Width + p.Location.X, TimeSpan.FromSeconds(2), p, "Left");
            };
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread.Sleep(1000);
            StUtil.Native.Input.Keyboard.Press(Keys.A);
        }
    }

    //public class Test : ApplicationMessageHook
    //{
    //    public Test(Process target)
    //        : base(target)
    //    {
    //    }
    //    protected override void MessageReceived(object sender, Generic.EventArgs<Message> e)
    //    {
    //        if (e.Value.Msg == (int)StUtil.Native.Internal.NativeEnums.WM.INITDIALOG)
    //        {
    //            MessageBoxDialog dialog = new MessageBoxDialog(e.Value.HWnd);
    //            dialog.Text = "Face";
    //            dialog.Message.Text = "Hello";
    //            dialog.Button1.Text = "Hello Paul";
    //            if (dialog.Button2 != null)
    //            {
    //                dialog.Button2.Text = "Nay";
    //                if (dialog.Button3 != null)
    //                {
    //                    dialog.Button3.Text = "Screw this";
    //                }
    //            }
    //            if (dialog.Image != null)
    //            {
    //                dialog.Image.SetIcon(new Icon(@"C:\Users\sot\Pictures\Icons\Corporate.ico"));
    //            }
    //        }
    //    }
    //}
}