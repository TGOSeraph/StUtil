using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using StUtil.Native.Keyboard;

namespace StUtil.Native.Keyboard
{
    public class KeyboardHookHotKeys
    {
        public KeyboardHook Listener { get; private set; }
        public List<HotKeyHandler> Handlers { get; set; }

        public KeyboardHookHotKeys(KeyboardHook listener)
        {
            this.Handlers = new List<HotKeyHandler>();
            this.Listener = listener;
            this.Listener.Asynchronous = false;
            this.Listener.KeyDown += new EventHandler<RawKeyEventArgs>(Listener_KeyDown);
            this.Listener.KeyUp += new EventHandler<RawKeyEventArgs>(Listener_KeyUp);
        }

        private HotKeyHandler FindHandler(Keys key)
        {
            var keys = Listener.KeysDown.Contains(key) ? Listener.KeysDown : Listener.KeysDown.Concat(new Keys[] { key });
            return this.Handlers.FirstOrDefault(h => h.Keys.Count == keys.Count() && h.Keys.All(k => keys.Contains(k)));
        }

        private void Listener_KeyUp(object sender, RawKeyEventArgs args)
        {
            HotKeyHandler handler = FindHandler(args.Key);
            if (handler != null)
            {
                args.Handled = handler.Handle(HotKeyHandler.KeyState.Up);
            }
        }

        private void Listener_KeyDown(object sender, RawKeyEventArgs args)
        {
            HotKeyHandler handler = FindHandler(args.Key);
            if (handler != null)
            {
                args.Handled = handler.Handle(HotKeyHandler.KeyState.Down);
            }
        }

