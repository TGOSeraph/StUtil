using Microsoft.VisualStudio.TestTools.UnitTesting;
using StUtil.Extensions;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace StUtil.Core.Extension.Tests
{
    [TestClass]
    public class ImageTests
    {
        private DialogResult ShowTestForm(Bitmap image)
        {
            Form f = new Form();
            f.BackgroundImage = image;
            f.TopMost = true;
            f.Text = "Hello World";
            f.ControlBox = false;
            Button success = new Button { Text = "Success" };
            success.Click += (a, b) => { f.DialogResult = DialogResult.OK; f.Close(); };
            Button fail = new Button { Text = "Fail", Left = success.Width };
            fail.Click += (a, b) => { f.DialogResult = DialogResult.Cancel; f.Close(); };

            f.Controls.Add(success);
            f.Controls.Add(fail);

            Application.Run(f);
            return f.DialogResult;
        }

        [TestMethod]
        public void GetGraphics()
        {
            using (Bitmap b = new Bitmap(100, 100))
            {
                using (Graphics g = b.GetGraphics())
                {
                    g.DrawString("Hello World", new Font("Arial", 10), Brushes.Red, new PointF(0, 0));
                }
                if (ShowTestForm(b) != DialogResult.OK)
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod]
        public void Clear()
        {
            using (Bitmap b = new Bitmap(100, 100))
            {
                b.Clear(Color.Yellow);
                if (ShowTestForm(b) != DialogResult.OK)
                {
                    Assert.Fail();
                }
            }
        }

        [TestMethod]
        public void CreateDisabled()
        {
            using (Bitmap b = new Bitmap(100, 100))
            {
                using (Graphics g = b.GetGraphics())
                {
                    using (LinearGradientBrush brush = new LinearGradientBrush(
                      new Rectangle(0, 0, 100, 100),
                      Color.Blue,
                      Color.Red,
                      LinearGradientMode.Vertical))
                    {
                        brush.SetSigmaBellShape(0.5f);
                        g.FillRectangle(brush, new Rectangle(0, 0, 100, 100));
                        using (Bitmap b2 = (Bitmap)b.CreateDisabled())
                        {
                            if (ShowTestForm(b2) != DialogResult.OK)
                            {
                                Assert.Fail();
                            }
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void CreateGreyscale()
        {
            using (Bitmap b = new Bitmap(100, 100))
            {
                using (Graphics g = b.GetGraphics())
                {
                    using (LinearGradientBrush brush = new LinearGradientBrush(
                      new Rectangle(0, 0, 100, 100),
                      Color.Blue,
                      Color.Red,
                      LinearGradientMode.Vertical))
                    {
                        brush.SetSigmaBellShape(0.5f);
                        g.FillRectangle(brush, new Rectangle(0, 0, 100, 100));
                        using (Bitmap b2 = (Bitmap)b.CreateGreyscale())
                        {
                            if (ShowTestForm(b2) != DialogResult.OK)
                            {
                                Assert.Fail();
                            }
                        }
                    }
                }
            }
        }
    }
}