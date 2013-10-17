using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace StUtil.Reflection
{
    /// <summary>
    /// Wrapper around an EventInfo member
    /// </summary>
    /// <remarks>
    /// 2013-04-15  - Initial version
    /// </remarks>
    public class ReflectedEvent : ReflectedMemberParameters<EventInfo>
    {
        /// <summary>
        /// The method to call when raising the event
        /// </summary>
        private ReflectedMethod _raise;

        /// <summary>
        /// Create a new event wrapper
        /// </summary>
        /// <param name="member">The event info to wrap</param>
        public ReflectedEvent(EventInfo member)
            : base(member)
        {
        }

        /// <summary>
        /// Get the parameters required by the raise event
        /// </summary>
        /// <returns>The parameters required by the raise event</returns>
        protected override IEnumerable<Parameter> GetParameters()
        {
            if (this._raise == null)
            {
                this._raise = new ReflectedMethod(base.Member.GetRaiseMethod(true));
            }
            return this._raise.Parameters;
        }

        /// <summary>
        /// Add a handler to this event
        /// </summary>
        /// <param name="target">The object to add the handler to</param>
        /// <param name="handler">The event handler delegate</param>
        public void AddHandler(object target, Delegate handler)
        {
            base.Member.AddEventHandler(target, handler);
        }

        /// <summary>
        /// Remove an event handler from this event
        /// </summary>
        /// <param name="target">The object to remove the handler from</param>
        /// <param name="handler">The handler to remove</param>
        public void RemoveHandler(object target, Delegate handler)
        {
            base.Member.RemoveEventHandler(target, handler);
        }

        /// <summary>
        /// Raise the event
        /// </summary>
        /// <param name="target">The object to raise the event on</param>
        /// <param name="args">The parameters to pass to the raising of the event</param>
        public void Raise(object target, params object[] args)
        {
            if (this._raise == null)
            {
                this._raise = new ReflectedMethod(base.Member.GetRaiseMethod(true));
            }
            this._raise.Invoke(target, args);
        }

        /// <summary>
        /// Implicit conversion to the type wrapper
        /// </summary>
        /// <param name="propInfo">The member to wrap</param>
        /// <returns>A wrapped MemberInfo</returns>
        public static implicit operator ReflectedEvent(EventInfo propInfo)
        {
            return new ReflectedEvent(propInfo);
        }

        /// <summary>
        /// Implicit conversion from the type wrapper to the wrapped type
        /// </summary>
        /// <param name="propInfo">The wrapper</param>
        /// <returns>The member that was wrapped</returns>
        public static implicit operator EventInfo(ReflectedEvent propInfo)
        {
            return propInfo.Member;
        }
    }

}
