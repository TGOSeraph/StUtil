using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.CodeGen.CodeObjects
{
    /// <summary>
    /// Base class for defining an object that can be converted to syntax
    /// </summary>
    public class BaseCodeObject : ICodeObject
    {
        /// <summary>
        /// Convert the object to a specific syntax using a generator
        /// </summary>
        /// <param name="generator">The syntax generator used to convert this object to syntax</param>
        /// <returns>The syntax representation of this object</returns>
        public string ToSyntax(IDotNetCodeGenerator generator)
        {
            return generator.ToSyntax(this);
        }
    }
}
