namespace StUtil.UI.Controls
{
    partial class EditableLabel
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
            this.EditLabel = new System.Windows.Forms.Label();
            this.EditTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // EditLabel
            // 
            this.EditLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EditLabel.AutoEllipsis = true;
            this.EditLabel.Location = new System.Drawing.Point(0, -1);
            this.EditLabel.Name = "EditLabel";
            this.EditLabel.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.EditLabel.Size = new System.Drawing.Size(272, 20);
            this.EditLabel.TabIndex = 0;
            this.EditLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // EditTextBox
            // 
            this.EditTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EditTextBox.Location = new System.Drawing.Point(3, 0);
            this.EditTextBox.Name = "EditTextBox";
            this.EditTextBox.Size = new System.Drawing.Size(266, 20);
            this.EditTextBox.TabIndex = 1;
            // 
            // EditableLabel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.EditLabel);
            this.Controls.Add(this.EditTextBox);
            this.Name = "EditableLabel";
            this.Size = new System.Drawing.Size(272, 20);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label EditLabel;
        public System.Windows.Forms.TextBox EditTextBox;
    }
}
