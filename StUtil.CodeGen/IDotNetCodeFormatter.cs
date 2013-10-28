using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.CodeGen
{
    public interface IDotNetCodeFormatter
    {
        string Format(string code);
    }
}
