using StUtil.UI.Utilities;
namespace StUtil.UI.Controls.Theme.Menu
{
    partial class MenuItemControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.themePanel1 = new StUtil.UI.Controls.Theme.ThemePanel();
            this.TitleLabel = new StUtil.UI.Controls.Theme.ThemeLabel();
            this.IconLabel = new UISymbolLabel();
            this.ArrowLabel = new UISymbolLabel();
            this.SuspendLayout();
            // 
            // themePanel1
            // 
            this.themePanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(53)))), ((int)(((byte)(60)))));
            this.themePanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.themePanel1.ForeColor = System.Drawing.Color.White;
            this.themePanel1.Location = new System.Drawing.Point(0, 36);
            this.themePanel1.MainColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(53)))), ((int)(((byte)(60)))));
            this.themePanel1.Name = "themePanel1";
            this.themePanel1.SecondaryColor = System.Drawing.Color.White;
            this.themePanel1.Size = new System.Drawing.Size(491, 1);
            this.themePanel1.Style = StUtil.UI.Controls.Theme.ThemeManager.Style.MediumDark;
            this.themePanel1.TabIndex = 4;
            // 
            // TitleLabel
            // 
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(151)))));
            this.TitleLabel.Location = new System.Drawing.Point(38, 10);
            this.TitleLabel.MainColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(151)))));
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.SecondaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(52)))));
            this.TitleLabel.Size = new System.Drawing.Size(44, 20);
            this.TitleLabel.Style = StUtil.UI.Controls.Theme.ThemeManager.Style.MediumLight;
            this.TitleLabel.TabIndex = 2;
            this.TitleLabel.Text = "Tasks";
            // 
            // IconLabel
            // 
            this.IconLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.IconLabel.AutoSize = true;
            this.IconLabel.Bold = true;
            this.IconLabel.Font = new System.Drawing.Font("Segoe UI Symbol", 14F, System.Drawing.FontStyle.Bold);
            this.IconLabel.FontSize = 14F;
            this.IconLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(151)))));
            this.IconLabel.Location = new System.Drawing.Point(1, 6);
            this.IconLabel.MainColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(151)))));
            this.IconLabel.Name = "IconLabel";
            this.IconLabel.SecondaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(52)))));
            this.IconLabel.Size = new System.Drawing.Size(36, 25);
            this.IconLabel.Style = StUtil.UI.Controls.Theme.ThemeManager.Style.MediumLight;
            this.IconLabel.Symbol = UISymbol.OpenLocalFolder;
            this.IconLabel.TabIndex = 3;
            this.IconLabel.Text = "";
            // 
            // ArrowLabel
            // 
            this.ArrowLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ArrowLabel.AutoSize = true;
            this.ArrowLabel.Bold = true;
            this.ArrowLabel.Font = new System.Drawing.Font("Segoe UI Symbol", 20F, System.Drawing.FontStyle.Bold);
            this.ArrowLabel.FontSize = 20F;
            this.ArrowLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(151)))));
            this.ArrowLabel.Location = new System.Drawing.Point(458, -4);
            this.ArrowLabel.MainColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(151)))));
            this.ArrowLabel.Name = "ArrowLabel";
            this.ArrowLabel.SecondaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(52)))));
            this.ArrowLabel.Size = new System.Drawing.Size(26, 37);
            this.ArrowLabel.Style = StUtil.UI.Controls.Theme.ThemeManager.Style.MediumLight;
            this.ArrowLabel.Symbol = UISymbol.ChevronRight;
            this.ArrowLabel.TabIndex = 1;
            this.ArrowLabel.Text = "";
            // 
            // MenuItemControl
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(52)))));
            this.Controls.Add(this.themePanel1);
            this.Controls.Add(this.TitleLabel);
            this.Controls.Add(this.IconLabel);
            this.Controls.Add(this.ArrowLabel);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.MainColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(52)))));
            this.Name = "TaskItem";
            this.Size = new System.Drawing.Size(491, 37);
            this.Style = StUtil.UI.Controls.Theme.ThemeManager.Style.Dark;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UISymbolLabel ArrowLabel;
        public Theme.ThemeLabel TitleLabel;
        public UISymbolLabel IconLabel;
        private Theme.ThemePanel themePanel1;
    }
}
