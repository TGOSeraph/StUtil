using StUtil.CodeGen.CodeObjects;
using StUtil.Parser;
using StUtil.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.CodeGen
{
    public class TypeDefinition
    {
        public List<Token> Matches { get; set; }
        public ReflectionHelper Reflector { get; set; }
        public Delegate Converter { get; set; }
        public string Definition { get; set; }
    }
}
