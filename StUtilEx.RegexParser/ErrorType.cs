using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtilEx.RegexParser
{
    public enum ErrorType
    {
        None,
        InvalidSyntax,
        MissingClosingPairing,
        MissingOpeningPairing,
        ExpectedNumber
    }
}
