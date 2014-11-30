using StUtil.Extensions;
using StUtil.Native.Hook;
using StUtil.Native.Input.Hook;
using StUtil.Native.Internal;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.Native.Input
{
    public static class Mouse
    {
        //public enum Button : ushort
        //{
        //    Left = Keys.LButton,
        //    Right = Keys.RButton,
        //    Middle = Keys.MButton,
        //    X1 = Keys.XButton1,
        //    X2 = Keys.XButton2
        //}

        /// <summary>
        /// Occurs on mouse click.
        /// </summary>
        public static event EventHandler<MouseEventArgs> MouseClick;

        /// <summary>
        /// Occurs on mouse double click.
        /// </summary>
        public static event EventHandler<MouseEventArgs> MouseDoubleClick;

        /// <summary>
        /// Occurs on mouse down.
        /// </summary>
        public static event EventHandler<MouseEventArgs> MouseDown;

        /// <summary>
        /// Occurs when the mouse moves.
        /// </summary>
        public static event EventHandler<MouseEventArgs> MouseMove;

        /// <summary>
        /// Occurs on mouse up.
        /// </summary>
        public static event EventHandler<MouseEventArgs> MouseUp;

        /// <summary>
        /// Occurs when the mouse wheel is scrolled.
        /// </summary>
        public static event EventHandler<MouseEventArgs> MouseWheel;

        private static bool enableRasingEvents = false;

        private static MouseHook hook = new MouseHook(new GlobalHook());

        /// <summary>
        /// Gets or sets a value indicating whether events are enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if events should be raised; otherwise, <c>false</c>.
        /// </value>
        public static bool EnableRasingEvents
        {
            get
            {
                return enableRasingEvents;
            }
            set
            {
                if (enableRasingEvents != value)
                {
                    if (value)
                    {
                        hook.SetHook();
                    }
                    else
                    {
                        hook.RemoveHook();
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the location of the mouse cursor.
        /// </summary>
        /// <value>
        /// The location of the mouse cursor.
        /// </value>
        public static Point Location
        {
            get
            {
                return Cursor.Position;
            }
            set
            {
                Cursor.Position = value;
            }
        }

        static Mouse()
        {
            hook.MouseClick += OnMouseClick;
            hook.MouseDoubleClick += OnMouseDoubleClick;
            hook.MouseDown += OnMouseDown;
            hook.MouseMove += OnMouseMove;
            hook.MouseUp += OnMouseUp;
            hook.MouseWheel += OnMouseWheel;
        }

        /// <summary>
        /// Checks if a mouse button is down
        /// </summary>
        /// <param name="button">The button to check the state of</param>
        /// <returns>If the button is down</returns>
        public static bool IsButtonDown(MouseButtons button)
        {
            short state = Internal.NativeMethods.GetAsyncKeyState((ushort)MouseButtonToKey(button));
            return 0 != (state & 0x8000);
        }

        private static Keys MouseButtonToKey(MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.Left:
                    return Keys.LButton;

                case MouseButtons.Middle:
                    return Keys.MButton;

                case MouseButtons.Right:
                    return Keys.RButton;

                case MouseButtons.XButton1:
                    return Keys.XButton1;

                case MouseButtons.XButton2:
                    return Keys.XButton2;

                default:
                    throw new NotImplementedException();
            }
        }

        private static NativeEnums.WM MouseButtonToMessage(MouseButtons button, bool up)
        {
            switch (button)
            {
                case MouseButtons.Left:
                    return up ? NativeEnums.WM.LBUTTONUP : NativeEnums.WM.LBUTTONDOWN;

                case MouseButtons.Middle:
                    return up ? NativeEnums.WM.MBUTTONUP : NativeEnums.WM.MBUTTONDOWN;

                case MouseButtons.Right:
                    return up ? NativeEnums.WM.RBUTTONUP : NativeEnums.WM.RBUTTONDOWN;

                default:
                    throw new NotImplementedException();
            }
        }

        private static NativeEnums.MouseEventFlags MouseButtonToMouseEventFlags(MouseButtons button, bool up)
        {
            switch (button)
            {
                case MouseButtons.Left:
                    return up ? NativeEnums.MouseEventFlags.LeftUp : NativeEnums.MouseEventFlags.LeftDown;

                case MouseButtons.Right:
                    return up ? NativeEnums.MouseEventFlags.RightUp : NativeEnums.MouseEventFlags.RightDown;

                case MouseButtons.Middle:
                    return up ? NativeEnums.MouseEventFlags.MiddleUp : NativeEnums.MouseEventFlags.MiddleDown;

                default:
                    throw new NotImplementedException();
            }
        }

        #region Mouse Actions

        /// <summary>
        /// Clicks the specified button at the specified location.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <param name="location">The location.</param>
        /// <param name="sleep">The amount of time to hold the button down for, blocking.</param>
        public static void Click(MouseButtons button, Point location, int sleep)
        {
            Down(button, location);
            Thread.Sleep(sleep);
            Up(button, location);
        }

        /// <summary>
        /// Clicks the specified button at the current location of the mouse cursor.
        /// </summary>
        /// <param name="button">The button.</param>
        public static void Click(MouseButtons button)
        {
            Click(button, Location, 0);
        }

        /// <summary>
        /// Clicks the mouse left button specified location.
        /// </summary>
        /// <param name="location">The location.</param>
        public static void Click(Point location)
        {
            Click(MouseButtons.Left, location, 0);
        }

        /// <summary>
        /// Clicks the mouse left button current location.
        /// </summary>
        public static void Click()
        {
            Click(MouseButtons.Left, Mouse.Location, 0);
        }

        /// <summary>
        /// Clicks the specified button at the specified location.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <param name="location">The location.</param>
        /// <param name="sleep">The amount of time to hold the button down for, blocking.</param>
        public async static Task ClickAsync(MouseButtons button, Point location, int sleep)
        {
            Down(button, location);
            Task t = Task.Delay(sleep);
            await t.ContinueWith(task => Up(button, location));
        }

        /// <summary>
        /// Clicks the specified button at the current location of the mouse cursor.
        /// </summary>
        /// <param name="button">The button.</param>
        public async static Task ClickAsync(MouseButtons button)
        {
            await ClickAsync(button, Location, 0);
        }

        /// <summary>
        /// Clicks the mouse left button specified location.
        /// </summary>
        /// <param name="location">The location.</param>
        public async static Task ClickAsync(Point location)
        {
            await ClickAsync(MouseButtons.Left, location, 0);
        }

        /// <summary>
        /// Presses the specified button down at the location specified.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <param name="location">The location.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public static void Down(MouseButtons button, Point location)
        {
            NativeMethods.mouse_event((uint)(MouseButtonToMouseEventFlags(button, false) | NativeEnums.MouseEventFlags.Absolute), (uint)location.X, (uint)location.Y, 0, IntPtr.Zero);
        }

        /// <summary>
        /// Presses the specified button down at the current location of the mouse cursor.
        /// </summary>
        /// <param name="button">The button.</param>
        public static void Down(MouseButtons button)
        {
            Down(button, Location);
        }

        /// <summary>
        /// Presses the left mouse button down at the specified location.
        /// </summary>
        /// <param name="location">The location.</param>
        public static void Down(Point location)
        {
            Down(MouseButtons.Left, location);
        }

        /// <summary>
        /// Presses the left mouse button down at the current location of the mouse cursor.
        /// </summary>
        public static void Down()
        {
            Down(MouseButtons.Left, Mouse.Location);
        }

        /// <summary>
        /// Releases the specified button at a specific location.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <param name="location">The location to release the button at.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public static void Up(MouseButtons button, Point location)
        {
            NativeMethods.mouse_event((uint)(MouseButtonToMouseEventFlags(button, true) | NativeEnums.MouseEventFlags.Absolute), (uint)location.X, (uint)location.Y, 0, IntPtr.Zero);
        }

        /// <summary>
        /// Releases the specified button at the current location of the mouse cursor.
        /// </summary>
        /// <param name="button">The button.</param>
        public static void Up(MouseButtons button)
        {
            Up(button, Location);
        }

        /// <summary>
        /// Releases the mouse left button at the specified location.
        /// </summary>
        /// <param name="location">The location.</param>
        public static void Up(Point location)
        {
            Up(MouseButtons.Left, location);
        }

        /// <summary>
        /// Releases the mouse left button at the current location of the mouse cursor.
        /// </summary>
        public static void Up()
        {
            Up(MouseButtons.Left, Mouse.Location);
        }

        #endregion Mouse Actions

        #region Handle Specific Mouse Actions

        /// <summary>
        /// Clicks the specified button at the specified location.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <param name="location">The location.</param>
        /// <param name="sleep">The amount of time to hold the button down for, blocking.</param>
        public static void Click(IntPtr hWnd, MouseButtons button, Point location, int sleep)
        {
            Down(hWnd, button, location);
            Thread.Sleep(sleep);
            Up(hWnd, button, location);
        }

        /// <summary>
        /// Clicks the specified button at the current location of the mouse cursor.
        /// </summary>
        /// <param name="button">The button.</param>
        public static void Click(IntPtr hWnd, MouseButtons button)
        {
            Click(hWnd, button, Location, 0);
        }

        /// <summary>
        /// Clicks the mouse left button specified location.
        /// </summary>
        /// <param name="location">The location.</param>
        public static void Click(IntPtr hWnd, Point location)
        {
            Click(hWnd, MouseButtons.Left, location, 0);
        }

        /// <summary>
        /// Clicks the mouse left button current location.
        /// </summary>
        public static void Click(IntPtr hWnd)
        {
            Click(hWnd, MouseButtons.Left, Location, 0);
        }

        /// <summary>
        /// Clicks the specified button at the specified location.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <param name="location">The location.</param>
        /// <param name="sleep">The amount of time to hold the button down for, blocking.</param>
        public async static Task ClickAsync(IntPtr hWnd, MouseButtons button, Point location, int sleep)
        {
            Down(hWnd, button, location);
            Task t = Task.Delay(sleep);
            await t.ContinueWith(task => Up(hWnd, button, location));
        }

        /// <summary>
        /// Clicks the specified button at the current location of the mouse cursor.
        /// </summary>
        /// <param name="button">The button.</param>
        public async static Task ClickAsync(IntPtr hWnd, MouseButtons button)
        {
            await ClickAsync(hWnd, button, Location, 0);
        }

        /// <summary>
        /// Clicks the mouse left button specified location.
        /// </summary>
        /// <param name="location">The location.</param>
        public async static Task ClickAsync(IntPtr hWnd, Point location)
        {
            await ClickAsync(hWnd, MouseButtons.Left, location, 0);
        }

        /// <summary>
        /// Clicks the mouse left button current location.
        /// </summary>
        public async static Task ClickAsync(IntPtr hWnd)
        {
            await ClickAsync(hWnd, MouseButtons.Left, Location, 0);
        }

        /// <summary>
        /// Presses the specified button down at the location specified.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <param name="location">The location.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public static void Down(IntPtr hWnd, MouseButtons button, Point location)
        {
            NativeUtilities.DispatchMessage(hWnd, (int)MouseButtonToMessage(button, false), IntPtr.Zero, new IntPtr(NativeUtilities.MakeLParam(location.X, location.Y)), false);
        }

        /// <summary>
        /// Presses the specified button down at the current location of the mouse cursor.
        /// </summary>
        /// <param name="button">The button.</param>
        public static void Down(IntPtr hWnd, MouseButtons button)
        {
            Down(hWnd, button, Location);
        }

        /// <summary>
        /// Presses the left mouse button down at the specified location.
        /// </summary>
        /// <param name="location">The location.</param>
        public static void Down(IntPtr hWnd, Point location)
        {
            Down(hWnd, MouseButtons.Left, location);
        }

        /// <summary>
        /// Presses the left mouse button down at the current location of the mouse cursor.
        /// </summary>
        public static void Down(IntPtr hWnd)
        {
            Down(hWnd, MouseButtons.Left, Mouse.Location);
        }

        /// <summary>
        /// Releases the specified button at a specific location.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <param name="location">The location to release the button at.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public static void Up(IntPtr hWnd, MouseButtons button, Point location)
        {
            NativeUtilities.DispatchMessage(hWnd, (int)MouseButtonToMessage(button, true), IntPtr.Zero, new IntPtr(NativeUtilities.MakeLParam(location.X, location.Y)), false);
        }

        /// <summary>
        /// Releases the specified button at the current location of the mouse cursor.
        /// </summary>
        /// <param name="button">The button.</param>
        public static void Up(IntPtr hWnd, MouseButtons button)
        {
            Up(hWnd, button, Location);
        }

        /// <summary>
        /// Releases the mouse left button at the specified location.
        /// </summary>
        /// <param name="location">The location.</param>
        public static void Up(IntPtr hWnd, Point location)
        {
            Up(hWnd, MouseButtons.Left, location);
        }

        /// <summary>
        /// Releases the mouse left button at the current location of the mouse cursor.
        /// </summary>
        public static void Up(IntPtr hWnd)
        {
            Up(hWnd, MouseButtons.Left, Mouse.Location);
        }

        #endregion Handle Specific Mouse Actions

        #region Mouse Events

        private static void OnMouseClick(object sender, MouseEventArgs e)
        {
            MouseClick.RaiseEvent(sender, e);
        }

        private static void OnMouseDoubleClick(object sender, MouseEventArgs e)
        {
            MouseDoubleClick.RaiseEvent(sender, e);
        }

        private static void OnMouseDown(object sender, MouseEventArgs e)
        {
            MouseDown.RaiseEvent(sender, e);
        }

        private static void OnMouseMove(object sender, MouseEventArgs e)
        {
            MouseMove.RaiseEvent(sender, e);
        }

        private static void OnMouseUp(object sender, MouseEventArgs e)
        {
            MouseUp.RaiseEvent(sender, e);
        }

        private static void OnMouseWheel(object sender, MouseEventArgs e)
        {
            MouseWheel.RaiseEvent(sender, e);
        }

        #endregion Mouse Events
    }
}