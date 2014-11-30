using System;
using System.Linq.Expressions;
using System.Reflection;

namespace StUtil.Core
{
    public class PropertyAccessor : PropertyAccessor<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyAccessor"/> class.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="property">The property.</param>
        public PropertyAccessor(object obj, string property)
            : base(obj, property)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyAccessor"/> class.
        /// </summary>
        /// <param name="property">The property.</param>
        public PropertyAccessor(Expression<Func<object>> property)
            : base(property)
        {
        }
    }

    public class PropertyAccessor<TValue>
    {
        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <value>
        /// The object.
        /// </value>
        public object Object { get; private set; }

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <value>
        /// The property.
        /// </value>
        public PropertyInfo Property { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyAccessor{TValue}"/> class.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="property">The property.</param>
        /// <exception cref="System.NullReferenceException">Property is null</exception>
        public PropertyAccessor(object obj, string property)
        {
            this.Property = obj.GetType().GetProperty(property, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            if (this.Property == null)
            {
                throw new NullReferenceException("Property is null");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyAccessor{TValue}"/> class.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <exception cref="System.ArgumentNullException">property;property is null.</exception>
        /// <exception cref="System.NullReferenceException">Property is null</exception>
        public PropertyAccessor(Expression<Func<TValue>> property)
        {
            if (property == null)
                throw new ArgumentNullException("property", "property is null.");

            MemberExpression memberExpr = property.Body as MemberExpression;
            if (memberExpr == null)
            {
                UnaryExpression unaryExpr = property.Body as UnaryExpression;
                if (unaryExpr != null && unaryExpr.NodeType == ExpressionType.Convert)
                    memberExpr = unaryExpr.Operand as MemberExpression;
            }

            if (memberExpr != null && memberExpr.Member.MemberType == MemberTypes.Property)
                this.Property = memberExpr.Member as PropertyInfo;

            if (this.Property == null)
            {
                throw new NullReferenceException("Property is null");
            }
        }
    }
}