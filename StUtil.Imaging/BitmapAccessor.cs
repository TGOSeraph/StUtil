using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

using StUtil.Imaging;

namespace StUtil.Imaging
{
    public class BitmapAccessor : IDisposable
    {
        public Bitmap Image { get; private set; }
        public BitmapData Data { get; private set; }

        private int pixelSize;

        public BitmapAccessor(Bitmap image)
        {
            this.Image = image;
        }

        public void Lock()
        {
            if (this.Data == null)
            {
                this.Data = this.Image.Lock();
                this.pixelSize = this.Image.GetPixelSize();
            }
            else
            {
                throw new InvalidOperationException("Image already locked");
            }
        }

        public void Unlock()
        {
            if (this.Data != null)
            {
                this.Image.UnlockBits(this.Data);
                this.Data = null;
            }
        }

        public unsafe Color GetPixel(int x, int y)
        {
            byte* dataPointer = (byte*)this.Data.Scan0;
            dataPointer = dataPointer + (y * this.Data.Stride) + (x * this.pixelSize);
            if (this.pixelSize == 3)
            {
                return Color.FromArgb(dataPointer[2], dataPointer[1], dataPointer[0]);
            }
            return Color.FromArgb(dataPointer[3], dataPointer[2], dataPointer[1], dataPointer[0]);
        }

        public unsafe void SetPixel(int x, int y, Color PixelColor)
        {
            byte* dataPointer = (byte*)this.Data.Scan0;
            dataPointer = dataPointer + (y * this.Data.Stride) + (x * this.pixelSize);
            if (this.pixelSize == 3)
            {
                dataPointer[2] = PixelColor.R;
                dataPointer[1] = PixelColor.G;
                dataPointer[0] = PixelColor.B;
            }
            else
            {
                dataPointer[3] = PixelColor.A;
                dataPointer[2] = PixelColor.R;
                dataPointer[1] = PixelColor.G;
                dataPointer[0] = PixelColor.B;
            }
        }

        ~BitmapAccessor()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            this.Unlock();
        }
    }
}
