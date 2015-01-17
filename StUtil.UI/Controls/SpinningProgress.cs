using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.UI.Controls
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class SpinningProgress : Control
    {
        private double[] angleLookup;
        private PointF centerPoint;
        private Color[] colorPallet;
        private int currentProgress;
        private int spinSpeed = 40;
        private int spokeCount = 10;
        private int spokeInner = 8;
        private int spokeOuter = 9;
        private int spokeThickness = 4;
        private Timer updateTimer;

        [DefaultValue(true)]
        public bool Clockwise { get; set; }

        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                CreateColorPallet();
            }
        }

        public bool IsSpinning
        {
            get
            {
                return this.updateTimer == null ? false : this.updateTimer.Enabled;
            }
            set
            {
                if (this.updateTimer != null)
                {
                    this.updateTimer.Enabled = value;
                }
                this.ActivateTimer();
            }
        }

        public int SpinSpeed
        {
            get
            {
                return spinSpeed;
            }
            set
            {
                if (this.updateTimer != null)
                {
                    this.updateTimer.Interval = value;
                }
                spinSpeed = value;
            }
        }

        public int SpokeCount
        {
            get { return spokeCount; }
            set
            {
                spokeCount = value;
                Update();
            }
        }

        public int SpokeInner
        {
            get
            {
                return this.spokeInner;
            }
            set
            {
                this.spokeInner = value;
                base.Invalidate();
            }
        }
        public int SpokeOuter
        {
            get
            {
                return this.spokeOuter;
            }
            set
            {
                this.spokeOuter = value;
                base.Invalidate();
            }
        }
        public int SpokeThickness
        {
            get { return spokeThickness; }
            set
            {
                spokeThickness = value;
                Update();
            }
        }
        public SpinningProgress()
        {
            this.Clockwise = true;
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.CreateColorPallet();
            this.CreateSpokeAngles();
            this.GetCenterPoint();
            this.updateTimer = new Timer();
            this.updateTimer.Tick += new EventHandler(this.updateTimer_Tick);
            this.updateTimer.Interval = spinSpeed;
            this.ActivateTimer();
            base.Resize += new EventHandler(this.SpinningProgress_Resize);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.updateTimer != null)
            {
                this.updateTimer.Tick -= new EventHandler(this.updateTimer_Tick);
                this.updateTimer.Stop();
                this.updateTimer.Dispose();
                this.updateTimer = null;
            }
            base.Dispose(disposing);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.SpokeCount > 0)
            {
                SmoothingMode smoothingMode = e.Graphics.SmoothingMode;
                try
                {
                    e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                    int num = this.currentProgress;
                    for (int i = 0; i < this.SpokeCount; i++)
                    {
                        num %= this.SpokeCount;
                        PointF point = this.GetPoint(this.centerPoint, this.spokeInner, this.angleLookup[num]);
                        PointF point2 = this.GetPoint(this.centerPoint, this.spokeOuter, this.angleLookup[num]);

                        using (Pen pen = new Pen(new SolidBrush(this.colorPallet[i]), (float)this.SpokeThickness))
                        {
                            pen.StartCap = LineCap.Round;
                            pen.EndCap = LineCap.Round;
                            e.Graphics.DrawLine(pen, point, point2);
                        }
                        num++;
                    }
                }
                finally
                {
                    e.Graphics.SmoothingMode = smoothingMode;
                }
            }
            base.OnPaint(e);
        }

        private void ActivateTimer()
        {
            if (this.updateTimer != null)
            {
                this.updateTimer.Interval = spinSpeed;
                if (this.updateTimer.Enabled)
                {
                    this.updateTimer.Start();
                    this.CreateColorPallet();
                }
                else
                {
                    this.updateTimer.Stop();
                    this.currentProgress = 0;
                }
                base.Invalidate();
            }
        }

        private void CreateColorPallet()
        {
            this.colorPallet = new Color[this.SpokeCount];
            byte b = (byte)(255 / this.SpokeCount);
            byte b2 = 0;
            for (int i = 0; i < this.SpokeCount; i++)
            {
                if (this.IsSpinning)
                {
                    if (i == 0 || i < this.SpokeCount - this.SpokeCount)
                    {
                        this.colorPallet[i] = this.ForeColor;
                    }
                    else
                    {
                        b2 += b;
                        if (b2 > 255)
                        {
                            b2 = 255;
                        }
                        this.colorPallet[i] = this.Darken(this.ForeColor, (int)b2);
                    }
                }
                else
                {
                    this.colorPallet[i] = this.ForeColor;
                }
            }
        }

        private void CreateSpokeAngles()
        {
            this.angleLookup = new double[this.SpokeCount];
            double num = 360.0 / (double)this.SpokeCount;
            for (int i = 0; i < this.SpokeCount; i++)
            {
                this.angleLookup[i] = ((i == 0) ? num : (this.angleLookup[i - 1] + num));
            }
        }

        private Color Darken(Color startColor, int percent)
        {
            return Color.FromArgb(percent, startColor);
        }

        private void GetCenterPoint()
        {
            this.centerPoint = this.GetCenterPoint(this);
        }

        private PointF GetCenterPoint(Control control)
        {
            return new PointF((float)(control.Width / 2), (float)(control.Height / 2 - 1));
        }

        private PointF GetPoint(PointF center, int radius, double angle)
        {
            double num = Math.PI * angle / 180.0;
            return new PointF(center.X + (float)radius * (float)Math.Cos(num), center.Y + (float)radius * (float)Math.Sin(num));
        }

        private void SpinningProgress_Resize(object sender, EventArgs e)
        {
            this.GetCenterPoint();
        }

        private new void Update()
        {
            this.CreateColorPallet();
            this.CreateSpokeAngles();
            base.Invalidate();
        }
        private void updateTimer_Tick(object sender, EventArgs e)
        {
            if (Clockwise)
            {
                this.currentProgress++;
            }
            else
            {
                this.currentProgress--;
            }
            if (this.currentProgress >= this.spokeCount)
            {
                this.currentProgress = 0;
            }
            else if (this.currentProgress < 0)
            {
                currentProgress = this.spokeCount;
            }
            base.Invalidate();
        }
    }
}