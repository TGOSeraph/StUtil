using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace StUtil.Extensions
{
    public static class SplitterPanelExtensions
    {
        public static void SetWidth(this SplitterPanel panel, int width)
        {
            ((SplitContainer)panel.Parent).SplitterDistance = width;
        }
    }
}
