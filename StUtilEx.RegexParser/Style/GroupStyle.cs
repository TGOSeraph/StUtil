using StUtil.UI.Controls.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtilEx.RegexParser.Style
{
    public class GroupStyle : StUtil.UI.Controls.Style.Style
    {
        public static int Interval { get; set; }
        public static int Id { get; set; }

        public int UId { get; set; }

        public override Color BackColor
        {
            get
            {
                return GetColor(UId);
            }
            set
            {
                base.BackColor = value;
            }
        }

        public GroupStyle()
        {
            BackColorOverrides = true;
            Bold = true;
        }

        public override void Apply(StylePart part, StyleRichTextBox textBox)
        {
            UId = Id++;
            base.Apply(part, textBox);

            RegexStylePart p = part as RegexStylePart;
            if (p.Part.Type == StUtilEx.RegexParser.PartType.NamedCapture)
            {
                BasicStylePart style = new BasicStylePart()
                {
                    Index = part.Index + 3,
                    Length = p.Part.ValueStart.IndexOf('>'),
                };
                style.Style.ForeColor = Color.Green;
                style.Style.BackColor = this.BackColor;
                style.Style.Bold = this.Bold;
                style.Style.Apply(style, textBox);
            }
        }

        public static Color GetColor(int index, int steps = 10)
        {
            int mod = 10;

            int div = index % mod;
            int h = 0;

            h = (360 / mod) * div;
            if (index % 2 == 0)
            {
                h = 360 - h;
                index = -index;
            }
            return FromAhsb(255, h + (index / mod), 0.91f, 0.95f);
        }

        public static Color FromAhsb(int alpha, float hue, float saturation, float brightness)
        {
            if (0 > alpha
                || 255 < alpha)
            {
                throw new ArgumentOutOfRangeException(
                    "alpha",
                    alpha,
                    "Value must be within a range of 0 - 255.");
            }

            if (0f > hue
                || 360f < hue)
            {
                throw new ArgumentOutOfRangeException(
                    "hue",
                    hue,
                    "Value must be within a range of 0 - 360.");
            }

            if (0f > saturation
                || 1f < saturation)
            {
                throw new ArgumentOutOfRangeException(
                    "saturation",
                    saturation,
                    "Value must be within a range of 0 - 1.");
            }

            if (0f > brightness
                || 1f < brightness)
            {
                throw new ArgumentOutOfRangeException(
                    "brightness",
                    brightness,
                    "Value must be within a range of 0 - 1.");
            }

            if (0 == saturation)
            {
                return Color.FromArgb(
                                    alpha,
                                    Convert.ToInt32(brightness * 255),
                                    Convert.ToInt32(brightness * 255),
                                    Convert.ToInt32(brightness * 255));
            }

            float fMax, fMid, fMin;
            int iSextant, iMax, iMid, iMin;

            if (0.5 < brightness)
            {
                fMax = brightness - (brightness * saturation) + saturation;
                fMin = brightness + (brightness * saturation) - saturation;
            }
            else
            {
                fMax = brightness + (brightness * saturation);
                fMin = brightness - (brightness * saturation);
            }

            iSextant = (int)Math.Floor(hue / 60f);
            if (300f <= hue)
            {
                hue -= 360f;
            }

            hue /= 60f;
            hue -= 2f * (float)Math.Floor(((iSextant + 1f) % 6f) / 2f);
            if (0 == iSextant % 2)
            {
                fMid = (hue * (fMax - fMin)) + fMin;
            }
            else
            {
                fMid = fMin - (hue * (fMax - fMin));
            }

            iMax = Convert.ToInt32(fMax * 255);
            iMid = Convert.ToInt32(fMid * 255);
            iMin = Convert.ToInt32(fMin * 255);

            switch (iSextant)
            {
                case 1:
                    return Color.FromArgb(alpha, iMid, iMax, iMin);
                case 2:
                    return Color.FromArgb(alpha, iMin, iMax, iMid);
                case 3:
                    return Color.FromArgb(alpha, iMin, iMid, iMax);
                case 4:
                    return Color.FromArgb(alpha, iMid, iMin, iMax);
                case 5:
                    return Color.FromArgb(alpha, iMax, iMin, iMid);
                default:
                    return Color.FromArgb(alpha, iMax, iMid, iMin);
            }
        }
    }
}
