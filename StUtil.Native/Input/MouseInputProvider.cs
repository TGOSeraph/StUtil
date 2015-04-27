using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.Native.Input
{
    public abstract class MouseInputProvider : IMouseInputProvider
    {
        private IntPtr handle = IntPtr.Zero;
        private MouseButtons state = MouseButtons.None;
        public MouseButtons ButtonsDown
        {
            get { return state; }
        }

        public IntPtr Handle
        {
            get
            {
                if (RequiresHandle && handle == IntPtr.Zero)
                {
                    throw new InvalidOperationException("This input provider requires a window handle");
                }
                return handle;
            }
            set
            {
                handle = value;
            }
        }

        protected abstract bool RequiresHandle
        {
            get;
        }
        public void Click(MouseButtons button)
        {
            Click(button, Cursor.Position);
        }

        public void Click(MouseButtons button, Point location)
        {
            Click(button, location.X, location.Y);
        }

        public void Click(MouseButtons button, int x, int y)
        {
            Down(button, x, y);
            Up(button, x, y);
        }

        public void Down(MouseButtons button)
        {
            Down(button, Cursor.Position);
        }

        public void Down(MouseButtons button, Point location)
        {
            Down(button, location.X, location.Y);
        }

        public void Down(MouseButtons button, int x, int y)
        {
            ButtonDown(button, x, y);
            state |= button;
        }

        public void LeftClick()
        {
            LeftClick(Cursor.Position);
        }

        public void LeftClick(Point location)
        {
            LeftClick(location.X, location.Y);
        }

        public void LeftClick(int x, int y)
        {
            Click(MouseButtons.Left, x, y);
        }

        public void LeftDown()
        {
            LeftDown(Cursor.Position);
        }

        public void LeftDown(Point location)
        {
            LeftDown(location.X, location.Y);
        }

        public void LeftDown(int x, int y)
        {
            Down(MouseButtons.Left, x, y);
        }

        public void LeftUp()
        {
            LeftUp(Cursor.Position);
        }

        public void LeftUp(Point location)
        {
            LeftUp(location.X, location.Y);
        }

        public void LeftUp(int x, int y)
        {
            Up(MouseButtons.Left, x, y);
        }

        public void MoveTo(Point location)
        {
            MoveTo(location.X, location.Y);
        }

        public abstract void MoveTo(int x, int y);

        public void MoveBy(Point location)
        {
            MoveBy(location.X, location.Y);
        }

        public void MoveBy(int x, int y)
        {
            Point pos = Cursor.Position;
            pos.Offset(x, y);
            MoveTo(pos);
        }

        public void RightClick()
        {
            RightClick(Cursor.Position);
        }

        public void RightClick(Point location)
        {
            RightClick(location.X, location.Y);
        }

        public void RightClick(int x, int y)
        {
            Click(MouseButtons.Right, x, y);
        }

        public void RightDown()
        {
            RightDown(Cursor.Position);
        }

        public void RightDown(Point location)
        {
            RightDown(location.X, location.Y);
        }

        public void RightDown(int x, int y)
        {
            Down(MouseButtons.Right, x, y);
        }

        public void RightUp()
        {
            RightUp(Cursor.Position);
        }

        public void RightUp(Point location)
        {
            RightUp(location.X, location.Y);
        }

        public void RightUp(int x, int y)
        {
            Up(MouseButtons.Right, x, y);
        }

        public void Up(MouseButtons button)
        {
            Up(button, Cursor.Position);
        }

        public void Up(MouseButtons button, Point location)
        {
            Up(button, location.X, location.Y);
        }

        public void Up(MouseButtons button, int x, int y)
        {
            ButtonUp(button, x, y);
            state &= ~button;
        }

        protected abstract void ButtonDown(MouseButtons button, int x, int y);
        protected abstract void ButtonUp(MouseButtons button, int x, int y);
    }
}
