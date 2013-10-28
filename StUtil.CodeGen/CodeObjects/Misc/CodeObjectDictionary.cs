using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.CodeGen.CodeObjects.Misc
{
    public class CodeObjectDictionary<T, V> : ICodeObjectDictionary<T, V>
    {
        public string JoinString { get; set; }
        public string SeparatorString { get; set; }

        public CodeObjectDictionary(string joinString, string keyJoinString)
        {
            this.JoinString = joinString;
            this.SeparatorString = keyJoinString;
            this.Items = new Dictionary<T, V>();
        }

        public Dictionary<T, V> Items
        {
            get;
            set;
        }

        System.Collections.IDictionary ICodeObjectDictionary.Items
        {
            get
            {
                return Items;
            }
            set
            {
            }
        }

        public string ToSyntax(IDotNetCodeGenerator generator)
        {
            return generator.ToSyntax(this);
        }
    }
}
