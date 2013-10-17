using DirectShowLib;
using DirectShowLib.DES;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaInfoNET;

namespace StUtil.Video
{
   	public class VideoFile
	{
		public string FileName
		{
			get;
			private set;
		}

        public VideoInfo Info { get; private set; }

        /// <summary>
        /// Stored size for a frame buffer
        /// </summary>
        private int frameBufferSize = -1;
        /// <summary>
        /// Retunrs the size in bytes of our frame buffer for this video
        /// </summary>
        public int FrameBufferSize
        {
            get
            {
                if (this.frameBufferSize == -1)
                {
                    //mediaDet.GetBitmapBits(position, out this.frameBufferSize, IntPtr.Zero, this.frameWidth, this.frameHeight);

                    //Calculate the size as the number of pixels * rgb, plus the header size
                    this.frameBufferSize = ((this.Info.Width * this.Info.Height) * 3) + VideoFrame.BmiHeaderSize;
                }
                return this.frameBufferSize;
            }
        }

		public VideoFile(string fileName)
		{
            this.Info = new VideoInfo(fileName);
			this.FileName = fileName;
		}

		public double ConvertFrameNumberToSeconds(int frameNumber)
		{
            return (double)frameNumber / this.Info.FrameRate;
		}
		public int ConvertSecondsToFrameNumber(double seconds)
		{
            return (int)System.Math.Round(seconds * this.Info.FrameRate);
		}
	}
}

