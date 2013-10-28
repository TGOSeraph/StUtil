using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.CodeGen.CodeObjects.Misc
{
    public class CodeObjectList<T> : ICodeObjectList<T>
    {
        public string JoinString { get; set; }

        private List<T> items;
        public List<T> Items
        {
            get
            {
                return items;
            }
            set
            {
                this.items = value;
            }
        }

        IList ICodeObjectList.Items
        {
            get
            {
                return Items;
            }
            set
            {
            }
        }

        public CodeObjectList(string joinString)
        {
            this.items = new List<T>();
            this.JoinString = joinString;
        }

        public CodeObjectList(string joinString, params T[] items)
        {
            this.items = new List<T>(items);
            this.JoinString = joinString;
        }

        public string ToSyntax(IDotNetCodeGenerator generator)
        {
            return generator.ToSyntax(this);
        }

        public void Add(T obj)
        {
            items.Add(obj);
        }

        public void AddRange(T[] obj)
        {
            items.AddRange(obj);
        }
    }
}
