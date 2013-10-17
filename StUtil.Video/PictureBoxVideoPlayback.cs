using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StUtil.Video
{
    public class PictureBoxVideoPlayback : VideoPlayback
    {
        public PictureBox Target { get; private set; }

        public PictureBoxVideoPlayback(string fileName, PictureBox target)
            :base(fileName)
        {
            this.Target = target;
            this.DisposeLastAutoBitmapFrame = true;
        }

        public override void RenderFrame(System.Drawing.Bitmap frame)
        {
            this.Target.Image = frame;
            base.RenderFrame(frame);
        }
    }
}
