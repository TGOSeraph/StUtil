using System;

namespace StUtil.Native.Internal
{
    public static partial class NativeEnums
    {
        [Flags]
        public enum MouseEventFlags : uint
        {
            Move = 0x0001,
            LeftDown = 0x0002,
            LeftUp = 0x0004,
            RightDown = 0x0008,
            RightUp = 0x0010,
            MiddleDown = 0x0020,
            MiddleUp = 0x0040,
            Absolute = 0x8000
        }
    }
}