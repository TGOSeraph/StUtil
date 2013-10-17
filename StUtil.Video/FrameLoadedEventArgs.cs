using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace StUtil.Video
{
    public class FrameLoadedEventArgs : EventArgs
    {
        public VideoFrame Frame { get; private set; }
        public bool DisposeFrame { get; set; }
        public Bitmap RenderFrame { get; set; }

        public FrameLoadedEventArgs(VideoFrame frame)
        {
            this.Frame = frame;
            this.DisposeFrame = true;
        }
    }
}
