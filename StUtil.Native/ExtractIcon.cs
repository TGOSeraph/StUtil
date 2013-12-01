using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace StUtil.Native
{
    public static class ExtractIcon
    {
        /// <summary>
        /// COM wrapper for SHFILEINFO
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        struct SHFILEINFO
        {
            public IntPtr hIcon;
            public IntPtr iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        /// <summary>
        /// Contains a collection of utility methods that invoke COM
        /// </summary>
        //--- Constants ---
        private const uint SHGFI_ICON = 0x100;
        private const uint SHGFI_LARGEICON = 0x0; // Large icon
        private const uint SHGFI_SMALLICON = 0x1; // Small icon
        private const uint SHGFI_USEFILEATTRIBUTES = 0x10; // Enables working files that don't exist locally
        private const uint FILE_ATTRIBUTE_DIRECTORY = 0x10;
        private const uint FILE_ATTRIBUTE_NORMAL = 0x0;

        //--- Class Methods ---
        [DllImport("shell32.dll")]
        private static extern uint SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

        [DllImport("Shlwapi.dll", CharSet = CharSet.Auto)]
        public static extern long StrFormatByteSize(long fileSize, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder buffer, int bufferSize);

        [DllImport("user32.dll")]
        private static extern bool DestroyIcon(IntPtr handle);

        //TODO: update this class to allow usage of SHGFI_LARGEICON

        /// <summary>
        /// Retrieves an icon corresponding to the specified filename 
        /// </summary>
        /// <param name="filename">The name of the file for which to retrieve the icon</param>
        /// <returns>The icon associated with the file extension</returns>
        public static Icon IconFromFileExtension(string filename)
        {
            Icon icon = null;
            SHFILEINFO shinfo = new SHFILEINFO();
            if (0 != SHGetFileInfo(filename, FILE_ATTRIBUTE_NORMAL, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_SMALLICON | SHGFI_USEFILEATTRIBUTES))
            {
                icon = (Icon)(Icon.FromHandle(shinfo.hIcon)).Clone();
                DestroyIcon(shinfo.hIcon);
            }
            return icon;
        }

        /// <summary>
        /// Retrieves the system directory icon
        /// </summary>
        /// <returns>The system directory icon</returns>
        public static Icon GetDirectoryIcon()
        {
            Icon icon = null;
            SHFILEINFO shinfo = new SHFILEINFO();
            if (0 != SHGetFileInfo("d", FILE_ATTRIBUTE_DIRECTORY, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_SMALLICON | SHGFI_USEFILEATTRIBUTES))
            {
                icon = (Icon)(Icon.FromHandle(shinfo.hIcon)).Clone();
                DestroyIcon(shinfo.hIcon);
            }
            return icon;
        }

        /// <summary>
        /// Retrieves a formatted file size string
        /// </summary>
        /// <param name="fileSize">The file size in bytes</param>
        /// <returns>A localized file size string</returns>
        public static string GetFileSizeString(int fileSize)
        {
            StringBuilder sbBuffer = new StringBuilder(20);
            StrFormatByteSize(fileSize, sbBuffer, 20);
            return sbBuffer.ToString();
        }
    }
}
