using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Reflection.Dynamic
{
    public class CustomProperty<T>
    {
        public Func<T, object> Getter {get; set;}
        public Action<T, object> Setter { get; set; }

        public string Name { get; set; }

        public CustomProperty(string name, Func<T, object> get, Action<T, object> set)
        {
            this.Name = name;
            this.Getter = get;
            this.Setter = set;
        }
    }
}
