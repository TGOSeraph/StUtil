using StUtil.CodeGen.CodeObjects.CodeStructures;
using StUtil.CodeGen.CodeObjects.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.CodeGen.CodeObjects.Base
{   
    /// <summary>
    /// An object that can contain members
    /// </summary>
    public class MemberContainerObject : NamedObject
    {
        /// <summary>
        /// The regions to place within the container
        /// </summary>
        public CodeObjectList<Region> Regions { get; set; }
        /// <summary>
        /// The classes to place within the container
        /// </summary>
        public CodeObjectList<Class> Classes { get; set; }
        /// <summary>
        /// The Events to place within the container
        /// </summary>
        public CodeObjectList<Event> Events { get; set; }
        /// <summary>
        /// The Fields to place within the container
        /// </summary>
        public CodeObjectList<Field> Fields { get; set; }
        /// <summary>
        /// The Methods to place within the container
        /// </summary>
        public CodeObjectList<Method> Methods { get; set; }

        /// <summary>
        /// Create a new member container with a name
        /// </summary>
        /// <param name="name">The name of the object</param>
        public MemberContainerObject(string name)
            : base(name)
        {
            Regions = new CodeObjectList<Region>("\n");
            Classes = new CodeObjectList<Class>("\n");
            Fields = new CodeObjectList<Field>("\n");
            Events = new CodeObjectList<Event>("\n");
            Methods = new CodeObjectList<Method>("\n");
        }
    }
}
