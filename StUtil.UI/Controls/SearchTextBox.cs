using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using StUtil.Generic;
using StUtil.Extensions;
using System.Drawing;
using StUtil.Reflection;

namespace StUtil.UI.Controls
{
    public class SearchBox : PlaceholderTextBoxRoundedCorners
    {
        public event EventHandler<CancelEventArgs> PopupOpening;
        public event EventHandler PopupShown;
        public event EventHandler<EventArgs<string>> DoSearch;

        private Panel contentPanel = new Panel();
        public PopupControl Popup { get; private set; }

        public Control SearchButton { get; private set; }
        public Control PopButton { get; private set; }

        public int SearchButtonWidth
        {
            get { return SearchButton == null ? -1 : SearchButton.Width; }
            set
            {
                if (SearchButton == null)
                {
                    return;
                }
                base.TextBoxControl.BackColor = Color.Red;
                base.TextBoxPadding = new Padding(this.TextBoxPadding.Left, this.TextBoxPadding.Top, value + this.TextBoxPadding.Left - 2, this.TextBoxPadding.Bottom);
                SearchButton.Left = this.TextBoxControl.Right + this.TextBoxPadding.Left;
            }
        }

        public Panel ContentPanel
        {
            get
            {
                return this.contentPanel;
            }
        }

        public override Padding TextBoxPadding
        {
            get
            {
                return base.TextBoxPadding;
            }
            set
            {
                base.TextBoxPadding = value;
                if (SearchButton != null)
                {
                    SearchButton.Width = TextBoxPadding.Right - this.TextBoxPadding.Left + 2;
                    SearchButton.Left = this.TextBoxControl.Right + this.TextBoxPadding.Left;
                }
            }
        }

        public SearchBox()
            : this(null, null)
        {
        }

        public SearchBox(Control searchBtn, Control popupButton)
        {
            Popup = new PopupControl(contentPanel);

            this.AutoSize = true;

            base.TextBoxPadding = new Padding(8, 4, 8, 4);

            SetButtons(searchBtn, popupButton);
            this.TextBoxControl.KeyDown += new KeyEventHandler(TextBoxControl_KeyDown);
        }

        private void TextBoxControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return || e.KeyCode == (Keys.LButton | Keys.MButton | Keys.Back))
            {
                e.Handled = true;
                ReflectionHelper helper = new ReflectionHelper(this.SearchButton);
                helper.GetMethod("InvokeOnClick").Invoke(this.SearchButton, new object[] { this.SearchButton, EventArgs.Empty });
            }
        }


        public void SetButtons(Control searchBtn, Control popupButton)
        {
            if (this.SearchButton != null)
                this.Controls.Remove(this.SearchButton);

            if (this.PopButton != null)
                this.Controls.Remove(this.PopButton);

            if (searchBtn != null)
            {
                SearchButton = searchBtn;
                SearchButton.Left = this.TextBoxControl.Right + 5;
                SearchButton.Width = this.Width - TextBoxControl.Right - 3 - this.BorderWidth;
                SearchButton.Top = TextBoxControl.Top - this.TextBoxPadding.Top + 1;
                SearchButton.Height = TextBoxControl.Height + this.TextBoxPadding.Vertical;
                SearchButton.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
                this.Controls.Add(SearchButton);
                searchBtn.Click += new EventHandler(searchBtn_Click);
            }

            if (popupButton != null)
            {
                PopButton = popupButton;
                PopButton.Left = base.BorderWidth - 1;
                PopButton.Width = TextBoxPadding.Left - 4;
                PopButton.Top = TextBoxControl.Top - this.TextBoxPadding.Top - 1;
                PopButton.Height = TextBoxControl.Height + this.TextBoxPadding.Vertical + 3;
                PopButton.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
                this.Controls.Add(PopButton);
                PopButton.Click += new EventHandler(popButton_Click);
            }
        }

        private void popButton_Click(object sender, EventArgs e)
        {
            CancelEventArgs cancel = new CancelEventArgs(false);
            PopupOpening.RaiseEvent(this, cancel);
            if (cancel.Cancel)
            {
                return;
            }
            Control b = (Control)sender;
            Popup.Size = this.ContentPanel.Size;
            Popup.Show(PointToScreen(new Point(b.Left, b.Bottom)));
            PopupShown.RaiseEvent(this);
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            DoSearch.RaiseEvent(this, TextBoxControl.Text);
        }
    }
}
