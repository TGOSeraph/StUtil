using StUtil.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using StUtil.UI.Utilities;
using System.Threading;

namespace StUtil.UI.Controls
{
    public abstract class CustomProgressBar : Panel
    {
        public enum ProgressBarType
        {
            Continuous,
            Marquee
        }

        public enum LabelTypeEnum
        {
            Percent,
            XofY,
            X,
            Custom
        }

        public enum LabelXOffsetEnum
        {
            FromLeft,
            FromMiddle,
            FromRight,
            FromBarLeft,
            FromBarMiddle,
            FromBarRight
        }

        public enum LabelYOffsetEnum
        {
            FromTop,
            FromMiddle,
            FromBottom,
            FromBarTop,
            FromBarMiddle,
            FromBarBottom,
        }

        private Padding barMargins;
        public Padding BarMargins
        {
            get { return barMargins; }
            set { barMargins = value; this.Refresh(); }
        }

        private string customLabel;

        public string CustomLabel
        {
            get { return customLabel; }
            set { customLabel = value; this.Refresh(); }
        }

        public ProgressBarType ProgressStyle { get; private set; }

        private long value;
        public long Value
        {
            get
            {
                return value;
            }
            set
            {
                if (ProgressStyle == ProgressBarType.Marquee)
                {
                    throw new InvalidOperationException("Value cannot be changed in Marquee mode");
                }
                if (!this.InDesignMode() && EnableAnimation)
                {
                    if (valueAnimator.IsRunning)
                    {
                        valueAnimator.PerformAnimation(valueAnimator.EndValue, value);
                    }
                    else
                    {
                        valueAnimator.PerformAnimation(value);
                    }
                }
                else
                {
                    this.value = value;
                }
                this.Refresh();
            }
        }
        private long BaseValue
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
                this.Refresh();
            }
        }

        private long maxValue;
        public long MaxValue
        {
            get { return maxValue; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("MaxValue must be positive");
                }
                maxValue = value;
                this.Refresh();
            }
        }

        private bool drawLabel;
        public bool DrawLabel
        {
            get { return drawLabel; }
            set { drawLabel = value; this.Refresh(); }
        }

        private string labelOfString;
        public string LabelOfString
        {
            get { return labelOfString; }
            set { labelOfString = value; this.Refresh(); }
        }

        private LabelTypeEnum labelType;
        public LabelTypeEnum LabelType
        {
            get { return labelType; }
            set { labelType = value; this.Refresh(); }
        }

        private Point labelOffset;
        public Point LabelOffset
        {
            get { return labelOffset; }
            set { labelOffset = value; this.Refresh(); }
        }

        private LabelXOffsetEnum labelOffsetX;
        public LabelXOffsetEnum LabelOffsetX
        {
            get { return labelOffsetX; }
            set { labelOffsetX = value; }
        }

        private LabelYOffsetEnum labelOffsetY;
        public LabelYOffsetEnum LabelOffsetY
        {
            get { return labelOffsetY; }
            set { labelOffsetY = value; }
        }

        public long Step { get; set; }

        public double Percent
        {
            get
            {
                return (double)Value / MaxValue;
            }
        }

        public bool EnableAnimation { get; set; }

        private ControlLongAnimator valueAnimator;

        public CustomProgressBar()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            this.barMargins = new Padding(10, 10, 50, 10);
            this.maxValue = 100;
            this.drawLabel = true;
            this.labelType = LabelTypeEnum.Percent;
            this.labelOfString = "/";
            this.labelOffset = new Point(11, -7);
            this.labelOffsetY = LabelYOffsetEnum.FromBarMiddle;
            this.labelOffsetX = LabelXOffsetEnum.FromBarRight;

            this.Step = 1;

            this.valueAnimator = new ControlLongAnimator(this, () => BaseValue);
        }

        public void PerformStep(bool boundCheck = true)
        {
            long v = this.value + this.Step;
            if (boundCheck)
            {
                if (v < 0)
                {
                    this.Value = 0;
                }
                else if (v > this.maxValue)
                {
                    this.Value = this.maxValue;
                }
                else
                {
                    this.Value = v;
                }
            }
            else
            {
                this.Value = v;
            }
        }

        private Thread marqueeThread = null;
        public void StartMarquee()
        {
            if (marqueeThread != null)
            {
                throw new InvalidOperationException("Marquee already running");
            }
            this.ProgressStyle = ProgressBarType.Marquee;
            Action a = () =>
            {
                while (true)
                {
                    this.Invoke((Action)delegate()
                    {
                        this.Refresh();
                    });
                    Thread.Sleep(20);
                }
            };
            marqueeThread = a.RunOnNewThread();
            marqueeThread.IsBackground = true;
        }

        public void StopMarquee()
        {
            this.ProgressStyle = ProgressBarType.Continuous;
            if (marqueeThread != null && marqueeThread.IsAlive)
            {
                try
                {
                    marqueeThread.Abort();
                    marqueeThread = null;
                }
                catch (Exception)
                {
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            DrawEmptyBar(e.Graphics);
            if (ProgressStyle == ProgressBarType.Marquee)
            {
                DrawBarMarquee(e.Graphics);
            }
            else
            {
                DrawBarFill(e.Graphics);
            }
            if (this.drawLabel)
            {
                DrawProgressLabel(e.Graphics);
            }
        }

        public virtual void DrawProgressLabel(Graphics g)
        {
            int x = 0;
            int y = 0;

            Rectangle pgb = new Rectangle(this.BarMargins.Left, this.BarMargins.Top, this.Width - this.BarMargins.Left - this.BarMargins.Right, this.Height - this.BarMargins.Top - this.BarMargins.Bottom);

            switch (labelOffsetX)
            {
                case LabelXOffsetEnum.FromBarLeft:
                    x = pgb.Left + labelOffset.X;
                    break;
                case LabelXOffsetEnum.FromBarMiddle:
                    x = pgb.Middle().X + labelOffset.X;
                    break;
                case LabelXOffsetEnum.FromBarRight:
                    x = pgb.Right + labelOffset.X;
                    break;
                case LabelXOffsetEnum.FromLeft:
                    x = labelOffset.X;
                    break;
                case LabelXOffsetEnum.FromMiddle:
                    x = this.ClientRectangle.Middle().X + labelOffset.X;
                    break;
                case LabelXOffsetEnum.FromRight:
                    x = this.Width + labelOffset.X;
                    break;
            }

            switch (labelOffsetY)
            {
                case LabelYOffsetEnum.FromBarBottom:
                    y = pgb.Bottom + labelOffset.Y;
                    break;
                case LabelYOffsetEnum.FromBarMiddle:
                    y = pgb.Middle().Y + labelOffset.Y;
                    break;
                case LabelYOffsetEnum.FromBarTop:
                    y = pgb.Top + labelOffset.Y;
                    break;
                case LabelYOffsetEnum.FromBottom:
                    y = this.Height + labelOffset.Y;
                    break;
                case LabelYOffsetEnum.FromMiddle:
                    y = this.ClientRectangle.Middle().Y + labelOffset.Y;
                    break;
                case LabelYOffsetEnum.FromTop:
                    y = labelOffset.Y;
                    break;
            }

            string message = "";
            switch (this.labelType)
            {
                case LabelTypeEnum.Percent:
                    message = (Math.Round(this.Percent * 100, 2).ToString() + "%").PadLeft(4);
                    break;
                case LabelTypeEnum.X:
                    message = this.value.ToString();
                    break;
                case LabelTypeEnum.XofY:
                    message = this.value.ToString() + this.labelOfString + this.maxValue.ToString();
                    break;
                case LabelTypeEnum.Custom:
                    message = this.customLabel;
                    break;
            }

            g.DrawString(message, this.Font, new SolidBrush(this.ForeColor), new PointF(x, y));
        }

        public abstract void DrawEmptyBar(Graphics g);

        public abstract void DrawBarFill(Graphics g);
        public abstract void DrawBarMarquee(Graphics g);
    }
}
