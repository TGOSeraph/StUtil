using StUtil.Native.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.Native.Windows.Controls
{
    public class ScrollBar
    {
        public Control TargetControl { get; private set; }
        public ScrollOrientation Orientation { get; private set; }
        public int Minimum { get; private set; }
        public int Maximum { get; private set; }
        public int Page { get; private set; }
        public int Position { get; private set; }
        public double PercentBottom
        {
            get
            {
                return Math.Min((((double)this.Position + this.Page) / this.Maximum) * 100, 100);
            }
        }
        public double PercentTop
        {
            get
            {
                return Math.Max((((double)this.Position) / this.Maximum) * 100, 0);
            }
        }
        public int ScrollAreaRemaining
        {
            get
            {
                return Math.Max(Math.Max(this.Maximum, this.Page) - this.Page - this.Position, 0);
            }
        }
        public ScrollBar(Control ctrl, ScrollOrientation orientation)
        {
            this.TargetControl = ctrl;
            this.Orientation = orientation;
            Update();
        }

        public void Update()
        {
            NativeStructs.SCROLLINFO inf = NativeUtilities.GetScrollInfo(this.TargetControl, Orientation == ScrollOrientation.VerticalScroll ? NativeEnums.SB.VERT : NativeEnums.SB.HORZ);
            this.Minimum = inf.nMin;
            this.Page = inf.nPage == 0 ? TargetControl.Height : (int)inf.nPage;
            this.Maximum = inf.nMax;
            this.Position = inf.nTrackPos;
        }

        public void ScrollTo(int value)
        {
            NativeStructs.SCROLLINFO inf = new NativeStructs.SCROLLINFO();
            inf.fMask = (uint)NativeEnums.SIF.POS;
            inf.nPos = value;
            NativeMethods.SetScrollInfo(this.TargetControl.Handle, (int)(Orientation == ScrollOrientation.VerticalScroll ? NativeEnums.SB.VERT : NativeEnums.SB.HORZ), ref inf, true);
        }
    }
}
