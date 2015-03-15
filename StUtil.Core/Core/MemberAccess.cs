using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Core
{
    public class MemberAccess<TModel, TValue>
    {
        private Func<TModel, TValue> get;
        private Action<TModel, TValue> set;
        public TModel Target { get; private set; }


        public MemberAccess(TModel target, FieldInfo field)
        {
            this.Target = target;
            HandleField(field);
        }

        public MemberAccess(TModel target, PropertyInfo property)
        {
            this.Target = target;
            HandleProperty(property);
        }

        public MemberAccess(TModel target, MemberInfo member)
        {
            this.Target = target;
            if (member.MemberType == MemberTypes.Property)
            {
                HandleProperty((PropertyInfo)member);
            }
            else if (member.MemberType == MemberTypes.Field)
            {
                HandleField((FieldInfo)member);
            }
            else
            {
                throw new ArgumentException("Member must be a property or field", "member");
            }
        }

        public MemberAccess(string member)
            : this(default(TModel), member, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
        {
        }

        public MemberAccess(TModel target, string member)
            : this(target, member, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
        {
        }

        public MemberAccess(TModel target, string member, BindingFlags binding)
        {
            this.Target = target;
            Type t = target == null ? typeof(TModel) : target.GetType();

            PropertyInfo prop = t.GetProperty(member, binding);
            if (prop != null)
            {
                HandleProperty(prop);
            }
            else
            {
                FieldInfo field = t.GetField(member, binding);
                if (field != null)
                {
                    HandleField(field);
                }
                else
                {
                    throw new ArgumentException("Could not find property or field matching '" + member + "'", "member");
                }
            }
        }
        public MemberAccess(TModel target, Func<TModel, TValue> get, Action<TModel, TValue> set)
        {
            this.Target = target;
            this.get = get;
            this.set = set;
        }
        public MemberAccess(Func<TValue> get, Action<TValue> set)
        {
            this.get = (o) => get();
            this.set = (o, v) => set(v);
        }
        public MemberAccess(Expression<Func<TModel, TValue>> expression)
        {
            ProcessExpression(expression.Body);
        }
        public MemberAccess(Expression<Func<TValue>> expression)
        {
            ProcessExpression(expression.Body);
        }

        public TValue Get()
        {
            return get(Target);
        }

        public void Set(TValue value)
        {
            set(Target, value);
        }

        public TValue Get(TModel target)
        {
            return get(target);
        }

        public void Set(TModel target, TValue value)
        {
            set(target, value);
        }

        public static implicit operator TValue(MemberAccess<TModel, TValue> mem)
        {
            return mem.Get();
        }

        private void HandleProperty(PropertyInfo prop)
        {
            get = new Func<TModel, TValue>(o => (TValue)StUtil.Utilities.TypeUtilities.ConvertType(prop.GetValue(o), typeof(TValue)));
            set = new Action<TModel, TValue>((o, v) => prop.SetValue(o, StUtil.Utilities.TypeUtilities.ConvertType(v, typeof(TValue))));
        }

        private void HandleField(FieldInfo field)
        {
            get = new Func<TModel, TValue>(o => (TValue)StUtil.Utilities.TypeUtilities.ConvertType(field.GetValue(o), typeof(TValue)));
            set = new Action<TModel, TValue>((o, v) => field.SetValue(o, StUtil.Utilities.TypeUtilities.ConvertType(v, typeof(TValue))));
        }

        private MemberExpression ExpressionToMemberExpression(Expression expression)
        {
            MemberExpression memberExpr = expression as MemberExpression;

            if (memberExpr == null)
            {
                UnaryExpression unaryExpr = expression as UnaryExpression;
                if (unaryExpr != null && unaryExpr.NodeType == ExpressionType.Convert)
                {
                    memberExpr = unaryExpr.Operand as MemberExpression;
                }
            }
            return memberExpr;
        }

        private MemberInfo GetMember(Expression body)
        {
            MemberExpression memberExpr = ExpressionToMemberExpression(body);

            if (memberExpr != null)
            {
                ConstantExpression constExpr = memberExpr.Expression as ConstantExpression;
                MemberExpression mem = memberExpr.Expression as MemberExpression;
                Stack<MemberInfo> getters = new Stack<MemberInfo>();

                while (constExpr == null)
                {
                    getters.Push(mem.Member);
                    constExpr = mem.Expression as ConstantExpression;
                    mem = ExpressionToMemberExpression(mem.Expression);
                }

                if (constExpr != null)
                {
                    object o = constExpr.Value;

                    while (getters.Count > 0)
                    {
                        MemberInfo getter = getters.Pop();
                        o = new MemberAccess(o, getter).Get();
                    }

                    Target = (TModel)o;
                }

                return memberExpr.Member;
            }
            else
            {
                throw new ArgumentException("Unable to extract member from expression", "expression");
            }
        }

        private void ProcessExpression(Expression body)
        {
            MemberInfo mem = GetMember(body);

            if (mem.MemberType == MemberTypes.Property)
            {
                HandleProperty((PropertyInfo)mem);
            }
            else if (mem.MemberType == MemberTypes.Field)
            {
                HandleField((FieldInfo)mem);
            }
            else
            {
                throw new ArgumentException("Expression must be a property or field", "expression");
            }
        }
    }

    public class MemberAccess<TValue> : MemberAccess<object, TValue>
    {
        public MemberAccess(object target, FieldInfo field)
            : base(target, field) { }

        public MemberAccess(object target, PropertyInfo property)
            : base(target, property) { }

        public MemberAccess(object target, MemberInfo member)
            : base(target, member) { }

        public MemberAccess(object target, string member)
            : base(target, member) { }

        public MemberAccess(object target, string member, BindingFlags binding)
            : base(target, member, binding) { }

        public MemberAccess(TValue target, Func<object, TValue> get, Action<object, TValue> set)
            : base(target, get, set) { }

        public MemberAccess(Func<TValue> get, Action<TValue> set)
            : base(null, (o) => get(), (o, v) => set(v)) { }

        public MemberAccess(Expression<Func<object, TValue>> expression)
            : base(expression) { }

        public MemberAccess(Expression<Func<TValue>> expression)
            : base(expression) { }
    }

    public class MemberAccess : MemberAccess<object>
    {
        public MemberAccess(object target, FieldInfo field)
            : base(target, field) { }

        public MemberAccess(object target, PropertyInfo property)
            : base(target, property) { }

        public MemberAccess(object target, MemberInfo member)
            : base(target, member) { }
        public MemberAccess(object target, string member)
            : base(target, member) { }

        public MemberAccess(object target, string member, BindingFlags binding)
            : base(target, member, binding) { }

        public MemberAccess(object target, Func<object, object> get, Action<object, object> set)
            : base(target, get, set) { }

        public MemberAccess(Func<object> get, Action<object> set)
            : base(null, (o) => get(), (o, v) => set(v)) { }

        public MemberAccess(Expression<Func<object, object>> expression)
            : base(expression) { }

        public MemberAccess(Expression<Func<object>> expression)
            : base(expression) { }
    }


}
