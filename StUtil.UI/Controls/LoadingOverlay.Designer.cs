namespace StUtil.UI.Controls
{
    partial class LoadingOverlay
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
            this.pbImage = new System.Windows.Forms.PictureBox();
            this.lblText = new System.Windows.Forms.Label();
            this.llblCancel = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).BeginInit();
            this.SuspendLayout();
            // 
            // pbImage
            // 
            this.pbImage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pbImage.Image = global::StUtil.UI.Properties.Resources.loading;
            this.pbImage.Location = new System.Drawing.Point(92, 97);
            this.pbImage.Name = "pbImage";
            this.pbImage.Size = new System.Drawing.Size(16, 16);
            this.pbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbImage.TabIndex = 0;
            this.pbImage.TabStop = false;
            // 
            // lblText
            // 
            this.lblText.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblText.Location = new System.Drawing.Point(-37, 121);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(274, 23);
            this.lblText.TabIndex = 1;
            this.lblText.Text = "Loading...";
            this.lblText.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // llblCancel
            // 
            this.llblCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.llblCancel.AutoSize = true;
            this.llblCancel.Location = new System.Drawing.Point(72, 144);
            this.llblCancel.Name = "llblCancel";
            this.llblCancel.Size = new System.Drawing.Size(40, 13);
            this.llblCancel.TabIndex = 2;
            this.llblCancel.TabStop = true;
            this.llblCancel.Text = "Cancel";
            this.llblCancel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblCancel_LinkClicked);
            // 
            // LoadingOverlay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.llblCancel);
            this.Controls.Add(this.lblText);
            this.Controls.Add(this.pbImage);
            this.Name = "LoadingOverlay";
            this.Size = new System.Drawing.Size(200, 210);
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbImage;
        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.LinkLabel llblCancel;
    }
}
