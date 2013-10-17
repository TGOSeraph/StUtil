using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Automation;

namespace StUtil.Automation
{
    /// <summary>
    /// Helper class for Automation Conditions to provide easier creation
    /// </summary>
    /// <remarks>
    /// 2013-06-28  - Initial version
    /// </remarks>
    public class AutomationSelector
    {
        /// <summary>
        /// The properties that are to be used in the condition
        /// </summary>
        public AutomationProperty[] Properties { get; set; }
        /// <summary>
        /// The values for each of the properties to be used in the condition
        /// </summary>
        public object[] Values { get; set; }

        /// <summary>
        /// Create a new selector from a property and value
        /// </summary>
        /// <param name="property">The property to use in the condition</param>
        /// <param name="value">The value to use in the condition</param>
        public AutomationSelector(AutomationProperty property, object value)
        {
            this.Properties = new AutomationProperty[] { property };
            this.Values = new object[] { value };
        }

        /// <summary>
        /// Create a new selector for a list of properties and values
        /// </summary>
        /// <param name="properties">The properties to use in the condition</param>
        /// <param name="values">The values to use in the condition</param>
        public AutomationSelector(AutomationProperty[] properties, object[] values)
        {
            this.Properties = properties;
            this.Values = values;
        }

        /// <summary>
        /// Create a new selector by combining two existing selectors
        /// </summary>
        /// <param name="s1">The first selector to combine</param>
        /// <param name="s2">The second selector to combine</param>
        public AutomationSelector(AutomationSelector s1, AutomationSelector s2)
        {
            this.Properties = s1.Properties.Concat(s2.Properties).ToArray();
            this.Values = s1.Values.Concat(s2.Values).ToArray();
        }

        /// <summary>
        /// Join a selector to the current instance
        /// </summary>
        /// <param name="selector">The selector to join to the current instance</param>
        /// <returns>The specified selector's values and properties combined with the current instances values and properties</returns>
        public AutomationSelector And(AutomationSelector selector)
        {
            return new AutomationSelector(this, selector);
        }

        /// <summary>
        /// Create a new selector from an automation id
        /// </summary>
        /// <param name="id">The automation ID to use in the condition</param>
        /// <returns>A selector containing the specified automation ID condition</returns>
        public static AutomationSelector FromId(string id)
        {
            return new AutomationSelector(AutomationElement.AutomationIdProperty, id);
        }


        public static AutomationSelector FromClass(string className)
        {
            return new AutomationSelector(AutomationElement.ClassNameProperty, className);
        }

        /// <summary>
        /// Create a new selector from a name
        /// </summary>
        /// <param name="name">The name used in the condition</param>
        /// <returns>A selector containing the specified name condition</returns>
        public static AutomationSelector FromName(string name)
        {
            return new AutomationSelector(AutomationElement.NameProperty, name);
        }

        /// <summary>
        /// Create a new selector for a specific control type
        /// </summary>
        /// <param name="type">The control type used in the condition</param>
        /// <returns>A selector containing the specified control type condition</returns>
        public static AutomationSelector FromControlType(ControlType type)
        {
            return new AutomationSelector(AutomationElement.ControlTypeProperty, type);
        }

        /// <summary>
        /// Create a new selector for a specific enabled state
        /// </summary>
        /// <param name="enabled">The enabled state used in the condition</param>
        /// <returns>A selector containing the specified enabled state condition</returns>
        public static AutomationSelector FromEnabled(bool enabled)
        {
            return new AutomationSelector(AutomationElement.IsEnabledProperty, enabled);
        }

        /// <summary>
        /// Create a selector that selects items that implement the expand collapse pattern
        /// </summary>
        /// <returns>A condition that specifies that the element must implement the expand collapse pattern</returns>
        public static AutomationSelector FromExpandable()
        {
            return new AutomationSelector(AutomationElement.IsExpandCollapsePatternAvailableProperty, true);
        }

        /// <summary>
        /// Create a selector that selects items that implement the selectable item pattern
        /// </summary>
        /// <returns>A condition that specifies that the element must implement the selectable item pattern</returns>
        public static AutomationSelector FromSelectable()
        {
            return new AutomationSelector(AutomationElement.IsSelectionItemPatternAvailableProperty, true);
        }

        /// <summary>
        /// Create a Condition instance from this selector
        /// </summary>
        /// <returns>Each of the properties/values AND'ed together as PropertyConditions</returns>
        public Condition ToCondition()
        {
            if (Properties.Length != Values.Length)
                throw new OverflowException();

            if (Properties.Length == 1)
            {
                return new PropertyCondition(Properties[0], Values[0]);
            }
            else
            {
                return new AndCondition(Properties.Select((p, i) => new PropertyCondition(p, Values[i])).ToArray());
            }
        }
    }
}
