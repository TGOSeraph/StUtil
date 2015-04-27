using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Reflection
{
    /// <summary>
    /// Class for accessing members of an object reflected using dynamic calls
    /// </summary>
    /// <remarks>TODO: Caching of reflected member infos</remarks>
    public class DynamicReflector : DynamicReflectionBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether the results of operations on the object should be wrapped in dynamic reflectors.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the results should be wrapped; otherwise, <c>false</c>.
        /// </value>
        public bool WrapReturn { get; set; }

        /// <summary>
        /// Binding flags for accessing members
        /// </summary>
        public virtual BindingFlags AccessFlags { get; set; }

        /// <summary>
        /// The event that the reflector is bound to
        /// </summary>
        protected EventInfo BoundEvent { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicReflector"/> class.
        /// </summary>
        /// <remarks>If this constructor is called you must manually set the <see cref="TargetType"/></remarks>
        public DynamicReflector()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicReflector"/> class for event binding.
        /// </summary>
        /// <param name="reflectedEvent">The reflected event that will be bound.</param>
        /// <param name="target">The target object.</param>
        public DynamicReflector(EventInfo reflectedEvent, object target)
            : this(target)
        {
            this.BoundEvent = reflectedEvent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicReflector"/> class for reflection of members.
        /// </summary>
        /// <param name="target">The target object.</param>
        public DynamicReflector(object target)
            : base(target)
        {
            this.WrapReturn = true;
            this.AccessFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
        }

        /// <summary>
        /// Finds the method on the target type.
        /// </summary>
        /// <param name="name">The name of the method.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The method matching the name and arguments</returns>
        private MethodInfo FindMethod(string name, object[] args)
        {
            MethodInfo method = null;
            Type t = base.TargetType;

            //Search for the member through the inheritance chain
            do
            {
                method = t.GetMethod(name, AccessFlags, null, args.Select(a => a == null ? null : a.GetType()).ToArray(), null);
            } while (method == null && (t = t.BaseType) != null);

            return method;
        }

        /// <summary>
        /// Finds the member on the target type.
        /// </summary>
        /// <param name="name">The name of the member to find.</param>
        /// <returns>The member matching the specified name</returns>
        /// <exception cref="AmbiguousMatchException">Multiple members with the name [binder.Name] found</exception>
        private MemberInfo FindMember(string name)
        {
            MemberInfo member = null;
            Type t = base.TargetType;

            //Search for the member through the inheritance chain
            do
            {
                var res = t.GetMember(name, this.AccessFlags);
                if (res.Length == 1)
                {
                    member = res[0];
                    break;
                }
                else if (res.Length > 1)
                {
                    //See if we have an event
                    member = res.FirstOrDefault(m => m.MemberType == MemberTypes.Event);
                    if (member == null)
                    {
                        throw new AmbiguousMatchException("Multiple members with the name " + name + " found");
                    }
                }
            } while (member == null && (t = t.BaseType) != null);
            return member;
        }

        /// <summary>
        /// Returns the enumeration of all dynamic member names.
        /// </summary>
        /// <returns>
        /// A sequence that contains dynamic member names.
        /// </returns>
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return base.TargetType.GetMembers(this.AccessFlags).Select(m => m.Name);
        }

        /// <summary>
        /// Provides implementation for binary operations. Classes derived from the <see cref="T:System.Dynamic.DynamicObject" /> class can override this method to specify dynamic behavior for operations such as addition and multiplication.
        /// </summary>
        /// <param name="binder">Provides information about the binary operation. The binder.Operation property returns an <see cref="T:System.Linq.Expressions.ExpressionType" /> object. For example, for the sum = first + second statement, where first and second are derived from the DynamicObject class, binder.Operation returns ExpressionType.Add.</param>
        /// <param name="arg">The right operand for the binary operation. For example, for the sum = first + second statement, where first and second are derived from the DynamicObject class, <paramref name="arg" /> is equal to second.</param>
        /// <param name="result">The result of the binary operation.</param>
        /// <returns>
        /// true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a language-specific run-time exception is thrown.)
        /// </returns>
        public override bool TryBinaryOperation(System.Dynamic.BinaryOperationBinder binder, object arg, out object result)
        {
            //If we are binding from another dynamic object then our method info will be bound to an object
            if (typeof(BoundMethodInfo).IsAssignableFrom(arg.GetType()))
            {
                BoundMethodInfo mi = (BoundMethodInfo)arg;
                //Create a delegate from the method instance on the bound object
                arg = Delegate.CreateDelegate(this.BoundEvent.EventHandlerType, mi.BoundObject, mi.MethodInfo);
            }

            //If we have a delegate then handle assigning the event handler
            if (typeof(Delegate).IsAssignableFrom(arg.GetType()))
            {
                switch (binder.Operation)
                {
                    case System.Linq.Expressions.ExpressionType.AddAssign:
                        this.BoundEvent.GetAddMethod(true).Invoke(base.Target, new[] { arg });
                        break;
                    case System.Linq.Expressions.ExpressionType.SubtractAssign:
                        this.BoundEvent.GetRemoveMethod(true).Invoke(base.Target, new[] { arg });
                        break;
                    default:
                        throw new NotImplementedException();
                }
                result = null;
                return true;
            }

            return base.TryBinaryOperation(binder, arg, out result);
        }

        /// <summary>
        /// Provides implementation for type conversion operations. Classes derived from the <see cref="T:System.Dynamic.DynamicObject" /> class can override this method to specify dynamic behavior for operations that convert an object from one type to another.
        /// </summary>
        /// <param name="binder">Provides information about the conversion operation. The binder.Type property provides the type to which the object must be converted. For example, for the statement (String)sampleObject in C# (CType(sampleObject, Type) in Visual Basic), where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject" /> class, binder.Type returns the <see cref="T:System.String" /> type. The binder.Explicit property provides information about the kind of conversion that occurs. It returns true for explicit conversion and false for implicit conversion.</param>
        /// <param name="result">The result of the type conversion operation.</param>
        /// <returns>
        /// true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a language-specific run-time exception is thrown.)
        /// </returns>
        public override bool TryConvert(System.Dynamic.ConvertBinder binder, out object result)
        {
            result = StUtil.Utilities.TypeUtilities.ConvertType(base.Target, binder.Type);
            return true;
        }

        /// <summary>
        /// Provides the implementation for operations that get member values. Classes derived from the <see cref="T:System.Dynamic.DynamicObject" /> class can override this method to specify dynamic behavior for operations such as getting a value for a property.
        /// </summary>
        /// <param name="binder">Provides information about the object that called the dynamic operation. The binder.Name property provides the name of the member on which the dynamic operation is performed. For example, for the Console.WriteLine(sampleObject.SampleProperty) statement, where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject" /> class, binder.Name returns "SampleProperty". The binder.IgnoreCase property specifies whether the member name is case-sensitive.</param>
        /// <param name="result">The result of the get operation. For example, if the method is called for a property, you can assign the property value to <paramref name="result" />.</param>
        /// <returns>
        /// true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a run-time exception is thrown.)
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public override bool TryGetMember(System.Dynamic.GetMemberBinder binder, out object result)
        {
            MemberInfo member = FindMember(binder.Name);

            //If we have a member then return that
            if (member != null)
            {
                switch (member.MemberType)
                {
                    case MemberTypes.Event:
                        //Return an event binder
                        result = new DynamicReflector((EventInfo)member, base.Target) { WrapReturn = this.WrapReturn };
                        break;
                    case MemberTypes.Property:
                        //Check if we should wrap the result in another dynamic reflector
                        result = ((PropertyInfo)member).GetValue(base.Target);
                        if (WrapReturn)
                        {
                            result = new DynamicReflector(result) { WrapReturn = this.WrapReturn };
                        }
                        break;
                    case MemberTypes.Field:
                        //Check if we should wrap the result in another dynamic reflector
                        result = ((FieldInfo)member).GetValue(base.Target);
                        if (WrapReturn)
                        {
                            result = new DynamicReflector(result) { WrapReturn = this.WrapReturn };
                        }
                        break;
                    case MemberTypes.Method:
                        //Return the method info bound to the target object
                        result = new BoundMethodInfo((MethodInfo)member, base.Target);
                        break;
                    case MemberTypes.NestedType:
                        //TODO: Implement
                        throw new NotImplementedException();
                    default:
                        throw new NotImplementedException();
                }

                return true;
            }

            return base.TryGetMember(binder, out result);
        }

        /// <summary>
        /// Provides the implementation for operations that invoke an object. Classes derived from the <see cref="T:System.Dynamic.DynamicObject" /> class can override this method to specify dynamic behavior for operations such as invoking an object or a delegate.
        /// </summary>
        /// <param name="binder">Provides information about the invoke operation.</param>
        /// <param name="args">The arguments that are passed to the object during the invoke operation. For example, for the sampleObject(100) operation, where sampleObject is derived from the <see cref="T:System.Dynamic.DynamicObject" /> class, <paramref name="args[0]" /> is equal to 100.</param>
        /// <param name="result">The result of the object invocation.</param>
        /// <returns>
        /// true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a language-specific run-time exception is thrown.
        /// </returns>
        public override bool TryInvoke(System.Dynamic.InvokeBinder binder, object[] args, out object result)
        {
            //Check if we are toggling the return wrapping mode
            if (args.Length == 1)
            {
                if (args[0] == DynamicReflector.Wrap)
                {
                    if (!typeof(DynamicReflector).IsAssignableFrom(base.Target.GetType()))
                    {
                        base.Target = new DynamicReflector(base.Target);
                    }
                    result = this;
                    return true;
                }
                else if (args[0] == DynamicReflector.Unwrap)
                {
                    if (typeof(DynamicReflector).IsAssignableFrom(base.Target.GetType()))
                    {
                        base.Target = ((DynamicReflector)base.Target).Target;
                    }
                    result = this;
                    return true;
                }
            }

            return base.TryInvoke(binder, args, out result);
        }

        /// <summary>
        /// Provides the implementation for operations that invoke a member. Classes derived from the <see cref="T:System.Dynamic.DynamicObject" /> class can override this method to specify dynamic behavior for operations such as calling a method.
        /// </summary>
        /// <param name="binder">Provides information about the dynamic operation. The binder.Name property provides the name of the member on which the dynamic operation is performed. For example, for the statement sampleObject.SampleMethod(100), where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject" /> class, binder.Name returns "SampleMethod". The binder.IgnoreCase property specifies whether the member name is case-sensitive.</param>
        /// <param name="args">The arguments that are passed to the object member during the invoke operation. For example, for the statement sampleObject.SampleMethod(100), where sampleObject is derived from the <see cref="T:System.Dynamic.DynamicObject" /> class, <paramref name="args[0]" /> is equal to 100.</param>
        /// <param name="result">The result of the member invocation.</param>
        /// <returns>
        /// true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a language-specific run-time exception is thrown.)
        /// </returns>
        public override bool TryInvokeMember(System.Dynamic.InvokeMemberBinder binder, object[] args, out object result)
        {
            //Try and find normal method
            MethodInfo method = FindMethod(binder.Name, args);
            Type t;
            if (method == null)
            {
                //If we cant find one look for an event
                EventInfo evt = null;
                t = base.TargetType;
                do
                {
                    evt = t.GetEvent(binder.Name, this.AccessFlags);
                } while (evt == null && (t = t.BaseType) != null);

                if (evt != null)
                {
                    //Try and get the raise method
                    method = evt.GetRaiseMethod(true);
                    if (method == null)
                    {
                        //Look for the events backing field
                        var field = t.GetField(evt.Name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
                        if (field == null)
                        {
                            field = t.GetField("_" + evt.Name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
                            if (field == null)
                            {
                                field = t.GetField("m_" + evt.Name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
                            }
                        }
                        if (field != null)
                        {
                            var eventDelegate = (MulticastDelegate)field.GetValue(base.Target);
                            if (eventDelegate != null)
                            {
                                //Trigger each handler
                                foreach (var handler in eventDelegate.GetInvocationList())
                                {
                                    handler.Method.Invoke(handler.Target, args);
                                }
                                result = new DynamicReflector(evt, Target) { WrapReturn = this.WrapReturn };
                                return true;
                            }
                        }

                        //If we still dont have one, look for an On[eventname] method to raise
                        method = FindMethod("On" + binder.Name, args);
                        if (method == null)
                        {
                            //Try without the sender argument
                            var a = args.Skip(1).ToArray();
                            method = FindMethod("On" + binder.Name, a);
                            if (method != null)
                            {
                                args = a;
                            }
                            else
                            {
                                result = new DynamicReflector(evt, Target) { WrapReturn = this.WrapReturn };
                                return false;
                            }
                        }
                    }
                }
            }

            if (method != null)
            {
                result = method.Invoke(Target, args);
                if (WrapReturn)
                {
                    result = new DynamicReflector(result) { WrapReturn = this.WrapReturn };
                }
                return true;
            }

            //We arent invoking a method or event, so check for a nested type to construct
            Type nested = null;
            t = base.TargetType;
            do
            {
                nested = t.GetNestedType(binder.Name, this.AccessFlags);
            } while (nested == null && (t = t.BaseType) != null);

            if (nested != null)
            {
                var ctor = nested.GetConstructor(this.AccessFlags, null, args.Select(a => a == null ? null : a.GetType()).ToArray(), null);
                if (ctor != null)
                {
                    result = ctor.Invoke(args);
                    if (this.WrapReturn)
                    {
                        result = new DynamicReflector(result) { WrapReturn = this.WrapReturn };
                    }
                    return true;
                }
            }

            return base.TryInvokeMember(binder, args, out result);
        }

        /// <summary>
        /// Provides the implementation for operations that set member values. Classes derived from the <see cref="T:System.Dynamic.DynamicObject" /> class can override this method to specify dynamic behavior for operations such as setting a value for a property.
        /// </summary>
        /// <param name="binder">Provides information about the object that called the dynamic operation. The binder.Name property provides the name of the member to which the value is being assigned. For example, for the statement sampleObject.SampleProperty = "Test", where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject" /> class, binder.Name returns "SampleProperty". The binder.IgnoreCase property specifies whether the member name is case-sensitive.</param>
        /// <param name="value">The value to set to the member. For example, for sampleObject.SampleProperty = "Test", where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject" /> class, the <paramref name="value" /> is "Test".</param>
        /// <returns>
        /// true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a language-specific run-time exception is thrown.)
        /// </returns>
        public override bool TrySetMember(System.Dynamic.SetMemberBinder binder, object value)
        {
            MemberInfo member = FindMember(binder.Name);

            //If we have a member then set that
            if (member != null)
            {
                switch (member.MemberType)
                {
                    case MemberTypes.Field:
                        ((FieldInfo)member).SetValue(base.Target, value);
                        return true;
                    case MemberTypes.Property:
                        ((PropertyInfo)member).SetValue(base.Target, value);
                        return true;
                    case MemberTypes.Event:
                        //This will be handled by binary operation
                        return true;
                }
            }
            return base.TrySetMember(binder, value);
        }

        /// <summary>
        /// Provides the implementation for operations that set a value by index. Classes derived from the <see cref="T:System.Dynamic.DynamicObject" /> class can override this method to specify dynamic behavior for operations that access objects by a specified index.
        /// </summary>
        /// <param name="binder">Provides information about the operation.</param>
        /// <param name="indexes">The indexes that are used in the operation. For example, for the sampleObject[3] = 10 operation in C# (sampleObject(3) = 10 in Visual Basic), where sampleObject is derived from the <see cref="T:System.Dynamic.DynamicObject" /> class, <paramref name="indexes[0]" /> is equal to 3.</param>
        /// <param name="value">The value to set to the object that has the specified index. For example, for the sampleObject[3] = 10 operation in C# (sampleObject(3) = 10 in Visual Basic), where sampleObject is derived from the <see cref="T:System.Dynamic.DynamicObject" /> class, <paramref name="value" /> is equal to 10.</param>
        /// <returns>
        /// true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a language-specific run-time exception is thrown.
        /// </returns>
        public override bool TrySetIndex(System.Dynamic.SetIndexBinder binder, object[] indexes, object value)
        {
            try
            {
                PropertyInfo prop = TargetType.GetProperty("Item", AccessFlags);
                prop.SetValue(Target, value, indexes);
                return true;
            }
            catch (Exception)
            {
                return base.TrySetIndex(binder, indexes, value);
            }
        }

        /// <summary>
        /// Provides the implementation for operations that get a value by index. Classes derived from the <see cref="T:System.Dynamic.DynamicObject" /> class can override this method to specify dynamic behavior for indexing operations.
        /// </summary>
        /// <param name="binder">Provides information about the operation.</param>
        /// <param name="indexes">The indexes that are used in the operation. For example, for the sampleObject[3] operation in C# (sampleObject(3) in Visual Basic), where sampleObject is derived from the DynamicObject class, <paramref name="indexes[0]" /> is equal to 3.</param>
        /// <param name="result">The result of the index operation.</param>
        /// <returns>
        /// true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a run-time exception is thrown.)
        /// </returns>
        public override bool TryGetIndex(System.Dynamic.GetIndexBinder binder, object[] indexes, out object result)
        {
            try
            {
                PropertyInfo prop = TargetType.GetProperty("Item", AccessFlags);
                result = prop.GetValue(Target, indexes);
                if (WrapReturn)
                {
                    result = new DynamicReflector(result) { WrapReturn = this.WrapReturn };
                }
                return true;
            }
            catch (Exception)
            {
                return base.TryGetIndex(binder, indexes, out result);
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return base.Target.ToString();
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Target.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Target.Equals(obj) || base.Equals(obj);
        }

        #region Result Wrapping
        public class WrapResult
        {
            private readonly static WrapResult instance;
            public static WrapResult Instance
            {
                get
                {
                    return instance;
                }
            }

            private WrapResult()
            {
            }

            static WrapResult()
            {
                instance = new WrapResult();
            }
        }

        public class UnwrapResult
        {
            private readonly static UnwrapResult instance;
            public static UnwrapResult Instance
            {
                get
                {
                    return instance;
                }
            }

            private UnwrapResult()
            {
            }

            static UnwrapResult()
            {
                instance = new UnwrapResult();
            }
        }

        public static UnwrapResult Unwrap
        {
            get
            {
                return UnwrapResult.Instance;
            }
        }

        public static WrapResult Wrap
        {
            get
            {
                return WrapResult.Instance;
            }
        }
        #endregion

    }
}
