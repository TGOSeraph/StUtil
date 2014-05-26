namespace StUtil.UI.Forms
{
    partial class ThemedForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.MainContent = new StUtil.UI.Controls.Theme.ThemePanel();
            this.TaskTitlePanel = new StUtil.UI.Controls.Theme.ThemePanel();
            this.TaskTitle = new StUtil.UI.Controls.Theme.ThemeLabel();
            this.themePanel2 = new StUtil.UI.Controls.Theme.ThemePanel();
            this.Menu = new StUtil.UI.Controls.Theme.Menu.MenuContainer();
            this.DescriptionPanel = new StUtil.UI.Controls.Theme.ThemePanel();
            this.pnlHeader = new StUtil.UI.Controls.Theme.ThemePanel();
            this.SettingsButton = new StUtil.UI.Controls.UISymbolLabel();
            this.themeLabel2 = new StUtil.UI.Controls.Theme.ThemeLabel();
            this.themeLabel1 = new StUtil.UI.Controls.Theme.ThemeLabel();
            this.pnlLogoMain = new StUtil.UI.Controls.Theme.ThemePanel();
            this.pnlLogoPart2 = new StUtil.UI.Controls.Theme.ThemePanel();
            this.pnlLogoPart1 = new StUtil.UI.Controls.Theme.ThemePanel();
            this.TaskTitlePanel.SuspendLayout();
            this.themePanel2.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.pnlLogoMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainContent
            // 
            this.MainContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(243)))));
            this.MainContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainContent.ForeColor = System.Drawing.Color.Black;
            this.MainContent.Location = new System.Drawing.Point(258, 90);
            this.MainContent.MainColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(243)))));
            this.MainContent.Name = "MainContent";
            this.MainContent.Padding = new System.Windows.Forms.Padding(10);
            this.MainContent.SecondaryColor = System.Drawing.Color.Black;
            this.MainContent.Size = new System.Drawing.Size(794, 427);
            this.MainContent.Style = StUtil.UI.Controls.Theme.ThemeManager.Style.Main;
            this.MainContent.TabIndex = 8;
            // 
            // TaskTitlePanel
            // 
            this.TaskTitlePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(123)))), ((int)(((byte)(71)))));
            this.TaskTitlePanel.Controls.Add(this.TaskTitle);
            this.TaskTitlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TaskTitlePanel.ForeColor = System.Drawing.Color.Black;
            this.TaskTitlePanel.Location = new System.Drawing.Point(258, 48);
            this.TaskTitlePanel.MainColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(123)))), ((int)(((byte)(71)))));
            this.TaskTitlePanel.Name = "TaskTitlePanel";
            this.TaskTitlePanel.SecondaryColor = System.Drawing.Color.Black;
            this.TaskTitlePanel.Size = new System.Drawing.Size(794, 42);
            this.TaskTitlePanel.Style = StUtil.UI.Controls.Theme.ThemeManager.Style.Highlight;
            this.TaskTitlePanel.TabIndex = 7;
            // 
            // TaskTitle
            // 
            this.TaskTitle.AutoSize = true;
            this.TaskTitle.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.TaskTitle.ForeColor = System.Drawing.Color.Black;
            this.TaskTitle.Location = new System.Drawing.Point(14, 11);
            this.TaskTitle.MainColor = System.Drawing.Color.Black;
            this.TaskTitle.Name = "TaskTitle";
            this.TaskTitle.SecondaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(123)))), ((int)(((byte)(71)))));
            this.TaskTitle.Size = new System.Drawing.Size(100, 21);
            this.TaskTitle.Style = StUtil.UI.Controls.Theme.ThemeManager.Style.Text;
            this.TaskTitle.TabIndex = 0;
            this.TaskTitle.Text = "themeLabel3";
            // 
            // themePanel2
            // 
            this.themePanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(52)))));
            this.themePanel2.Controls.Add(this.Menu);
            this.themePanel2.Controls.Add(this.DescriptionPanel);
            this.themePanel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.themePanel2.ForeColor = System.Drawing.Color.White;
            this.themePanel2.Location = new System.Drawing.Point(0, 48);
            this.themePanel2.MainColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(46)))), ((int)(((byte)(52)))));
            this.themePanel2.Name = "themePanel2";
            this.themePanel2.SecondaryColor = System.Drawing.Color.White;
            this.themePanel2.Size = new System.Drawing.Size(258, 469);
            this.themePanel2.Style = StUtil.UI.Controls.Theme.ThemeManager.Style.Dark;
            this.themePanel2.TabIndex = 6;
            // 
            // Menu
            // 
            this.Menu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Menu.HighlightedItem = null;
            this.Menu.Location = new System.Drawing.Point(0, 0);
            this.Menu.Name = "Menu";
            this.Menu.Root = null;
            this.Menu.Size = new System.Drawing.Size(258, 264);
            this.Menu.TabIndex = 5;
            // 
            // DescriptionPanel
            // 
            this.DescriptionPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(41)))));
            this.DescriptionPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.DescriptionPanel.ForeColor = System.Drawing.Color.White;
            this.DescriptionPanel.Location = new System.Drawing.Point(0, 264);
            this.DescriptionPanel.MainColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(41)))));
            this.DescriptionPanel.Name = "DescriptionPanel";
            this.DescriptionPanel.SecondaryColor = System.Drawing.Color.White;
            this.DescriptionPanel.Size = new System.Drawing.Size(258, 205);
            this.DescriptionPanel.Style = StUtil.UI.Controls.Theme.ThemeManager.Style.Darker;
            this.DescriptionPanel.TabIndex = 3;
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(19)))), ((int)(((byte)(22)))));
            this.pnlHeader.Controls.Add(this.SettingsButton);
            this.pnlHeader.Controls.Add(this.themeLabel2);
            this.pnlHeader.Controls.Add(this.themeLabel1);
            this.pnlHeader.Controls.Add(this.pnlLogoMain);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.ForeColor = System.Drawing.Color.White;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.MainColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(19)))), ((int)(((byte)(22)))));
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.SecondaryColor = System.Drawing.Color.White;
            this.pnlHeader.Size = new System.Drawing.Size(1052, 48);
            this.pnlHeader.Style = StUtil.UI.Controls.Theme.ThemeManager.Style.Darkest;
            this.pnlHeader.TabIndex = 5;
            // 
            // SettingsButton
            // 
            this.SettingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SettingsButton.Bold = false;
            this.SettingsButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SettingsButton.Font = new System.Drawing.Font("Segoe UI Symbol", 14F);
            this.SettingsButton.FontSize = 14F;
            this.SettingsButton.ForeColor = System.Drawing.Color.White;
            this.SettingsButton.Location = new System.Drawing.Point(1013, 10);
            this.SettingsButton.MainColor = System.Drawing.Color.White;
            this.SettingsButton.Name = "SettingsButton";
            this.SettingsButton.SecondaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(19)))), ((int)(((byte)(22)))));
            this.SettingsButton.Size = new System.Drawing.Size(27, 24);
            this.SettingsButton.Style = StUtil.UI.Controls.Theme.ThemeManager.Style.White;
            this.SettingsButton.Symbol = StUtil.UI.Utilities.UISymbol.Settings;
            this.SettingsButton.TabIndex = 0;
            this.SettingsButton.Text = "";
            this.SettingsButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // themeLabel2
            // 
            this.themeLabel2.AutoSize = true;
            this.themeLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(188)))), ((int)(((byte)(196)))));
            this.themeLabel2.Location = new System.Drawing.Point(202, 19);
            this.themeLabel2.MainColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(188)))), ((int)(((byte)(196)))));
            this.themeLabel2.Name = "themeLabel2";
            this.themeLabel2.SecondaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(19)))), ((int)(((byte)(22)))));
            this.themeLabel2.Size = new System.Drawing.Size(41, 13);
            this.themeLabel2.Style = StUtil.UI.Controls.Theme.ThemeManager.Style.Lightest;
            this.themeLabel2.TabIndex = 5;
            this.themeLabel2.Text = "version";
            // 
            // themeLabel1
            // 
            this.themeLabel1.AutoSize = true;
            this.themeLabel1.Font = new System.Drawing.Font("Segoe UI", 12.25F, System.Drawing.FontStyle.Bold);
            this.themeLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(123)))), ((int)(((byte)(71)))));
            this.themeLabel1.Location = new System.Drawing.Point(41, 13);
            this.themeLabel1.MainColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(123)))), ((int)(((byte)(71)))));
            this.themeLabel1.Name = "themeLabel1";
            this.themeLabel1.SecondaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(19)))), ((int)(((byte)(22)))));
            this.themeLabel1.Size = new System.Drawing.Size(155, 23);
            this.themeLabel1.Style = StUtil.UI.Controls.Theme.ThemeManager.Style.Highlight;
            this.themeLabel1.TabIndex = 0;
            this.themeLabel1.Text = "Application Name";
            // 
            // pnlLogoMain
            // 
            this.pnlLogoMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(123)))), ((int)(((byte)(71)))));
            this.pnlLogoMain.Controls.Add(this.pnlLogoPart2);
            this.pnlLogoMain.Controls.Add(this.pnlLogoPart1);
            this.pnlLogoMain.ForeColor = System.Drawing.Color.Black;
            this.pnlLogoMain.Location = new System.Drawing.Point(9, 9);
            this.pnlLogoMain.MainColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(123)))), ((int)(((byte)(71)))));
            this.pnlLogoMain.Name = "pnlLogoMain";
            this.pnlLogoMain.SecondaryColor = System.Drawing.Color.Black;
            this.pnlLogoMain.Size = new System.Drawing.Size(30, 30);
            this.pnlLogoMain.Style = StUtil.UI.Controls.Theme.ThemeManager.Style.Highlight;
            this.pnlLogoMain.TabIndex = 4;
            // 
            // pnlLogoPart2
            // 
            this.pnlLogoPart2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(41)))));
            this.pnlLogoPart2.ForeColor = System.Drawing.Color.White;
            this.pnlLogoPart2.Location = new System.Drawing.Point(23, 23);
            this.pnlLogoPart2.MainColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(41)))));
            this.pnlLogoPart2.Name = "pnlLogoPart2";
            this.pnlLogoPart2.SecondaryColor = System.Drawing.Color.White;
            this.pnlLogoPart2.Size = new System.Drawing.Size(4, 4);
            this.pnlLogoPart2.Style = StUtil.UI.Controls.Theme.ThemeManager.Style.Darker;
            this.pnlLogoPart2.TabIndex = 9;
            // 
            // pnlLogoPart1
            // 
            this.pnlLogoPart1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(41)))));
            this.pnlLogoPart1.ForeColor = System.Drawing.Color.White;
            this.pnlLogoPart1.Location = new System.Drawing.Point(23, 18);
            this.pnlLogoPart1.MainColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(41)))));
            this.pnlLogoPart1.Name = "pnlLogoPart1";
            this.pnlLogoPart1.SecondaryColor = System.Drawing.Color.White;
            this.pnlLogoPart1.Size = new System.Drawing.Size(4, 4);
            this.pnlLogoPart1.Style = StUtil.UI.Controls.Theme.ThemeManager.Style.Darker;
            this.pnlLogoPart1.TabIndex = 8;
            // 
            // ThemedForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1052, 517);
            this.Controls.Add(this.MainContent);
            this.Controls.Add(this.TaskTitlePanel);
            this.Controls.Add(this.themePanel2);
            this.Controls.Add(this.pnlHeader);
            this.Name = "ThemedForm";
            this.Text = "ThemedForm";
            this.TaskTitlePanel.ResumeLayout(false);
            this.TaskTitlePanel.PerformLayout();
            this.themePanel2.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlLogoMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public Controls.Theme.ThemePanel MainContent;
        public Controls.Theme.ThemePanel TaskTitlePanel;
        public Controls.Theme.ThemeLabel TaskTitle;
        public Controls.Theme.ThemePanel themePanel2;
        public Controls.Theme.Menu.MenuContainer Menu;
        public Controls.Theme.ThemePanel DescriptionPanel;
        public Controls.Theme.ThemePanel pnlHeader;
        public Controls.UISymbolLabel SettingsButton;
        public Controls.Theme.ThemeLabel themeLabel2;
        public Controls.Theme.ThemeLabel themeLabel1;
        public Controls.Theme.ThemePanel pnlLogoMain;
        public Controls.Theme.ThemePanel pnlLogoPart2;
        public Controls.Theme.ThemePanel pnlLogoPart1;
    }
}