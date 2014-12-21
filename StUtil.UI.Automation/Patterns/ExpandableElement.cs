using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Automation;

namespace StUtil.UI.Automation
{
    /// <summary>
    /// Automation helper implementing the ExpandCollapse pattern
    /// </summary>
    public class ExpandableElement
        : BaseAutomationHelper<ExpandableElement>
    {
        /// <summary>
        /// The expand collapse pattern
        /// </summary>
        private ExpandCollapsePattern pattern;

        /// <summary>
        /// Create a new automation helper for the pattern
        /// </summary>
        /// <param name="element">The automation element to wrap</param>
        public ExpandableElement(AutomationElement element)
        {
            base.element = element;
            pattern = Element.GetCurrentPattern(ExpandCollapsePattern.Pattern) as ExpandCollapsePattern;
            if (pattern == null)
            {
                throw new ArgumentException("Helper must represent a selectable item");
            }
        }

        /// <summary>
        /// Expand the element
        /// </summary>
        /// <returns>The current helper</returns>
        public ExpandableElement Expand()
        {
            pattern.Expand();
            return this;
        }

        /// <summary>
        /// Collapse the element
        /// </summary>
        /// <returns>The current helper</returns>
        public ExpandableElement Collapse()
        {
            pattern.Collapse();
            return this;
        }

        /// <summary>
        /// Get the state of the element
        /// </summary>
        public ExpandCollapseState State
        {
            get
            {
                return pattern.Current.ExpandCollapseState;
            }
        }

        /// <summary>
        /// Create a new wrapper for the specified element
        /// </summary>
        /// <param name="element">The element to wrap in a new automation helper</param>
        /// <returns>A new automation helper instance wrapping the specified element</returns>
        protected override ExpandableElement CreateInstance(System.Windows.Automation.AutomationElement element)
        {
            return new ExpandableElement(element);
        }
    }
}
