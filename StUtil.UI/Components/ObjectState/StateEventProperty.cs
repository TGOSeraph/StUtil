using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.UI.Components.ObjectState
{
    public class StateEventProperty : ComponentChildItem<StateEventProperties>
    {
        public string PropertyName { get; set; }
        public object Value { get; set; }
    }
}
