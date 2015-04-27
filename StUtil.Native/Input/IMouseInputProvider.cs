using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.Native.Input
{
    public interface IMouseInputProvider
    {
        IntPtr Handle { get; set; }
        MouseButtons ButtonsDown { get; }

        void Click(MouseButtons button);
        void Click(MouseButtons button, Point location);
        void Click(MouseButtons button, int x, int y);

        void Down(MouseButtons button);
        void Down(MouseButtons button, Point location);
        void Down(MouseButtons button, int x, int y);

        void LeftClick();

        void LeftClick(Point location);
        void LeftClick(int x, int y);

        void LeftDown();

        void LeftDown(Point location);
        void LeftDown(int x, int y);

        void LeftUp();

        void LeftUp(Point location);
        void LeftUp(int x, int y);

        void MoveTo(Point location);
        void MoveTo(int x, int y);

        void MoveBy(Point location);
        void MoveBy(int x, int y);

        void RightClick();

        void RightClick(Point location);
        void RightClick(int x, int y);

        void RightDown();

        void RightDown(Point location);
        void RightDown(int x, int y);

        void RightUp();

        void RightUp(Point location);
        void RightUp(int x, int y);

        void Up(MouseButtons button);
        void Up(MouseButtons button, Point location);
        void Up(MouseButtons button, int x, int y);
    }
}
