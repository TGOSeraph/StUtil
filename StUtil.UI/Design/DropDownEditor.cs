using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace StUtil.UI.Design
{
    public abstract class DropDownEditor : UITypeEditor
    {
        private IWindowsFormsEditorService service;

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            service = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;

            ListBox lb = new ListBox();
            lb.SelectionMode = SelectionMode.One;
            lb.SelectedValueChanged += OnListBoxSelectedValueChanged;

            lb.Items.AddRange(GetItems(context.Instance));

            service.DropDownControl(lb);
            if (lb.SelectedItem == null)
            {
                return value;
            }

            return lb.SelectedItem;
        }

        private void OnListBoxSelectedValueChanged(object sender, EventArgs e)
        {
            service.CloseDropDown();
        }

        protected abstract object[] GetItems(object instance);

        public override bool IsDropDownResizable
        {
            get
            {
                return true;
            }
        }
    }
}
