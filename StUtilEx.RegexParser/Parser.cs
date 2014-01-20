using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StUtilEx.RegexParser
{
    public class Parser
    {
        private static Dictionary<string, Regex> regexCache = new Dictionary<string, Regex>();

        private bool EscapeNext;
        private RegexPart CurrentToken;
        private int Index;
        private string Text;

        private RegexPart AddChildNode(int index)
        {
            if (CurrentToken.Type == PartType.Text)
            {
                CurrentToken = CurrentToken.Parent;
            }

            CurrentToken = new RegexPart(CurrentToken);
            CurrentToken.Index = index;
            return CurrentToken;
        }

        private RegexPart EndChildNode()
        {
            CurrentToken = CurrentToken.Parent;
            return CurrentToken;
        }

        private bool MatchPattern(string pattern, ref string output)
        {
            Regex regex;
            if (regexCache.ContainsKey(pattern))
            {
                regex = regexCache[pattern];
            }
            else
            {
                regex = new Regex("^(" + pattern + ")");
                regexCache[pattern] = regex;
            }

            Match m = regex.Match(Text.Substring(Index - 1));
            if (m.Success)
            {
                output = m.Groups[1].Captures[0].Value;
                Index += output.Length - 1;
                return true;
            }
            return false;
        }

        private void HandleEscapes(char c)
        {
            RegexPart token = AddChildNode(Index - 2);
            string value = c.ToString();
            switch (c)
            {
                case 'b':
                    if (token.Parent.Type == PartType.CharacterGroup || token.Parent.Type == PartType.NegatedCharacterGroup)
                    {
                        token.Type = PartType.BackspaceEscaped;
                    }
                    else
                    {
                        token.Type = PartType.DefinedEscaped;
                    }
                    break;

                case 'A':
                case 'Z':
                case 'z':
                case 'G':
                case 'B':
                case 'a':
                case 't':
                case 'r':
                case 'v':
                case 'f':
                case 'n':
                case 'e':
                    token.Type = PartType.DefinedEscaped;
                    break;

                case 'x':
                    if (MatchPattern(@"x[0-9A-Fa-f]{2}", ref value))
                    {
                        token.Type = PartType.HexCharacter;
                    }
                    else
                    {
                        token.Type = PartType.EscapedCharacter;
                    }
                    break;

                case 'u':
                    if (MatchPattern(@"u[0-9A-Fa-f]{4}", ref value))
                    {
                        token.Type = PartType.UnicodeCharacter;
                    }
                    else
                    {
                        token.Type = PartType.EscapedCharacter;
                    }
                    break;

                case 'c':
                    if (Index < Text.Length)
                    {
                        value = "c" + Text[Index++].ToString();
                        token.Type = PartType.ControlCharacter;
                    }
                    else
                    {
                        value = "c";
                        token.Type = PartType.EscapedCharacter;
                    }
                    break;

                case 'k':
                    if (MatchPattern(@"k<[^>]+>", ref value))
                    {
                        token.Type = PartType.NamedBackreference;
                    }
                    else
                    {
                        token.Type = PartType.EscapedCharacter;
                    }
                    break;

                default:
                    if (MatchPattern(@"\d{1,3}", ref value))
                    {
                        if (value.Length == 1)
                        {
                            token.Type = PartType.Backreference;
                        }
                        else
                        {
                            token.Type = PartType.OctalCharacter;
                        }
                    }
                    else
                    {
                        token.Type = PartType.EscapedCharacter;
                    }
                    break;
            }
            token.ValueStart = "\\" + value;
            CurrentToken = CurrentToken.Parent;
            EscapeNext = false;
        }

        private void HandleOperators(char c)
        {
            AddChildNode(Index - 1);

            string v = c.ToString();
            switch (c)
            {
                case '*':
                    if (Index < Text.Length && Text[Index] == '?')
                    {
                        v += "?";
                        Index++;
                    }
                    break;

                case '+':
                    if (Index < Text.Length && Text[Index] == '?')
                    {
                        v += "?";
                        Index++;
                    }
                    break;

                case '?':
                    if (Index < Text.Length && Text[Index] == '?')
                    {
                        v += "?";
                        Index++;
                    }
                    break;
            }

            CurrentToken.Type = PartType.Operator;
            CurrentToken.ValueStart = v;
            CurrentToken = CurrentToken.Parent;
        }

        private void HandleClass()
        {
            /* Handles
             * [A-Z...]
             * [^A-Z...]
             */
            AddChildNode(Index - 1);

            CurrentToken.ValueStart = "[";
            if (Index < Text.Length && Text[Index] == '^')
            {
                CurrentToken.Type = PartType.NegatedCharacterGroup;
                CurrentToken = AddChildNode(Index);
                CurrentToken.Type = PartType.NegateCharacterGroup;
                CurrentToken.ValueStart = "^";
                CurrentToken = CurrentToken.Parent;
                Index++;
            }
            else
            {
                CurrentToken.Type = PartType.CharacterGroup;
            }

            for (; Index < Text.Length; )
            {
                char c = Text[Index++];
                if (!EscapeNext && c == ']')
                {
                    if (CurrentToken.Type != PartType.CharacterGroup && CurrentToken.Type != PartType.NegatedCharacterGroup)
                    {
                        CurrentToken = CurrentToken.Parent;
                    }
                    CurrentToken.ValueEnd = "]";
                    break;
                }
                else
                {
                    HandleCharacter(c);
                }
            }
            if (CurrentToken.Type != PartType.CharacterGroup && CurrentToken.Type != PartType.NegatedCharacterGroup)
            {
                CurrentToken = CurrentToken.Parent;
            }
            if (CurrentToken.ValueEnd != "]")
            {
                CurrentToken.Error = ErrorType.MissingClosingPairing;
            }
            CurrentToken = CurrentToken.Parent;
        }

        private void HandleQuantifier()
        {
            /*
             * Handles:
             * {n}
             * {n,}
             * {n,m}
             * {n}?
             * {n,}?
             * {n,m}?
             */

            RegexPart start = CurrentToken;
            if (start.Type == PartType.Text)
            {
                start = start.Parent;
            }
            AddChildNode(Index - 1);
            Index++;
            string value = "";
            if (MatchPattern(@"\d+,\d+\}\?", ref value))
            {
                CurrentToken.Type = PartType.MinimumRangeQuantifier;
            }
            else if (MatchPattern(@"\d+,\d+\}", ref value))
            {
                CurrentToken.Type = PartType.RangeQuantifier;
            }
            else if (MatchPattern(@"\d+,}\?", ref value))
            {
                CurrentToken.Type = PartType.AtLeastMinimumQuantifier;
            }
            else if (MatchPattern(@"\d+,}", ref value))
            {
                CurrentToken.Type = PartType.AtLeastQuantifier;
            }
            else if (MatchPattern(@"\d+}\??", ref value))
            {
                CurrentToken.Type = PartType.ExactQuantifier;
            }
            else if (MatchPattern(@"[^\}]+}\??", ref value))
            {
                CurrentToken.Type = PartType.InvalidQuantifier;
                AddChildNode(Index - value.Length);
                CurrentToken.ValueStart = value.Substring(0, value.Length - 1);
                CurrentToken.Error = ErrorType.ExpectedNumber;
                CurrentToken.Type = PartType.Text;
                CurrentToken = CurrentToken.Parent;
                CurrentToken.ValueEnd = value.Substring(value.LastIndexOf("}"));
                value = "";
            }
            else
            {
                CurrentToken.Type = PartType.InvalidQuantifier;
                CurrentToken.Error = ErrorType.MissingClosingPairing;
                Index--;
            }
            CurrentToken.ValueStart = "{" + value;
            //Restore the last token
            CurrentToken = start;
        }

        private void HandleGroup()
        {
            /*
             * Handles
             * (?#comment)
             * (exp)
             * (?<name>exp)
             * (?:exp)
             * (?=exp) PositiveLookahead
             * (?!exp) NegativeLookahead
             * (?<=exp) PositiveLookbehind
             * (?<!exp) NegativeLookbehind
             * (?>exp) Greedy
             */

            string value = "";
            string format = @"\((?:\?(?:{0}))";
            if (CurrentToken.Type == PartType.Text)
            {
                CurrentToken = CurrentToken.Parent;
            }
            RegexPart start = CurrentToken;
            AddChildNode(Index);

            //Match the different patterns
            if (MatchPattern(string.Format(format, "#"), ref value))
            {
                CurrentToken.Type = PartType.Comment;
            }
            else if (MatchPattern(string.Format(format, ":"), ref value))
            {
                CurrentToken.Type = PartType.NonCapturing;
            }
            else if (MatchPattern(string.Format(format, "="), ref value))
            {
                CurrentToken.Type = PartType.PositiveLookahead;
            }
            else if (MatchPattern(string.Format(format, "!"), ref value))
            {
                CurrentToken.Type = PartType.NegativeLookahead;
            }
            else if (MatchPattern(string.Format(format, "<="), ref value))
            {
                CurrentToken.Type = PartType.PositiveLookbehind;
            }
            else if (MatchPattern(string.Format(format, "<!"), ref value))
            {
                CurrentToken.Type = PartType.NegativeLookbehind;
            }
            else if (MatchPattern(string.Format(format, ">"), ref value))
            {
                CurrentToken.Type = PartType.Greedy;
            }
            else if (MatchPattern(string.Format(format, "<[^>]+>"), ref value))
            {
                CurrentToken.Type = PartType.NamedCapture;
            }
            else
            {
                CurrentToken.Type = PartType.Group;
                value = "(";
            }

            //Move the index back to the beginning of the match
            CurrentToken.Index = Index - value.Length;
            CurrentToken.ValueStart = value;

            for (; Index < Text.Length; )
            {
                char c = Text[Index++];
                if (!EscapeNext && c == ')')
                {
                    //If we are closing the group, but we are not currently on a group node, go the the parent
                    if (!CurrentToken.IsGroup())
                    {
                        CurrentToken = CurrentToken.Parent;
                    }
                    CurrentToken.ValueEnd = ")";

                    break;
                }
                else
                {
                    HandleCharacter(c);
                }
            }
            if (start.Parts[start.Parts.Count - 1].ValueEnd != ")")
            {
                start.Parts[start.Parts.Count - 1].Error = ErrorType.MissingClosingPairing;
            }
            CurrentToken = start;
        }

        private void HandleCharacter(char c)
        {
            if (EscapeNext)
            {
                HandleEscapes(c);
            }
            else
            {
                switch (c)
                {
                    case '\\':
                        EscapeNext = true;
                        break;

                    case '(':
                        HandleGroup();
                        break;

                    case '{':
                        HandleQuantifier();
                        break;

                    case '[':
                        HandleClass();
                        break;

                    case ')':
                    case ']':
                    case '}':
                        AddChildNode(Index - 1);
                        CurrentToken.ValueStart += c;
                        CurrentToken.Error = ErrorType.MissingOpeningPairing;
                        CurrentToken.Type = PartType.Invalid;
                        CurrentToken = CurrentToken.Parent;
                        break;

                    case '^':
                    case '$':
                    case '?':
                    case '+':
                    case '*':
                    case '|':
                    case '.':
                        HandleOperators(c);
                        break;

                    default:
                        //Handle generic text
                        if (CurrentToken.Type != PartType.Text)
                        {
                            AddChildNode(Index - 1);
                            CurrentToken.Type = PartType.Text;
                        }
                        CurrentToken.ValueStart += c;
                        break;
                }
            }
        }

        public RegexPart Parse(string text)
        {
            this.Text = text;
            this.EscapeNext = false;

            CurrentToken = new RegexPart(null) { Type = PartType.Root };
            for (Index = 0; Index < Text.Length; )
            {
                HandleCharacter(Text[Index++]);
            }
            RegexPart p = CurrentToken;
            while (p.Parent != null && p.Type != PartType.Root)
                p = p.Parent;
            return p;
        }
    }
}
