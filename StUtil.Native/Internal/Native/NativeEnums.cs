using System;

namespace StUtil.Internal.Native
{
    public static class NativeEnums
    {
        public enum ABMsg : int
        {
            ABM_NEW = 0,
            ABM_REMOVE,
            ABM_QUERYPOS,
            ABM_SETPOS,
            ABM_GETSTATE,
            ABM_GETTASKBARPOS,
            ABM_ACTIVATE,
            ABM_GETAUTOHIDEBAR,
            ABM_SETAUTOHIDEBAR,
            ABM_WINDOWPOSCHANGED,
            ABM_SETSTATE
        }

        public enum ABNotify : int
        {
            ABN_STATECHANGE = 0,
            ABN_POSCHANGED,
            ABN_FULLSCREENAPP,
            ABN_WINDOWARRANGE
        }

        [Flags]
        public enum AllocationType
        {
            Commit = 0x1000,
            Reserve = 0x2000,
            Decommit = 0x4000,
            Release = 0x8000,
            Reset = 0x80000,
            Physical = 0x400000,
            TopDown = 0x100000,
            WriteWatch = 0x200000,
            LargePages = 0x20000000
        }

        [Flags]
        public enum AnimationFlags : int
        {
            Roll = 0x0000, // Uses a roll animation.
            HorizontalPositive = 0x00001, // Animates the window from left to right. This flag can be used with roll or slide animation.
            HorizontalNegative = 0x00002, // Animates the window from right to left. This flag can be used with roll or slide animation.
            VerticalPositive = 0x00004, // Animates the window from top to bottom. This flag can be used with roll or slide animation.
            VerticalNegative = 0x00008, // Animates the window from bottom to top. This flag can be used with roll or slide animation.
            Center = 0x00010, // Makes the window appear to collapse inward if Hide is used or expand outward if the Hide is not used.
            Hide = 0x10000, // Hides the window. By default, the window is shown.
            Activate = 0x20000, // Activates the window.
            Slide = 0x40000, // Uses a slide animation. By default, roll animation is used.
            Blend = 0x80000, // Uses a fade effect. This flag can be used only with a top-level window.
            Mask = 0xfffff,
        }

        [Flags]
        public enum CSIDL : uint
        {
            CSIDL_DESKTOP = 0x0000,
            CSIDL_WINDOWS = 0x0024
        }

        [Flags]
        public enum FreeType
        {
            Decommit = 0x4000,
            Release = 0x8000,
        }

        /// <summary>
        /// Flags controlling how the Image List item is 
        /// drawn
        /// </summary>
        [Flags]
        public enum ImageListDrawItemConstants : int
        {
            /// <summary>
            /// Draw item normally.
            /// </summary>
            ILD_NORMAL = 0x0,
            /// <summary>
            /// Draw item transparently.
            /// </summary>
            ILD_TRANSPARENT = 0x1,
            /// <summary>
            /// Draw item blended with 25% of the specified foreground colour
            /// or the Highlight colour if no foreground colour specified.
            /// </summary>
            ILD_BLEND25 = 0x2,
            /// <summary>
            /// Draw item blended with 50% of the specified foreground colour
            /// or the Highlight colour if no foreground colour specified.
            /// </summary>
            ILD_SELECTED = 0x4,
            /// <summary>
            /// Draw the icon's mask
            /// </summary>
            ILD_MASK = 0x10,
            /// <summary>
            /// Draw the icon image without using the mask
            /// </summary>
            ILD_IMAGE = 0x20,
            /// <summary>
            /// Draw the icon using the ROP specified.
            /// </summary>
            ILD_ROP = 0x40,
            /// <summary>
            /// Preserves the alpha channel in dest. XP only.
            /// </summary>
            ILD_PRESERVEALPHA = 0x1000,
            /// <summary>
            /// Scale the image to cx, cy instead of clipping it.  XP only.
            /// </summary>
            ILD_SCALE = 0x2000,
            /// <summary>
            /// Scale the image to the current DPI of the display. XP only.
            /// </summary>
            ILD_DPISCALE = 0x4000
        }

