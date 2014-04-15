using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.Extensions
{
    public static class TreeViewNativeExtensions
    {
        public static StUtil.Native.WndProcHandler SetFlickerFree(this TreeView tree)
        {
            return new StUtil.Native.WndProcHandler(tree, StUtil.Native.WndProcHandler.CreateFlickerFreeHandler(tree));
        }
    }
}
