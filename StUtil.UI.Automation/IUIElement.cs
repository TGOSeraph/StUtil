using StUtil.UI.Automation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace StUtil.UI.Automation
{
    /// <summary>
    /// Interface for Automation helpers
    /// </summary>
    public interface IAutomationHelper
    {
        /// <summary>
        /// The base element for this automation helper
        /// </summary>
        AutomationElement Element { get; }
        /// <summary>
        /// The name of the element
        /// </summary>
        string Name { get; }
        /// <summary>
        /// The control type of the element
        /// </summary>
        ControlType Type { get; }
        /// <summary>
        /// The automation ID of the element
        /// </summary>
        int ID { get; }

        /// <summary>
        /// Get the first child matching the specified condition
        /// </summary>
        /// <param name="condition">The conditions to match</param>
        /// <returns>An automation helper wrapping the first child matching the specified condition</returns>
        IAutomationHelper FirstChildWhere(Condition condition);
        /// <summary>
        /// Get the first child matching the specified selector
        /// </summary>
        /// <param name="selector">The selector to match</param>
        /// <returns>An automation helper wrapping the first child matching the specified selector</returns>
        IAutomationHelper FirstChildWhere(AutomationSelector selector);
        /// <summary>
        /// Get the all children matching the specified condition
        /// </summary>
        /// <param name="condition">The conditions to match</param>
        /// <returns>A list of automation helpers wrapping the children matching the specified condition</returns>
        List<IAutomationHelper> ChildrenWhere(Condition condition);
        /// <summary>
        /// Get the all children matching the specified selector
        /// </summary>
        /// <param name="selector">The selector to match</param>
        /// <returns>A list of automation helpers wrapping the children matching the specified selector</returns>
        List<IAutomationHelper> ChildrenWhere(AutomationSelector selector);

        /// <summary>
        /// Wait for a child to be found for a specified period of time
        /// </summary>
        /// <param name="condition">The condition to match</param>
        /// <param name="timeout">The amount of time to wait before throwing an exception</param>
        /// <returns>An automation helper wrapping the first child matching the specified condition</returns>
        IAutomationHelper WaitForChildWhere(Condition condition, int timeout);
        /// <summary>
        /// Wait for a child to be found for a specified period of time
        /// </summary>
        /// <param name="selector">The selector to match</param>
        /// <param name="timeout">The amount of time to wait before throwing an exception</param>
        /// <returns>An automation helper wrapping the first child matching the specified selector</returns>
        IAutomationHelper WaitForChildWhere(AutomationSelector selector, int timeout);

        /// <summary>
        /// Descend the children of the element by traversing children until one that does not match the specified critera is found.
        /// </summary>
        /// <param name="props">The properties to check on each of the elements</param>
        /// <param name="validators">Functions to return values for the current element, corresponding index->index with props, of each of the conditions</param>
        /// <param name="validate">A function to process the element and returns TRUE to continue descending, FALSE to stop</param>
        /// <returns>The last child of the initial element to match the specified conditions</returns>
        IAutomationHelper Descend(AutomationProperty[] props, Func<object>[] validators, Func<IAutomationHelper, bool> validate);
        /// <summary>
        /// Descend the children of the element by traversing children until one that does not match the specified critera is found.
        /// </summary>
        /// <param name="validators">A dictionary of Property->Values to use as the checking conditions on each of the children</param>
        /// <param name="validate">A function to process the element and returns TRUE to continue descending, FALSE to stop</param>
        /// <returns>The last child of the initial element to match the specified conditions</returns>
        IAutomationHelper Descend(Dictionary<AutomationProperty, Func<object>> validators, Func<IAutomationHelper, bool> validate);

        /// <summary>
        /// Create a helper that implements the Selectable Item pattern
        /// </summary>
        /// <returns>A helper that implements the Selectable Item pattern</returns>
        SelectableElement AsSelectableItem();
        /// <summary>
        /// Create a helper that implements the ExpandContract pattern
        /// </summary>
        /// <returns>A helper that implements the ExpandContract pattern</returns>
        ExpandableElement AsExpandable();
        /// <summary>
        /// Create a helper that implements the Value pattern
        /// </summary>
        /// <returns>A helper that implements the Value pattern</returns>
        ValueElement AsValueItem();
        /// <summary>
        /// Create a helper that implements the Invoke pattern
        /// </summary>
        /// <returns>A helper that implements the Invoke pattern</returns>
        InvokableElement AsInvokable();
    }
}
