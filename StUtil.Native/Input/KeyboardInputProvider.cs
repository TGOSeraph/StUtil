using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.Native.Input
{
    public abstract class KeyboardInputProvider : IKeyboardInputProvider
    {
        private IntPtr handle = IntPtr.Zero;

        public IntPtr Handle
        {
            get
            {
                if (RequiresHandle && handle == IntPtr.Zero)
                {
                    throw new InvalidOperationException("This input provider requires a window handle");
                }
                return handle;
            }
            set
            {
                handle = value;
            }
        }

        private List<Keys> keysDown = new List<Keys>();
        public IEnumerable<Keys> KeysDown
        {
            get { return keysDown; }
        }

        protected abstract bool RequiresHandle
        {
            get;
        }

        public void KeyDown(System.Windows.Forms.Keys key)
        {
            Down(key);
            keysDown.Add(key);
        }

        public void KeyUp(System.Windows.Forms.Keys key)
        {
            Up(key);
            keysDown.Remove(key);
        }

        protected abstract void Down(Keys key);
        protected abstract void Up(Keys key);

        public void KeyPress(params System.Windows.Forms.Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                KeyDown(key);
                KeyUp(key);
            }
        }

        public void ModifiedKeyPress(System.Windows.Forms.Keys key, params System.Windows.Forms.Keys[] modifiers)
        {
            for (int i = 0; i < modifiers.Length; i++)
            {
                KeyDown(modifiers[i]);
            }

            KeyPress(key);

            for (int i = modifiers.Length - 1; i >= 0; i--)
            {
                KeyUp(modifiers[i]);
            }
        }

        public void Type(string message)
        {
            foreach (char c in message)
            {
                var keys = CharToKey(c);
                ModifiedKeyPress(keys.First(), keys.Skip(1).ToArray());
            }
        }

        protected List<Keys> CharToKey(char c)
        {
            List<Keys> keys = new List<Keys>();

            short vkey = StUtil.Native.Internal.NativeMethods.VkKeyScan(c);
            keys.Add((Keys)(vkey & 0xff));

            int modifiers = vkey >> 8;

            if (modifiers == 6)
            {
                keys.Add(Keys.RMenu);
            }
            else
            {
                if ((modifiers & 1) != 0) keys.Add(Keys.ShiftKey);
                if ((modifiers & 2) != 0) keys.Add(Keys.ControlKey);
                if ((modifiers & 4) != 0) keys.Add(Keys.Alt);
            }
            return keys;
        }


    }
}
