using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StUtil.Imaging.ColorSpaces;
using StUtil.Net;
using StUtil.Net.Mail;
using StUtil.Imaging;
using System.Threading;
using StUtil.Extensions;

namespace StUtil.Tests
{
    public partial class Form1 : Form
    {
        StUtil.Video.PictureBoxVideoPlayback playback;
        public Form1()
        {
            InitializeComponent();

            playback = new Video.PictureBoxVideoPlayback(@"C:\Users\Simon\Desktop\FYP Demo\Demo\Videos\00395_xvid.avi", this.pictureBox1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            playback.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            playback.Pause();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            playback.Stop();
        }
    }
}
