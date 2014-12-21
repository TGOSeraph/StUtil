using System.Windows.Forms;

namespace StUtil.UI.Controls.FlickerFree
{
    /// <summary>
    /// A panel that is flicker free
    /// </summary>
    public class Panel : System.Windows.Forms.Panel
    {
        public Panel()
        {
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.DoubleBuffer,
                true);
        }
    }
}