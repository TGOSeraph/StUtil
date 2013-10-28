using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.CodeGen.CodeObjects.Misc
{
    public interface ICodeObjectList : ICodeObject
    {
        string JoinString { get; set; }
        IList Items { get; set; }
    }
    public interface ICodeObjectList<T> : ICodeObjectList
    {
        new List<T> Items { get; set; }
    }
}
