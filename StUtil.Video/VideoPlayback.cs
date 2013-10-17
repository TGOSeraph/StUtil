using StUtil.Generic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using StUtil.Extensions;

namespace StUtil.Video
{
    public class VideoPlayback : IDisposable
    {
        public event EventHandler<EventArgs<VideoFrameEnumerator>> VideoFileLoaded;
        public event EventHandler<FrameLoadedEventArgs> FrameLoaded;
        public VideoFile Video { get; private set; }

        private Thread PlaybackThread;
        private VideoFrameEnumerator Enumerator;
        public bool Playing { get; private set; }
        public bool Paused { get; private set; }

        private object videoLock = new object();
        private AutoResetEvent pauseLock = new AutoResetEvent(true);

        private bool stop = false;

        public string FilePath { get; private set; }

        public bool DisposeLastAutoBitmapFrame { get; set; }
        private Bitmap lastBitmapFrame;

        public double Step
        {
            get;
            set;
        }

        public VideoPlayback(string filePath)
        {
            this.Step = 0.1;
            this.FilePath = filePath;
        }

        public void Seek(StUtil.Video.VideoFrameEnumerator.SeekType seek, double pos)
        {
            Enumerator.Seek(seek, pos);
        }

        public void Seek(StUtil.Video.VideoFrameEnumerator.SeekType seek, int pos)
        {
            Enumerator.Seek(seek, pos);
        }

        public void Start()
        {
            lock (videoLock)
            {
                if (Playing)
                {
                    return;
                }
                else if (Paused)
                {
                    pauseLock.Set();
                    Paused = false;
                    Playing = true;
                    return;
                }
                Playing = true;
            }
            stop = false;
            pauseLock.Set();
            PlaybackThread = new Thread(ThreadProc);
            PlaybackThread.Start();
        }

        public void Stop()
        {
            stop = true;
            if (Paused)
            {
                pauseLock.Set();
            }
        }

        public void Pause()
        {
            Paused = true;
        }

        protected void ThreadProc()
        {
            this.Video = new VideoFile(this.FilePath);
            Enumerator = new VideoFrameEnumerator(new FrameExtractor(this.Video))
            {
                Step = Step
            };

            VideoFileLoaded.RaiseEvent(this, Enumerator);

            while (!stop && Enumerator.CanRead)
            {
                if (Paused)
                {
                    Playing = false;
                    pauseLock.WaitOne();
                }
                VideoFrame frame = Enumerator.GetNextFrame();
                FrameLoadedEventArgs arg = new FrameLoadedEventArgs(frame);
                if (FrameLoaded != null)
                {
                    FrameLoaded(this, arg);
                }
                if (arg.RenderFrame == null)
                {
                    if (DisposeLastAutoBitmapFrame && lastBitmapFrame != null)
                    {
                        lastBitmapFrame.Dispose();
                    }
                    lastBitmapFrame = arg.Frame.GetRawImageCopy();
                    arg.RenderFrame = lastBitmapFrame;
                }
                if (arg.DisposeFrame)
                {
                    frame.Dispose();
                }
                RenderFrame(arg.RenderFrame);
                if (Paused)
                {
                    Playing = false;
                    pauseLock.WaitOne();
                }
            }
            this.Playing = false;
            this.PlaybackThread = null;
        }

        public virtual void RenderFrame(Bitmap frame)
        {
        }

        public void Dispose()
        {
            if (Playing || Paused)
            {
                Stop();
            }
        }
    }
}
