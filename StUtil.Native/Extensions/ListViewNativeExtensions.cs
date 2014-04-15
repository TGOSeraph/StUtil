using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.Native.Extensions
{
    public static class ListViewNativeExtensions
    {
        public static StUtil.Native.WndProcHandler SetFlickerFree(this ListView listView)
        {
            return new StUtil.Native.WndProcHandler(listView, StUtil.Native.WndProcHandler.CreateFlickerFreeHandler(listView));
        }
    }
}
