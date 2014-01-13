using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace StUtil.UI.Controls
{
    public class InterpolatedPictureBox : PictureBox
    {
        public InterpolationMode InterpolationMode { get; set; }
        public PixelOffsetMode PixelOffsetMode { get; set; }
        public SmoothingMode SmoothingMode { get; set; }

        public InterpolatedPictureBox()
        {
            this.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            this.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            this.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            InterpolationMode im = pe.Graphics.InterpolationMode;
            PixelOffsetMode pom = pe.Graphics.PixelOffsetMode;
            SmoothingMode sm = pe.Graphics.SmoothingMode;

            pe.Graphics.InterpolationMode = this.InterpolationMode;
            pe.Graphics.PixelOffsetMode = this.PixelOffsetMode;
            pe.Graphics.SmoothingMode = this.SmoothingMode;

            DoPaint(pe);

            pe.Graphics.InterpolationMode = im;
            pe.Graphics.PixelOffsetMode = pom;
            pe.Graphics.SmoothingMode = sm;
        }

        protected virtual void DoPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
