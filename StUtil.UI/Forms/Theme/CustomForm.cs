using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.UI.Forms.Theme
{
    public partial class CustomForm : Form
    {
        public Size? CustomSize { get; set; }

        public CustomForm()
        {
            InitializeComponent();
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (CustomSize != null)
            {
                width = CustomSize.Value.Width;
                height = CustomSize.Value.Height;
                CustomSize = null;
            }
            base.SetBoundsCore(x, y, width, height, specified);
        }
    }
}
