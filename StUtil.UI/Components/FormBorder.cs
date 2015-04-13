using StUtil.Extensions;
using StUtil.Native.Internal;
using StUtil.UI.Forms.Theme;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.UI.Components
{
    public class FormBorder : IDisposable
    {
        protected OuterBorderForm BorderLeft { get; private set; }
        protected OuterBorderForm BorderRight { get; private set; }
        protected OuterBorderForm BorderTop { get; private set; }
        protected OuterBorderForm BorderBottom { get; private set; }

        private int borderSize = 8;
        private bool visible = true;

        [DefaultValue(8)]
        [Description("The size of the border in pixels")]
        public int BorderSize
        {
            get { return borderSize; }
            set { borderSize = value; UpdateBorders(); }
        }

        [DefaultValue(true)]
        [Description("If the border is visible or not")]
        public bool Visible
        {
            get
            {
                return visible;
            }
            set
            {
                visible = value;
                BorderLeft.Visible = BorderRight.Visible = BorderTop.Visible = BorderBottom.Visible = value;
            }
        }

        private Color borderColor = Color.DodgerBlue;

        [DefaultValue(typeof(Color), "DodgerBlue")]
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                try
                {
                    Update(value);
                }
                catch (Exception)
                {
                }
            }
        }

        public Form Form { get; private set; }

        public FormBorder(Form form)
        {
            Form = form;
        }

        private void CreateBorders()
        {
            BorderLeft = CreateBorder(TabAlignment.Left, borderSize);
            BorderRight = CreateBorder(TabAlignment.Right, borderSize);
            BorderTop = CreateBorder(TabAlignment.Top, borderSize);
            BorderBottom = CreateBorder(TabAlignment.Bottom, borderSize);
            BorderLeft.Owner = BorderRight.Owner = BorderTop.Owner = BorderBottom.Owner = this.Form;
            Update(borderColor);
        }

        private void UpdateBorders()
        {
            if (BorderLeft != null)
            {
                if (this.Form.WindowState == FormWindowState.Normal && !Form.InDesignMode())
                {
                    BorderLeft.Visible = BorderRight.Visible = BorderTop.Visible = BorderBottom.Visible = Visible;
                    BorderLeft.Width = BorderRight.Width = BorderTop.Height = BorderBottom.Height = borderSize;
                    BorderLeft.Height = BorderRight.Height = this.Form.Height;
                    BorderTop.Width = BorderBottom.Width = this.Form.Width + borderSize + borderSize;
                    BorderLeft.Location = new System.Drawing.Point(this.Form.Left - borderSize, this.Form.Top);
                    BorderRight.Location = new System.Drawing.Point(this.Form.Right, this.Form.Top);
                    BorderTop.Location = new System.Drawing.Point(this.Form.Left - borderSize, this.Form.Top - borderSize);
                    BorderBottom.Location = new System.Drawing.Point(this.Form.Left - borderSize, this.Form.Bottom);
                }
                else
                {
                    BorderLeft.Visible = BorderRight.Visible = BorderTop.Visible = BorderBottom.Visible = false;
                }
            }
        }

        protected OuterBorderForm CreateBorder(TabAlignment side, int borderSize)
        {
            return new ShadowBorderForm(side, borderSize);
        }

        public void Update(object args)
        {
            if (!Form.InDesignMode())
            {
                if (BorderLeft != null)
                {
                    BorderLeft.Update(args);
                    BorderTop.Update(args);
                    BorderRight.Update(args);
                    BorderBottom.Update(args);
                }
            }
        }

        public void Apply()
        {
            if (BorderLeft == null)
            {
                CreateBorders();
            }

            this.Form.Shown += target_Shown;
            this.Form.Move += target_Move;
            this.Form.SizeChanged += target_SizeChanged;
            this.Form.GotFocus += target_GotFocus;
            this.Form.LostFocus += target_LostFocus;
            this.Form.Activated += target_Activated;
            this.Form.Deactivate += target_Deactivate;
        }

        void target_Deactivate(object sender, EventArgs e)
        {
            UpdateActive();
        }

        void target_Activated(object sender, EventArgs e)
        {
            UpdateActive();
        }

        void target_LostFocus(object sender, EventArgs e)
        {
            UpdateActive();
        }

        void target_GotFocus(object sender, EventArgs e)
        {
            UpdateActive();
        }

        private void UpdateActive()
        {
            BorderLeft.Active = BorderRight.Active = BorderTop.Active = BorderBottom.Active = NativeMethods.GetForegroundWindow() == this.Form.Handle;
        }

        private void target_SizeChanged(object sender, EventArgs e)
        {
            UpdateBorders();
        }

        private void target_Move(object sender, EventArgs e)
        {
            UpdateBorders();
        }

        private void target_Shown(object sender, EventArgs e)
        {
            BorderLeft.Show();
            BorderRight.Show();
            BorderTop.Show();
            BorderBottom.Show();
            UpdateBorders();
        }

        public void Dispose()
        {
            BorderLeft.Dispose();
            BorderRight.Dispose();
            BorderTop.Dispose();
            BorderBottom.Dispose();
        }
    }
}
