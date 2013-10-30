using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Parser
{
    public abstract class BoundedStringParser : BaseParser
    {
        public class StringBounding
        {
            public char BoundingStartCharacter { get; set; }
            public char BoundingEndCharacter { get; set; }

            public char? EscapeCharacter { get; set; }

            public StringBounding(char boundingStartCharacter, char boundingEndCharacter, char? escapeCharacter = null)
            {
                this.BoundingStartCharacter = boundingStartCharacter;
                this.BoundingEndCharacter = boundingEndCharacter;
                this.EscapeCharacter = escapeCharacter;
            }

            public static StringBounding Quotes
            {
                get
                {
                    return new StringBounding('"', '"');
                }
            }
            public static StringBounding Apostrophes
            {
                get
                {
                    return new StringBounding('\'', '\'');
                }
            }
            public static StringBounding Brackets
            {
                get
                {
                    return new StringBounding('(', ')');
                }
            }
            public static StringBounding SquareBrackets
            {
                get
                {
                    return new StringBounding('[', ']');
                }
            }
            public static StringBounding Braces
            {
                get
                {
                    return new StringBounding('{', '}');
                }
            }

            public StringBounding SetEscapeCharacter(char c)
            {
                this.EscapeCharacter = c;
                return this;
            }
        }

        public List<StringBounding> StringBoundings { get; set; }

        public StringBounding CurrentBounding = null;

        public BoundedStringParser()
        {
            this.StringBoundings = new List<StringBounding>();
        }

        public override char? HandleCharacter(char c)
        {
            if (CurrentBounding == null)
            {
                foreach (StringBounding bounding in StringBoundings)
                {
                    if (bounding.BoundingStartCharacter == c)
                    {
                        StoreCurrentToken();
                        CurrentBounding = bounding;
                        CurrentTokenIndex = ParseIndex;
                        return null;
                    }
                }
            }
            else
            {
                if (c == CurrentBounding.BoundingEndCharacter)
                {
                    int escapeChars = 0;
                    while (CurrentBounding.EscapeCharacter.HasValue && PreviousCharacter(escapeChars + 2) == CurrentBounding.EscapeCharacter)
                    {
                        escapeChars++;
                    }
                    if (escapeChars % 2 == 0)
                    {
                        StoreCurrentToken("BOUNDED_STRING", CurrentBounding);
                        CurrentBounding = null;
                        return null;
                    }
                }
            }
            return c;
        }
    }
}
