using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StUtil.Video
{
    public class VideoFrameEnumerator
    {
        public FrameExtractor Extractor { get; private set; }

        public VideoFrameEnumerator(FrameExtractor extractor)
        {
            this.Extractor = extractor;
            this.Position = 0.0;
            this.Step = 0.1;
        }

        public enum SeekType
        {
            Start,
            End,
            Frame,
            Second,
            RelativeFrame,
            RelativeSecond
        }
        public double Position
        {
            get;
            private set;
        }
        public double Step
        {
            get;
            set;
        }

        public bool CanRead
        {
            get
            {
                return this.Position + this.Step < this.Extractor.Video.Info.Duration.TotalSeconds;
            }
        }

        public VideoFrame GetNextFrame()
        {
            VideoFrame frame = this.Extractor.GetFrame(this.Position);
            this.Position += this.Step;
            return frame;
        }
        public void Seek(SeekType seek, double pos)
        {
            switch (seek)
            {
                case SeekType.Start:
                    this.Position = 0.0;
                    return;
                case SeekType.End:
                    this.Position = this.Extractor.Video.Info.Duration.TotalSeconds;
                    return;
                case SeekType.Frame:
                    this.Position = this.Extractor.Video.ConvertFrameNumberToSeconds((int)pos);
                    return;
                case SeekType.Second:
                    this.Position = pos;
                    return;
                case SeekType.RelativeFrame:
                    this.Position = this.Extractor.Video.ConvertFrameNumberToSeconds((int)(this.Position + pos));
                    return;
                case SeekType.RelativeSecond:
                    this.Position += pos;
                    return;
                default:
                    return;
            }
        }
        public void Seek(SeekType seek, int pos)
        {
            this.Seek(seek, (double)pos);
        }
    }
}