        /// <summary>
        /// Enumeration containing XP ImageList Draw State options
        /// </summary>
        [Flags]
        public enum ImageListDrawStateConstants : int
        {
            /// <summary>
            /// The image state is not modified. 
            /// </summary>
            ILS_NORMAL = (0x00000000),
            /// <summary>
            /// Adds a glow effect to the icon, which causes the icon to appear to glow 
            /// with a given color around the edges. (Note: does not appear to be
            /// implemented)
            /// </summary>
            ILS_GLOW = (0x00000001), //The color for the glow effect is passed to the IImageList::Draw method in the crEffect member of IMAGELISTDRAWPARAMS. 
            /// <summary>
            /// Adds a drop shadow effect to the icon. (Note: does not appear to be
            /// implemented)
            /// </summary>
            ILS_SHADOW = (0x00000002), //The color for the drop shadow effect is passed to the IImageList::Draw method in the crEffect member of IMAGELISTDRAWPARAMS. 
            /// <summary>
            /// Saturates the icon by increasing each color component 
            /// of the RGB triplet for each pixel in the icon. (Note: only ever appears
            /// to result in a completely unsaturated icon)
            /// </summary>
            ILS_SATURATE = (0x00000004), // The amount to increase is indicated by the frame member in the IMAGELISTDRAWPARAMS method. 
            /// <summary>
            /// Alpha blends the icon. Alpha blending controls the transparency 
            /// level of an icon, according to the value of its alpha channel. 
            /// (Note: does not appear to be implemented).
            /// </summary>
            ILS_ALPHA = (0x00000008) //The value of the alpha channel is indicated by the frame member in the IMAGELISTDRAWPARAMS method. The alpha channel can be from 0 to 255, with 0 being completely transparent, and 255 being completely opaque. 
        }

        [Flags]
        public enum MemoryProtection
        {
            Execute = 0x10,
            ExecuteRead = 0x20,
            ExecuteReadWrite = 0x40,
            ExecuteWriteCopy = 0x80,
            NoAccess = 0x01,
            ReadOnly = 0x02,
            ReadWrite = 0x04,
            WriteCopy = 0x08,
            GuardModifierflag = 0x100,
            NoCacheModifierflag = 0x200,
            WriteCombineModifierflag = 0x400
        }

        [Flags()]
        public enum ProcessAccess : int
        {
            AllAccess = CreateThread | DuplicateHandle | QueryInformation | SetInformation | Terminate | VMOperation | VMRead | VMWrite | Synchronize,
            CreateThread = 0x2,
            DuplicateHandle = 0x40,
            QueryInformation = 0x400,
            SetInformation = 0x200,
            Terminate = 0x1,
            VMOperation = 0x8,
            VMRead = 0x10,
            VMWrite = 0x20,
            Synchronize = 0x100000
        }
        public enum ScrollBarDirection
        {
            SB_HORZ = 0,
            SB_VERT = 1,
            SB_CTL = 2,
            SB_BOTH = 3
        }

        [Flags]
        public enum ScrollInfoMask
        {
            SIF_RANGE = 0x1,
            SIF_PAGE = 0x2,
            SIF_POS = 0x4,
            SIF_DISABLENOSCROLL = 0x8,
            SIF_TRACKPOS = 0x16,
            SIF_ALL = SIF_RANGE + SIF_PAGE + SIF_POS + SIF_TRACKPOS
        }

        [Flags]
        public enum SFGAOF : uint
        {
            SFGAO_CANCOPY = 0x1,                   // Objects can be copied  (DROPEFFECT_COPY)
            SFGAO_CANMOVE = 0x2,                   // Objects can be moved   (DROPEFFECT_MOVE)
            SFGAO_CANLINK = 0x4,                   // Objects can be linked  (DROPEFFECT_LINK)
            SFGAO_STORAGE = 0x00000008,            // Supports BindToObject(IID_IStorage)
            SFGAO_CANRENAME = 0x00000010,          // Objects can be renamed
            SFGAO_CANDELETE = 0x00000020,          // Objects can be deleted
            SFGAO_HASPROPSHEET = 0x00000040,       // Objects have property sheets
            SFGAO_DROPTARGET = 0x00000100,         // Objects are drop target
            SFGAO_CAPABILITYMASK = 0x00000177,
            SFGAO_ENCRYPTED = 0x00002000,          // Object is encrypted (use alt color)
            SFGAO_ISSLOW = 0x00004000,             // 'Slow' object
            SFGAO_GHOSTED = 0x00008000,            // Ghosted icon
            SFGAO_LINK = 0x00010000,               // Shortcut (link)
            SFGAO_SHARE = 0x00020000,              // Shared
            SFGAO_READONLY = 0x00040000,           // Read-only
            SFGAO_HIDDEN = 0x00080000,             // Hidden object
            SFGAO_DISPLAYATTRMASK = 0x000FC000,
            SFGAO_FILESYSANCESTOR = 0x10000000,    // May contain children with SFGAO_FILESYSTEM
            SFGAO_FOLDER = 0x20000000,             // Support BindToObject(IID_IShellFolder)
            SFGAO_FILESYSTEM = 0x40000000,         // Is a win32 file system object (file/folder/root)
            SFGAO_HASSUBFOLDER = 0x80000000,       // May contain children with SFGAO_FOLDER
            SFGAO_CONTENTSMASK = 0x80000000,
            SFGAO_VALIDATE = 0x01000000,           // Invalidate cached information
            SFGAO_REMOVABLE = 0x02000000,          // Is this removeable media?
            SFGAO_COMPRESSED = 0x04000000,         // Object is compressed (use alt color)
            SFGAO_BROWSABLE = 0x08000000,          // Supports IShellFolder, but only implements CreateViewObject() (non-folder view)
            SFGAO_NONENUMERATED = 0x00100000,      // Is a non-enumerated object
            SFGAO_NEWCONTENT = 0x00200000,         // Should show bold in explorer tree
            SFGAO_CANMONIKER = 0x00400000,         // Defunct
            SFGAO_HASSTORAGE = 0x00400000,         // Defunct
            SFGAO_STREAM = 0x00400000,             // Supports BindToObject(IID_IStream)
            SFGAO_STORAGEANCESTOR = 0x00800000,    // May contain children with SFGAO_STORAGE or SFGAO_STREAM
            SFGAO_STORAGECAPMASK = 0x70C50008,     // For determining storage capabilities, ie for open/save semantics
        }

