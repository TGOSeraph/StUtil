using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StUtil.CodeGen
{
    public interface IDotNetCodeExecutor
    {
        object ExecuteCode(string code, string @namespace, string @class, string function, bool isStatic, string[] referencedAssemblies, object[] args);
    }
}
