using StUtil.Extensions;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Linq;

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
            GenerateScheme(Color.Orange);
        }

        public class ThemeColors
        {
            public Color Max { get; set; }
            public Color Mid { get; set; }
            public Color Min { get; set; }

            private Color highlight;
            public Color Highlight
            {
                get
                {
                    return highlight;
                }
                set
                {
                    highlight = value;
                    HighlightLight = ControlPaint.Light(highlight);
                    HighlightLightLight = ControlPaint.LightLight(highlight);
                    HighlightDark = ControlPaint.Dark(highlight);
                    HighlightDarkDark = ControlPaint.DarkDark(highlight);
                    HighlightInverse = highlight.Invert();
                    HighlightBW = GetBlackOrWhiteContrast(highlight);
                }
            }
            public Color HighlightLight { get; set; }
            public Color HighlightLightLight { get; set; }
            public Color HighlightDark { get; set; }
            public Color HighlightDarkDark { get; set; }
            public Color HighlightInverse { get; set; }
            public Color HighlightBW { get; set; }

            private Color background;
            public Color Background
            {
                get
                {
                    return background;
                }
                set
                {
                    background = value;
                    BackgroundLight = ControlPaint.Light(background);
                    BackgroundLightLight = ControlPaint.LightLight(background);
                    BackgroundDark = ControlPaint.Dark(background);
                    BackgroundDarkDark = ControlPaint.DarkDark(background);
                    Foreground = GetBlackOrWhiteContrast(background);
                }
            }
            public Color BackgroundLight { get; set; }
            public Color BackgroundLightLight { get; set; }
            public Color BackgroundDark { get; set; }
            public Color BackgroundDarkDark { get; set; }
            public Color Foreground { get; set; }

            public ThemeColors(Color background, Color highlight)
            {
                Max = Color.White;
                Mid = Color.Gray;
                Min = Color.Black;

                Highlight = highlight;

                Background = background;
            }

            public static Color GetBlackOrWhiteContrast(Color color)
            {
                var l = 0.2126 * (color.R / 255.0) + 0.7152 * (color.G / 255.0) + 0.0722 * (color.B / 255.0);
                if (l < 0.6)
                {
                    return Color.White;
                }
                else
                {
                    return Color.Black;
                }
            }
        }

        public void GenerateScheme(Color v)
        {
            ThemeColors tc = new ThemeColors(Color.FromArgb(50, 50, 50), Color.HotPink);
            this.BackColor = tc.Background;
            this.ForeColor = tc.Foreground;
            pnlHighlight.BackColor = tc.Highlight;
            pnlHighlight.ForeColor = tc.HighlightBW;
            pnlBorder.BackColor = tc.HighlightLight;
            pnlContent.BackColor = tc.BackgroundDark;
            pnlBgBorder.BackColor = tc.BackgroundLight;
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