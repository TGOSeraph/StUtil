using StUtil.Extensions;
using StUtil.UI.Utilities;
using System.Drawing;
using System.Windows.Forms;

namespace StUtil.UI.Controls
{
    public class ColorFaderTextBox : TextBox
    {
        private ControlColorAnimator backColorAnimator;
        private ControlColorAnimator foreColorAnimator;

        public new Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                if (!this.InDesignMode() && BackgroundFadeEnabled)
                {
                    backColorAnimator.PerformAnimation(value);
                }
                else
                {
                    base.BackColor = value;
                }
            }
        }
        private Color BaseBackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
            }
        }

        public bool BackgroundFadeEnabled { get; set; }
        public int BackgroundFadeSpeed { get { return backColorAnimator.Steps; } set { backColorAnimator.Steps = value; } }

        public new Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                if (!this.InDesignMode() && ForegroundFadeEnabled)
                {
                    foreColorAnimator.PerformAnimation(value);
                }
                else
                {
                    base.ForeColor = value;
                }
            }
        }
        private Color BaseForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
            }
        }

        public bool ForegroundFadeEnabled { get; set; }
        public int ForegroundFadeSpeed { get { return foreColorAnimator.Steps; } set { foreColorAnimator.Steps = value; } }

        public ColorFaderTextBox()
        {
            backColorAnimator = new ControlColorAnimator(this, () => BaseBackColor);
            foreColorAnimator = new ControlColorAnimator(this, () => BaseForeColor);
        }
    }
}
