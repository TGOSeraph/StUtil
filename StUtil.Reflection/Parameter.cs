using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StUtil.Reflection
{
    /// <summary>
    /// Class for storing parameter info
    /// </summary>
    /// <remarks>
    /// 2013-07-16  - Created to replace using a Tuple
    /// </remarks>
    public class Parameter
    {
        /// <summary>
        /// The name of the parameter
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The type of the parameter
        /// </summary>
        public Type Type { get; set; }

        public Parameter()
        {
        }

        public Parameter(string name, Type type)
        {
            this.Name = name;
            this.Type = type;
        }
    }
}
