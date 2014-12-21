using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace StUtil.UI.Automation
{
    /// <summary>
    /// Automation helper implementing the selection item pattern
    /// </summary>
    public class SelectableElement : BaseAutomationHelper<SelectableElement>
    {
        /// <summary>
        /// The expand collapse pattern
        /// </summary>
        private SelectionItemPattern pattern;

        /// <summary>
        /// Create a new automation helper for the pattern
        /// </summary>
        /// <param name="element">The automation element to wrap</param>
        public SelectableElement(AutomationElement element)
        {
            base.element = element;
            pattern = Element.GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern;
            if (pattern == null)
            {
                throw new ArgumentException("Helper must represent a selectable item");
            }
        }

        /// <summary>
        /// Add the element to the selection
        /// </summary>
        /// <returns>The current helper</returns>
        public SelectableElement AddToSelection()
        {
            pattern.AddToSelection();
            return this;
        }

        /// <summary>
        /// Remove the element from the selection
        /// </summary>
        /// <returns>The current helper</returns>
        public SelectableElement RemoveFromSelection()
        {
            pattern.RemoveFromSelection();
            return this;
        }

        /// <summary>
        /// Select the element
        /// </summary>
        /// <returns>The current helper</returns>
        public SelectableElement Select()
        {
            pattern.Select();
            return this;
        }

        /// <summary>
        /// If the element is selected
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return pattern.Current.IsSelected;
            }
        }

        /// <summary>
        /// The owner of the element (i.e. list, tree, etc)
        /// </summary>
        public AutomationHelper Container
        {
            get
            {
                return new AutomationHelper(pattern.Current.SelectionContainer);
            }
        }

        /// <summary>
        /// Create a new wrapper for the specified element
        /// </summary>
        /// <param name="element">The element to wrap in a new automation helper</param>
        /// <returns>A new automation helper instance wrapping the specified element</returns>
        protected override SelectableElement CreateInstance(AutomationElement element)
        {
            return new SelectableElement(element);
        }
    }
}
