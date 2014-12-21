using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace StUtil.UI.Automation
{
    public class ValueElement : BaseAutomationHelper<ValueElement>
    {
         /// <summary>
        /// The value pattern
        /// </summary>
        private ValuePattern pattern;

        /// <summary>
        /// Create a new automation helper for the pattern
        /// </summary>
        /// <param name="element">The automation element to wrap</param>
        public ValueElement(AutomationElement element)
        {
            base.element = element;
            pattern = Element.GetCurrentPattern(ValuePattern.Pattern) as ValuePattern;
            if (pattern == null)
            {
                throw new ArgumentException("Helper must represent a value item");
            }
        }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        protected override ValueElement CreateInstance(AutomationElement element)
        {
            return new ValueElement(element);
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value
        {
            get { return pattern.Current.Value; }
            set { pattern.SetValue(value); }
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public ValueElement SetValue(string value)
        {
            pattern.SetValue(value);
            return this;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <returns></returns>
        public string GetValue()
        {
            return this.pattern.Current.Value;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public ValueElement GetValue(out string value)
        {
            value = this.GetValue();
            return this;
        }
        
    }
}