        public static string KeyToString(Keys key)
        {
            switch (key)
            {
                //letters
                case Keys.A:
                case Keys.B:
                case Keys.C:
                case Keys.D:
                case Keys.E:
                case Keys.F:
                case Keys.G:
                case Keys.H:
                case Keys.I:
                case Keys.J:
                case Keys.K:
                case Keys.L:
                case Keys.M:
                case Keys.N:
                case Keys.O:
                case Keys.P:
                case Keys.Q:
                case Keys.R:
                case Keys.S:
                case Keys.T:
                case Keys.U:
                case Keys.V:
                case Keys.W:
                case Keys.X:
                case Keys.Y:
                case Keys.Z:
                    return Enum.GetName(typeof(Keys), key);

                //digits
                case Keys.D0:
                    return "0";
                case Keys.NumPad0:
                    return "Number Pad 0";
                case Keys.D1:
                    return "1";
                case Keys.NumPad1:
                    return "Number Pad 1";
                case Keys.D2:
                    return "2";
                case Keys.NumPad2:
                    return "Number Pad 2";
                case Keys.D3:
                    return "3";
                case Keys.NumPad3:
                    return "Number Pad 3";
                case Keys.D4:
                    return "4";
                case Keys.NumPad4:
                    return "Number Pad 4";
                case Keys.D5:
                    return "5";
                case Keys.NumPad5:
                    return "Number Pad 5";
                case Keys.D6:
                    return "6";
                case Keys.NumPad6:
                    return "Number Pad 6";
                case Keys.D7:
                    return "7";
                case Keys.NumPad7:
                    return "Number Pad 7";
                case Keys.D8:
                    return "8";
                case Keys.NumPad8:
                    return "Number Pad 8";
                case Keys.D9:
                    return "9";
                case Keys.NumPad9:
                    return "Number Pad 9";

                //punctuation
                case Keys.Add:
                    return "Number Pad +";
                case Keys.Subtract:
                    return "Number Pad -";
                case Keys.Divide:
                    return "Number Pad /";
                case Keys.Multiply:
                    return "Number Pad *";
                case Keys.Space:
                    return "Spacebar";
                case Keys.Decimal:
                    return "Number Pad .";

                //function
                case Keys.F1:
                case Keys.F2:
                case Keys.F3:
                case Keys.F4:
                case Keys.F5:
                case Keys.F6:
                case Keys.F7:
                case Keys.F8:
                case Keys.F9:
                case Keys.F10:
                case Keys.F11:
                case Keys.F12:
                case Keys.F13:
                case Keys.F14:
                case Keys.F15:
                case Keys.F16:
                case Keys.F17:
                case Keys.F18:
                case Keys.F19:
                case Keys.F20:
                case Keys.F21:
                case Keys.F22:
                case Keys.F23:
                case Keys.F24:
                    return Enum.GetName(typeof(Keys), key);

                //navigation
                case Keys.Up:
                    return "Up Arrow";
                case Keys.Down:
                    return "Down Arrow";
                case Keys.Left:
                    return "Left Arrow";
                case Keys.Right:
                    return "Right Arrow";
                case Keys.Prior:
                    return "Page Up";
                case Keys.Next:
                    return "Page Down";
                case Keys.Home:
                    return "Home";
                case Keys.End:
                    return "End";

                //control keys
                case Keys.Back:
                    return "Backspace";
                case Keys.Tab:
                    return "Tab";
                case Keys.Escape:
                    return "Escape";
                case Keys.Enter:
                    return "Enter";
                case Keys.Shift:
                case Keys.ShiftKey:
                    return "Shift";
                case Keys.LShiftKey:
                    return "Shift (Left)";
                case Keys.RShiftKey:
                    return "Shift (Right)";
                case Keys.Control:
                case Keys.ControlKey:
                    return "Control";
                case Keys.LControlKey:
                    return "Control (Left)";
                case Keys.RControlKey:
                    return "Control (Right)";
                case Keys.Menu:
                case Keys.Alt:
                    return "Alt";
                case Keys.LMenu:
                    return "Alt (Left)";
                case Keys.RMenu:
                    return "Alt (Right)";
                case Keys.Pause:
                    return "Pause";
                case Keys.CapsLock:
                    return "Caps Lock";
                case Keys.NumLock:
                    return "Num Lock";
                case Keys.Scroll:
                    return "Scroll Lock";
                case Keys.PrintScreen:
                    return "Print Screen";
                case Keys.Insert:
                    return "Insert";
                case Keys.Delete:
                    return "Delete";
                case Keys.Help:
                    return "Help";
                case Keys.LWin:
                    return "Windows (Left)";
                case Keys.RWin:
                    return "Windows (Right)";
                case Keys.Apps:
                    return "Context Menu";

                //browser keys
                case Keys.BrowserBack:
                    return "Browser Back";
                case Keys.BrowserFavorites:
                    return "Browser Favorites";
                case Keys.BrowserForward:
                    return "Browser Forward";
                case Keys.BrowserHome:
                    return "Browser Home";
                case Keys.BrowserRefresh:
                    return "Browser Refresh";
                case Keys.BrowserSearch:
                    return "Browser Search";
                case Keys.BrowserStop:
                    return "Browser Stop";

                //media keys
                case Keys.VolumeDown:
                    return "Volume Down";
                case Keys.VolumeMute:
                    return "Volume Mute";
                case Keys.VolumeUp:
                    return "Volume Up";
                case Keys.MediaNextTrack:
                    return "Next Track";
                case Keys.Play:
                case Keys.MediaPlayPause:
                    return "Play";
                case Keys.MediaPreviousTrack:
                    return "Previous Track";
                case Keys.MediaStop:
                    return "Stop";
                case Keys.SelectMedia:
                    return "Select Media";

                //IME keys
                case Keys.HanjaMode:
                case Keys.JunjaMode:
                case Keys.HangulMode:
                case Keys.FinalMode:    //duplicate values: Hanguel, Kana, Kanji  
                case Keys.IMEAccept:
                case Keys.IMEConvert:   //duplicate: IMEAceept
                case Keys.IMEModeChange:
                case Keys.IMENonconvert:
                    return null;

                //special keys
                case Keys.LaunchMail:
                    return "Launch Mail";
                case Keys.LaunchApplication1:
                    return "Launch Favorite Application 1";
                case Keys.LaunchApplication2:
                    return "Launch Favorite Application 2";
                case Keys.Zoom:
                    return "Zoom";

                //oem keys 
                case Keys.OemSemicolon: //oem1
                    return ";";
                case Keys.OemQuestion:  //oem2
                    return "?";
                case Keys.Oemtilde:     //oem3
                    return "~";
                case Keys.OemOpenBrackets:  //oem4
                    return "[";
                case Keys.OemPipe:  //oem5
                    return "|";
                case Keys.OemCloseBrackets:    //oem6
                    return "]";
                case Keys.OemQuotes:        //oem7
                    return "'";
                case Keys.OemBackslash: //oem102
                    return "/";
                case Keys.Oemplus:
                    return "+";
                case Keys.OemMinus:
                    return "-";
                case Keys.Oemcomma:
                    return ",";
                case Keys.OemPeriod:
                    return ".";

                //unsupported oem keys
                case Keys.Oem8:
                case Keys.OemClear:
                    return null;

                //unsupported other keys
                case Keys.None:
                case Keys.LButton:
                case Keys.RButton:
                case Keys.MButton:
                case Keys.XButton1:
                case Keys.XButton2:
                case Keys.Clear:
                case Keys.Sleep:
                case Keys.Cancel:
                case Keys.LineFeed:
                case Keys.Select:
                case Keys.Print:
                case Keys.Execute:
                case Keys.Separator:
                case Keys.ProcessKey:
                case Keys.Packet:
                case Keys.Attn:
                case Keys.Crsel:
                case Keys.Exsel:
                case Keys.EraseEof:
                case Keys.NoName:
                case Keys.Pa1:
                case Keys.KeyCode:
                case Keys.Modifiers:
                    return null;

                default:
                    throw new NotSupportedException(Enum.GetName(typeof(Keys), key));
            }
        }

        public static string FormatKeyList(List<Keys> keys)
        {
            keys.Sort();
            keys.Reverse();
            return String.Join("+", keys.Select(k => KeyToString(k)));
        }
    }
}
