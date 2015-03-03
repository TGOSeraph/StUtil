namespace StUtil.Native.Internal
{
    public static partial class NativeEnums
    {
        public enum BM : uint
        {
            GETCHECK = 0x00F0,
            SETCHECK = 0x00F1,
            GETSTATE = 0x00F2,
            SETSTATE = 0x00F3,
            SETSTYLE = 0x00F4,
            CLICK = 0x00F5,
            GETIMAGE = 0x00F6,
            SETIMAGE = 0x00F7
        }
        public enum TPM
        {
            LEFTBUTTON = 0x0,
            RIGHTBUTTON = 0x2,
            LEFTALIGN = 0x0,
            CENTERALIGN = 0x4,
            RIGHTALIGN = 0x8,
            TOPALIGN = 0x0,
            VCENTERALIGN = 0x10,
            BOTTOMALIGN = 0x20,
            HORIZONTAL = 0x0,
            VERTICAL = 0x40,
            NONOTIFY = 0x80,
            RETURNCMD = 0x100
        }
    }
}