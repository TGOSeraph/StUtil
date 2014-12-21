namespace StUtil.Dev.WinForm
{
    partial class DevForm
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
            this.pnlHighlight = new System.Windows.Forms.Panel();
            this.pnlBorder = new System.Windows.Forms.Panel();
            this.pnlBgBorder = new System.Windows.Forms.Panel();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlBgBorder.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHighlight
            // 
            this.pnlHighlight.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHighlight.Location = new System.Drawing.Point(0, 0);
            this.pnlHighlight.Name = "pnlHighlight";
            this.pnlHighlight.Size = new System.Drawing.Size(641, 74);
            this.pnlHighlight.TabIndex = 0;
            // 
            // pnlBorder
            // 
            this.pnlBorder.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBorder.Location = new System.Drawing.Point(0, 74);
            this.pnlBorder.Name = "pnlBorder";
            this.pnlBorder.Size = new System.Drawing.Size(641, 1);
            this.pnlBorder.TabIndex = 1;
            // 
            // pnlBgBorder
            // 
            this.pnlBgBorder.Controls.Add(this.pnlContent);
            this.pnlBgBorder.Location = new System.Drawing.Point(12, 81);
            this.pnlBgBorder.Name = "pnlBgBorder";
            this.pnlBgBorder.Padding = new System.Windows.Forms.Padding(2);
            this.pnlBgBorder.Size = new System.Drawing.Size(267, 276);
            this.pnlBgBorder.TabIndex = 2;
            // 
            // pnlContent
            // 
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(2, 2);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Padding = new System.Windows.Forms.Padding(2);
            this.pnlContent.Size = new System.Drawing.Size(263, 272);
            this.pnlContent.TabIndex = 3;
            // 
            // DevForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(641, 369);
            this.Controls.Add(this.pnlBgBorder);
            this.Controls.Add(this.pnlBorder);
            this.Controls.Add(this.pnlHighlight);
            this.Name = "DevForm";
            this.Text = "Form1";
            this.pnlBgBorder.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHighlight;
        private System.Windows.Forms.Panel pnlBorder;
        private System.Windows.Forms.Panel pnlBgBorder;
        private System.Windows.Forms.Panel pnlContent;


    }
}

