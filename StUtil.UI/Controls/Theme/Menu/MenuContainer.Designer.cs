namespace StUtil.UI.Controls.Theme.Menu
{
    partial class MenuContainer
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
            this.tdpTasks = new StUtil.UI.Controls.TopDockPanel();
            this.themePanel3 = new StUtil.UI.Controls.Theme.ThemePanel();
            this.Header = new StUtil.UI.Controls.Theme.Menu.MenuHeaderControl();
            this.themePanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tdpTasks
            // 
            this.tdpTasks.AutoDockControls = true;
            this.tdpTasks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tdpTasks.Location = new System.Drawing.Point(0, 42);
            this.tdpTasks.Name = "tdpTasks";
            this.tdpTasks.Size = new System.Drawing.Size(413, 355);
            this.tdpTasks.TabIndex = 6;
            // 
            // themePanel3
            // 
            this.themePanel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(41)))));
            this.themePanel3.Controls.Add(this.Header);
            this.themePanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.themePanel3.ForeColor = System.Drawing.Color.White;
            this.themePanel3.Location = new System.Drawing.Point(0, 0);
            this.themePanel3.MainColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(41)))));
            this.themePanel3.Name = "themePanel3";
            this.themePanel3.SecondaryColor = System.Drawing.Color.White;
            this.themePanel3.Size = new System.Drawing.Size(413, 42);
            this.themePanel3.Style = StUtil.UI.Controls.Theme.ThemeManager.Style.Darker;
            this.themePanel3.TabIndex = 5;
            // 
            // Header
            // 
            this.Header.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Header.Location = new System.Drawing.Point(0, 0);
            this.Header.Name = "Header";
            this.Header.Size = new System.Drawing.Size(413, 42);
            this.Header.TabIndex = 0;
            // 
            // MenuContainer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tdpTasks);
            this.Controls.Add(this.themePanel3);
            this.Name = "MenuContainer";
            this.Size = new System.Drawing.Size(413, 397);
            this.themePanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private StUtil.UI.Controls.TopDockPanel tdpTasks;
        private Theme.ThemePanel themePanel3;
        private MenuHeaderControl Header;
    }
}
