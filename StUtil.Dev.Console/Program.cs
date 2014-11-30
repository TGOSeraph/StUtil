using StUtil.Native.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.Dev.ConsoleTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
         

            Console.Read();
        }

        static void Mouse_MouseClick(object sender, MouseEventArgs e)
        {
            Console.WriteLine("Clicked " + e.Location.ToString() + " " + e.Button.ToString());
        }

    }
}