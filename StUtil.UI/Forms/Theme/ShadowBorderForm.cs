using StUtil.Extensions;
using StUtil.Native.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.UI.Forms.Theme
{
    public class ShadowBorderForm : OuterBorderForm
    {
        public ShadowBorderForm(TabAlignment side, int size)
            : base(side, size) { }

        private Bitmap activeMain;
        private Bitmap activeCorner1;
        private Bitmap activeCorner2;

        private Bitmap inactiveMain;
        private Bitmap inactiveCorner1;
        private Bitmap inactiveCorner2;

        private byte opacity = 210;
        [DefaultValue(210)]
        public new byte Opacity
        {
            get
            {
                return opacity;
            }
            set
            {
                opacity = value;
                GenerateBorderImages();
                UpdateBorders();
            }
        }

        public override bool Active
        {
            get
            {
                return base.Active;
            }
            set
            {
                base.Active = value;
                if (!this.Disposing && !this.IsDisposed)
                {
                    UpdateBorders();
                }
            }
        }

        private void GenerateBorderImages()
        {
            GenerateBorderImages(this.BackColor, ref activeMain, ref activeCorner1, ref activeCorner2);
            GenerateBorderImages(Color.DarkGray, ref inactiveMain, ref inactiveCorner1, ref inactiveCorner2);
        }

        private void GenerateBorderImages(Color color, ref Bitmap main, ref Bitmap corner1, ref Bitmap corner2)
        {
            int o = 255 - opacity;

            if (o < 10)
            {
                o = 10;
            }
            main = new Bitmap(base.BorderSize, 1);

            Bitmap b = new Bitmap(2, 2);
            using (Pen p = new Pen(color))
            {
                using (SolidBrush sb = new SolidBrush(color))
                {
                    using (Graphics g = Graphics.FromImage(b))
                    {
                        g.FillRectangle(sb, new Rectangle(0, 0, b.Width, b.Height));
                    }
                    Bitmap s = new Bitmap(o, o);
                    using (Graphics g = Graphics.FromImage(s))
                    {
                        g.DrawImage(b, 0, 0, s.Width, s.Height);
                    }

                    using (Graphics g = Graphics.FromImage(main))
                    {
                        g.DrawLine(p, 0, 0, 0, 1);
                        g.DrawImage(s, new Rectangle(0, 0, main.Width, main.Height), new Rectangle(s.Width - main.Width, s.Height - base.BorderSize - main.Height, main.Width, main.Height), GraphicsUnit.Pixel);
                    }

                    switch (base.Side)
                    {
                        case TabAlignment.Right:
                            break;
                        case TabAlignment.Left:
                            main.RotateFlip(RotateFlipType.RotateNoneFlipX);
                            break;
                        case TabAlignment.Top:
                            main.RotateFlip(RotateFlipType.Rotate270FlipNone);

                            corner1 = new Bitmap(base.BorderSize, base.BorderSize);
                            using (Graphics g = Graphics.FromImage(corner1))
                            {
                                g.DrawImage(s, new Rectangle(0, 0, corner1.Width, corner1.Height), new Rectangle(s.Width - corner1.Width, s.Height - corner1.Height, corner1.Width, corner1.Height), GraphicsUnit.Pixel);
                                g.FillRectangle(sb, 0, 0, 1, 1);
                            }
                            corner1.RotateFlip(RotateFlipType.RotateNoneFlipY);
                            corner2 = (Bitmap)corner1.Clone();
                            corner1.RotateFlip(RotateFlipType.RotateNoneFlipX);

                            break;
                        case TabAlignment.Bottom:
                            main.RotateFlip(RotateFlipType.Rotate90FlipNone);

                            corner1 = new Bitmap(base.BorderSize, base.BorderSize);
                            using (Graphics g = Graphics.FromImage(corner1))
                            {
                                g.DrawImage(s, new Rectangle(0, 0, corner1.Width, corner1.Height), new Rectangle(s.Width - corner1.Width, s.Height - corner1.Height, corner1.Width, corner1.Height), GraphicsUnit.Pixel);
                                g.FillRectangle(sb, 0, 0, 1, 1);
                            }
                            corner2 = (Bitmap)corner1.Clone();
                            corner1.RotateFlip(RotateFlipType.RotateNoneFlipX);

                            break;
                    }
                }
            }
        }

        private void UpdateBorders()
        {
            if (this.ClientSize.Width == 0 || this.ClientSize.Height == 0)
            {
                return;
            }

            Bitmap main, corner1, corner2;
            if (this.Active)
            {
                main = activeMain;
                corner1 = activeCorner1;
                corner2 = activeCorner2;
            }
            else
            {
                main = inactiveMain;
                corner1 = inactiveCorner1;
                corner2 = inactiveCorner2;
            }

            Bitmap bmp = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.None;
                g.PixelOffsetMode = PixelOffsetMode.Half;
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                if (base.Side == TabAlignment.Bottom || base.Side == TabAlignment.Top)
                {
                    g.DrawImage(corner1, 0, 0);
                    for (int i = corner1.Width; i < this.ClientSize.Width - corner2.Width; i++)
                    {
                        g.DrawImage(main, i, 0);
                    }
                    g.DrawImage(corner2, bmp.Width - activeCorner2.Width, 0);
                }
                else
                {
                    g.DrawImage(main, 0, 0, bmp.Width, bmp.Height);
                }
            }
            SetBitmap(bmp);
        }

        private void SetBitmap(Bitmap bitmap)
        {
            SetBitmap(bitmap, 255);
        }

        private void SetBitmap(Bitmap bitmap, byte opacity)
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

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            UpdateBorders();
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            GenerateBorderImages();
            UpdateBorders();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateBorders();
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

        public override void Update(object args)
        {
            if (args is Color)
            {
                this.BackColor = (Color)args;
            }
            else
            {
                UpdateBorders();
            }
        }
    }
}
