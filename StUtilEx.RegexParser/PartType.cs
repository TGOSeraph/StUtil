using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtilEx.RegexParser
{
    public enum PartType
    {
        Undefined,
        Root,
        Text,
        DefinedEscaped,
        OctalCharacter,
        HexCharacter,
        ControlCharacter,
        UnicodeCharacter,
        EscapedCharacter,
        Backreference,
        CharacterGroup,
        NegatedCharacterGroup,
        NegateCharacterGroup,
        ExactQuantifier,
        AtLeastQuantifier,
        RangeQuantifier,
        AtLeastMinimumQuantifier,
        Operator,
        MinimumRangeQuantifier,
        BackspaceEscaped,
        NamedBackreference,
        Group,
        NegativeLookahead,
        PositiveLookbehind,
        NegativeLookbehind,
        Greedy,
        NamedCapture,
        PositiveLookahead,
        NonCapturing,
        Comment,
        Invalid,
        InvalidQuantifier
    }
}