        [Flags]
        public enum SHCONTF : uint
        {
            SHCONTF_FOLDERS = 0x0020,              // Only want folders enumerated (SFGAO_FOLDER)
            SHCONTF_NONFOLDERS = 0x0040,           // Include non folders
            SHCONTF_INCLUDEHIDDEN = 0x0080,        // Show items normally hidden
            SHCONTF_INIT_ON_FIRST_NEXT = 0x0100,   // Allow EnumObject() to return before validating enum
            SHCONTF_NETPRINTERSRCH = 0x0200,       // Hint that client is looking for printers
            SHCONTF_SHAREABLE = 0x0400,            // Hint that client is looking sharable resources (remote shares)
            SHCONTF_STORAGE = 0x0800,              // Include all items with accessible storage and their ancestors
        }

        /// <summary>
        /// Flags specifying the state of the icon to draw from the Shell
        /// </summary>
        [Flags]
        public enum ShellIconStateConstants
        {
            /// <summary>
            /// Get icon in normal state
            /// </summary>
            ShellIconStateNormal = 0,
            /// <summary>
            /// Put a link overlay on icon 
            /// </summary>
            ShellIconStateLinkOverlay = 0x8000,
            /// <summary>
            /// show icon in selected state 
            /// </summary>
            ShellIconStateSelected = 0x10000,
            /// <summary>
            /// get open icon 
            /// </summary>
            ShellIconStateOpen = 0x2,
            /// <summary>
            /// apply the appropriate overlays
            /// </summary>
            ShellIconAddOverlays = 0x000000020,
        }

        [Flags]
        public enum SHGetFileInfoConstants : int
        {
            SHGFI_ICON = 0x100,                // get icon 
            SHGFI_DISPLAYNAME = 0x200,         // get display name 
            SHGFI_TYPENAME = 0x400,            // get type name 
            SHGFI_ATTRIBUTES = 0x800,          // get attributes 
            SHGFI_ICONLOCATION = 0x1000,       // get icon location 
            SHGFI_EXETYPE = 0x2000,            // return exe type 
            SHGFI_SYSICONINDEX = 0x4000,       // get system icon index 
            SHGFI_LINKOVERLAY = 0x8000,        // put a link overlay on icon 
            SHGFI_SELECTED = 0x10000,          // show icon in selected state 
            SHGFI_ATTR_SPECIFIED = 0x20000,    // get only specified attributes 
            SHGFI_LARGEICON = 0x0,             // get large icon 
            SHGFI_SMALLICON = 0x1,             // get small icon 
            SHGFI_OPENICON = 0x2,              // get open icon 
            SHGFI_SHELLICONSIZE = 0x4,         // get shell size icon 
            //SHGFI_PIDL = 0x8,                  // pszPath is a pidl 
            SHGFI_USEFILEATTRIBUTES = 0x10,    // use passed dwFileAttribute 
            SHGFI_ADDOVERLAYS = 0x000000020,   // apply the appropriate overlays
            SHGFI_OVERLAYINDEX = 0x000000040   // Get the index of the overlay
        }
        [Flags]
        public enum SHGFI
        {
            SHGFI_ICON = 0x000000100,
            SHGFI_DISPLAYNAME = 0x000000200,
            SHGFI_TYPENAME = 0x000000400,
            SHGFI_ATTRIBUTES = 0x000000800,
            SHGFI_ICONLOCATION = 0x000001000,
            SHGFI_EXETYPE = 0x000002000,
            SHGFI_SYSICONINDEX = 0x000004000,
            SHGFI_LINKOVERLAY = 0x000008000,
            SHGFI_SELECTED = 0x000010000,
            SHGFI_ATTR_SPECIFIED = 0x000020000,
            SHGFI_LARGEICON = 0x000000000,
            SHGFI_SMALLICON = 0x000000001,
            SHGFI_OPENICON = 0x000000002,
            SHGFI_SHELLICONSIZE = 0x000000004,
            SHGFI_PIDL = 0x000000008,
            SHGFI_USEFILEATTRIBUTES = 0x000000010,
            SHGFI_ADDOVERLAYS = 0x000000020,
            SHGFI_OVERLAYINDEX = 0x000000040
        }

