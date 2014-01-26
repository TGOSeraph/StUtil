using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace StUtil.Native.Keyboard
{
    public class HotKeyHandler
    {
        public enum KeyState
        {
            Up,
            Down
        }

        public List<Keys> Keys { get; set; }
        public Func<HotKeyHandler, KeyState, bool> Handler { get; set; }
        public Object Tag { get; set; }

        public HotKeyHandler(Func<HotKeyHandler, KeyState, bool> handler, params Keys[] keys)
        {
            this.Keys = keys.ToList();
            this.Handler = handler;
        }

        public HotKeyHandler(params Keys[] keys)
        {
            this.Keys = keys.ToList();
        }

        public HotKeyHandler(List<Keys> keys, Func<HotKeyHandler, KeyState, bool> handler)
        {
            this.Keys = keys;
            this.Handler = handler;
        }

        public bool Handle(KeyState state)
        {
            if (Handler != null)
            {
                return Handler(this, state);
            }
            return false;
        }
    }
}
