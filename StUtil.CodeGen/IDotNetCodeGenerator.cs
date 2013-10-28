using StUtil.CodeGen.CodeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.CodeGen
{
    public interface IDotNetCodeGenerator
    {
        string ToSyntax(ICodeObject obj);
        void RegisterDefinition<T>(string definition) where T : ICodeObject;
    }
}
