using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Parser
{
    public class Token
    {
        public string Value { get; set; }
        public int Index { get; set; }
        
        public string Type { get; set; }
        public object Tag { get; set; }

        public override string ToString()
        {
            return Value;
        }
    }
}
