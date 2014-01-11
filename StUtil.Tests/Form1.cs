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
using StUtil.Parser;

namespace StUtil.Tests
{
    public partial class Form1 : Form
    {
        [System.Runtime.InteropServices.DllImport(@"C:\Users\sot.PROSPECTSOFT\Documents\visual studio 2012\Projects\WrapperTesting\Win32Project2\x64\Debug\Win32Project2.dll", EntryPoint = "?Test@@YAXXZ")]
        public static extern void Test();

        public Form1()
        {
            InitializeComponent();
            Test();
            StUtil.Native.PE.WindowsPE pe = new Native.PE.WindowsPE(@"C:\Users\sot.PROSPECTSOFT\Documents\visual studio 2012\Projects\WrapperTesting\Win32Project2\Debug\Win32Project2.dll");
            var x = pe.Exports[0].UndecoratedName;

            Debugger.Break();
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
        }
    }
}
