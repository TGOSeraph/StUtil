using System;
using System.Collections.Generic;
using System.Drawing;

namespace StUtil.UI.Controls
{
    public class FormTextBox : PlaceholderTextbox
    { 
        private Color? storeBackColor = null;
        private Color? storePlaceholderForeColor = null;
        private Color? storeForeColor = null;
        private string storePlaceholderText = null;
        private string storeText = null;

        public Color InvalidBackColor { get; set; }
        public Color InvalidForeColor { get; set; }

        public bool Required { get; set; }
        public bool Numeric { get; set; }
        public int MinLength { get; set; }
        public HashSet<char> AllowedCharacters { get; set; }
        public HashSet<char> DisallowedCharacters { get; set; }

        public Func<string, bool> ValidationFunction { get; private set; }

        public FormTextBox()
        {
            this.MinLength = -1;
            this.GotFocus += new EventHandler(RequiredFormTextBox_GotFocus);
        }

        private void RequiredFormTextBox_GotFocus(object sender, EventArgs e)
        {
            if (this.storeBackColor.HasValue)
            {
                this.BackColor = this.storeBackColor.Value;
                this.storeBackColor = null;
            }
            if (this.storeForeColor.HasValue)
            {
                this.ForeColor = this.storeForeColor.Value;
                this.storeForeColor = null;
            }
            if (this.storePlaceholderForeColor.HasValue)
            {
                this.PlaceholderForeColor = this.storePlaceholderForeColor.Value;
                this.storePlaceholderForeColor = null;
            }
            if (this.storePlaceholderText != null)
            {
                this.PlaceholderText = this.storePlaceholderText;
                this.storePlaceholderText = null;
            }
            if (this.storeText != null)
            {
                this.Text = this.storeText;
                this.storeText = null;
            }
        }

        public bool IsValid()
        {
            bool valid = true;
            this.storeText = this.Text;
            this.storePlaceholderText = this.PlaceholderText;

            if (this.Required)
            {
                if (this.Text == string.Empty)
                {
                    valid = false;
                    this.PlaceholderText = "This field is required";
                }
            }
            if (valid && this.Numeric)
            {
                foreach (char c in this.Text)
                {
                    if (!char.IsNumber(c))
                    {
                        valid = false;
                        this.PlaceholderText = "This field must be numeric";
                        this.Text = "";
                        break;
                    }
                }
            }
            if (valid && this.MinLength > 0)
            {
                if (this.MinLength > this.Text.Length)
                {
                    valid = false;
                    this.PlaceholderText = "This field must be at least " + this.MinLength.ToString() + " characters";
                    this.Text = "";
                }
            }
            if (valid && this.AllowedCharacters != null)
            {
                foreach (char c in this.Text)
                {
                    if (!AllowedCharacters.Contains(c))
                    {
                        valid = false;
                        this.PlaceholderText = "Entered value contains invalid characters";
                        this.Text = "";
                        break;
                    }
                }
            }
            if (valid && this.DisallowedCharacters != null)
            {
                foreach (char c in this.Text)
                {
                    if (DisallowedCharacters.Contains(c))
                    {
                        valid = false;
                        this.PlaceholderText = "Entered value contains invalid characters";
                        this.Text = "";
                        break;
                    }
                }
            }
            if (valid && this.ValidationFunction != null)
            {
                if (!ValidationFunction(this.Text))
                {
                    valid = false;
                    this.PlaceholderText = "Invalid";
                    this.Text = "";
                }
            }
            if (!valid)
            {
                this.storeBackColor = base.BackColor;
                base.BackColor = InvalidBackColor;

                this.storeForeColor = base.ForeColor;
                this.ForeColor = InvalidForeColor;

                this.storePlaceholderForeColor = base.PlaceholderForeColor;
                this.PlaceholderForeColor = InvalidForeColor;
            }
            else
            {
                this.storeText = null;
                this.storePlaceholderText = null;
            }
            return valid;
        }
    }
}
