using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.UI.Controls.Style
{
    public abstract class StylePart
    {
        public abstract Style Style
        {
            get;
        }

        public int Index { get; set; }
        public int Length { get; set; }

        public StylePart Parent { get; private set; }
        public BindingList<StylePart> Children { get; private set; }

        public StylePart()
        {
            Children = new BindingList<StylePart>();
            Children.ListChanged += Children_ListChanged;
        }

        private void Children_ListChanged(object sender, ListChangedEventArgs e)
        {
            Children[e.NewIndex].Parent = this;
        }
    }

}
