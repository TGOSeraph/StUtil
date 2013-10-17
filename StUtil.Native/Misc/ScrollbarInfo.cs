using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using StUtil.Internal.Native;
using System.Runtime.InteropServices;

namespace StUtil.Native.Misc
{
    public class ScrollbarInfo
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

        public ScrollbarInfo(Control ctrl, ScrollOrientation orientation)
        {
            this.TargetControl = ctrl;
            this.Orientation = orientation;

            Update();
        }

        public void Update()
        {
            NativeStructs.SCROLLINFO inf = NativeUtils.GetScrollInfo(this.TargetControl, Orientation == ScrollOrientation.VerticalScroll ? NativeEnums.ScrollBarDirection.SB_VERT : NativeEnums.ScrollBarDirection.SB_HORZ);
            this.Minimum = inf.nMin;
            this.Page = inf.nPage == 0? TargetControl.Height : (int)inf.nPage;
            this.Maximum = inf.nMax;
            this.Position = inf.nTrackPos;
        }
    }
}
