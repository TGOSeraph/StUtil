using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.Extensions
{
    public static class TreeViewExtensions
    {
        public static void SetExplorerTheme(this TreeView tree)
        {
            StUtil.Internal.Native.NativeMethods.SetWindowTheme(tree.Handle, "Explorer", null);
        }
    }
}
