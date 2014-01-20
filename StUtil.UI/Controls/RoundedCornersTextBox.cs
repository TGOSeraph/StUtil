using StUtil.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace StUtil.UI.Controls
{
    public partial class TextBoxRoundedCorners : TextBoxRoundedCorners<TextBox> 
    {
    }

    public partial class PlaceholderTextBoxRoundedCorners : TextBoxRoundedCorners<PlaceholderTextbox>
    {
    }

    public partial class TextBoxRoundedCorners<T> : UserControl where T : TextBox, new()
    {
        private int borderRadius = 5;
        public int BorderRadius
        {
            get { return borderRadius; }
            set
            {
                this.borderRadius = value;
                this.Refresh();
            }
        }

        private Color borderColor = Color.Black;
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                this.borderColor = value;
                this.borderPen.Dispose();
                this.borderPen = new Pen(this.borderColor, this.borderWidth);
                this.Refresh();
            }
        }

        private int borderWidth = 3;
        public int BorderWidth
        {
            get { return borderWidth; }
            set
            {
                this.borderWidth = value;
                this.borderPen.Dispose();
                this.borderPen = new Pen(this.borderColor, this.borderWidth);
                this.Refresh();
            }
        }

        private Pen borderPen;
        public Pen BorderPen
        {
            get { return borderPen; }
            set
            {
                this.borderPen = value;
                this.borderWidth = (int)this.borderPen.Width;
                this.borderColor = this.borderPen.Color;
                this.Refresh();
            }
        }

        private Padding textboxPadding;
        public virtual Padding TextBoxPadding
        {
            get { return textboxPadding; }
            set
            {
                textboxPadding = value;
                ComputePadding();
                this.Refresh();
            }
        }

        public override string Text
        {
            get { return this.internalTb.Text; }
            set { this.internalTb.Text = value; }
        }

        private bool autoSize = false;
        public override bool AutoSize
        {
            get
            {
                return this.autoSize;
            }
            set
            {
                this.autoSize = value;
                if (value)
                {
                    this.ComputePadding();
                    this.Refresh();
                }
            }
        }

        public T TextBoxControl
        {
            get
            {
                return this.internalTb;
            }
        }

        private void ComputePadding()
        {
            this.Padding = new System.Windows.Forms.Padding(this.textboxPadding.Left + this.borderWidth, this.textboxPadding.Top + this.borderWidth, this.textboxPadding.Right + this.borderWidth, this.textboxPadding.Bottom + this.borderWidth);
            if (this.AutoSize)
            {
                this.Height = this.borderWidth + this.borderWidth + this.textboxPadding.Vertical + this.internalTb.Height;
            }
        }

        public TextBoxRoundedCorners()
        {
            this.textboxPadding = new Padding(8, 4, 8, 4);
            this.ResizeRedraw = true;
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint,  true);
            InitializeComponent();
            this.borderPen = new Pen(this.borderColor, this.borderWidth);
            ComputePadding();
            this.internalTb.KeyDown += new KeyEventHandler(internalTb_KeyDown);
            this.internalTb.Multiline = false;
        }

        private void internalTb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control & e.KeyCode == Keys.A)
                ((TextBox)sender).SelectAll();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                Rectangle r = new Rectangle(internalTb.Left - (this.textboxPadding.Left), internalTb.Top - (this.textboxPadding.Top), internalTb.Width + this.textboxPadding.Horizontal, internalTb.Height + this.textboxPadding.Vertical);
                e.Graphics.DrawRoundedRectangle(this.borderPen, r, this.BorderRadius);
                e.Graphics.FillRoundedRectangle(new SolidBrush(this.internalTb.BackColor), r, this.BorderRadius);
            }
            catch (Exception)
            {
            }
        }

        public static implicit operator T(TextBoxRoundedCorners<T> txtBox)
        {
            return txtBox.internalTb;
        }
    }
}
