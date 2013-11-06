using StUtil.Internal.Native;
using StUtil.Native;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace StUtil.UI.Forms.Utilities
{
    public static class AppBar
    {
        public enum Edge : int
        {
            Left = 0,
            Top,
            Right,
            Bottom,
            None
        }

        private static Dictionary<Form, NativeStructs.APPBARDATA> registrations = new Dictionary<Form, NativeStructs.APPBARDATA>();

        public static void SetStyles(Form form)
        {
            NativeUtils.SetWindowLongPtr(form.Handle, (int)NativeEnums.WindowLongFlags.GWL_STYLE, new IntPtr(0x16010000));
            NativeUtils.SetWindowLongPtr(form.Handle, (int)NativeEnums.WindowLongFlags.GWL_EXSTYLE, new IntPtr(0x88));
        }

        public static void RegisterBar(Form form, int size, Screen screen, Edge edge)
        {
            if (!registrations.ContainsKey(form))
            {
                NativeStructs.APPBARDATA abd = new NativeStructs.APPBARDATA();
                abd.cbSize = Marshal.SizeOf(abd);
                abd.hWnd = form.Handle;

                int uCallBack = NativeMethods.RegisterWindowMessage("AppBarMessage");
                abd.uCallbackMessage = uCallBack;
                abd.uEdge = (int)edge;

                WndProcHandler handler = new WndProcHandler(form, new WndProcHandler.MessageHandler(uCallBack, new WndProcHandler.MessageHandlerProc(delegate(WndProcHandler h, ref Message m)
                {
                    switch (m.WParam.ToInt32())
                    {
                        case (int)NativeEnums.ABNotify.ABN_POSCHANGED:
                            ABSetPos(form, screen, size);
                            break;
                    }
                    return false;
                })));

                uint ret = NativeMethods.SHAppBarMessage((int)NativeEnums.ABMsg.ABM_NEW, ref abd);
                registrations.Add(form, abd);
                SetStyles(form);

                ABSetPos(form, screen, size);
            }
        }

        public static void UnregisterBar(Form form)
        {
            StUtil.Internal.Native.NativeStructs.APPBARDATA abd = registrations[form];
            NativeMethods.SHAppBarMessage((int)NativeEnums.ABMsg.ABM_REMOVE, ref abd);
            registrations.Remove(form);
        }

        private static void ABSetPos(Form form, Screen screen, int size)
        {
            NativeStructs.APPBARDATA abd = registrations[form];

            if (abd.uEdge == (int)Edge.Left || abd.uEdge == (int)Edge.Right)
            {
                abd.rc.top = screen.WorkingArea.Top;
                abd.rc.bottom = screen.WorkingArea.Bottom;
                if (abd.uEdge == (int)Edge.Left)
                {
                    abd.rc.left = screen.WorkingArea.Left;
                    abd.rc.right = abd.rc.left + size;
                }
                else
                {
                    abd.rc.right = screen.WorkingArea.Right;
                    abd.rc.left = abd.rc.right - size;
                }
            }
            else
            {
                abd.rc.left = screen.WorkingArea.Left;
                abd.rc.right = screen.WorkingArea.Right;
                if (abd.uEdge == (int)Edge.Top)
                {
                    abd.rc.top = screen.WorkingArea.Top;
                    abd.rc.bottom = abd.rc.top + size;
                }
                else
                {
                    abd.rc.bottom = screen.WorkingArea.Bottom;
                    abd.rc.top = abd.rc.bottom - size;
                }
            }

            // Query the system for an approved size and position. 
            NativeMethods.SHAppBarMessage((int)NativeEnums.ABMsg.ABM_QUERYPOS, ref abd);

            // Adjust the rectangle, depending on the edge to which the 
            // appbar is anchored. 
            switch (abd.uEdge)
            {
                case (int)Edge.Left:
                    abd.rc.right = abd.rc.left + size;
                    break;
                case (int)Edge.Right:
                    abd.rc.left = abd.rc.right - size;
                    break;
                case (int)Edge.Top:
                    abd.rc.bottom = abd.rc.top + size;
                    break;
                case (int)Edge.Bottom:
                    abd.rc.top = abd.rc.bottom - size;
                    break;
            }

            // Pass the final bounding rectangle to the system. 
            NativeMethods.SHAppBarMessage((int)NativeEnums.ABMsg.ABM_SETPOS, ref abd);

            // Move and size the appbar so that it conforms to the 
            // bounding rectangle passed to the system. 
            NativeMethods.MoveWindow(abd.hWnd, abd.rc.left, abd.rc.top,
                abd.rc.right - abd.rc.left, abd.rc.bottom - abd.rc.top, true);

            form.Height = abd.rc.bottom - abd.rc.top;
            form.Width = abd.rc.right - abd.rc.left;

            registrations[form] = abd;
        }
    }
}