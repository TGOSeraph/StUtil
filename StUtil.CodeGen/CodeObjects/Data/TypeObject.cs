using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.CodeGen.CodeObjects.Data
{
    [Keyword]
    public class TypeObject : DataObject
    {
        public TypeObject(string typeName)
            : base(typeName)
        {
        }

        public TypeObject(Type type)
            : base(type)
        {
        }

        public static implicit operator Type(TypeObject d)
        {
            return (Type)d.Value;
        }
        public static implicit operator TypeObject(Type d)
        {
            return new TypeObject(d);
        }
        public static implicit operator string(TypeObject d)
        {
            return ((Type)d.Value).FullName;
        }
        public static implicit operator TypeObject(string d)
        {
            return new TypeObject(d);
        }
    }
}
