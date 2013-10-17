using System;
using System.IO;
using System.Security.Cryptography;
using System.Linq;

namespace StUtil.File
{
    /// <summary>
    /// Utilities for dealing with files
    /// </summary>
    /// <remarks>
    /// 2013-07-04  - Initial version
    /// </remarks>
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
            string dir = Path.GetDirectoryName(filePath);
            string name = Path.GetFileNameWithoutExtension(filePath);
            string ext = Path.GetExtension(filePath);

            string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            string invalidReStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);
            return dir + "\\" + System.Text.RegularExpressions.Regex.Replace(name, invalidReStr, replace) + ext;
        }

        /// <summary>
        /// Compute the MD5 Hash of a file
        /// </summary>
        /// <param name="fileName">The file to compute the MD5 hash of</param>
        /// <returns>The MD5 hash of the specified file</returns>
        public static string ComputeMD5Hash(string fileName)
        {
            using (MD5CryptoServiceProvider alg = new MD5CryptoServiceProvider())
            {
                return ComputeHash(fileName, alg);
            }
        }

        /// <summary>
        /// Compute the SHA1 Hash of a file
        /// </summary>
        /// <param name="fileName">The file to compute the SHA1 hash of</param>
        /// <returns>The SHA1 hash of the specified file</returns>
        public static string ComputeSHA1Hash(string fileName)
        {
            using (SHA1CryptoServiceProvider alg = new SHA1CryptoServiceProvider())
            {
                return ComputeHash(fileName, alg);
            }
        }

        /// <summary>
        /// Compute the hash of a file
        /// </summary>
        /// <param name="fileName">The file to compute the hash of</param>
        /// <param name="hashAlgorithm">The hashing algorithm to use</param>
        /// <returns>The computed hash of the file</returns>
        public static string ComputeHash(string fileName, HashAlgorithm hashAlgorithm)
        {
            FileStream stmcheck = System.IO.File.OpenRead(fileName);
            try
            {
                byte[] hash = hashAlgorithm.ComputeHash(stmcheck);
                string computed = BitConverter.ToString(hash).Replace("-", "");
                return computed;
            }
            finally
            {
                stmcheck.Close();
            }
        }

        /// <summary>
        /// Open the file in windows explorer
        /// </summary>
        /// <param name="filePath">The path to open</param>
        public static void OpenInExplorer(string filePath)
        {
            System.Diagnostics.Process.Start("explorer.exe", @"/select, " + filePath);
        }

        /// <summary>
        /// Backup a file by either moving or copying it with a new extension
        /// </summary>
        /// <param name="filePath">The path to the file to backup</param>
        /// <param name="backupExt">The file extension to append to the file</param>
        /// <param name="copy">If the file should be copied, else it will be moved</param>
        /// <param name="overwriteExisting">If an existing backup should be overwritten or an exception should be thrown</param>
        /// <returns>If the file was overwritten or not</returns>
        public static bool BackupFile(string filePath, string backupExt = ".bkp", bool copy = false, bool overwriteExisting = true)
        {
            string bkpFile = filePath + backupExt;
            bool ex = false;
            if (System.IO.File.Exists(bkpFile))
            {
                ex = true;
                if (overwriteExisting)
                {
                    System.IO.File.Delete(bkpFile);
                }
                else
                {
                    throw new IOException("File already exists");
                }
            }
            if (copy)
            {
                System.IO.File.Copy(filePath, bkpFile);
            }
            else
            {
                System.IO.File.Move(filePath, bkpFile);
            }
            return ex;
        }

        public enum MediaType
        {
            Image,
            ImageExtended,
            Video,
            VideoExtended,
            System,
            Archive
        }

        public static bool IsValidFileMediaType(string filePath, MediaType type)
        {
            string[] image = {"png", "gif", "ico", "bmp", "jpg", "jpeg"};
            string[] imageExtended = {"png", "gif", "ico", "bmp", "jpg", "jpeg", "tiff", "raw", "cr2", "dds", "apng"};

            string ext = Path.GetExtension(filePath).ToLower().Substring(1);
            switch (type)
            {
                case MediaType.Image:
                    return image.Contains(ext);
                case MediaType.ImageExtended:
                    return imageExtended.Contains(ext);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
