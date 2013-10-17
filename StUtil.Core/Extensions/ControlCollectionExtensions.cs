using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StUtil.Extensions
{
    public static class ControlCollectionExtensions
    {
        public static Control AddReturnItem(this Control.ControlCollection list, Control item)
        {
            list.Add(item);
            return item;
        }
        public static Control.ControlCollection AddReturnList(this Control.ControlCollection list, Control item)
        {
            list.Add(item);
            return list;
        }
    }
}
