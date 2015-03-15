using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Data.Dynamic
{
    public class DynamicEventBinder : DynamicReflectionBase
    {
        protected EventInfo ReflectedEvent { get; private set; }

        public DynamicEventBinder()
        {
        }

        public DynamicEventBinder(EventInfo reflectedEvent, object target)
            : base(target)
        {
            this.ReflectedEvent = reflectedEvent;
        }

        public override bool TryBinaryOperation(BinaryOperationBinder binder, object arg, out object result)
        {
            switch (binder.Operation)
            {
                case System.Linq.Expressions.ExpressionType.AddAssign:
                    ReflectedEvent.AddEventHandler(Target, (Delegate)arg);
                    break;
                case System.Linq.Expressions.ExpressionType.SubtractAssign:
                    ReflectedEvent.RemoveEventHandler(Target, (Delegate)arg);
                    break;
                default:
                    throw new NotImplementedException();
            }
            result = Target;
            return true;
        }
    }
}
