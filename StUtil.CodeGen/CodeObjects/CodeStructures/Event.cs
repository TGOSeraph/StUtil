using StUtil.CodeGen.CodeObjects.Base;
using StUtil.CodeGen.CodeObjects.Attributes;
using StUtil.CodeGen.CodeObjects.Data;
using StUtil.CodeGen.CodeObjects.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.CodeGen.CodeObjects.CodeStructures
{
    public class Event : AccessAttributedNamedObject
    {
        public TypeObject HandlerType { get; set; }
        public EventModifiers Modifier { get; set; }

        public Event(string name, TypeObject handlerType, AccessModifiers accessModifiers, params AttributeSection[] attributes)
            : base(name, accessModifiers, attributes)
        {
            this.HandlerType = handlerType;
        }
    }
}
