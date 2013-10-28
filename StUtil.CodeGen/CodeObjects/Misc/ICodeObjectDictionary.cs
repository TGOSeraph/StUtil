using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.CodeGen.CodeObjects.Misc
{
    public interface ICodeObjectDictionary : ICodeObject
    {
        string JoinString { get; set; }
        string SeparatorString { get; set; }
        IDictionary Items { get; set; }
    }
    public interface ICodeObjectDictionary<T, U> : ICodeObjectDictionary
    {
        new Dictionary<T, U> Items { get; set; }
    }
}
