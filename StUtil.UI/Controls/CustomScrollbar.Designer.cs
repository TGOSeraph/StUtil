namespace StUtil.UI.Controls
{
    partial class CustomScrollbar
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
            this.pnlArrowBottom = new System.Windows.Forms.Panel();
            this.pnlArrowTop = new System.Windows.Forms.Panel();
            this.pnlScrollArea = new System.Windows.Forms.Panel();
            this.pnlThumb = new System.Windows.Forms.Panel();
            this.pnlScrollArea.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlArrowBottom
            // 
            this.pnlArrowBottom.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pnlArrowBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlArrowBottom.Location = new System.Drawing.Point(0, 453);
            this.pnlArrowBottom.Name = "pnlArrowBottom";
            this.pnlArrowBottom.Size = new System.Drawing.Size(19, 20);
            this.pnlArrowBottom.TabIndex = 1;
            this.pnlArrowBottom.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlArrowBottom_MouseDown);
            this.pnlArrowBottom.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlArrowBottom_MouseUp);
            // 
            // pnlArrowTop
            // 
            this.pnlArrowTop.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pnlArrowTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlArrowTop.Location = new System.Drawing.Point(0, 0);
            this.pnlArrowTop.Name = "pnlArrowTop";
            this.pnlArrowTop.Size = new System.Drawing.Size(19, 20);
            this.pnlArrowTop.TabIndex = 2;
            this.pnlArrowTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlArrowTop_MouseDown);
            this.pnlArrowTop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlArrowTop_MouseUp);
            // 
            // pnlScrollArea
            // 
            this.pnlScrollArea.BackColor = System.Drawing.Color.Black;
            this.pnlScrollArea.Controls.Add(this.pnlThumb);
            this.pnlScrollArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlScrollArea.Location = new System.Drawing.Point(0, 20);
            this.pnlScrollArea.Name = "pnlScrollArea";
            this.pnlScrollArea.Size = new System.Drawing.Size(19, 433);
            this.pnlScrollArea.TabIndex = 3;
            this.pnlScrollArea.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnlScrollArea_MouseClick);
            // 
            // pnlThumb
            // 
            this.pnlThumb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlThumb.BackColor = System.Drawing.Color.Coral;
            this.pnlThumb.Location = new System.Drawing.Point(0, 166);
            this.pnlThumb.Name = "pnlThumb";
            this.pnlThumb.Size = new System.Drawing.Size(19, 100);
            this.pnlThumb.TabIndex = 1;
            this.pnlThumb.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlThumb_MouseDown);
            this.pnlThumb.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlThumb_MouseMove);
            // 
            // CustomScrollbar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlScrollArea);
            this.Controls.Add(this.pnlArrowBottom);
            this.Controls.Add(this.pnlArrowTop);
            this.Name = "CustomScrollbar";
            this.Size = new System.Drawing.Size(19, 473);
            this.pnlScrollArea.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlArrowBottom;
        private System.Windows.Forms.Panel pnlArrowTop;
        private System.Windows.Forms.Panel pnlScrollArea;
        private System.Windows.Forms.Panel pnlThumb;
    }
}
