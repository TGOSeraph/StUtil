using System;
using System.IO;

namespace StUtil.IO
{
    public static class Utilities
    {
        /// <summary>
        /// Get a unique file name by appending an integer to the end of the file name
        /// </summary>
        /// <param name="filePath">The initial file path</param>
        /// <param name="format">The format in which to generate the name in the format {0}{1} where {0} is the initial file path and {1} is the generated number</param>
        /// <returns>A unique file name in the specified format</returns>
        public static string GenerateUniqueFileName(string filePath, string format = "{0}_{1}")
        {
            if (!System.IO.File.Exists(filePath))
            {
                return filePath;
            }
            else
            {
                string dir = Path.GetDirectoryName(filePath);
                string name = Path.GetFileNameWithoutExtension(filePath);
                string ext = Path.GetExtension(filePath);
                int i = 0;
                string f = Path.Combine(dir, string.Format(format, name, i++)) + ext;
                while (System.IO.File.Exists(f))
                {
                    f = Path.Combine(dir, string.Format(format, name, i++)) + ext;
                }
                return f;
            }
        }

        /// <summary>
        /// Make a file name valid for the Windows by replacing all invalid characters
        /// </summary>
        /// <param name="filePath">The path to validate</param>
        /// <param name="replace">The character to replace invalid characters with</param>
        /// <returns>A file name with all invalid characters removed</returns>
        public static string MakeValidFileName(string filePath, string replace = "_")
        {
            string dir = null;
            string name = null;
            string ext = "";
            try
            {
                dir = Path.GetDirectoryName(filePath);
                name = Path.GetFileNameWithoutExtension(filePath);
                ext = Path.GetExtension(filePath);
            }
            catch (Exception)
            {
                name = filePath;
            }

            string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            string invalidReStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);
            return (dir == null ? "" : dir + "\\") + System.Text.RegularExpressions.Regex.Replace(name, invalidReStr, replace) + ext;
        }

        /// <summary>
        /// Normalizes the path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The normalized path</returns>
        public static string NormalizePath(string path)
        {
            return Path.GetFullPath(path)
                       .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                       .ToUpperInvariant();
        }
    }
}