using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace StUtil.UI.Controls
{
    [Serializable]
    public class DrawingAddedEventArgs : EventArgs
    {
        public System.Drawing.Drawing2D.GraphicsPath Path;

        public DrawingAddedEventArgs(System.Drawing.Drawing2D.GraphicsPath path)
        {
            this.Path = path;
        }
    }
}
