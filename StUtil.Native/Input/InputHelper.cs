using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StUtil.Native.Input
{
    public class InputHelper : IMouseInputProvider, IKeyboardInputProvider
    {
        private MouseInputMethod mouseInputMethod;
        public IMouseInputProvider MouseInputProvider { get; private set; }

        private KeyboardInputMethod keyboardInputMethod;
        public IKeyboardInputProvider KeyboardInputProvider { get; private set; }

        public IntPtr Handle { get; set; }

        public MouseInputMethod MouseInputMethod
        {
            get { return mouseInputMethod; }
            set
            {
                switch (value)
                {
                    case MouseInputMethod.Event:
                        MouseInputProvider = new MouseEventInputProvider();
                        break;
                    case MouseInputMethod.SendInput:
                        MouseInputProvider = new MouseSendInputProvider();
                        break;
                    case MouseInputMethod.Message:
                        MouseInputProvider = new MouseMessageInputProvider();
                        break;
                    default:
                        throw new NotImplementedException(value.ToString());
                }
                MouseInputProvider.Handle = Handle;
                mouseInputMethod = value;
            }
        }

        public KeyboardInputMethod KeyboardInputMethod
        {
            get { return keyboardInputMethod; }
            set
            {
                switch (value)
                {
                    case KeyboardInputMethod.Event:
                        KeyboardInputProvider = new KeyboardEventInputProvider();
                        break;
                    case KeyboardInputMethod.Message:
                        KeyboardInputProvider = new KeyboardMessageInputProvider();
                        break;
                    default:
                        throw new NotImplementedException(value.ToString());
                }
                KeyboardInputProvider.Handle = Handle;
                keyboardInputMethod = value;
            }
        }

        public IEnumerable<System.Windows.Forms.Keys> KeysDown
        {
            get { throw new NotImplementedException(); }
        }

        public System.Windows.Forms.MouseButtons ButtonsDown
        {
            get { return MouseInputProvider.ButtonsDown; }
        }

        public InputHelper()
        {
            MouseInputMethod = Input.MouseInputMethod.Event;
            KeyboardInputMethod = Input.KeyboardInputMethod.Event;
        }

        public InputHelper(IntPtr handle)
            : this()
        {
            this.Handle = handle;
        }

        public void Click(System.Windows.Forms.MouseButtons button)
        {
            MouseInputProvider.Click(button);
        }

        public void Click(System.Windows.Forms.MouseButtons button, System.Drawing.Point location)
        {
            MouseInputProvider.Click(button, location);
        }

        public void Down(System.Windows.Forms.MouseButtons button)
        {
            MouseInputProvider.Down(button);
        }

        public void Down(System.Windows.Forms.MouseButtons button, System.Drawing.Point location)
        {
            MouseInputProvider.Down(button, location);
        }

        public void LeftClick()
        {
            MouseInputProvider.LeftClick();
        }

        public void LeftClick(System.Drawing.Point location)
        {
            MouseInputProvider.LeftClick(location);
        }

        public void LeftDown()
        {
            MouseInputProvider.LeftDown();
        }

        public void LeftDown(System.Drawing.Point location)
        {
            MouseInputProvider.LeftDown(location);
        }

        public void LeftUp()
        {
            MouseInputProvider.LeftUp();
        }

        public void LeftUp(System.Drawing.Point location)
        {
            MouseInputProvider.LeftUp(location);
        }

        public void Move(System.Drawing.Point location)
        {
            MouseInputProvider.Move(location);
        }

        public void RightClick()
        {
            MouseInputProvider.RightClick();
        }

        public void RightClick(System.Drawing.Point location)
        {
            MouseInputProvider.RightClick(location);
        }

        public void RightDown()
        {
            MouseInputProvider.RightDown();
        }

        public void RightDown(System.Drawing.Point location)
        {
            MouseInputProvider.RightDown(location);
        }

        public void RightUp()
        {
            MouseInputProvider.RightUp();
        }

        public void RightUp(System.Drawing.Point location)
        {
            MouseInputProvider.RightUp(location);
        }

        public void Up(System.Windows.Forms.MouseButtons button)
        {
            MouseInputProvider.Up(button);
        }

        public void Up(System.Windows.Forms.MouseButtons button, System.Drawing.Point location)
        {
            MouseInputProvider.Up(button, location);
        }

        public void Click(System.Windows.Forms.MouseButtons button, int x, int y)
        {
            MouseInputProvider.Click(button, x, y);
        }

        public void Down(System.Windows.Forms.MouseButtons button, int x, int y)
        {
            MouseInputProvider.Down(button, x, y);
        }

        public void LeftClick(int x, int y)
        {
            MouseInputProvider.LeftClick(x, y);
        }

        public void LeftDown(int x, int y)
        {
            MouseInputProvider.LeftDown(x, y);
        }

        public void LeftUp(int x, int y)
        {
            MouseInputProvider.LeftUp(x, y);
        }

        public void Move(int x, int y)
        {
            MouseInputProvider.Move(x, y);
        }

        public void MoveRelative(System.Drawing.Point location)
        {
            MouseInputProvider.MoveRelative(location);
        }

        public void MoveRelative(int x, int y)
        {
            MouseInputProvider.MoveRelative(x, y);
        }

        public void RightClick(int x, int y)
        {
            MouseInputProvider.RightClick(x, y);
        }

        public void RightDown(int x, int y)
        {
            MouseInputProvider.RightDown(x, y);
        }

        public void RightUp(int x, int y)
        {
            MouseInputProvider.RightUp(x, y);
        }

        public void Up(System.Windows.Forms.MouseButtons button, int x, int y)
        {
            MouseInputProvider.Up(button, x, y);
        }

        public void KeyDown(System.Windows.Forms.Keys key)
        {
            KeyboardInputProvider.KeyDown(key);
        }

        public void KeyUp(System.Windows.Forms.Keys key)
        {
            KeyboardInputProvider.KeyUp(key);
        }

        public void KeyPress(params System.Windows.Forms.Keys[] keys)
        {
            KeyboardInputProvider.KeyPress(keys);
        }

        public void ModifiedKeyPress(System.Windows.Forms.Keys key, params System.Windows.Forms.Keys[] modifiers)
        {
            KeyboardInputProvider.ModifiedKeyPress(key, modifiers);
        }

        public void Type(string message)
        {
            KeyboardInputProvider.Type(message);
        }

        public void GiveFocus()
        {
            StUtil.Native.Internal.NativeMethods.SetForegroundWindow(Handle);
        }

        public bool WaitForFocus(TimeSpan timeout)
        {
            DateTime now = DateTime.Now;
            while (DateTime.Now - now < timeout)
            {
                if (IsFocused)
                {
                    return true;
                }
                Thread.Sleep(100);
            }
            return IsFocused;
        }

        public bool IsFocused
        {
            get
            {
                if (Handle == IntPtr.Zero)
                {
                    throw new Exception("No window handle set");
                }
                return StUtil.Native.Internal.NativeMethods.GetForegroundWindow() == Handle;
            }
        }


    }
}
