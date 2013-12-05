using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Parser
{
    public class BoundedStringParser : BoundedStringParser<List<Token>>
    {
        public override List<Token> GetResults()
        {
            return base.Tokens;
        }
    }

    public abstract class BoundedStringParser<T> : BaseParser<T>
    {
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
                    if (bounding.BoundingStartCharacter == c && (ParseIndex > 1 ? PreviousCharacter(1) != bounding.EscapeCharacter : true))
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
                    while (CurrentBounding.EscapeCharacter.HasValue && PreviousCharacter(escapeChars + 1) == CurrentBounding.EscapeCharacter)
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
