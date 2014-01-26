using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StUtil.Native.Keyboard
{
    /// <summary>
    /// Raw KeyEvent arguments.
    /// </summary>
    public class RawKeyEventArgs : EventArgs
    {
        /// <summary>
        /// VKCode of the key.
        /// </summary>
        public int VKCode;

        /// <summary>
        /// Enum Key of the key.
        /// </summary>
        public Keys Key;

        /// <summary>
        /// Is the hitted key system key.
        /// </summary>
        public bool IsSysKey;

        /// <summary>
        /// If the key event was handled by the application
        /// </summary>
        public bool Handled { get; set; }

        /// <summary>
        /// Create raw keyevent arguments.
        /// </summary>
        /// <param name="VKCode"></param>
        /// <param name="isSysKey"></param>
        public RawKeyEventArgs(int VKCode, bool isSysKey)
        {
            this.VKCode = VKCode;
            this.IsSysKey = isSysKey;
            this.Key = (Keys)(VKCode);
            this.Handled = false;
        }
    }
}
