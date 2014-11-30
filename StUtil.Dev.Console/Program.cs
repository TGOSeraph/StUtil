using System;
using System.Windows.Forms;

namespace StUtil.Dev.ConsoleTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.Read();
        }

        private static void Mouse_MouseClick(object sender, MouseEventArgs e)
        {
            Console.WriteLine("Clicked " + e.Location.ToString() + " " + e.Button.ToString());
        }
    }
}