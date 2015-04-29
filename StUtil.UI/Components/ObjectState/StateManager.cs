using StUtil.Extensions;
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
        public class StateChangedEventArgs : StUtil.Generic.ValueChangedEventArgs<string>
        {
            public StateItem Item { get; set; }

            public StateChangedEventArgs(string newValue, string oldValue, StateItem item)
                : base(newValue, oldValue)
            {
                this.Item = item;
            }
        }

        /// <summary>
        /// Occurs when the state of a component changes.
        /// </summary>
        public event EventHandler<StateChangedEventArgs> StateChanged;

        /// <summary>
        /// Gets or sets the states.
        /// </summary>
        /// <value>
        /// The states.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category("General")]
        [Description("A collection of state items related to items on the form")]
        public ComponentChildItemCollection<StateManager, StateItem> States { get; set; }

        public StateManager()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StateManager"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public StateManager(IContainer parent)
            : base(parent)
        {
            this.States = new ComponentChildItemCollection<StateManager, StateItem>(this);
        }

        /// <summary>
        /// Signals the object that initialization is starting.
        /// </summary>
        public void BeginInit()
        {
        }

        /// <summary>
        /// Handles the StateChanged event of the state control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Generic.ValueChangedEventArgs{System.String}"/> instance containing the event data.</param>
        void state_StateChanged(object sender, Generic.ValueChangedEventArgs<string> e)
        {
            StateChanged.RaiseEvent(this, new StateChangedEventArgs(e.NewValue, e.OldValue, (StateItem)sender));
        }

        /// <summary>
        /// Signals the object that initialization is complete.
        /// </summary>
        public void EndInit()
        {
            if (!this.DesignMode)
            {
                foreach (StateItem state in States)
                {
                    if (state.Target != null)
                    {
                        Type t = state.Target.GetType();
                        foreach (var evt in state.HandledEvents)
                        {
                            var evtInfo = t.GetEvent(evt.Event);
                            Dictionary<string, PropertyInfo> cache = new Dictionary<string, PropertyInfo>();

                            Action<object, object> action = (sender, e) =>
                            {
                                if (evt.RequiredState == null || state.State == evt.RequiredState)
                                {
                                    foreach (var p in evt.Properties.Properties)
                                    {
                                        PropertyInfo pi;
                                        if (cache.ContainsKey(p.PropertyName))
                                        {
                                            pi = cache[p.PropertyName];
                                        }
                                        else
                                        {
                                            pi = t.GetProperty(p.PropertyName);
                                            cache.Add(p.PropertyName, pi);
                                        }
                                        if (p.Value.GetType() == typeof(string) && (string)p.Value == "$(State)")
                                        {
                                            pi.SetValue(state.Target, state.State);
                                        }
                                        else
                                        {
                                            pi.SetValue(state.Target, p.Value);
                                        }
                                    }
                                    if (evt.StateList != null && evt.StateList.Length > 0)
                                    {
                                        int index = Array.IndexOf(evt.StateList, state.State);
                                        if (index > -1)
                                        {
                                            if (index++ >= evt.StateList.Length - 1)
                                            {
                                                index = 0;
                                            }
                                            state.State = evt.StateList[index];
                                        }
                                    }
                                }
                            };

                            evtInfo.AddEventHandler(state.Target, action.ConvertDelegate(evtInfo.EventHandlerType));
                        }
                    }
                }

                foreach (var state in States)
                {
                    state.StateChanged += state_StateChanged;
                }
            }
        }

    }
}