        [Flags]
        public enum SHGNO : uint
        {
            SHGDN_NORMAL = 0x0000,                 // Default (display purpose)
            SHGDN_INFOLDER = 0x0001,               // Displayed under a folder (relative)
            SHGDN_FOREDITING = 0x1000,             // For in-place editing
            SHGDN_FORADDRESSBAR = 0x4000,          // UI friendly parsing name (remove ugly stuff)
            SHGDN_FORPARSING = 0x8000,             // Parsing name for ParseDisplayName()
        }

        [Flags]
        public enum STRRET : uint
        {
            STRRET_WSTR = 0,
            STRRET_OFFSET = 0x1,
            STRRET_CSTR = 0x2,
        }

        /// <summary>
        /// Available system image list sizes
        /// </summary>
        public enum SystemImageListSize : int
        {
            /// <summary>
            /// System Large Icon Size (typically 32x32)
            /// </summary>
            LargeIcons = 0x0,
            /// <summary>
            /// System Small Icon Size (typically 16x16)
            /// </summary>
            SmallIcons = 0x1,
            /// <summary>
            /// System Extra Large Icon Size (typically 48x48).
            /// Only available under XP; under other OS the
            /// Large Icon ImageList is returned.
            /// </summary>
            ExtraLargeIcons = 0x2
        }
        [Flags]
        public enum UnDecorateFlags
        {
            UNDNAME_COMPLETE = (0x0000),  // Enable full undecoration
            UNDNAME_NO_LEADING_UNDERSCORES = (0x0001),  // Remove leading underscores from MS extended keywords
            UNDNAME_NO_MS_KEYWORDS = (0x0002),  // Disable expansion of MS extended keywords
            UNDNAME_NO_FUNCTION_RETURNS = (0x0004),  // Disable expansion of return type for primary declaration
            UNDNAME_NO_ALLOCATION_MODEL = (0x0008),  // Disable expansion of the declaration model
            UNDNAME_NO_ALLOCATION_LANGUAGE = (0x0010),  // Disable expansion of the declaration language specifier
            UNDNAME_NO_MS_THISTYPE = (0x0020),  // NYI Disable expansion of MS keywords on the 'this' type for primary declaration
            UNDNAME_NO_CV_THISTYPE = (0x0040),  // NYI Disable expansion of CV modifiers on the 'this' type for primary declaration
            UNDNAME_NO_THISTYPE = (0x0060),  // Disable all modifiers on the 'this' type
            UNDNAME_NO_ACCESS_SPECIFIERS = (0x0080),  // Disable expansion of access specifiers for members
            UNDNAME_NO_THROW_SIGNATURES = (0x0100),  // Disable expansion of 'throw-signatures' for functions and pointers to functions
            UNDNAME_NO_MEMBER_TYPE = (0x0200),  // Disable expansion of 'static' or 'virtual'ness of members
            UNDNAME_NO_RETURN_UDT_MODEL = (0x0400),  // Disable expansion of MS model for UDT returns
            UNDNAME_32_BIT_DECODE = (0x0800),  // Undecorate 32-bit decorated names
            UNDNAME_NAME_ONLY = (0x1000),  // Crack only the name for primary declaration;
            // return just [scope::]name.  Does expand template params
            UNDNAME_NO_ARGUMENTS = (0x2000),  // Don't undecorate arguments to function
            UNDNAME_NO_SPECIAL_SYMS = (0x4000),  // Don't undecorate special names (v-table, vcall, vector xxx, metatype, etc)
        }

        [Flags]
        public enum WindowLongFlags : int
        {
            GWL_EXSTYLE = -20,
            GWLP_HINSTANCE = -6,
            GWLP_HWNDPARENT = -8,
            GWL_ID = -12,
            GWL_STYLE = -16,
            GWL_USERDATA = -21,
            GWL_WNDPROC = -4,
            DWLP_USER = 0x8,
            DWLP_MSGRESULT = 0x0,
            DWLP_DLGPROC = 0x4
        }
    }
}
