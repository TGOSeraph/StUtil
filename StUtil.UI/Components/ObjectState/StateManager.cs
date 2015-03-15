using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.UI.Components.ObjectState
{
    public class StateManager : HostedComponent, ISupportInitialize
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category("General")]
        [Description("A collection of state items related to items on the form")]
        public ComponentChildItemCollection<StateManager, StateItem> States { get; set; }

        public StateManager()
            : this(null)
        {
        }

        public StateManager(IContainer parent)
            : base(parent)
        {
            this.States = new ComponentChildItemCollection<StateManager, StateItem>(this);
        }

        public void BeginInit()
        {
        }

        public void EndInit()
        {
            if (!this.DesignMode)
            {
                foreach (StateItem state in States)
                {
                    Type t = state.Target.GetType();
                    foreach (var evt in state.HandledEvents)
                    {
                        var evtInfo = t.GetEvent(evt.Event);
                        Dictionary<string, PropertyInfo> cache = new Dictionary<string, PropertyInfo>();

                        Action<object, object> action = (sender, e) =>
                        {
                            foreach (var p in evt.Properties.Properties)
                            {
                                if (cache.ContainsKey(p.PropertyName))
                                {
                                    cache[p.PropertyName].SetValue(state.Target, p.Value);
                                }
                                else
                                {
                                    var prop = t.GetProperty(p.PropertyName);
                                    cache.Add(p.PropertyName, prop);
                                    prop.SetValue(state.Target, p.Value);
                                }
                            }
                        };

                        evtInfo.AddEventHandler(state.Target, ConvertDelegate(action, evtInfo.EventHandlerType));
                    }
                }
            }
        }

        public static Delegate ConvertDelegate(Delegate originalDelegate, Type targetDelegateType)
        {
            return Delegate.CreateDelegate(targetDelegateType, originalDelegate.Target, originalDelegate.Method);
        }
    }
}
