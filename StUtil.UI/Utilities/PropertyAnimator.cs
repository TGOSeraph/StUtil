using StUtil.Generic.Progressable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StUtil.UI.Utilities
{
    public abstract class PropertyAnimator<TValue> : ProgressableProcess
    {
        public int Steps { get; set; }
        public TValue StartValue { get; set; }
        public TValue EndValue { get; set; }

        private int sleepTime = 10;
        public int SleepTime { get { return sleepTime; } set { sleepTime = value; } }

        public bool Enabled { get; set; }

        protected PropertyInfo property;
        public object Instance { get; set; }

        public PropertyAnimator()
        {
            this.Steps = 20;
        }

        public void SetProperty(Expression<Func<TValue>> property)
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
                this.property = memberExpr.Member as PropertyInfo;

            if (this.property == null)
            {
                throw new NullReferenceException("Property is null");
            }
        }

        protected abstract TValue ComputeStep(int step);
        protected abstract bool IsValidState();
        protected abstract void InvokeUpdate(TValue value);

        protected void UpdateValue(TValue v)
        {
            property.SetValue(Instance, v);
        }

        protected override void OnProgressChanged(object sender, ProgressableProgressChangedEventArgs e)
        {
            if (!IsValidState())
            {
                this.Cancel();
            }
            else
            {
                try
                {
                    InvokeUpdate((TValue)e.Data);
                }
                catch (Exception)
                {
                }
            }

            base.OnProgressChanged(sender, e);
        }

        private void StartAnimation(TValue end)
        {
            this.EndValue = end;
            this.Start();
        }

        public void PerformAnimation(TValue start, TValue end)
        {
            if (this.IsRunning)
            {
                this.Cancel();
                Thread t = new Thread(delegate()
                {
                    while (this.IsRunning)
                    {
                        Thread.Sleep(0);
                    }
                    this.StartValue = start;
                    StartAnimation(end);
                });
                t.Start();
            }
            else
            {
                this.StartValue = start;
                StartAnimation(end);
            }
        }

        public void PerformAnimation(TValue end)
        {
            if (this.IsRunning)
            {
                this.Cancel();
                Thread t = new Thread(delegate()
                {
                    while (this.IsRunning)
                    {
                        Thread.Sleep(0);
                    }
                    this.StartValue = (TValue)Convert.ChangeType(property.GetValue(Instance), typeof(TValue));
                    StartAnimation(end);
                });
                t.Start();
            }
            else
            {
                this.StartValue = (TValue)Convert.ChangeType(property.GetValue(Instance), typeof(TValue));
                StartAnimation(end);
            }
        }

        #region Progressable
        protected override void PerformProcess()
        {
            for (int i = 0; i < Steps; i++)
            {
                OnProgressChanged(this, new ProgressableProgressChangedEventArgs((decimal)i, (decimal)100, ComputeStep(i)));
                Thread.Sleep(SleepTime);
                if (base.IsCancelling)
                {
                    OnCancel(this, new EventArgs());
                    return;
                }
            }
        }

        protected override decimal GetMaximumProgressValue()
        {
            return (decimal)Steps;
        }

        protected override bool UseNewThread()
        {
            return true;
        }

        protected override object GetStartEventData()
        {
            return null;
        }
        #endregion
    }
}
