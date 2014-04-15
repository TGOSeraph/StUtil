using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.Extensions
{
    public static class ListViewExtensions
    {
        public static void SetExplorerTheme(this ListView listView)
        {
            StUtil.Internal.Native.NativeMethods.SetWindowTheme(listView.Handle, "Explorer", null);
        }
    }
}
