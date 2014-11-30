namespace StUtil.Native.Internal
{
    public static partial class NativeEnums
    {
        public enum PBM : uint
        {
            /// <summary>
            /// Advances the current position of a progress bar by a specified increment and redraws the bar to reflect the new position.
            /// </summary>
            /// <remarks>
            /// wParam - Amount to advance the position. lParam - Must be zero.
            /// </remarks>
            PBM_DELTAPOS = WM.USER + 3,

            /// <summary>
            /// Gets the color of the progress bar.
            /// </summary>
            PBM_GETBARCOLOR = WM.USER + 15,

            /// <summary>
            /// Gets the background color of the progress bar.
            /// </summary>
            PBM_GETBKCOLOR = 0x040E,

            /// <summary>
            /// Retrieves the current position of the progress bar.
            /// </summary>
            PBM_GETPOS = WM.USER + 8,

            /// <summary>
            /// Retrieves information about the current high and low limits of a given progress bar control.
            /// </summary>
            /// <remarks>
            /// wParam - Flag value specifying which limit value is to be used as the message's return value. This parameter can be one of the following values:
            /// TRUE - Return the low limit.
            /// FALSE - Return the high limit.
            /// lParam - Pointer to a PBRANGE structure that is to be filled with the high and low limits of the progress bar control. If this parameter is set to NULL, the control will return only the limit specified by wParam.
            /// </remarks>
            PBM_GETRANGE = WM.USER + 7,

            /// <summary>
            /// Gets the state of the progress bar.
            /// </summary>
            PBM_GETSTATE = 0x0411,

            /// <summary>
            /// Retrieves the step increment from a progress bar. The step increment is the amount by which the progress bar increases its current position whenever it receives a PBM_STEPIT message. By default, the step increment is set to 10.
            /// </summary>
            PBM_GETSTEP = 0x040D,

            /// <summary>
            /// Sets the color of the progress indicator bar in the progress bar control.
            /// </summary>
            /// <remarks>
            /// wParam Must be zero.
            /// lParam The COLORREF value that specifies the new progress indicator bar color. Specifying the CLR_DEFAULT value causes the progress bar to use its default progress indicator bar color.
            /// </remarks>
            PBM_SETBARCOLOR = WM.USER + 9,

            /// <summary>
            /// Sets the background color in the progress bar.
            /// </summary>
            /// <remarks>
            /// wParam Must be zero.
            /// lParam COLORREF value that specifies the new background color. Specify the CLR_DEFAULT value to cause the progress bar to use its default background color.
            /// </remarks>
            PBM_SETBKCOLOR = 0x2001,

            ///<summary>
            ///Sets the progress bar to marquee mode. This causes the progress bar to move like a marquee.
            /// </summary>
            /// <remarks>
            /// wParam Indicates whether to turn the marquee mode on or off.
            /// lParam Time, in milliseconds, between marquee animation updates. If this parameter is zero, the marquee animation is updated every 30 milliseconds.
            /// </remarks>
            PBM_SETMARQUEE = WM.USER + 10,

            /// <summary>
            /// Sets the current position for a progress bar and redraws the bar to reflect the new position.
            /// </summary>
            /// <remarks>
            /// wParam - Signed integer that becomes the new position.
            /// lParam -Must be zero.
            /// </remarks>
            PBM_SETPOS = WM.USER + 2,

            /// <summary>
            /// Sets the minimum and maximum values for a progress bar and redraws the bar to reflect the new range.
            /// </summary>
            /// <remarks>
            /// wParam - Must be zero.
            /// lParam - The LOWORD specifies the minimum range value, and the HIWORD specifies the maximum range value. The minimum range value must not be negative. By default, the minimum value is zero. The maximum range value must be greater than the minimum range value. By default, the maximum range value is 100.
            /// </remarks>
            PBM_SETRANGE = WM.USER + 1,

            /// <summary>
            /// Sets the minimum and maximum values for a progress bar to 32-bit values, and redraws the bar to reflect the new range.
            /// </summary>
            /// <remarks>
            /// wParam - Minimum range value. By default, the minimum value is zero.
            /// lParam - Maximum range value. This value must be greater than wParam. By default, the maximum value is 100.
            /// </remarks>
            PBM_SETRANGE32 = WM.USER + 6,

            /// <summary>
            /// Sets the state of the progress bar.
            /// </summary>
            /// <remarks>
            /// State of the progress bar that is being set. One of the values in the <see cref="ProgressBarState"/> enum
            /// </remarks>
            PBM_SETSTATE = 0x0410,

            /// <summary>
            /// Specifies the step increment for a progress bar. The step increment is the amount by which the progress bar increases its current position whenever it receives a PBM_STEPIT message. By default, the step increment is set to 10.
            /// </summary>
            /// <remarks>
            /// wParam - New step increment.
            /// lParam - Must be zero.
            /// </remarks>
            PBM_SETSTEP = WM.USER + 4,

            /// <summary>
            /// Advances the current position for a progress bar by the step increment and redraws the bar to reflect the new position. An application sets the step increment by sending the PBM_SETSTEP message.
            /// </summary>
            PBM_STEPIT = WM.USER + 5
        }

        public enum ProgressBarState
        {
            /// <summary>
            /// The normal color, usually green
            /// </summary>
            Normal = 1,

            /// <summary>
            /// The error color, usually red
            /// </summary>
            Error = 2,

            /// <summary>
            /// The paused color, usually yellow
            /// </summary>
            Paused = 3
        };
    }
}