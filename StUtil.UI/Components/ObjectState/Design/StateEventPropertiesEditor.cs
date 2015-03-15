using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace StUtil.UI.Components.ObjectState.Design
{
    public class StateEventPropertiesEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService service = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            StateEventPropertiesForm editor = new StateEventPropertiesForm(((StateEvent)context.Instance).Properties);
            editor.Target = ((StateEvent)context.Instance).Parent.Target;
            if (service.ShowDialog(editor) == System.Windows.Forms.DialogResult.OK)
            {
                return editor.GetResults();
            }
            return value;
        }
    }
}
