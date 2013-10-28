using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.CodeGen
{
    public class KeyMatch
    {
        public int Index { get; set; }
        public string Text { get; set; }

        public int Length
        {
            get
            {
                return Text.Length + 2;
            }
        }
    }
}
