using StUtil.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.UI.Controls
{
    public partial class EditableLabel : UserControl
    {
        public new event EventHandler TextChanged;

        private bool isEditable;

        private Point textBoxShift = Point.Empty;

        [DefaultValue(true)]
        public bool DoubleClickToEdit { get; set; }

        [DefaultValue(true)]
        public bool EnterAccepts { get; set; }

        public bool IsEditable
        {
            get { return isEditable; }
            set
            {
                if (value != isEditable)
                {
                    isEditable = value;
                    UpdateEditable();
                }
            }
        }

        [DefaultValue(true)]
        public bool StopEditOnFocusLost { get; set; }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get
            {
                return EditTextBox.Text;
            }
            set
            {
                if (EditTextBox.Text != value || EditLabel.Text != value)
                {
                    EditTextBox.Text = value;
                    EditLabel.Text = value;
                    TextChanged.RaiseEvent(this);
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Point TextBoxShift
        {
            get
            {
                return textBoxShift;
            }
            set
            {
                EditTextBox.Top -= textBoxShift.Y;
                EditTextBox.Left -= textBoxShift.X;
                textBoxShift = value;
                EditTextBox.Top += textBoxShift.Y;
                EditTextBox.Left += textBoxShift.X;
            }
        }

        public EditableLabel()
        {
            EnterAccepts = true;
            DoubleClickToEdit = true;
            StopEditOnFocusLost = true;

            InitializeComponent();
            UpdateEditable();

            EditTextBox.LostFocus += EditTextBox_LostFocus;
            EditLabel.DoubleClick += EditLabel_DoubleClick;
            EditTextBox.KeyDown += EditTextBox_KeyDown;
        }

        private void EditLabel_DoubleClick(object sender, EventArgs e)
        {
            if (DoubleClickToEdit)
            {
                IsEditable = true;
                EditTextBox.Focus();
                EditTextBox.SelectionStart = EditTextBox.GetCharIndexFromPosition(EditLabel.PointToClient(Cursor.Position));
                EditTextBox.SelectionLength = 0;
            }
        }

        private void EditTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (EnterAccepts)
            {
                if (e.KeyCode == Keys.Return)
                {
                    IsEditable = false;
                }
            }
        }

        private void EditTextBox_LostFocus(object sender, EventArgs e)
        {
            if (StopEditOnFocusLost)
            {
                IsEditable = false;
            }
        }

        private void UpdateEditable()
        {
            Text = EditTextBox.Text;

            EditLabel.Visible = !isEditable;
            EditTextBox.Visible = isEditable;
        }
    }
}
