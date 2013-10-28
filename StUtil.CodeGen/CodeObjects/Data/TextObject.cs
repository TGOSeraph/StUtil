using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.CodeGen.CodeObjects.Data
{
    public class TextObject : BaseCodeObject
    {
        private string value = null;
        public string Value
        {
            get
            {
                return value;//(Quote && value != null) ? "\"" + value + "\"" : value;
            }
            set
            {
                this.value = value;
            }
        }
        //public bool Quote { get; set; }

        public TextObject(string value)
        {
            this.value = value;
        }
    }
}
