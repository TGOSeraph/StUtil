using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace StUtil.Reflection
{
    /// <summary>
    /// Typeless reflection helper used to gain access to a types members
    /// </summary>
    public class ReflectionHelper
    {
        /// <summary>
        /// An instance of an empty argument array
        /// </summary>
        public static Type[] EmptyArgs = new Type[] { typeof(void) };

        /// <summary>
        /// Allow the reflector to find members of any protection/type
        /// </summary>
        public const BindingFlags AnyBinding = BindingFlags.FlattenHierarchy | BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static;

        /// <summary>
        /// The type the reflection will be performed on
        /// </summary>
        public Type TargetType { get; private set; }

        /// <summary>
        /// Member caches
        /// </summary>
        private IEnumerable<ReflectedConstructor> constructorCache = null;
        private Dictionary<string, IEnumerable<ReflectedEvent>> eventCache = null;
        private Dictionary<string, IEnumerable<ReflectedField>> fieldCache = null;
        private Dictionary<string, IEnumerable<ReflectedMethod>> methodCache = null;
        private Dictionary<string, IEnumerable<ReflectedProperty>> propertyCache = null;

        /// <summary>
        /// Create new reflection helper from type
        /// </summary>
        /// <param name="targetType">The type to reflect</param>
        public ReflectionHelper(Type targetType)
        {
            this.TargetType = targetType;
        }

        /// <summary>
        /// Create new reflection helper from object
        /// </summary>
        /// <param name="obj">The object, the type of which will be relected</param>
        public ReflectionHelper(object obj)
        {
            this.TargetType = obj.GetType();
        }

        /// <summary>
        /// Loads members from the type
        /// </summary>
        /// <typeparam name="U">The MemberInfo type of object that will be loaded</typeparam>
        /// <typeparam name="K">The reflection wrapper type</typeparam>
        /// <param name="flags">The flags to use for the searching</param>
        /// <param name="dictionary">The cache to add the results to</param>
        /// <param name="getter">The function used to get the members</param>
        private void LoadMembers<U, K>(BindingFlags flags, ref Dictionary<string, IEnumerable<K>> dictionary, Func<BindingFlags, U[]> getter)
            where U : MemberInfo
            where K : ReflectedMemberBase<U>
        {
            if (dictionary == null)
            {
                dictionary = getter(flags)
                    .GroupBy((g) => g.Name)
                    .ToDictionary(g => g.Key,
                                  g => g.Select(e => (K)Activator.CreateInstance(typeof(K), e)));
            }
        }

        /// <summary>
        /// Loads constructors for the type
        /// </summary>
        /// <param name="flags">The flags used to search for members</param>
        private void LoadConstructors(BindingFlags flags)
        {
            if (this.constructorCache == null)
            {
                ConstructorInfo[] ctrs = this.TargetType.GetConstructors(flags);
                this.constructorCache = ctrs.Select(c => new ReflectedConstructor(c));
            }
        }

        /// <summary>
        /// Get constructors for the type
        /// </summary>
        /// <param name="flags">The flags used to search for members</param>
        /// <returns>A list of the constructors for the type</returns>
        public IEnumerable<ReflectedConstructor> GetConstructors(BindingFlags flags = AnyBinding)
        {
            LoadConstructors(flags);
            return this.constructorCache;
        }

        /// <summary>
        /// Get a specific constructor matching a type array
        /// </summary>
        /// <param name="args">The parameters used to match against the constructor</param>
        /// <param name="flags">The flags used to search for members</param>
        /// <returns>The constructor matching the parameters, or null</returns>
        public ReflectedConstructor GetConstructor(Type[] args, BindingFlags flags = AnyBinding)
        {
            LoadConstructors(flags);
            return this.constructorCache.FirstOrDefault(c => c.CheckParametersMatch(args));
        }

        /// <summary>
        /// Get a specific constructor matching a list of strings of parameter names
        /// </summary>
        /// <param name="args">The parameters used to match against the constructor</param>
        /// <param name="flags">The flags used to search for members</param>
        /// <returns>The constructor matching the parameters, or null</returns>
        public ReflectedConstructor GetConstructor(string[] args, BindingFlags flags = AnyBinding)
        {
            LoadConstructors(flags);
            return this.constructorCache.FirstOrDefault(c => c.CheckParametersMatch(args));
        }

        /// <summary>
        /// Gets the first constructor
        /// </summary>
        /// <param name="flags">The flags used to search for members</param>
        /// <returns>The first constructor found</returns>
        public ReflectedConstructor GetConstructor(BindingFlags flags = AnyBinding)
        {
            LoadConstructors(flags);
            return this.constructorCache.FirstOrDefault();
        }

        /// <summary>
        /// Load the events from the type
        /// </summary>
        /// <param name="flags">The flags used to search for members</param>
        private void LoadEvents(BindingFlags flags)
        {
            LoadMembers<EventInfo, ReflectedEvent>(flags, ref this.eventCache, this.TargetType.GetEvents);
        }

        /// <summary>
        /// Get the events from the type
        /// </summary>
        /// <param name="flags">The flags used to search for members</param>
        /// <returns>All events that are on the type</returns>
        public IEnumerable<ReflectedEvent> GetEvents(BindingFlags flags = AnyBinding)
        {
            LoadEvents(flags);
            return this.eventCache.SelectMany(k => k.Value);
        }

        /// <summary>
        /// Get a specific event by name
        /// </summary>
        /// <param name="name">The name of the event to get</param>
        /// <param name="flags">The flags used to search for members</param>
        /// <returns>The event matching the name, or null</returns>
        public ReflectedEvent GetEvent(string name, BindingFlags flags = AnyBinding)
        {
            LoadEvents(flags);
            return this.eventCache[name].FirstOrDefault();
        }

        /// <summary>
        /// Load all fields in the type
        /// </summary>
        /// <param name="flags">The flags used to search for members</param>
        private void LoadFields(BindingFlags flags)
        {
            LoadMembers<FieldInfo, ReflectedField>(flags, ref this.fieldCache, this.TargetType.GetFields);
        }

        /// <summary>
        /// Get all fields from the type
        /// </summary>
        /// <param name="flags">The flags used to search for members</param>
        /// <returns>All of the fields within the type</returns>
        public IEnumerable<ReflectedField> GetFields(BindingFlags flags = AnyBinding)
        {
            LoadFields(flags);
            return this.fieldCache.SelectMany(k => k.Value);
        }

        /// <summary>
        /// Get a specific field by name
        /// </summary>
        /// <param name="name">The name of the field to get</param>
        /// <param name="flags">The flags used to search for members</param>
        /// <returns>The field matching the specified name, or null</returns>
        public ReflectedField GetField(string name, BindingFlags flags = AnyBinding)
        {
            LoadFields(flags);
            if (this.fieldCache.ContainsKey(name))
            {
                return this.fieldCache[name].FirstOrDefault();
            }
            return null;
        }

        /// <summary>
        /// Load all methods in the type
        /// </summary>
        /// <param name="flags">The flags used to search for members</param>
        private void LoadMethods(BindingFlags flags)
        {
            LoadMembers<MethodInfo, ReflectedMethod>(flags, ref this.methodCache, this.TargetType.GetMethods);
        }

        /// <summary>
        /// Gets all methods in the type
        /// </summary>
        /// <param name="flags">The flags used to search for members</param>
        /// <returns>A list of all methods on the type</returns>
        public IEnumerable<ReflectedMethod> GetMethods(BindingFlags flags = AnyBinding)
        {
            LoadMethods(flags);
            return this.methodCache.SelectMany(k => k.Value);
        }

        /// <summary>
        /// Gets a specific method matching the name
        /// </summary>
        /// <param name="name">The name of the method</param>
        /// <param name="flags">The flags used to search for members</param>
        /// <returns>The method with the specified name, or null</returns>
        public ReflectedMethod GetMethod(string name, BindingFlags flags = AnyBinding)
        {
            LoadMethods(flags);
            return this.methodCache[name].FirstOrDefault();
        }

        /// <summary>
        /// Gets a specific method matching the name and parameters
        /// </summary>
        /// <param name="name">The name of the method</param>
        /// <param name="args">The methods parameters</param>
        /// <param name="flags">The flags used to search for members</param>
        /// <returns>The method with the specified name, or null</returns>
        public ReflectedMethod GetMethod(string name, Type[] args, BindingFlags flags = AnyBinding)
        {
            LoadMethods(flags);
            return this.methodCache[name].FirstOrDefault(m => m.CheckParametersMatch(args));
        }
        /// <summary>
        /// Gets a specific method matching the name, parameters and return type
        /// </summary>
        /// <param name="name">The name of the method</param>
        /// <param name="args">The methods parameters</param>
        /// <param name="ret">The return type of the method</param>
        /// <param name="flags">The flags used to search for members</param>
        /// <returns>The method with the specified name, or null</returns>
        public ReflectedMethod GetMethod(string name, Type[] args, Type ret, BindingFlags flags = AnyBinding)
        {
            LoadMethods(flags);
            return this.methodCache[name].FirstOrDefault(m => m.CheckParametersMatch(args) && m.CheckReturnTypeMatch(ret));
        }

        /// <summary>
        /// Loads all properties in the type
        /// </summary>
        /// <param name="flags">The flags used to search for members</param>
        private void LoadProperties(BindingFlags flags)
        {
            LoadMembers<PropertyInfo, ReflectedProperty>(flags, ref this.propertyCache, this.TargetType.GetProperties);
        }

        /// <summary>
        /// Gets all properties in the type
        /// </summary>
        /// <param name="flags">The flags used to search for members</param>
        /// <returns>All the properties in the type</returns>
        public IEnumerable<ReflectedProperty> GetProperties(BindingFlags flags = AnyBinding)
        {
            LoadProperties(flags);
            return this.propertyCache.SelectMany(k => k.Value);
        }

        /// <summary>
        /// Get a property by name
        /// </summary>
        /// <param name="name">The name of the property</param>
        /// <param name="flags">The flags used to search for members</param>
        /// <returns>The property matching the specified name, or null</returns>
        public ReflectedProperty GetProperty(string name, BindingFlags flags = AnyBinding)
        {
            LoadProperties(flags);
            if (this.propertyCache.ContainsKey(name))
            {
                return this.propertyCache[name].FirstOrDefault();
            }
            return null;
        }
    }

    /// <summary>
    /// Typed reflection helper used to gain access to type members
    /// </summary>
    /// <typeparam name="T">The type to reflect</typeparam>
    public class ReflectionHelper<T> : ReflectionHelper
    {
        /// <summary>
        /// Create new reflector
        /// </summary>
        public ReflectionHelper()
            : base(typeof(T))
        {
        }

        /// <summary>
        /// Gets all constructors
        /// </summary>
        /// <param name="flags">The flags used to search for members</param>
        /// <returns></returns>
        public new IEnumerable<ReflectedConstructor<T>> GetConstructors(BindingFlags flags = AnyBinding)
        {
            return base.GetConstructors(flags).Select(c => new ReflectedConstructor<T>(c.Member));
        }

        /// <summary>
        /// Get a specific constructor matching a type array
        /// </summary>
        /// <param name="args">The parameters used to match against the constructor</param>
        /// <param name="flags">The flags used to search for members</param>
        /// <returns>The constructor matching the parameters, or null</returns>
        public new ReflectedConstructor<T> GetConstructor(Type[] args, BindingFlags flags = AnyBinding)
        {
            return new ReflectedConstructor<T>(base.GetConstructor(args, flags).Member);
        }

        /// <summary>
        /// Gets the first constructor
        /// </summary>
        /// <param name="flags">The flags used to search for members</param>
        /// <returns>The first constructor found, or null</returns>
        public new ReflectedConstructor<T> GetConstructor(BindingFlags flags = AnyBinding)
        {
            return new ReflectedConstructor<T>(base.GetConstructor(flags).Member);
        }
    }
}
