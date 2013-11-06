using StUtil.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StUtil.CodeGen
{
    public class DefinitionParser : StUtil.Parser.BoundedStringParser<List<Token>>
    {
        public DefinitionParser()
        {
            base.StringBoundings.Add(StringBounding.Braces.SetEscapeCharacter('\\'));
            base.StringBoundings.Add(StringBounding.SquareBrackets.SetEscapeCharacter('\\'));
        }

        public override char? HandleCharacter(char c)
        {
            char? cc = base.HandleCharacter(c);
            if (!cc.HasValue)
            {
                Token last = Tokens.LastOrDefault();
                if (last != null)
                {
                    if (last.Value.StartsWith("+"))
                    {
                        last.Type = "BOUNDED_STRING_IF_AFTER";
                    }
                    else if (last.Value.StartsWith("-"))
                    {
                        last.Type = "BOUNDED_STRING_IF_BEFORE";
                    }
                }
            }

            return cc;
        }

        public override List<Token> GetResults()
        {
            return Tokens;
        }
    }
}
