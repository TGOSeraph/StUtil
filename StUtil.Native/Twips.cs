using StUtil.Native.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Native
{
    public static class Twips
    {
        public enum MeasureDirection
        {
            Horizontal,
            Vertical
        }
        public static int ToPixels(int twips, MeasureDirection direction)
        {
            return (int)(((double)twips) * ((double)GetPixelsPerInch(direction)) / 1440.0);
        }

        public static int FromPixels(int pixels, MeasureDirection direction)
        {
            return (int)((((double)pixels) * 1440.0) / ((double)GetPixelsPerInch(direction)));
        }

        public static int GetPixelsPerInch(MeasureDirection direction)
        {
            const int LOGPIXELSX = 88;
            const int LOGPIXELSY = 90;

            int ppi;
            IntPtr dc = NativeMethods.GetDC(IntPtr.Zero);

            if (direction == MeasureDirection.Horizontal)
            {
                ppi = NativeMethods.GetDeviceCaps(dc, LOGPIXELSX);
            }
            else
            {
                ppi = NativeMethods.GetDeviceCaps(dc, LOGPIXELSY);
            }

            NativeMethods.ReleaseDC(IntPtr.Zero, dc);
            return ppi;
        }
    }
}
