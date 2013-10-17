using DirectShowLib;
using DirectShowLib.DES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StUtil.Video
{
    public class FrameExtractor
    {
        private IMediaDet mediaDet;
        private AMMediaType mediaType;
        private VideoInfoHeader videoInfo;

        public VideoFile Video { get; private set; }

        public FrameExtractor(VideoFile video)
        {
            this.Video = video;

            this.mediaDet = (IMediaDet)new MediaDet();
            DsError.ThrowExceptionForHR(this.mediaDet.put_Filename(video.FileName));
            int num = 0;
            System.Guid empty = System.Guid.Empty;
            while (empty != MediaType.Video)
            {
                this.mediaDet.put_CurrentStream(num++);
                this.mediaDet.get_StreamType(out empty);
            }
            this.mediaType = new AMMediaType();
            this.mediaDet.get_StreamMediaType(this.mediaType);
            this.videoInfo = (VideoInfoHeader)System.Runtime.InteropServices.Marshal.PtrToStructure(this.mediaType.formatPtr, typeof(VideoInfoHeader));
            System.Runtime.InteropServices.Marshal.FreeCoTaskMem(this.mediaType.formatPtr);
        }

        public VideoFrame GetFrame(int frame)
        {
            return this.GetFrame(this.Video.ConvertFrameNumberToSeconds(frame));
        }
        public VideoFrame GetFrame(double position)
        {
            if (position >= 0.0 && position <= this.Video.Info.Duration.TotalSeconds)
            {
                if (this.mediaDet != null)
                {
                    System.IntPtr intPtr = System.IntPtr.Zero;
                    try
                    {
                        intPtr = System.Runtime.InteropServices.Marshal.AllocHGlobal(this.Video.FrameBufferSize);
                        int frameBufferSize = this.Video.FrameBufferSize;
                        this.mediaDet.GetBitmapBits(position, out frameBufferSize, intPtr, this.Video.Info.Width, this.Video.Info.Height);
                        return new VideoFrame(this.Video, intPtr);
                    }
                    catch
                    {
                        if (intPtr != System.IntPtr.Zero)
                        {
                            System.Runtime.InteropServices.Marshal.FreeHGlobal(intPtr);
                        }
                        throw;
                    }
                }
                throw new System.NullReferenceException("MediaDet was null");
            }
            throw new System.ArgumentException(string.Format("Seconds must be between 0 and {0} inclusive", this.Video.Info.Duration.TotalSeconds));
        }
    }
}
