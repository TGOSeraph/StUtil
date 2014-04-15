using StUtil.Generic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;


namespace StUtil.Extensions
{
    /// <summary>
    /// Extensions for EventHandlers
    /// </summary>
    /// <remarks>
    /// 2013-06-23  - Initial version
    /// </remarks>
    public static class EventHandlerExtensions
    {
        public static void RaiseEvent(this PropertyChangedEventHandler handler, object sender, string property)
        {
            if (handler != null)
            {
                handler(sender, new PropertyChangedEventArgs(property));
            }
        }

        /// <summary>
        /// Raise an event handler if it is not null
        /// </summary>
        /// <param name="handler">The handler to raise</param>
        /// <param name="sender">The sender of the event to pass to the handler raise</param>
        public static void RaiseEvent(this EventHandler handler, object sender) 
        {
            EventHandler copy = handler;
            if (copy != null)
            {
                copy(sender, null);
            }
        }
        /// <summary>
        /// Raise an event handler if it is not null
        /// </summary>
        /// <typeparam name="T">The type of the event handler</typeparam>
        /// <param name="handler">The handler to raise</param>
        /// <param name="sender">The sender of the event to pass to the handler raise</param>
        /// <param name="args">The arguments to pass as the eventarg object</param>
        public static void RaiseEvent<T>(this EventHandler<T> handler, object sender, T args) where T : EventArgs
        {
            EventHandler<T> copy = handler;
            if (copy != null)
            {
                copy(sender, args);
            }
        }
        /// <summary>
        /// Raise an event handler with a generic argument if it is not null
        /// </summary>
        /// <typeparam name="T">The type of the event handler</typeparam>
        /// <param name="handler">The handler to raise</param>
        /// <param name="sender">The sender of the event to pass to the handler raise</pa
        /// <param name="args">The value of the generic argument</param>
        public static void RaiseEvent<T>(this EventHandler<EventArgs<T>> handler, object sender, T args)
        {
            EventHandler<EventArgs<T>> copy = handler;
            if (copy != null)
            {
                copy(sender, new EventArgs<T>(args));
            }
        }
        /// <summary>
        /// Raise an event handler with a generic argument if it is not null
        /// </summary>
        /// <typeparam name="T">The type of the event handler</typeparam>
        /// <param name="handler">The handler to raise</param>
        /// <param name="sender">The sender of the event to pass to the handler raise</pa
        /// <param name="args">The value of the first generic argument</param>
        /// <param name="arg2">The value of the second generic argument</param>
        public static void RaiseEvent<T, U>(this EventHandler<EventArgs<T, U>> handler, object sender, T arg, U arg2)
        {
            EventHandler<EventArgs<T, U>> copy = handler;
            if (copy != null)
            {
                copy(sender, new EventArgs<T, U>(arg, arg2));
            }
        }
        /// <summary>
        /// Raise an event handler with a generic argument if it is not null
        /// </summary>
        /// <typeparam name="T">The type of the event handler</typeparam>
        /// <param name="handler">The handler to raise</param>
        /// <param name="sender">The sender of the event to pass to the handler raise</pa
        /// <param name="args">The value of the first generic argument</param>
        /// <param name="arg2">The value of the second generic argument</param>
        /// <param name="arg3">The value of the third generic argument</param>
        public static void RaiseEvent<T, U, V>(this EventHandler<EventArgs<T, U, V>> handler, object sender, T arg, U arg2, V arg3)
        {
            EventHandler<EventArgs<T, U, V>> copy = handler;
            if (copy != null)
            {
                copy(sender, new EventArgs<T, U, V>(arg, arg2, arg3));
            }
        }
    }
}
