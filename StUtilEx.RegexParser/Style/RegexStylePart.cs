using StUtil.UI.Controls.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtilEx.RegexParser.Style
{
    public class RegexStylePart : StylePart
    {
        public static Dictionary<PartType, StUtil.UI.Controls.Style.Style> Styles = new Dictionary<PartType, StUtil.UI.Controls.Style.Style>();
        public RegexPart Part { get; set; }
        public static StUtil.UI.Controls.Style.Style ErrorStyle { get; set; }

        public override StUtil.UI.Controls.Style.Style Style
        {
            get
            {
                if (Part.Error != ErrorType.None)
                {
                    return ErrorStyle;
                }
                return Styles[Part.Type];
            }
        }

        static RegexStylePart()
        {
            StUtil.UI.Controls.Style.Style error = new StUtil.UI.Controls.Style.Style()
            {
                BackColor = Color.Red,
                BackColorOverrides = true
            };
            StUtil.UI.Controls.Style.Style def = new StUtil.UI.Controls.Style.Style();

            StUtil.UI.Controls.Style.Style keyWord = new StUtil.UI.Controls.Style.Style()
            {
                ForeColor = Color.CadetBlue
            };
            StUtil.UI.Controls.Style.Style op = new StUtil.UI.Controls.Style.Style()
            {
                ForeColor = Color.CadetBlue,
                Bold = true
            };
            StUtil.UI.Controls.Style.Style escape = new StUtil.UI.Controls.Style.Style()
            {
                ForeColor = Color.Orange
            };
            StUtil.UI.Controls.Style.Style backref = new StUtil.UI.Controls.Style.Style()
            {
                ForeColor = Color.CadetBlue,
                Bold = true
            };
            StUtil.UI.Controls.Style.Style set = new StUtil.UI.Controls.Style.Style()
            {
                BackColor = Color.LightYellow, //Color.FromArgb(222, 255, 225),
                Bold = true,
                BackColorOverrides = true
            };
            StUtil.UI.Controls.Style.Style quantifier = new StUtil.UI.Controls.Style.Style()
            {
                ForeColor = Color.Purple
            };

            StUtil.UI.Controls.Style.Style group = new GroupStyle();

            StUtil.UI.Controls.Style.Style nonCapturing = new GroupStyle()
            {
                ForeColor = Color.Gray
            };

            StUtil.UI.Controls.Style.Style comment = new StUtil.UI.Controls.Style.Style()
            {
                ForeColor = Color.DarkGreen,
                ForeColorOverrides = true,
                Bold = true
            };

            ErrorStyle = error;
            Styles.Add(PartType.Undefined, error);
            Styles.Add(PartType.Root, def);
            Styles.Add(PartType.Text, def);
            Styles.Add(PartType.DefinedEscaped, keyWord);
            Styles.Add(PartType.OctalCharacter, escape);
            Styles.Add(PartType.HexCharacter, escape);
            Styles.Add(PartType.ControlCharacter, escape);
            Styles.Add(PartType.UnicodeCharacter, escape);
            Styles.Add(PartType.EscapedCharacter, escape);
            Styles.Add(PartType.Backreference, backref);
            Styles.Add(PartType.CharacterGroup, set);
            Styles.Add(PartType.NegatedCharacterGroup, set);
            Styles.Add(PartType.NegateCharacterGroup, keyWord);
            Styles.Add(PartType.ExactQuantifier, quantifier);
            Styles.Add(PartType.AtLeastQuantifier, quantifier);
            Styles.Add(PartType.RangeQuantifier, quantifier);
            Styles.Add(PartType.AtLeastMinimumQuantifier, quantifier);
            Styles.Add(PartType.MinimumRangeQuantifier, quantifier);
            Styles.Add(PartType.Operator, op);
            Styles.Add(PartType.BackspaceEscaped, keyWord);
            Styles.Add(PartType.NamedBackreference, backref);
            Styles.Add(PartType.Group, group);
            Styles.Add(PartType.NegativeLookahead, group);
            Styles.Add(PartType.PositiveLookbehind, group);
            Styles.Add(PartType.NegativeLookbehind, group);
            Styles.Add(PartType.Greedy, group);
            Styles.Add(PartType.NamedCapture, group);
            Styles.Add(PartType.PositiveLookahead, group);
            Styles.Add(PartType.NonCapturing, nonCapturing);
            Styles.Add(PartType.Comment, comment);
            Styles.Add(PartType.Invalid, error);
            Styles.Add(PartType.InvalidQuantifier, error);
        }

        public RegexStylePart(RegexPart part)
        {
            this.Part = part;
        }
    }
}
