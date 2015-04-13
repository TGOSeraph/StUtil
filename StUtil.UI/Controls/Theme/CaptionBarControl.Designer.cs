namespace StUtil.UI.Controls.Theme
{
    partial class CaptionBarControl
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
            StUtil.UI.Components.ObjectState.StateItem stateItem1 = new StUtil.UI.Components.ObjectState.StateItem();
            StUtil.UI.Components.ObjectState.StateEvent stateEvent1 = new StUtil.UI.Components.ObjectState.StateEvent();
            StUtil.UI.Components.ObjectState.StateEventProperties stateEventProperties1 = new StUtil.UI.Components.ObjectState.StateEventProperties();
            StUtil.UI.Components.ObjectState.StateEventProperty stateEventProperty1 = new StUtil.UI.Components.ObjectState.StateEventProperty();
            this.pbIcon = new System.Windows.Forms.PictureBox();
            this.lblText = new System.Windows.Forms.Label();
            this.stateManager1 = new StUtil.UI.Components.ObjectState.StateManager(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stateManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // pbIcon
            // 
            this.pbIcon.Location = new System.Drawing.Point(4, 3);
            this.pbIcon.Name = "pbIcon";
            this.pbIcon.Size = new System.Drawing.Size(26, 26);
            this.pbIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbIcon.TabIndex = 7;
            this.pbIcon.TabStop = false;
            this.pbIcon.Click += new System.EventHandler(this.pbIcon_Click);
            this.pbIcon.DoubleClick += new System.EventHandler(this.pbIcon_DoubleClick);
            // 
            // lblText
            // 
            this.lblText.AutoSize = true;
            this.lblText.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.lblText.Location = new System.Drawing.Point(36, 8);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(104, 17);
            this.lblText.TabIndex = 6;
            this.lblText.Text = "Window Caption";
            // 
            // stateManager1
            // 
            this.stateManager1.ContainerControl = this;
            stateEvent1.Event = "MouseEnter";
            stateEvent1.Parent = stateItem1;
            stateEventProperties1.Parent = stateEvent1;
            stateEventProperty1.Parent = stateEventProperties1;
            stateEventProperty1.PropertyName = "BackColor";
            stateEventProperty1.Value = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            stateEventProperties1.Properties.Add(stateEventProperty1);
            stateEvent1.Properties = stateEventProperties1;
            stateItem1.HandledEvents.Add(stateEvent1);
            stateItem1.Parent = this.stateManager1;
            this.stateManager1.States.Add(stateItem1);
            // 
            // CaptionBarControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pbIcon);
            this.Controls.Add(this.lblText);
            this.Name = "CaptionBarControl";
            this.Size = new System.Drawing.Size(792, 32);
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stateManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbIcon;
        private System.Windows.Forms.Label lblText;
        private Components.ObjectState.StateManager stateManager1;
    }
}
