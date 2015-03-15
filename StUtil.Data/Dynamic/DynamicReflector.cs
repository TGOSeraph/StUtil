using StUtil.Data.Generic;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Data.Dynamic
{
    public class DynamicReflector : DynamicReflectionBase
    {
        public delegate object ResultProcessor(DynamicReflector sender, object result);

        private Dictionary<object, Pair<ResultProcessor, bool>> processors = new Dictionary<object, Pair<ResultProcessor, bool>>();

        public virtual BindingFlags AccessFlags { get; set; }
        
        public DynamicReflector(object target)
        {
            Target = target;
            AccessFlags =  BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return TargetType.GetMembers(AccessFlags).Select(m => m.Name);
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            try
            {
                result = ProcessResult(Convert.ChangeType(Target, binder.Type));
                return true;
            }
            catch (Exception)
            {
                try
                {
                    if (base.TryConvert(binder, out result))
                    {
                        return true;
                    }
                }
                catch (Exception)
                {
                }
                result = ProcessResult(Target);
                return true;
            }
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            PropertyInfo prop = TargetType.GetProperty("Item", AccessFlags);
            result = ProcessResult(prop.GetValue(Target, indexes));
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            PropertyInfo prop = TargetType.GetProperty(binder.Name, AccessFlags);
            if (prop != null)
            {
                result = ProcessResult(prop.GetValue(Target));
            }
            else
            {
                FieldInfo field = TargetType.GetField(binder.Name, AccessFlags);
                if (field != null)
                {
                    result = ProcessResult(field.GetValue(Target));
                }
                else
                {
                    EventInfo evt = TargetType.GetEvent(binder.Name, AccessFlags);
                    if (evt != null)
                    {
                        result = new DynamicEventBinder(evt, Target);
                    }
                    else
                    {
                        throw new MissingMemberException(TargetType.FullName, binder.Name);
                    }
                }
            }
            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            MethodInfo method = null;
            try
            {
                method = TargetType.GetMethod(binder.Name, AccessFlags, null, args.Select(a => a == null ? null : a.GetType()).ToArray(), null);
            }
            catch (Exception)
            {
            }

            if (method != null)
            {
                result = ProcessResult(method.Invoke(Target, args));
            }
            else
            {
                try
                {
                    method = TargetType.GetMethod(binder.Name, AccessFlags);
                }
                catch (Exception)
                {
                }

                if (method != null)
                {
                    result = ProcessResult(method.Invoke(Target, args));
                }
                else
                {
                    EventInfo evt = TargetType.GetEvent(binder.Name, AccessFlags);
                    if (evt != null)
                    {
                        method = evt.GetRaiseMethod(true);
                        if (method == null)
                        {
                            method = TargetType.GetMethod("On" + binder.Name);
                        }
                        if (method == null)
                        {
                            throw new MissingMemberException(TargetType.FullName, binder.Name);
                        }
                        method.Invoke(Target, args);
                    }
                    else
                    {
                        throw new MissingMemberException(TargetType.FullName, binder.Name);
                    }
                    result = null;
                }
            }

            return true;
        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            PropertyInfo prop = TargetType.GetProperty("Item", AccessFlags);
            prop.SetValue(Target, value, indexes);
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            PropertyInfo prop = TargetType.GetProperty(binder.Name, AccessFlags);
            if (prop != null)
            {
                prop.SetValue(Target, value);
            }
            else
            {
                FieldInfo field = TargetType.GetField(binder.Name, AccessFlags);
                if (field != null)
                {
                    field.SetValue(Target, value);
                }
                else
                {
                    EventInfo evt = TargetType.GetEvent(binder.Name, AccessFlags);
                    if (evt != null)
                    {
                    }
                    else
                    {
                        throw new MissingMemberException(TargetType.FullName, binder.Name);
                    }
                }
            }
            return true;
        }

        public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
        {
            foreach (object arg in args)
            {
                if (processors.ContainsKey(arg))
                {
                    processors[arg].Second = !processors[arg].Second;
                    result = this;
                    return true;
                }
                else
                {
                    break;
                }
            }

            bool success = base.TryInvoke(binder, args, out result);
            result = ProcessResult(result);
            return success;
        }

        public override string ToString()
        {
            return Target.ToString();
        }

        public object RegisterProcessor(ResultProcessor processor, bool enabled = false)
        {
            object obj = new object();
            processors.Add(obj, new Pair<ResultProcessor, bool>(processor, enabled));
            return obj;
        }

        private object ProcessResult(object value)
        {
            foreach (var kvp in processors.Values)
            {
                if (kvp.Second)
                {
                    return kvp.First(this, value);
                }
            }
            return value;
        }
    }
}
