using StUtil.CodeGen.CodeObjects.Base;
using StUtil.CodeGen.CodeObjects.Data;
using StUtil.CodeGen.CodeObjects.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.CodeGen.CodeObjects.Attributes
{
    /// <summary>
    /// Code object representing an Attribute
    /// </summary>
    [Keyword]
    public class Attribute : NamedObject
    {
        /// <summary>
        /// The parameters to pass to the attributes constructor
        /// </summary>
        public CodeObjectList<DataObject> Parameters { get; set; }
        /// <summary>
        /// The properties on the attribute to set by name
        /// </summary>
        public CodeObjectDictionary<string, DataObject> NamedParameters { get; set; }

        /// <summary>
        /// Create a new attribute object
        /// </summary>
        /// <param name="name">The name of the attribute</param>
        /// <param name="parameters">The parameters to pass to the attributes constructor</param>
        public Attribute(string name, params DataObject[] parameters)
            :base(name)
        {
            Parameters = new CodeObjectList<DataObject>(", ");
            Parameters.AddRange(parameters);
            NamedParameters = new CodeObjectDictionary<string, DataObject>(", ", "=");
        }
    }
}
