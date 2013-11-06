using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Parser
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
}
