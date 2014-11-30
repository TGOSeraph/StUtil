using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StUtil.Plugins
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field)]
    public class StatefulAttribute : Attribute
    {
        public bool MaintainsState { get; set; }
        public StatefulAttribute(bool maintainState = true)
        {
            this.MaintainsState = maintainState;
        }
    }
}
