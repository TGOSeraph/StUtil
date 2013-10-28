using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.CodeGen.CodeObjects.Data
{
    public class DataObject : BaseCodeObject
    {
        public object Value { get; set; }
        public DataObject(object value)
        {
            this.Value = value;
        }
    }
}
