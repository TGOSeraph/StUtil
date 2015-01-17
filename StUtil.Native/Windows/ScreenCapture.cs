using StUtil.Extensions;
using StUtil.Native.Internal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.Native.Windows
{
    public static class ScreenCapture
    {
        /// <summary>
        /// Draws the control to a bitmap.
        /// </summary>
        /// <param name="control">The control to draw.</param>
        /// <returns>An image with the control drawn onto it.</returns>
        /// <remarks>Internally uses WM_PRINT to draw the control to an HDC</remarks>
        public static Bitmap Draw(Control control)
        {
            Bitmap dest = new Bitmap(control.Width, control.Height);
            control.DrawToBitmap(dest, new Rectangle(0, 0, control.Width, control.Height));
            return dest;
        }

        /// <summary>
        /// Draws the form to a bitmap.
        /// </summary>
        /// <param name="form">The form to draw</param>
        /// <returns>An image with the form drawn onto it.</returns>
        /// <remarks>Internally uses WM_PRINT to draw the form to an HDC.</remarks>
        public static Bitmap Draw(Form form)
        {
            return Draw(form, true);
        }

        /// <summary>
        /// Draws the form to a bitmap.
        /// </summary>
        /// <param name="form">The form to draw.</param>
        /// <param name="clientArea">If only the client area (non-chrome area) should be drawn.</param>
        /// <returns>An image with the form drawn onto it.</returns>
        /// <remarks>Internally uses WM_PRINT to draw the form to an HDC.</remarks>
        public static Bitmap Draw(Form form, bool clientArea)
        {
            Bitmap bmp = new Bitmap(form.Width, form.Height);
            form.DrawToBitmap(bmp, new Rectangle(0, 0, form.Width, form.Height));

            if (clientArea)
            {
                Bitmap crop = new Bitmap(form.ClientRectangle.Width, form.ClientRectangle.Height);
                using (Graphics g = Graphics.FromImage(crop))
                {
                    Rectangle screenRectangle = form.RectangleToScreen(form.ClientRectangle);
                    int titleHeight = screenRectangle.Top - form.Top;
                    g.DrawImage(bmp, new Point((form.ClientRectangle.Width - form.Width) / 2, -titleHeight));
                }
                bmp.Dispose();
                bmp = crop;
            }
            return bmp;
        }

        /// <summary>
        /// Captures the specified form from the screen including any overlapped windows.
        /// </summary>
        /// <param name="form">The form to capture</param>
        /// <returns>An image with the form captured on it.</returns>
        public static Bitmap Capture(Form form)
        {
            return Capture(form, true);
        }

        /// <summary>
        /// Captures the specified form from the screen including any overlapped windows.
        /// </summary>
        /// <param name="form">The form to capture</param>
        /// <param name="clientArea">If only the client area (non-chrome area) should be captured.</param>
        /// <returns>An image with the form captured on it.</returns>
        public static Bitmap Capture(Form form, bool clientArea)
        {
            return Capture(clientArea ? form.RectangleToScreen(form.ClientRectangle) : form.Bounds);
        }

        /// <summary>
        /// Captures the specified rectangle from the screen
        /// </summary>
        /// <param name="rect">The rectangle bounds to capture</param>
        /// <returns>An image with the captured bounds drawn on it.</returns>
        public static Bitmap Capture(Rectangle rect)
        {
            Bitmap result = new Bitmap(rect.Width, rect.Height);

            using (var g = Graphics.FromImage(result))
            {
                g.CopyFromScreen(new Point(rect.Left, rect.Top), Point.Empty, rect.Size);
            }

            return result;
        }

        /// <summary>
        /// Captures the foreground window from the screen including any overlapped windows.
        /// </summary>
        /// <returns>An image with the foreground window drawn on it.</returns>
        public static Bitmap Capture()
        {
            return Capture(Native.Internal.NativeMethods.GetForegroundWindow());
        }

        /// <summary>
        /// Captures the specified window from the screen including any overlapped windows.
        /// </summary>
        /// <param name="hWnd">The handle of the window to capture</param>
        /// <returns>An image with the window drawn on it</returns>
        public static Bitmap Capture(IntPtr hWnd)
        {
            return Capture(hWnd, true);
        }

        /// <summary>
        /// Captures the specified window from the screen including any overlapped windows.
        /// </summary>
        /// <param name="hWnd">The handle of the window to capture</param>
        /// <param name="clientArea">If only the client area (non-chrome area) should be captured.</param>
        /// <returns>An image with the window drawn on it</returns>
        public static Bitmap Capture(IntPtr hWnd, bool clientArea)
        {
            Native.Internal.NativeStructs.RECT r = new Native.Internal.NativeStructs.RECT();
            if (clientArea)
            {
                Native.Internal.NativeMethods.GetClientRect(hWnd, ref r);
                Point pt = Point.Empty.ClientToScreen(hWnd, false);
                r = new Rectangle(pt, ((Rectangle)r).Size);
            }
            else
            {
                Native.Internal.NativeMethods.GetWindowRect(hWnd, ref r);
            }

            return Capture(r);
        }


        /// <summary>
        /// Requests the specified window to draw itself to a Bitmap
        /// </summary>
        /// <param name="handle">The handle of the window to draw</param>
        /// <remarks>This method only works on windows that handle WM_PRINT. It allows for capturing non-visible and minimized applications.</remarks>
        /// <returns>An image with the window drawn onto it.</returns>
        public static Bitmap Print(IntPtr hWnd)
        {
            return Print(hWnd, false);
        }

        /// <summary>
        /// Requests the specified window to draw itself to a Bitmap
        /// </summary>
        /// <param name="handle">The handle of the window to draw</param>
        /// <param name="clientArea">If the client area should be drawn or the whole window.</param>
        /// <remarks>This method only works on windows that handle WM_PRINT. It allows for capturing non-visible and minimized applications.</remarks>
        /// <returns>An image with the window drawn onto it.</returns>
        public static Bitmap Print(IntPtr hWnd, bool clientArea)
        {
            NativeStructs.RECT r = new NativeStructs.RECT();
            NativeMethods.GetWindowRect(hWnd, ref r);
            Rectangle rect = r;
            Bitmap bmp = new Bitmap(rect.Width, rect.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                IntPtr hdc = g.GetHdc();
                NativeMethods.PrintWindow(hWnd, hdc, 0);
                g.ReleaseHdc(hdc);
            }

            if (clientArea)
            {
                NativeMethods.GetClientRect(hWnd, ref r);
                Point pt = Point.Empty.ClientToScreen(hWnd);
                Rectangle client = new Rectangle(pt, ((Rectangle)r).Size);

                Bitmap crop = new Bitmap(client.Width, client.Height);
                using (Graphics g = Graphics.FromImage(crop))
                {
                    int titleHeight = client.Top - rect.Top;
                    g.DrawImage(bmp, new Point((client.Width - rect.Width) / 2, -titleHeight));
                }
                bmp.Dispose();
                bmp = crop;
            }

            return bmp;
        }
    }
}
