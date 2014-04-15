using StUtil.Internal.Native;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace StUtil.Internal.Shell
{
    /// <summary>
    /// Summary description for SysImageList.
    /// </summary>
    public class SystemImageList : IDisposable
    {
        private bool disposed = false;
        private IntPtr hIml = IntPtr.Zero;
        private NativeInterfaces.IImageList iImageList = null;
        private NativeEnums.SystemImageListSize size = NativeEnums.SystemImageListSize.SmallIcons;

        /// <summary>
        /// Gets the hImageList handle
        /// </summary>
        public IntPtr Handle
        {
            get
            {
                return this.hIml;
            }
        }

        /// <summary>
        /// Gets/sets the size of System Image List to retrieve.
        /// </summary>
        public NativeEnums.SystemImageListSize ImageListSize
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
                Create();
            }
        }

        /// <summary>
        /// Returns the size of the Image List Icons.
        /// </summary>
        public System.Drawing.Size Size
        {
            get
            {
                int cx = 0;
                int cy = 0;

                if (iImageList == null)
                    NativeMethods.ImageList_GetIconSize(hIml, ref cx, ref cy);
                else
                    iImageList.GetIconSize(ref cx, ref cy);

                System.Drawing.Size sz = new System.Drawing.Size(cx, cy);

                return sz;
            }
        }

        /// <summary>
        /// Creates a Small Icons SystemImageList 
        /// </summary>
        public SystemImageList()
        {
            Create();
        }
        /// <summary>
        /// Creates a SystemImageList with the specified size
        /// </summary>
        /// <param name="size">Size of System ImageList</param>
        public SystemImageList(NativeEnums.SystemImageListSize size)
        {
            this.size = size;
            Create();
        }

        /// <summary>
        /// Finalise for SysImageList
        /// </summary>
        ~SystemImageList()
        {
            Dispose(false);
        }

        /// <summary>
        /// Clears up any resources associated with the SystemImageList
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Clears up any resources associated with the SystemImageList
        /// when disposing is true.
        /// </summary>
        /// <param name="disposing">Whether the object is being disposed</param>
        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (iImageList != null)
                    {
                        Marshal.ReleaseComObject(iImageList);
                    }
                    iImageList = null;
                }
            }
            disposed = true;
        }

        /// <summary>
        /// Draws an image
        /// </summary>
        /// <param name="hdc">Device context to draw to</param>
        /// <param name="index">Index of image to draw</param>
        /// <param name="x">X Position to draw at</param>
        /// <param name="y">Y Position to draw at</param>
        public void DrawImage(IntPtr hdc, int index, int x, int y)
        {
            DrawImage(hdc, index, x, y, NativeEnums.ImageListDrawItemConstants.ILD_TRANSPARENT);
        }

        /// <summary>
        /// Draws an image using the specified flags
        /// </summary>
        /// <param name="hdc">Device context to draw to</param>
        /// <param name="index">Index of image to draw</param>
        /// <param name="x">X Position to draw at</param>
        /// <param name="y">Y Position to draw at</param>
        /// <param name="flags">Drawing flags</param>
        public void DrawImage(IntPtr hdc, int index, int x, int y, NativeEnums.ImageListDrawItemConstants flags)
        {
            if (iImageList == null)
            {
                int ret = NativeMethods.ImageList_Draw(hIml, index, hdc, x, y, (int)flags);
            }
            else
            {
                NativeStructs.IMAGELISTDRAWPARAMS pimldp = new NativeStructs.IMAGELISTDRAWPARAMS();
                pimldp.hdcDst = hdc;
                pimldp.cbSize = Marshal.SizeOf(pimldp.GetType());
                pimldp.i = index;
                pimldp.x = x;
                pimldp.y = y;
                pimldp.rgbFg = -1;
                pimldp.fStyle = (int)flags;
                iImageList.Draw(ref pimldp);
            }

        }

        /// <summary>
        /// Draws an image using the specified flags and specifies
        /// the size to clip to (or to stretch to if ILD_SCALE
        /// is provided).
        /// </summary>
        /// <param name="hdc">Device context to draw to</param>
        /// <param name="index">Index of image to draw</param>
        /// <param name="x">X Position to draw at</param>
        /// <param name="y">Y Position to draw at</param>
        /// <param name="flags">Drawing flags</param>
        /// <param name="cx">Width to draw</param>
        /// <param name="cy">Height to draw</param>
        public void DrawImage(IntPtr hdc, int index, int x, int y, NativeEnums.ImageListDrawItemConstants flags, int cx, int cy)
        {
            NativeStructs.IMAGELISTDRAWPARAMS pimldp = new NativeStructs.IMAGELISTDRAWPARAMS();
            pimldp.hdcDst = hdc;
            pimldp.cbSize = Marshal.SizeOf(pimldp.GetType());
            pimldp.i = index;
            pimldp.x = x;
            pimldp.y = y;
            pimldp.cx = cx;
            pimldp.cy = cy;
            pimldp.fStyle = (int)flags;
            if (iImageList == null)
            {
                pimldp.himl = hIml;
                int ret = NativeMethods.ImageList_DrawIndirect(ref pimldp);
            }
            else
            {

                iImageList.Draw(ref pimldp);
            }
        }

        /// <summary>
        /// Draws an image using the specified flags and state on XP systems.
        /// </summary>
        /// <param name="hdc">Device context to draw to</param>
        /// <param name="index">Index of image to draw</param>
        /// <param name="x">X Position to draw at</param>
        /// <param name="y">Y Position to draw at</param>
        /// <param name="flags">Drawing flags</param>
        /// <param name="cx">Width to draw</param>
        /// <param name="cy">Height to draw</param>
        /// <param name="foreColor">Fore colour to blend with when using the 
        /// ILD_SELECTED or ILD_BLEND25 flags</param>
        /// <param name="stateFlags">State flags</param>
        /// <param name="glowOrShadowColor">If stateFlags include ILS_GLOW, then
        /// the colour to use for the glow effect.  Otherwise if stateFlags includes 
        /// ILS_SHADOW, then the colour to use for the shadow.</param>
        /// <param name="saturateColorOrAlpha">If stateFlags includes ILS_ALPHA,
        /// then the alpha component is applied to the icon. Otherwise if 
        /// ILS_SATURATE is included, then the (R,G,B) components are used
        /// to saturate the image.</param>
        public void DrawImage(IntPtr hdc, int index, int x, int y, NativeEnums.ImageListDrawItemConstants flags, int cx, int cy, System.Drawing.Color foreColor, NativeEnums.ImageListDrawStateConstants stateFlags, System.Drawing.Color saturateColorOrAlpha, System.Drawing.Color glowOrShadowColor)
        {
            NativeStructs.IMAGELISTDRAWPARAMS pimldp = new NativeStructs.IMAGELISTDRAWPARAMS();
            pimldp.hdcDst = hdc;
            pimldp.cbSize = Marshal.SizeOf(pimldp.GetType());
            pimldp.i = index;
            pimldp.x = x;
            pimldp.y = y;
            pimldp.cx = cx;
            pimldp.cy = cy;
            pimldp.rgbFg = Color.FromArgb(0,
              foreColor.R, foreColor.G, foreColor.B).ToArgb();
            Console.WriteLine("{0}", pimldp.rgbFg);
            pimldp.fStyle = (int)flags;
            pimldp.fState = (int)stateFlags;
            if ((stateFlags & NativeEnums.ImageListDrawStateConstants.ILS_ALPHA) == NativeEnums.ImageListDrawStateConstants.ILS_ALPHA)
            {
                // Set the alpha:
                pimldp.Frame = (int)saturateColorOrAlpha.A;
            }
            else if ((stateFlags & NativeEnums.ImageListDrawStateConstants.ILS_SATURATE) == NativeEnums.ImageListDrawStateConstants.ILS_SATURATE)
            {
                // discard alpha channel:
                saturateColorOrAlpha = Color.FromArgb(0,
                  saturateColorOrAlpha.R,
                  saturateColorOrAlpha.G,
                  saturateColorOrAlpha.B);
                // set the saturate color
                pimldp.Frame = saturateColorOrAlpha.ToArgb();
            }
            glowOrShadowColor = Color.FromArgb(0, glowOrShadowColor.R, glowOrShadowColor.G, glowOrShadowColor.B);
            pimldp.crEffect = glowOrShadowColor.ToArgb();
            if (iImageList == null)
            {
                pimldp.himl = hIml;
                int ret = NativeMethods.ImageList_DrawIndirect(ref pimldp);
            }
            else
            {

                iImageList.Draw(ref pimldp);
            }
        }

        /// <summary>
        /// Returns a GDI+ copy of the icon from the ImageList
        /// at the specified index.
        /// </summary>
        /// <param name="index">The index to get the icon for</param>
        /// <returns>The specified icon</returns>
        public Icon Icon(int index)
        {
            Icon icon = null;

            IntPtr hIcon = IntPtr.Zero;
            if (iImageList == null)
            {
                hIcon = NativeMethods.ImageList_GetIcon(hIml, index, (int)NativeEnums.ImageListDrawItemConstants.ILD_TRANSPARENT);

            }
            else
            {
                iImageList.GetIcon(index, (int)NativeEnums.ImageListDrawItemConstants.ILD_TRANSPARENT, ref hIcon);
            }

            if (hIcon != IntPtr.Zero)
            {
                icon = System.Drawing.Icon.FromHandle(hIcon);
            }
            return icon;
        }

        /// <summary>
        /// Return the index of the icon for the specified file, always using 
        /// the cached version where possible.
        /// </summary>
        /// <param name="fileName">Filename to get icon for</param>
        /// <returns>Index of the icon</returns>
        public int IconIndex(string fileName)
        {
            return IconIndex(fileName, false);
        }

        /// <summary>
        /// Returns the index of the icon for the specified file
        /// </summary>
        /// <param name="fileName">Filename to get icon for</param>
        /// <param name="forceLoadFromDisk">If True, then hit the disk to get the icon,
        /// otherwise only hit the disk if no cached icon is available.</param>
        /// <returns>Index of the icon</returns>
        public int IconIndex(
          string fileName,
          bool forceLoadFromDisk)
        {
            return IconIndex(fileName, forceLoadFromDisk, NativeEnums.ShellIconStateConstants.ShellIconStateNormal);
        }

        /// <summary>
        /// Returns the index of the icon for the specified file
        /// </summary>
        /// <param name="fileName">Filename to get icon for</param>
        /// <param name="forceLoadFromDisk">If True, then hit the disk to get the icon,
        /// otherwise only hit the disk if no cached icon is available.</param>
        /// <param name="iconState">Flags specifying the state of the icon
        /// returned.</param>
        /// <returns>Index of the icon</returns>
        public int IconIndex(string fileName, bool forceLoadFromDisk, NativeEnums.ShellIconStateConstants iconState)
        {
            NativeEnums.SHGetFileInfoConstants dwFlags = NativeEnums.SHGetFileInfoConstants.SHGFI_SYSICONINDEX;
            int dwAttr = 0;
            if (size == NativeEnums.SystemImageListSize.SmallIcons)
            {
                dwFlags |= NativeEnums.SHGetFileInfoConstants.SHGFI_SMALLICON;
            }

            // We can choose whether to access the disk or not. If you don't
            // hit the disk, you may get the wrong icon if the icon is
            // not cached. Also only works for files.
            if (!forceLoadFromDisk)
            {
                dwFlags |= NativeEnums.SHGetFileInfoConstants.SHGFI_USEFILEATTRIBUTES;
                dwAttr = NativeConsts.FILE_ATTRIBUTE_NORMAL;
            }
            else
            {
                dwAttr = 0;
            }

            // sFileSpec can be any file. You can specify a
            // file that does not exist and still get the
            // icon, for example sFileSpec = "C:\PANTS.DOC"
            NativeStructs.SHFILEINFO shfi = new NativeStructs.SHFILEINFO();
            uint shfiSize = (uint)Marshal.SizeOf(shfi.GetType());
            IntPtr retVal = NativeMethods.SHGetFileInfo(fileName, dwAttr, ref shfi, shfiSize, ((uint)(dwFlags) | (uint)iconState));

            if (retVal.Equals(IntPtr.Zero))
            {
                System.Diagnostics.Debug.Assert((!retVal.Equals(IntPtr.Zero)), "Failed to get icon index");
                return 0;
            }
            else
            {
                return shfi.iIcon;
            }
        }

        /// <summary>
        /// Creates the SystemImageList
        /// </summary>
        private void Create()
        {
            // forget last image list if any:
            hIml = IntPtr.Zero;

            if (isXpOrAbove())
            {
                // Get the System NativeInterfaces.IImageList object from the Shell:
                Guid iidImageList = new Guid("46EB5926-582E-4017-9FDF-E8998DAA0950");
                int ret = NativeMethods.SHGetImageList((int)size, ref iidImageList, ref iImageList);
                // the image list handle is the IUnknown pointer, but 
                // using Marshal.GetIUnknownForObject doesn't return
                // the right value.  It really doesn't hurt to make
                // a second call to get the handle:
                NativeMethods.SHGetImageListHandle((int)size, ref iidImageList, ref hIml);
            }
            else
            {
                // Prepare flags:
                NativeEnums.SHGetFileInfoConstants dwFlags = NativeEnums.SHGetFileInfoConstants.SHGFI_USEFILEATTRIBUTES | NativeEnums.SHGetFileInfoConstants.SHGFI_SYSICONINDEX;
                if (size == NativeEnums.SystemImageListSize.SmallIcons)
                {
                    dwFlags |= NativeEnums.SHGetFileInfoConstants.SHGFI_SMALLICON;
                }
                // Get image list
                NativeStructs.SHFILEINFO shfi = new NativeStructs.SHFILEINFO();
                uint shfiSize = (uint)Marshal.SizeOf(shfi.GetType());

                // Call SHGetFileInfo to get the image list handle
                // using an arbitrary file:
                hIml = NativeMethods.SHGetFileInfo(".txt", NativeConsts.FILE_ATTRIBUTE_NORMAL, ref shfi, shfiSize, (uint)dwFlags);
                System.Diagnostics.Debug.Assert((hIml != IntPtr.Zero), "Failed to create Image List");
            }
        }

        /// <summary>
        /// Determines if the system is running Windows XP
        /// or above
        /// </summary>
        /// <returns>True if system is running XP or above, False otherwise</returns>
        private bool isXpOrAbove()
        {
            bool ret = false;
            if (Environment.OSVersion.Version.Major > 5)
            {
                ret = true;
            }
            else if ((Environment.OSVersion.Version.Major == 5) &&
              (Environment.OSVersion.Version.Minor >= 1))
            {
                ret = true;
            }
            return ret;
            //return false;
        }
    }


    /// <summary>
    /// Helper Methods for Connecting SystemImageList to Common Controls
    /// </summary>
    public class SystemImageListHelper
    {
        /// <summary>
        /// Associates a SysImageList with a ListView control
        /// </summary>
        /// <param name="listView">ListView control to associate ImageList with</param>
        /// <param name="sysImageList">System Image List to associate</param>
        /// <param name="forStateImages">Whether to add ImageList as StateImageList</param>
        public static void SetImageList(ListView listView, SystemImageList sysImageList, bool forStateImages
          )
        {
            IntPtr wParam = (IntPtr)NativeConsts.LVSIL_NORMAL;
            if (sysImageList.ImageListSize == NativeEnums.SystemImageListSize.SmallIcons)
            {
                wParam = (IntPtr)NativeConsts.LVSIL_SMALL;
            }
            if (forStateImages)
            {
                wParam = (IntPtr)NativeConsts.LVSIL_STATE;
            }
            NativeMethods.SendMessage(listView.Handle, NativeConsts.LVM_SETIMAGELIST, wParam, sysImageList.Handle);
        }

        /// <summary>
        /// Associates a SysImageList with a TreeView control
        /// </summary>
        /// <param name="treeView">TreeView control to associated ImageList with</param>
        /// <param name="sysImageList">System Image List to associate</param>
        /// <param name="forStateImages">Whether to add ImageList as StateImageList</param>
        public static void SetImageList(TreeView treeView, SystemImageList sysImageList, bool forStateImages
          )
        {
            IntPtr wParam = (IntPtr)NativeConsts.TVSIL_NORMAL;
            if (forStateImages)
            {
                wParam = (IntPtr)NativeConsts.TVSIL_STATE;
            }
            NativeMethods.SendMessage(treeView.Handle, NativeConsts.TVM_SETIMAGELIST, wParam, sysImageList.Handle);
        }

    }
}
