using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StUtil.Extensions;

namespace StUtil.UI.Controls
{
    public class ToolStripRadioButton : ToolStripButton
    {
        private int radioButtonGroupId = 0;
        private bool updateButtonGroup = true;

        private Color checkedColor1 = Color.FromArgb(71, 113, 179);
        private Color checkedColor2 = Color.FromArgb(98, 139, 205);

        public ToolStripRadioButton()
        {
            this.CheckOnClick = true;
        }

        [Category("Behavior")]
        public int RadioButtonGroupId
        {
            get
            {
                return radioButtonGroupId;
            }
            set
            {
                radioButtonGroupId = value;

                // Make sure no two radio buttons are checked at the same time
                UpdateGroup();
            }
        }

        [Category("Appearance")]
        public Color CheckedColor1
        {
            get { return checkedColor1; }
            set { checkedColor1 = value; }
        }

        [Category("Appearance")]
        public Color CheckedColor2
        {
            get { return checkedColor2; }
            set { checkedColor2 = value; }
        }

        private Color alternateColor = Color.White;
        [Category("Appearance")]
        public Color AlternateColor
        {
            get
            {
                return alternateColor;
            }
            set
            {
                alternateColor = value;
            }
        }

        // Set check value without updating (disabling) other radio buttons in the group
        private void SetCheckValue(bool checkValue)
        {
            updateButtonGroup = false;
            this.Checked = checkValue;
            updateButtonGroup = true;
        }

        // To make sure no two radio buttons are checked at the same time
        private void UpdateGroup()
        {
            if (this.Parent != null)
            {
                // Get number of checked radio buttons in group
                int checkedCount = this.Parent.Items.OfType<ToolStripRadioButton>().Count(x => x.RadioButtonGroupId == RadioButtonGroupId && x.Checked);

                if (checkedCount > 1)
                {
                    this.Checked = false;
                }
            }
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            this.Checked = true;
        }

        protected override void OnCheckedChanged(EventArgs e)
        {
            Color c = this.AlternateColor;
            this.AlternateColor = this.ForeColor;
            this.ForeColor = c;

            if (this.Parent != null && updateButtonGroup)
            {
                foreach (ToolStripRadioButton radioButton in this.Parent.Items.OfType<ToolStripRadioButton>())
                {
                    // Disable all other radio buttons with same group id
                    if (radioButton != this && radioButton.RadioButtonGroupId == this.RadioButtonGroupId)
                    {
                        radioButton.SetCheckValue(false);
                    }
                }
            }

            base.OnCheckedChanged(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.Checked)
            {
                var checkedBackgroundBrush = new LinearGradientBrush(new Point(0, 0), new Point(0, this.Height), CheckedColor1, CheckedColor2);
                if (this.Parent.RenderMode == ToolStripRenderMode.System || this.Parent.RenderMode == ToolStripRenderMode.Custom)
                {
                    e.Graphics.FillRoundedRectangle(checkedBackgroundBrush, 0, 1, this.Width - 1, this.Height - 2, 3);
                }
                else
                {
                    e.Graphics.FillRectangle(checkedBackgroundBrush, new Rectangle(new Point(0, 0), this.Size));
                }
            }

            base.OnPaint(e);
        }
    }
}
