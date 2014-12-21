using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace StUtil.UI.Automation
{
    public class InvokableElement : BaseAutomationHelper<InvokableElement>
    {
        /// <summary>
        /// The invoke pattern
        /// </summary>
        private InvokePattern pattern;

        /// <summary>
        /// Create a new automation helper for the pattern
        /// </summary>
        /// <param name="element">The automation element to wrap</param>
        public InvokableElement(AutomationElement element)
        {
            base.element = element;
            pattern = Element.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
            if (pattern == null)
            {
                throw new ArgumentException("Helper must represent an invokable item");
            }
        }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        protected override InvokableElement CreateInstance(System.Windows.Automation.AutomationElement element)
        {
            return new InvokableElement(element);
        }

        /// <summary>
        /// Invokes this instance.
        /// </summary>
        public InvokableElement Invoke()
        {
            pattern.Invoke();
            return this;
        }
    }
}
