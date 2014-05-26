namespace StUtil.UI.Controls.Theme.Menu
{
    partial class MenuHeaderControl
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
            this.PathLabel = new StUtil.UI.Controls.Theme.ThemeLabel();
            this.ArrowLabel = new StUtil.UI.Controls.Theme.ThemeLabel();
            this.TitleLabel = new StUtil.UI.Controls.Theme.ThemeLabel();
            this.SuspendLayout();
            // 
            // PathLabel
            // 
            this.PathLabel.AutoSize = true;
            this.PathLabel.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PathLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(94)))), ((int)(((byte)(106)))));
            this.PathLabel.Location = new System.Drawing.Point(85, 16);
            this.PathLabel.MainColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(94)))), ((int)(((byte)(106)))));
            this.PathLabel.Name = "PathLabel";
            this.PathLabel.SecondaryColor = System.Drawing.SystemColors.Control;
            this.PathLabel.Size = new System.Drawing.Size(70, 13);
            this.PathLabel.Style = StUtil.UI.Controls.Theme.ThemeManager.Style.LightDark;
            this.PathLabel.TabIndex = 4;
            this.PathLabel.Text = "themeLabel1";
            // 
            // ArrowLabel
            // 
            this.ArrowLabel.AutoSize = true;
            this.ArrowLabel.Font = new System.Drawing.Font("Segoe UI Symbol", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ArrowLabel.ForeColor = System.Drawing.Color.White;
            this.ArrowLabel.Location = new System.Drawing.Point(5, -1);
            this.ArrowLabel.MainColor = System.Drawing.Color.White;
            this.ArrowLabel.Name = "ArrowLabel";
            this.ArrowLabel.SecondaryColor = System.Drawing.SystemColors.Control;
            this.ArrowLabel.Size = new System.Drawing.Size(26, 37);
            this.ArrowLabel.Style = StUtil.UI.Controls.Theme.ThemeManager.Style.White;
            this.ArrowLabel.TabIndex = 3;
            this.ArrowLabel.Text = "";
            // 
            // TitleLabel
            // 
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleLabel.ForeColor = System.Drawing.Color.White;
            this.TitleLabel.Location = new System.Drawing.Point(13, 11);
            this.TitleLabel.MainColor = System.Drawing.Color.White;
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.SecondaryColor = System.Drawing.SystemColors.Control;
            this.TitleLabel.Size = new System.Drawing.Size(47, 20);
            this.TitleLabel.Style = StUtil.UI.Controls.Theme.ThemeManager.Style.White;
            this.TitleLabel.TabIndex = 2;
            this.TitleLabel.Text = "Tasks";
            // 
            // MenuHeaderControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PathLabel);
            this.Controls.Add(this.ArrowLabel);
            this.Controls.Add(this.TitleLabel);
            this.Name = "MenuHeaderControl";
            this.Size = new System.Drawing.Size(413, 42);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Theme.ThemeLabel TitleLabel;
        private Theme.ThemeLabel ArrowLabel;
        private Theme.ThemeLabel PathLabel;
    }
}
