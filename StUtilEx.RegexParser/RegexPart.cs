using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtilEx.RegexParser
{
    public class RegexPart
    {
        public string ValueStart { get; set; }
        public string ValueEnd { get; set; }
        public List<RegexPart> Parts { get; set; }
        public RegexPart Parent { get; set; }

        public int Index { get; set; }
        public PartType Type { get; set; }
        public ErrorType Error { get; set; }

        public RegexPart(RegexPart parent)
        {
            Type = PartType.Undefined;

            Parts = new List<RegexPart>();
            Parent = parent;
            if (parent != null)
            {
                Parent.Parts.Add(this);
            }
        }

        public bool IsGroup()
        {
            return Type == PartType.Group
                        || Type == PartType.Comment
                        || Type == PartType.NonCapturing
                        || Type == PartType.PositiveLookahead
                        || Type == PartType.NegativeLookahead
                        || Type == PartType.PositiveLookbehind
                        || Type == PartType.NegativeLookbehind
                        || Type == PartType.Greedy
                        || Type == PartType.NamedCapture;
        }

        public override string ToString()
        {
            return this.ValueStart + string.Join("", Parts.Select(p => p.ToString())) + this.ValueEnd;
        }
    }
}
