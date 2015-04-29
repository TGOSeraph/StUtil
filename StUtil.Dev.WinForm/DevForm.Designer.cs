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
            this.components = new System.ComponentModel.Container();
            StUtil.UI.Components.ObjectState.StateItem stateItem1 = new StUtil.UI.Components.ObjectState.StateItem();
            StUtil.UI.Components.ObjectState.StateEvent stateEvent1 = new StUtil.UI.Components.ObjectState.StateEvent();
            StUtil.UI.Components.ObjectState.StateEventProperties stateEventProperties1 = new StUtil.UI.Components.ObjectState.StateEventProperties();
            StUtil.UI.Components.ObjectState.StateEventProperty stateEventProperty1 = new StUtil.UI.Components.ObjectState.StateEventProperty();
            StUtil.UI.Components.ObjectState.StateEvent stateEvent2 = new StUtil.UI.Components.ObjectState.StateEvent();
            StUtil.UI.Components.ObjectState.StateEventProperties stateEventProperties2 = new StUtil.UI.Components.ObjectState.StateEventProperties();
            StUtil.UI.Components.ObjectState.StateEventProperty stateEventProperty2 = new StUtil.UI.Components.ObjectState.StateEventProperty();
            StUtil.UI.Components.ObjectState.StateEvent stateEvent3 = new StUtil.UI.Components.ObjectState.StateEvent();
            StUtil.UI.Components.ObjectState.StateEventProperties stateEventProperties3 = new StUtil.UI.Components.ObjectState.StateEventProperties();
            StUtil.UI.Components.ObjectState.StateEvent stateEvent4 = new StUtil.UI.Components.ObjectState.StateEvent();
            StUtil.UI.Components.ObjectState.StateEventProperties stateEventProperties4 = new StUtil.UI.Components.ObjectState.StateEventProperties();
            StUtil.UI.Components.ObjectState.StateEventProperty stateEventProperty3 = new StUtil.UI.Components.ObjectState.StateEventProperty();
            this.stateManager1 = new StUtil.UI.Components.ObjectState.StateManager(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.captionBarControl1 = new StUtil.UI.Controls.Theme.CaptionBarControl();
            ((System.ComponentModel.ISupportInitialize)(this.stateManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // stateManager1
            // 
            this.stateManager1.ContainerControl = this;
            stateEvent1.Event = "MouseEnter";
            stateEvent1.Parent = stateItem1;
            stateEventProperties1.Parent = stateEvent1;
            stateEventProperty1.Parent = stateEventProperties1;
            stateEventProperty1.PropertyName = "BackColor";
            stateEventProperty1.Value = System.Drawing.SystemColors.ActiveCaption;
            stateEventProperties1.Properties.Add(stateEventProperty1);
            stateEvent1.Properties = stateEventProperties1;
            stateEvent1.RequiredState = "Maximized";
            stateEvent1.StateList = new string[0];
            stateEvent2.Event = "MouseEnter";
            stateEvent2.Parent = stateItem1;
            stateEventProperties2.Parent = stateEvent2;
            stateEventProperty2.Parent = stateEventProperties2;
            stateEventProperty2.PropertyName = "BackColor";
            stateEventProperty2.Value = System.Drawing.Color.Red;
            stateEventProperties2.Properties.Add(stateEventProperty2);
            stateEvent2.Properties = stateEventProperties2;
            stateEvent2.RequiredState = "Normal";
            stateEvent2.StateList = null;
            stateEvent3.Event = "Click";
            stateEvent3.Parent = stateItem1;
            stateEventProperties3.Parent = stateEvent3;
            stateEvent3.Properties = stateEventProperties3;
            stateEvent3.RequiredState = null;
            stateEvent3.StateList = new string[] {
        "Maximized",
        "Normal"};
            stateEvent4.Event = "MouseEnter";
            stateEvent4.Parent = stateItem1;
            stateEventProperties4.Parent = stateEvent4;
            stateEventProperty3.Parent = stateEventProperties4;
            stateEventProperty3.PropertyName = "Text";
            stateEventProperty3.Value = "$(State)";
            stateEventProperties4.Properties.Add(stateEventProperty3);
            stateEvent4.Properties = stateEventProperties4;
            stateEvent4.RequiredState = null;
            stateEvent4.StateList = null;
            stateItem1.HandledEvents.Add(stateEvent1);
            stateItem1.HandledEvents.Add(stateEvent2);
            stateItem1.HandledEvents.Add(stateEvent3);
            stateItem1.HandledEvents.Add(stateEvent4);
            stateItem1.Parent = this.stateManager1;
            stateItem1.State = "Normal";
            stateItem1.Target = this.button1;
            this.stateManager1.States.Add(stateItem1);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(217, 251);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // captionBarControl1
            // 
            this.captionBarControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.captionBarControl1.Location = new System.Drawing.Point(0, 0);
            this.captionBarControl1.Name = "captionBarControl1";
            this.captionBarControl1.Size = new System.Drawing.Size(977, 32);
            this.captionBarControl1.TabIndex = 0;
            // 
            // DevForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(977, 1092);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.captionBarControl1);
            this.Name = "DevForm";
            ((System.ComponentModel.ISupportInitialize)(this.stateManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Controls.Theme.CaptionBarControl captionBarControl1;
        private UI.Components.ObjectState.StateManager stateManager1;
        private System.Windows.Forms.Button button1;



    }
}

