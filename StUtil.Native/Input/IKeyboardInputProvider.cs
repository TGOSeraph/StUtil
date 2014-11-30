using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.Native.Input
{
    public interface IKeyboardInputProvider
    {
        IntPtr Handle { get; set; }
        IEnumerable<Keys> KeysDown { get; }

        void KeyDown(Keys key);
        void KeyUp(Keys key);
        void KeyPress(params Keys[] keys);
        void ModifiedKeyPress(Keys key, params Keys[] modifiers);
        void Type(string message);
    }
}
