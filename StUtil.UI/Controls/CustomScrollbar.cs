using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.UI.Controls
{
    public partial class CustomScrollbar : UserControl
    {
        private const int SKIPTICK = 6;

        private Timer scrollTimer = new Timer();
        private int skipTick = 0;
        private ListSortDirection scrollDirection;
        private Control boundControl;
        private ScrollableControl scrollableControl;
        private Point mouseDownLocation;
        private StUtil.Native.WndProcOverride wndProc;
        private StUtil.Native.Windows.Controls.ScrollBar scrollBar;

        [DefaultValue(5)]
        public int DefaultSmallChange { get; set; }

        [DefaultValue(null)]
        public Control BoundTo
        {
            get
            {
                return boundControl;
            }
            set
            {
                if (boundControl != null)
                {
                    if (scrollableControl != null)
                    {
                        scrollableControl.Scroll -= Target_Scroll;
                    }
                    else
                    {
                        wndProc.Dispose();
                    }
                    boundControl.SizeChanged -= Target_SizeChanged;
                    boundControl.MouseWheel -= Target_MouseWheel;
                    boundControl.Move -= boundControl_Move;
                }
                boundControl = value;
                scrollableControl = value as ScrollableControl;
                if (scrollableControl != null)
                {
                    scrollableControl.Scroll += Target_Scroll;
                }
                else
                {
                    scrollBar = new Native.Windows.Controls.ScrollBar(boundControl, ScrollOrientation.VerticalScroll);
                    wndProc = new Native.WndProcOverride(boundControl, new StUtil.Native.WndProcHandler(new Native.WndProcHandler.WndProcHandlerDelegate(delegate(ref Message m, out bool handled)
                    {
                        handled = false;
                    })) { MessageId = (int)StUtil.Native.Internal.NativeEnums.WM.VSCROLL });
                }
                boundControl.SizeChanged += Target_SizeChanged;
                boundControl.MouseWheel += Target_MouseWheel;
                boundControl.Move += boundControl_Move;
                UpdateLocation();
            }
        }

        public CustomScrollbar()
        {
            InitializeComponent();
            DefaultSmallChange = 5;
            scrollTimer.Interval = 75;
            scrollTimer.Tick += scrollTimer_Tick;
            this.pnlArrowBottom.Height = this.pnlArrowTop.Height = SystemInformation.VerticalScrollBarArrowHeight;
            this.Width = SystemInformation.VerticalScrollBarWidth;
            this.pnlThumb.MinimumSize = new Size(0, SystemInformation.VerticalScrollBarThumbHeight);
        }

        void scrollTimer_Tick(object sender, EventArgs e)
        {
            if (skipTick > 0)
            {
                skipTick--;
                return;
            }

            int change = scrollableControl != null ? scrollableControl.VerticalScroll.SmallChange : DefaultSmallChange;
            if (scrollDirection == ListSortDirection.Descending)
            {
                DoScroll(change);
            }
            else
            {
                DoScroll(-change);
            }
        }

        void Target_MouseWheel(object sender, MouseEventArgs e)
        {
            UpdateScroll();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (BoundTo != null)
            {
                UpdateLocation();
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            if (BoundTo != null)
            {
                UpdateLocation();
            }
        }

        void Target_SizeChanged(object sender, EventArgs e)
        {
            UpdateLocation();
        }

        private void boundControl_Move(object sender, EventArgs e)
        {
            UpdateLocation();
        }

        void Target_Scroll(object sender, ScrollEventArgs e)
        {
            UpdateScroll();
        }

        protected virtual void UpdateLocation()
        {
            this.Location = new Point(BoundTo.Right - SystemInformation.VerticalScrollBarWidth, BoundTo.Top);
            this.Height = BoundTo.Height;
            UpdateScroll();
        }

        private void UpdateScroll()
        {
            if (scrollableControl != null ? scrollableControl.VerticalScroll.Visible : true) 
            {
                this.Visible = true;
                pnlThumb.Height = ComputeThumbSize();
                pnlThumb.Top = ComputeThumbPos();
            }
            else
            {
                this.Visible = false;
            }
        }

        private void DoScroll(int change)
        {
            if (scrollableControl != null)
            {
                scrollableControl.AutoScrollPosition = new Point(0, scrollableControl.VerticalScroll.Value + change);
            }
            else
            {
                scrollBar.Update();
                scrollBar.ScrollTo(scrollBar.Position + change);
            }
            UpdateScroll();
        }

        private int ComputeThumbSize()
        {
            int max, min;
            if (scrollableControl != null)
            {
                max = scrollableControl.VerticalScroll.Maximum;
                min = scrollableControl.VerticalScroll.Minimum;
            }
            else
            {
                scrollBar.Update();
                max = scrollBar.Maximum;
                min = scrollBar.Minimum;
            }
            return (int)(pnlScrollArea.Height * (BoundTo.Height / (double)(max - min)));
        }

        private int ComputeThumbPos()
        {
            int max, min, val;
            if (scrollableControl != null)
            {
                max = scrollableControl.VerticalScroll.Maximum;
                min = scrollableControl.VerticalScroll.Minimum;
                val = scrollableControl.VerticalScroll.Value;
            }
            else
            {
                scrollBar.Update();
                max = scrollBar.Maximum;
                min = scrollBar.Minimum;
                val = scrollBar.Position;
            }
            double scrollPercent =val / (double)(max - min);
            return (int)(this.pnlScrollArea.Height * scrollPercent);
        }

        private void pnlArrowBottom_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                DoScroll(scrollableControl != null ? scrollableControl.VerticalScroll.SmallChange : DefaultSmallChange);
                scrollDirection = ListSortDirection.Descending;
                skipTick = SKIPTICK;
                scrollTimer.Start();
            }
        }

        private void pnlArrowBottom_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                scrollTimer.Stop();
            }
        }

        private void pnlArrowTop_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                DoScroll(-(scrollableControl != null ? scrollableControl.VerticalScroll.SmallChange : DefaultSmallChange));
                scrollDirection = ListSortDirection.Ascending;
                skipTick = SKIPTICK;
                scrollTimer.Start();
            }
        }

        private void pnlArrowTop_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                scrollTimer.Stop();
            }
        }

        private void pnlScrollArea_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Point pt = pnlScrollArea.PointToClient(Cursor.Position);
                
                int change;
                if (scrollableControl != null)
                {
                    change = scrollableControl.VerticalScroll.LargeChange;
                }
                else
                {
                    scrollBar.Update();
                    change = scrollBar.Page;
                }

                if (pt.Y > pnlThumb.Top)
                {
                    DoScroll(change);
                }
                else
                {
                    DoScroll(-change);
                }
            }
        }

        private void pnlThumb_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                int diff = mouseDownLocation.Y - e.Y;
                int newV = pnlThumb.Top - diff;
                if (newV < 0) newV = 0;
                if (newV + pnlThumb.Height > pnlScrollArea.Height) newV = pnlScrollArea.Height - pnlThumb.Height;

                pnlThumb.Top = newV;
                DoScroll(-diff);
            }
        }

        private void pnlThumb_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                mouseDownLocation = e.Location;
            }
        }

    }
}
