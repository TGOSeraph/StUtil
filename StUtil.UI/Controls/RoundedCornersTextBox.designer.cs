namespace StUtil.UI.Controls
{
    partial class TextBoxRoundedCorners<T>
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
            this.internalTb = new T();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.internalTb.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.internalTb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.internalTb.Multiline = true;
            this.internalTb.Name = "internalTb";
            this.internalTb.TabIndex = 0;
            // 
            // TextBoxRoundedCorners
            // 
            this.Controls.Add(this.internalTb);
            this.Name = "TextBoxRoundedCorners";
            this.Size = new System.Drawing.Size(300, 35);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private T internalTb;
    }
}
