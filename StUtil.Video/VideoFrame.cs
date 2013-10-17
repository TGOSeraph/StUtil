using DirectShowLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Video
{
   	public class VideoFrame : System.IDisposable
	{
		public static readonly int BmiHeaderSize;

        public VideoFile Video { get; private set; }

		private Bitmap rawImage;
        public Bitmap RawImage
        {
            get
            {
                if (rawImage == null)
                {
                    this.rawImage = new Bitmap(this.Video.Info.Width, this.Video.Info.Height, this.Video.Info.Width * 3, PixelFormat.Format24bppRgb, this.Scan0);
                    this.rawImage.RotateFlip(RotateFlipType.Rotate180FlipX);
                }
                return rawImage;
            }
        }
        public System.IntPtr Scan0
        {
            get;
            private set;
        }
		public System.IntPtr BmpPtr
		{
			get;
			private set;
		}
		public int Stride
		{
			get;
			private set;
		}
		static VideoFrame()
		{
			VideoFrame.BmiHeaderSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(BitmapInfoHeader));
		}
		public VideoFrame(VideoFile video, System.IntPtr bmpPtr)
		{
            this.Video = video;
			this.BmpPtr = bmpPtr;
            this.Stride = this.Video.Info.Width * 3;
			this.Scan0 = bmpPtr + VideoFrame.BmiHeaderSize;
		}
		public Bitmap GetRawImageCopy()
		{
            return new Bitmap(this.RawImage);
		}
		public void Dispose()
		{
			if (this.rawImage != null)
			{
				this.rawImage.Dispose();
				this.rawImage = null;
			}
			if (this.BmpPtr != System.IntPtr.Zero)
			{
				System.Runtime.InteropServices.Marshal.FreeHGlobal(this.BmpPtr);
				this.BmpPtr = System.IntPtr.Zero;
			}
		}
	}
}

