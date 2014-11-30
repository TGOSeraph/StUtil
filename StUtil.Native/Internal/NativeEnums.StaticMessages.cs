using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Native.Internal
{
    public static partial class NativeEnums
    {
        public enum STM : uint
        {
            /// <summary>
            /// An application sends the STM_GETICON message to retrieve a handle to the icon associated with a static control that has the SS_ICON style.
            /// </summary>
            STM_GETICON = 0x0171,
            /// <summary>
            /// An application sends an STM_GETIMAGE message to retrieve a handle to the image (icon or bitmap) associated with a static control.
            /// </summary>
            /// <remarks>
            /// wParam - Specifies the type of image to retrieve. This parameter can be one of the following values:
            /// IMAGE_BITMAP
            /// IMAGE_CURSOR
            /// IMAGE_ENHMETAFILE
            /// IMAGE_ICON
            /// </remarks>
            STM_GETIMAGE = 0x0173,
            /// <summary>
            /// An application sends the STM_SETICON message to associate an icon with an icon control.
            /// </summary>
            /// <remarks>
            /// wParam - Handle to the icon to associate with the icon control.
            /// </remarks>
            STM_SETICON = 0x0170,
            /// <summary>
            /// An application sends an STM_SETIMAGE message to associate a new image with a static control.
            /// </summary>
            /// <remarks>
            /// wParam - Specifies the type of image to associate with the static control. This parameter can be one of the following values:
            /// IMAGE_BITMAP
            /// IMAGE_CURSOR
            /// IMAGE_ENHMETAFILE
            /// IMAGE_ICON
            /// lParam - Handle to the image to associate with the static control.
            /// </remarks>
            STM_SETIMAGE = 0x0172
        }

        public enum STMImageTypes
        {
            IMAGE_BITMAP = 0,
            IMAGE_CURSOR = 2,
            IMAGE_ENHMETAFILE = 3,
            IMAGE_ICON = 1
        }
    }
}
