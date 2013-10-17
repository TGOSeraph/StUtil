using MediaInfoNET;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace StUtil.Video
{
    public class VideoInfo
    {
        public MediaFile Info { get; private set; }

        private int frameCount = -1;
        public int FrameCount
        {
            get
            {
                if (frameCount == -1)
                {
                    frameCount = Info.FrameCount;
                }
                return frameCount;
            }
        }

        private double frameRate = -1;
        public double FrameRate
        {
            get
            {
                if (frameRate == -1)
                {
                    frameRate = Info.Video.First().FrameRate;
                    if (frameRate == 0)
                    {
                        string fps = Info.Video.First().GetProperty("Nominal frame rate");
                        int pos = fps.IndexOf(" ");
                        if (pos > -1)
                        {
                            frameRate = double.Parse(fps.Substring(0, pos));
                        }
                        else
                        {
                            Debugger.Break();
                        }
                    }
                }
                return frameRate;
            }
        }

        private TimeSpan duration = TimeSpan.MinValue;
        public TimeSpan Duration
        {
            get
            {
                if (duration == TimeSpan.MinValue)
                {
                    duration = TimeSpan.FromMilliseconds(this.Info.General.DurationMillis);
                }
                return duration;
            }
        }

        private int width = -1;
        public int Width
        {
            get
            {
                if (width == -1)
                {
                    width = Info.Video.First().Width;
                }
                return width;
            }
        }

        private int height = -1;
        public int Height
        {
            get
            {
                if (height == -1)
                {
                    height = Info.Video.First().Height;
                }
                return height;
            }
        }

        public VideoInfo(string fileName)
        {
            this.Info = new MediaFile(fileName);
        }
    }
}
