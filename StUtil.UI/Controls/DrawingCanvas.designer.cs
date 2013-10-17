namespace StUtil.UI.Controls
{
    partial class DrawingCanvas
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
            this.pbBackground = new System.Windows.Forms.PictureBox();
            this.pbShapes = new System.Windows.Forms.PictureBox();
            this.pbDrawing = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbBackground)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbShapes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDrawing)).BeginInit();
            this.SuspendLayout();
            // 
            // pbBackground
            // 
            this.pbBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbBackground.Location = new System.Drawing.Point(0, 0);
            this.pbBackground.Name = "pbBackground";
            this.pbBackground.Size = new System.Drawing.Size(1002, 678);
            this.pbBackground.TabIndex = 0;
            this.pbBackground.TabStop = false;
            // 
            // pbShapes
            // 
            this.pbShapes.BackColor = System.Drawing.Color.Transparent;
            this.pbShapes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbShapes.Location = new System.Drawing.Point(0, 0);
            this.pbShapes.Name = "pbShapes";
            this.pbShapes.Size = new System.Drawing.Size(1002, 678);
            this.pbShapes.TabIndex = 1;
            this.pbShapes.TabStop = false;
            // 
            // pbDrawing
            // 
            this.pbDrawing.BackColor = System.Drawing.Color.Transparent;
            this.pbDrawing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbDrawing.Location = new System.Drawing.Point(0, 0);
            this.pbDrawing.Name = "pbDrawing";
            this.pbDrawing.Size = new System.Drawing.Size(1002, 678);
            this.pbDrawing.TabIndex = 2;
            this.pbDrawing.TabStop = false;
            // 
            // DrawingCanvas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pbDrawing);
            this.Controls.Add(this.pbShapes);
            this.Controls.Add(this.pbBackground);
            this.Name = "DrawingCanvas";
            this.Size = new System.Drawing.Size(1002, 678);
            ((System.ComponentModel.ISupportInitialize)(this.pbBackground)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbShapes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDrawing)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbBackground;
        private System.Windows.Forms.PictureBox pbShapes;
        private System.Windows.Forms.PictureBox pbDrawing;
    }
}
