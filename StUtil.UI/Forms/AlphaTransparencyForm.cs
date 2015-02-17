using StUtil.Native.Internal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StUtil.Extensions;

namespace StUtil.UI.Forms
{
    public class AlphaTransparencyForm : Form
    {
        public AlphaTransparencyForm()
        {
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        }

        public new FormBorderStyle FormBorderStyle
        {
            get
            {
                return System.Windows.Forms.FormBorderStyle.None;
            }
        }

        public void SetBitmap(Bitmap bitmap)
        {
            SetBitmap(bitmap, 255);
        }

        public void SetBitmap(Bitmap bitmap, byte opacity)
        {
            if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
                throw new FormatException("Format32bppArgb bitmap required");

            IntPtr screenDc = NativeMethods.GetDC(IntPtr.Zero);
            IntPtr memDc = NativeMethods.CreateCompatibleDC(screenDc);
            IntPtr hBitmap = IntPtr.Zero;
            IntPtr oldBitmap = IntPtr.Zero;

            try
            {
                hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));
                oldBitmap = NativeMethods.SelectObject(memDc, hBitmap);

                NativeStructs.SIZE size = new NativeStructs.SIZE(bitmap.Width, bitmap.Height);
                NativeStructs.POINT pointSource = new NativeStructs.POINT(0, 0);
                NativeStructs.POINT topPos = new NativeStructs.POINT(Left, Top);
                NativeStructs.BLENDFUNCTION blend = new NativeStructs.BLENDFUNCTION();
                blend.BlendOp = NativeConsts.AC_SRC_OVER;
                blend.BlendFlags = 0;
                blend.SourceConstantAlpha = opacity;
                blend.AlphaFormat = NativeConsts.AC_SRC_ALPHA;

                NativeMethods.UpdateLayeredWindow(Handle, screenDc, ref topPos, ref size, memDc, ref pointSource, 0, ref blend, NativeConsts.ULW_ALPHA);
            }
            finally
            {
                NativeMethods.ReleaseDC(IntPtr.Zero, screenDc);
                if (hBitmap != IntPtr.Zero)
                {
                    NativeMethods.SelectObject(memDc, oldBitmap);
                    NativeMethods.DeleteObject(hBitmap);
                }
                NativeMethods.DeleteDC(memDc);
            }
        }


        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                if (!this.InDesignMode())
                {
                    cp.ExStyle |= 0x00080000;
                }
                return cp;
            }
        }
    }
}
