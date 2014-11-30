namespace StUtil.Dev.WinForm.Controls
{
    partial class TaskWorkerItem
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
            this.components = new System.ComponentModel.Container();
            this.TaskName = new System.Windows.Forms.Label();
            this.TaskIcon = new System.Windows.Forms.PictureBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.TaskIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // TaskName
            // 
            this.TaskName.AutoSize = true;
            this.TaskName.Location = new System.Drawing.Point(30, 7);
            this.TaskName.Name = "TaskName";
            this.TaskName.Size = new System.Drawing.Size(35, 13);
            this.TaskName.TabIndex = 1;
            this.TaskName.Text = "label1";
            // 
            // TaskIcon
            // 
            this.TaskIcon.Location = new System.Drawing.Point(4, 3);
            this.TaskIcon.Name = "TaskIcon";
            this.TaskIcon.Size = new System.Drawing.Size(20, 20);
            this.TaskIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.TaskIcon.TabIndex = 0;
            this.TaskIcon.TabStop = false;
            // 
            // TaskWorkerItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TaskName);
            this.Controls.Add(this.TaskIcon);
            this.Name = "TaskWorkerItem";
            this.Size = new System.Drawing.Size(114, 27);
            ((System.ComponentModel.ISupportInitialize)(this.TaskIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox TaskIcon;
        private System.Windows.Forms.Label TaskName;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
