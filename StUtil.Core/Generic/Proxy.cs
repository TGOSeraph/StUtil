using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace StUtil.Generic
{
    public class Proxy<TInvoke> : RealProxy
    {
        private static Dictionary<Type, Dictionary<MethodInfo, bool>> invokeLookup = new Dictionary<Type, Dictionary<MethodInfo, bool>>();
        private static Dictionary<MethodInfo, bool> lookup = new Dictionary<MethodInfo, bool>();

        private Type type;
        private object target;
        private ProxyInvoker proxy;

        /// <summary>
        /// Creates the specified proxy.
        /// </summary>
        /// <param name="proxy">The proxy.</param>
        /// <returns></returns>
        public static TInvoke Create(ProxyInvoker proxy)
        {
            return (TInvoke)new Proxy<TInvoke>(proxy, Activator.CreateInstance<TInvoke>()).GetTransparentProxy();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Proxy{TInvoke}"/> class.
        /// </summary>
        /// <param name="proxy">The proxy.</param>
        /// <param name="target">The target.</param>
        public Proxy(ProxyInvoker proxy, TInvoke target)
            : base(typeof(TInvoke))
        {
            this.proxy = proxy;
            this.target = target;
            type = typeof(TInvoke);
            if (!invokeLookup.ContainsKey(type))
            {
                lookup = new Dictionary<MethodInfo, bool>();
                invokeLookup.Add(type, lookup);
            }
            else
            {
                lookup = invokeLookup[type];
            }
        }

        /// <summary>
        /// When overridden in a derived class, invokes the method that is specified in the provided <see cref="T:System.Runtime.Remoting.Messaging.IMessage" /> on the remote object that is represented by the current instance.
        /// </summary>
        /// <param name="msg">A <see cref="T:System.Runtime.Remoting.Messaging.IMessage" /> that contains a <see cref="T:System.Collections.IDictionary" /> of information about the method call.</param>
        /// <returns>
        /// The message returned by the invoked method, containing the return value and any out or ref parameters.
        /// </returns>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
        /// </PermissionSet>
        public override System.Runtime.Remoting.Messaging.IMessage Invoke(System.Runtime.Remoting.Messaging.IMessage msg)
        {
            IMethodCallMessage message = msg as IMethodCallMessage;
            MethodInfo method = (MethodInfo)message.MethodBase;

            bool doProxy;
            if (lookup.ContainsKey(method))
            {
                doProxy = lookup[method];
            }
            else
            {
                doProxy = proxy.ShouldProxy(method);
                lookup.Add(method, doProxy);
            }

            try
            {
                object result;
                if (doProxy)
                {
                    result = proxy.Invoke(method, target, message.Args, method.ReturnType);
                }
                else
                {
                    result = method.Invoke(target, message.Args);
                }
                return new ReturnMessage(result, null, 0, null, message);
            }
            catch (Exception ex)
            {
                return new ReturnMessage(ex, message);
            }
        }
    }
}