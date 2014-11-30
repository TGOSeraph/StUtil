using System.Drawing;
using System.IO;

namespace StUtil.Imaging
{
    public class Utilities
    {
        /// <summary>
        /// Opens the file without locking the file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static Image OpenFile(string fileName)
        {
            Image img;
            using (var bmpTemp = new Bitmap(fileName))
            {
                img = new Bitmap(bmpTemp);
            }
            return img;
        }

        /// <summary>
        /// Creats a bitmap from a byte array.
        /// </summary>
        /// <param name="byteArray">The byte array.</param>
        /// <returns></returns>
        public static Bitmap BitmapFromBytes(byte[] byteArray)
        {
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                using (Bitmap img = (Bitmap)Image.FromStream(ms))
                {
                    return new Bitmap(img);
                }
            }
        }
    }
}