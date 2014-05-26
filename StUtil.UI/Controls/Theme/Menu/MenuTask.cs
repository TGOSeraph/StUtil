using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StUtil.Extensions;

namespace StUtil.UI.Controls.Theme.Menu
{
    public abstract class MenuTask
    {
        public event EventHandler TitleChanged;

        public abstract Control DescriptionControl { get; }
        public abstract Control MainControl { get; }
        public abstract string Title { get; }
        public Panel TitleControl { get; set; }
        public MenuItem MenuItem { get; internal set; }

        public abstract void Activated();
        public abstract void Deactivated();

        protected void OnTitleChanged()
        {
            TitleChanged.RaiseEvent(this);
        }
    }
}
