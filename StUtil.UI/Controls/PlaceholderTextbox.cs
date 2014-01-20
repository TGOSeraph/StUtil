using System;
using System.Drawing;
using System.Windows.Forms;

namespace StUtil.UI.Controls
{
    public class PlaceholderTextbox : ColorFaderTextBox
    {
        private Color storeForeColor;
        private Char storePasswordChar;

        private string placeholderText = "";
        public String PlaceholderText
        {
            get
            {
                return this.placeholderText;
            }
            set
            { 
                this.placeholderText = value;
                if (this.PlaceholderSet)
                {
                    base.Text = value;
                }
            }
        }

        private Color placeholderTextColor;
        public Color PlaceholderForeColor
        {
            get
            {
                return placeholderTextColor;
            }
            set
            {
                placeholderTextColor = value;
                if (PlaceholderSet)
                {
                    this.ForeColor = placeholderTextColor;
                }
            }
        }

        public bool PlaceholderSet { get; private set; }

        /*Overridden properties*/
        public new Color ForeColor
        {
            get
            {
                if (this.PlaceholderSet)
                {
                    return this.storeForeColor;
                }
                else
                {
                    return base.ForeColor;
                }
            }
            set
            {
                if (this.PlaceholderSet)
                {
                        this.storeForeColor = value;
                }
                else
                {
                    base.ForeColor = value;
                }
            }
        }
        public new char PasswordChar
        {
            get
            {
                if (this.PlaceholderSet)
                {
                    return this.storePasswordChar;
                }
                else
                {
                    return base.PasswordChar;
                }
            }
            set
            {
                if (this.PlaceholderSet)
                {
                    this.storePasswordChar = value;
                }
                else
                {
                    base.PasswordChar = value;
                }
            }
        }
        public new String Text
        {
            get
            {
                if (this.PlaceholderSet)
                {
                    return string.Empty;
                }
                else
                {
                    return base.Text;
                }
            }
            set
            {
                base.Text = value;
                if (this.PlaceholderSet)
                {
                    CheckRemovePlaceholder();
                    base.Text = value;
                }
                else if(!this.Focused)
                {
                    CheckSetPlaceholder();
                }
            }
        }

        private bool useSystemPasswordChar = false;
        private char systemPasswordChar = '\0';
        private char storePasswordSetChar = '\0';
        public new bool UseSystemPasswordChar
        {
            get
            {
                return this.useSystemPasswordChar;
            }
            set
            {
                if (value)
                {
                    if (this.systemPasswordChar == '\0')
                    {
                        this.systemPasswordChar = new TextBox() { UseSystemPasswordChar = true }.PasswordChar;
                    }
                    this.storePasswordSetChar = this.PasswordChar;
                    this.PasswordChar = this.systemPasswordChar;
                }
                else
                {
                    this.systemPasswordChar = '\0';
                    this.PasswordChar = this.storePasswordSetChar;
                }
                this.useSystemPasswordChar = value;
            }
        }

        public PlaceholderTextbox()
            : base()
        {
            this.GotFocus += new EventHandler(PlaceholderTextbox_GotFocus);
            this.LostFocus += new EventHandler(PlaceholderTextbox_LostFocus);
            this.KeyDown += new KeyEventHandler(PlaceholderTextbox_KeyDown);
            this.MouseDown += new MouseEventHandler(PlaceholderTextbox_MouseDown);
        }

        private void PlaceholderTextbox_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.PlaceholderSet)
            {
                this.CheckRemovePlaceholder();
            }
        }

        private void PlaceholderTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.PlaceholderSet)
            {
                this.CheckRemovePlaceholder();
            }
        }

        public void Init()
        {
            CheckSetPlaceholder();
        }

        private void PlaceholderTextbox_LostFocus(object sender, EventArgs e)
        {
            CheckSetPlaceholder();
        }

        private void PlaceholderTextbox_GotFocus(object sender, EventArgs e)
        {
            CheckRemovePlaceholder();
        }

        private void CheckSetPlaceholder()
        {
            if (this.PlaceholderSet || !String.IsNullOrEmpty(base.Text)) return;
            this.PlaceholderSet = true;
            base.Text = this.PlaceholderText;
            this.storeForeColor = base.ForeColor;
            this.storePasswordChar = base.PasswordChar;
            if (base.ForegroundFadeEnabled)
            {
                base.ForegroundFadeEnabled = false;
                base.ForeColor = base.BackColor;
                base.ForegroundFadeEnabled = true;
            }
            base.ForeColor = this.PlaceholderForeColor;
            base.PasswordChar = '\0';
        }

        private void CheckRemovePlaceholder()
        {
            if (!this.PlaceholderSet) return;
            this.PlaceholderSet = false;
            base.Text = "";
            base.ForeColor = storeForeColor;
            base.PasswordChar = storePasswordChar;
        }
    }
}
