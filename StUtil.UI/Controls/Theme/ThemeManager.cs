using StUtil.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace StUtil.UI.Controls.Theme
{
    public static class ThemeManager
    {
        private class MessageHandlerWnd : NativeWindow
        {
            public event EventHandler ColorizationChanged;

            public MessageHandlerWnd()
            {
                base.CreateHandle(new CreateParams());
            }

            protected override void WndProc(ref Message m)
            {
                const int WM_DWMCOLORIZATIONCOLORCHANGED = 0x320;
                switch (m.Msg)
                {
                    case WM_DWMCOLORIZATIONCOLORCHANGED:
                        ColorizationChanged.RaiseEvent(this);
                        break;
                }
                base.WndProc(ref m);
            }
        }

        public enum Style
        {
            LightDark = MediumDark - 1,
            MediumDark = Dark - 1,
            Dark = Darker - 1,
            Darker = Darkest - 1,
            Darkest = Black - 1,
            Black = Main - 1,

            Main = 0,

            White = Main + 1,
            Lightest = White + 1,
            Lighter = Lightest + 1,
            Light = Lighter + 1,
            MediumLight = Light + 1,
            DarkLight = MediumLight + 1,


            Highlight = -999,
            Custom = -9999,
            Text = -99999
        }

        [StructLayout(LayoutKind.Explicit)]
        struct DWMCOLORIZATIONPARAMS
        {
            [FieldOffset(0)]
            public UInt32 ColorizationColor;
            [FieldOffset(4)]
            public UInt32 ColorizationAfterglow;
            [FieldOffset(8)]
            public UInt32 ColorizationColorBalance;
            [FieldOffset(12)]
            public UInt32 ColorizationAfterglowBalance;
            [FieldOffset(16)]
            public UInt32 ColorizationBlurBalance;
            [FieldOffset(20)]
            public UInt32 ColorizationGlassReflectionIntensity;
            [FieldOffset(24)]
            public UInt32 ColorizationOpaqueBlend;
        }

        public static event EventHandler ColorizationChanged;

        private static MessageHandlerWnd handler;
        private static Color highlightColor;
        private static StUtil.Reflection.ReflectionHelper reflector;
        private static List<Color> dark;
        private static List<Color> light;

        private static Color? overrideThemeColor = null;
        public static Color? OverrideThemeColor
        {
            get
            {
                return overrideThemeColor;
            }
            set
            {
                if (overrideThemeColor != value)
                {
                    overrideThemeColor = value;
                    GetColors();
                }
            }
        }

        static ThemeManager()
        {
            reflector = new StUtil.Reflection.ReflectionHelper(typeof(ThemeManager));
            handler = new MessageHandlerWnd();
            handler.ColorizationChanged += handler_ColorizationChanged;

            dark = new List<Color>()
            {
                Color.Black, //Black
                Color.FromArgb(19, 19, 22), //Darkest
                Color.FromArgb(36, 36, 41), //Darker
                Color.FromArgb(46, 46, 52), //Dark
                Color.FromArgb(53, 53, 60), //MediumDark
                Color.FromArgb(94, 94, 106), //LightDark
            };
            light = new List<Color>()
            {
                Color.White,//White
                Color.FromArgb(188, 188, 196), //Lightest
                Color.FromArgb(162, 162, 174), //Lighter
                Color.FromArgb(150, 150, 162), //Light
                Color.FromArgb(140, 140, 151), //MediumLight
                Color.FromArgb(128, 128, 135), //DarkLight
            };
            GetColors();
        }
        public static Color GetContrasting(Color color)
        {
            return GetContrasting(color, Color.Black, Color.White);
        }
        public static Color GetContrasting(Color color, Color dark, Color light)
        {
            return (((color.R * 299) + (color.G * 587) + (color.B * 114)) / 1000 >= 128)
                ? dark
                : light;
        }

        public static Color? GetThemeColor(Style style)
        {
            if (style == Style.Custom)
            {
                return null;
            }
            else if (style == Style.Highlight)
            {
                return highlightColor;
            }
            else if (style == Style.Text)
            {
                return GetContrasting(highlightColor);
            }
            bool isDark = (highlightColor.R + highlightColor.G + highlightColor.B) < 250;
            if (style == Style.Main)
            {
                if (isDark)
                {
                    return Color.FromArgb(46, 46, 52);
                }
                else
                {
                    return Color.FromArgb(242, 242, 243);
                }
            }

            int s = (int)style;

            if (isDark)
            {
                if (s < 0)
                {
                    return light[-s - 1];
                }
                else
                {
                    return dark[s - 1];
                }
            }
            else
            {
                if (s < 0)
                {
                    return dark[-s - 1];
                }
                else
                {
                    return light[s - 1];
                }
            }
        }

        public static Color GetThemeColor(Style style, Color current)
        {
            return GetThemeColor(style) ?? current;
        }

        [DllImport("dwmapi.dll", EntryPoint = "#127", PreserveSig = false)]
        private static extern void DwmGetColorizationParameters(out DWMCOLORIZATIONPARAMS parameters);
        private static Color GetColor(uint part, bool opaque)
        {
            return Color.FromArgb(
                (byte)(opaque ? 255 : part >> 24),
                (byte)(part >> 16),
                (byte)(part >> 8),
                (byte)part
            );
        }

        private static void GetColors()
        {
            if (overrideThemeColor.HasValue)
            {
                highlightColor = overrideThemeColor.Value;
            }
            else
            {
                DWMCOLORIZATIONPARAMS p;
                DwmGetColorizationParameters(out p);

                highlightColor = GetColor(p.ColorizationColor, true);
            }
            ColorizationChanged.RaiseEvent(null);
        }

        private static void handler_ColorizationChanged(object sender, EventArgs e)
        {
            GetColors();
        }
    }
}