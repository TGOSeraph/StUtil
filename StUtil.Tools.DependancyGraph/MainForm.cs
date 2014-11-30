using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.Tools.DependancyGraph
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StUtil.Data.Specialised.DependancyHelper<string> helper = new Data.Specialised.DependancyHelper<string>();
            HashSet<string> all = new HashSet<string>();
            foreach (string line in textBox1.Text.Split(new string[] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] parts = line.Split('|');
                if (parts.Length != 2)
                {
                    continue;
                }
                string p1 = parts[0].Trim();
                string p2 = parts[1].Trim();

                all.Add(p1);
                all.Add(p2);

                helper.CreateDependancy(p1, p2);
            }
            pictureBox1.Image = yUml.GetDiagram(all, helper);
        }
    }
}
