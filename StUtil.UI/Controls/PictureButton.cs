using StUtil.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.UI.Controls
{
    public class PictureButton : PictureBox
    {
        private Image normalImage, overImage, downImage, disabledImage;
        private Color normalBackColor, overBackColor, downBackColor, disabledBackColor;
        private bool mouseover, mousedown;

        [DefaultValue(typeof(Color), "Control")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), EditorBrowsable( EditorBrowsableState.Never)]
        public new Color BackColor
        {
            get
            {
                return base.BackColor;
            }
        }

        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new Image Image
        {
            get
            {
                return base.Image;
            }
            internal set
            {
            }
        }

        public Image NormalImage
        {
            get
            {
                return normalImage;
            }
            set
            {
                normalImage = value;
                UpdateStyle();
            }
        }

        public Image OverImage
        {
            get
            {
                return overImage;
            }
            set
            {
                overImage = value;
                UpdateStyle();
            }
        }

        public Image DisabledImage
        {
            get
            {
                return disabledImage;
            }
            set
            {
                disabledImage = value;
                UpdateStyle();
            }
        }

        public Image DownImage
        {
            get
            {
                return downImage;
            }
            set
            {
                downImage = value;
                UpdateStyle();
            }
        }

        public Color NormalBackColor
        {
            get
            {
                return normalBackColor;
            }
            set
            {
                normalBackColor = value;
                UpdateStyle();
            }
        }

        public Color OverBackColor
        {
            get
            {
                return overBackColor;
            }
            set
            {
                overBackColor = value;
                UpdateStyle();
            }
        }

        public Color DisabledBackColor
        {
            get
            {
                return disabledBackColor;
            }
            set
            {
                disabledBackColor = value;
                UpdateStyle();
            }
        }

        public Color DownBackColor
        {
            get
            {
                return downBackColor;
            }
            set
            {
                downBackColor = value;
                UpdateStyle();
            }
        }

        private void UpdateStyle()
        {
            if (this.Enabled)
            {
                if (mousedown)
                {
                    base.Image = downImage;
                    base.BackColor = downBackColor;
                }
                else if (mouseover)
                {
                    base.Image = overImage;
                    base.BackColor = overBackColor;
                }
                else
                {
                    base.Image = normalImage;
                    base.BackColor = normalBackColor;
                }
            }
            else
            {
                base.Image = disabledImage;
                base.BackColor = disabledBackColor;
            }
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            UpdateStyle();
        }

        public void SetImage(Image img, Color overTint, Color downTint, Color disabledTint)
        {
            NormalImage = img;
            OverImage = img.Tint(overTint);
            DownImage = img.Tint(downTint);
            DisabledImage = img.Tint(disabledTint);
        }

        public void SetImage(Image img, Color overTint, Color downTint)
        {
            NormalImage = img;
            OverImage = img.Tint(overTint);
            DownImage = img.Tint(downTint);
            DisabledImage = ToolStripRenderer.CreateDisabledImage(img);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            mouseover = true;
            UpdateStyle();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            mouseover = false;
            UpdateStyle();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            mousedown = true;
            UpdateStyle();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            mousedown = false;
            UpdateStyle();
        }
    }
}
