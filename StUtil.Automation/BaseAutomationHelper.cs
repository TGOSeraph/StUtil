using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using System.Windows.Automation;

namespace StUtil.Automation
{
    /// <summary>
    /// Base abstract generic class for automation helpers to return helper elements of the generic type
    /// </summary>
    /// <remarks>
    /// 2013-06-28  - Initial version
    /// </remarks>
    public abstract class BaseAutomationHelper<T>
        : BaseAutomationHelper where T : BaseAutomationHelper
    {
        /// <summary>
        /// Get the first child matching the specified condition
        /// </summary>
        /// <param name="condition">The conditions to match</param>
        /// <returns>An automation helper wrapping the first child matching the specified condition</returns>
        public new T FirstChildWhere(Condition condition)
        {
            return (T)base.FirstChildWhere(condition);
        }
        /// <summary>
        /// Get the first child matching the specified selector
        /// </summary>
        /// <param name="selector">The selector to match</param>
        /// <returns>An automation helper wrapping the first child matching the specified selector</returns>
        public new T FirstChildWhere(AutomationSelector selector)
        {
            return (T)base.FirstChildWhere(selector);
        }
        /// <summary>
        /// Get the all children matching the specified condition
        /// </summary>
        /// <param name="condition">The conditions to match</param>
        /// <returns>A list of automation helpers wrapping the children matching the specified condition</returns>
        public new List<T> ChildrenWhere(Condition condition)
        {
            return base.ChildrenWhere(condition).Select(v => (T)v).ToList();
        }
        /// <summary>
        /// Get the all children matching the specified selector
        /// </summary>
        /// <param name="selector">The selector to match</param>
        /// <returns>A list of automation helpers wrapping the children matching the specified selector</returns>
        public new List<T> ChildrenWhere(AutomationSelector selector)
        {
            return base.ChildrenWhere(selector).Select(v => (T)v).ToList();
        }

        /// <summary>
        /// Wait for a child to be found for a specified period of time
        /// </summary>
        /// <param name="condition">The condition to match</param>
        /// <param name="timeout">The amount of time to wait before throwing an exception</param>
        /// <returns>An automation helper wrapping the first child matching the specified condition</returns>
        public new T WaitForChildWhere(Condition condition, int timeout)
        {
            return (T)base.WaitForChildWhere(condition, timeout);
        }
        /// <summary>
        /// Wait for a child to be found for a specified period of time
        /// </summary>
        /// <param name="selector">The selector to match</param>
        /// <param name="timeout">The amount of time to wait before throwing an exception</param>
        /// <returns>An automation helper wrapping the first child matching the specified selector</returns>
        public new T WaitForChildWhere(AutomationSelector selector, int timeout)
        {
            return (T)base.WaitForChildWhere(selector, timeout);
        }

        /// <summary>
        /// Descend the children of the element by traversing children until one that does not match the specified critera is found.
        /// </summary>
        /// <param name="props">The properties to check on each of the elements</param>
        /// <param name="validators">Functions to return values for the current element, corresponding index->index with props, of each of the conditions</param>
        /// <param name="validate">A function to process the element and returns TRUE to continue descending, FALSE to stop</param>
        /// <returns>The last child of the initial element to match the specified conditions</returns>
        public new T Descend(AutomationProperty[] props, Func<object>[] validators, Func<IAutomationHelper, bool> validate)
        {
            return (T)base.Descend(props, validators, validate);
        }
        /// <summary>
        /// Descend the children of the element by traversing children until one that does not match the specified critera is found.
        /// </summary>
        /// <param name="validators">A dictionary of Property->Values to use as the checking conditions on each of the children</param>
        /// <param name="validate">A function to process the element and returns TRUE to continue descending, FALSE to stop</param>
        /// <returns>The last child of the initial element to match the specified conditions</returns>
        public new T Descend(Dictionary<AutomationProperty, Func<object>> validators, Func<IAutomationHelper, bool> validate)
        {
            return (T)base.Descend(validators, validate);
        }

        /// <summary>
        /// Create a new wrapper for the specified element
        /// </summary>
        /// <param name="element">The element to wrap in a new automation helper</param>
        /// <returns>A new automation helper instance wrapping the specified element</returns>
        protected abstract T CreateInstance(AutomationElement element);

        /// <summary>
        /// Create a new wrapper for the specified element
        /// </summary>
        /// <param name="element">The element to wrap in a new automation helper</param>
        /// <returns>A new automation helper instance wrapping the specified element</returns>
        protected override BaseAutomationHelper _CreateInstance(AutomationElement element)
        {
            return (BaseAutomationHelper)CreateInstance(element);
        }
    }

    /// <summary>
    /// Base abstract class for automation helpers, implementing the IAutomationHelper interface
    /// </summary>
    /// <remarks>
    /// 2013-06-28  - Initial version
    /// </remarks>
    public abstract class BaseAutomationHelper : IAutomationHelper
    {
        /// <summary>
        /// The base element for this automation helper
        /// </summary>
        protected AutomationElement element;
        /// <summary>
        /// The base element for this automation helper
        /// </summary>
        public AutomationElement Element
        {
            get { return element; }
        }

        /// <summary>
        /// The name of the element
        /// </summary>
        public string Name
        {
            get
            {
                return Element.GetCurrentPropertyValue(AutomationElement.NameProperty).ToString();
            }
        }

        /// <summary>
        /// The control type of the element
        /// </summary>
        public ControlType Type
        {
            get
            {
                return Element.GetCurrentPropertyValue(AutomationElement.ControlTypeProperty) as ControlType;
            }
        }

        /// <summary>
        /// The automation ID of the element
        /// </summary>
        public int ID
        {
            get
            {
                return int.Parse(Element.GetCurrentPropertyValue(AutomationElement.AutomationIdProperty).ToString());
            }
        }

        /// <summary>
        /// Get the first child matching the specified condition
        /// </summary>
        /// <param name="condition">The conditions to match</param>
        /// <returns>An automation helper wrapping the first child matching the specified condition</returns>
        public virtual IAutomationHelper FirstChildWhere(Condition condition)
        {
            return _CreateInstance(Element.FindFirst(TreeScope.Children, condition));
        }
        /// <summary>
        /// Get the first child matching the specified selector
        /// </summary>
        /// <param name="selector">The selector to match</param>
        /// <returns>An automation helper wrapping the first child matching the specified selector</returns>
        public virtual IAutomationHelper FirstChildWhere(AutomationSelector selector)
        {
            return FirstChildWhere(selector.ToCondition());
        }
        /// <summary>
        /// Get the all children matching the specified condition
        /// </summary>
        /// <param name="condition">The conditions to match</param>
        /// <returns>A list of automation helpers wrapping the children matching the specified condition</returns>
        public virtual List<IAutomationHelper> ChildrenWhere(Condition condition)
        {
            List<IAutomationHelper> helpers = new List<IAutomationHelper>();
            foreach (AutomationElement el in Element.FindAll(TreeScope.Children, condition))
            {
                helpers.Add(_CreateInstance(el));
            }
            return helpers;
        }
        /// <summary>
        /// Get the all children matching the specified selector
        /// </summary>
        /// <param name="selector">The selector to match</param>
        /// <returns>A list of automation helpers wrapping the children matching the specified selector</returns>
        public virtual List<IAutomationHelper> ChildrenWhere(AutomationSelector selector)
        {
            return ChildrenWhere(selector.ToCondition());
        }

        /// <summary>
        /// Wait for a child to be found for a specified period of time
        /// </summary>
        /// <param name="condition">The condition to match</param>
        /// <param name="timeout">The amount of time to wait before throwing an exception</param>
        /// <returns>An automation helper wrapping the first child matching the specified condition</returns>
        public IAutomationHelper WaitForChildWhere(Condition condition, int timeout)
        {
            IAutomationHelper item = null;
            DateTime dt = DateTime.Now.Add(TimeSpan.FromMilliseconds(timeout));
            while (DateTime.Now < dt)
            {
                item = FirstChildWhere(condition);
                if (item.Element != null)
                {
                    return item;
                }
            }
            throw new TimeoutException();
        }
        /// <summary>
        /// Wait for a child to be found for a specified period of time
        /// </summary>
        /// <param name="selector">The selector to match</param>
        /// <param name="timeout">The amount of time to wait before throwing an exception</param>
        /// <returns>An automation helper wrapping the first child matching the specified selector</returns>
        public IAutomationHelper WaitForChildWhere(AutomationSelector selector, int timeout)
        {
            return WaitForChildWhere(selector.ToCondition(), timeout);
        }

        /// <summary>
        /// Descend the children of the element by traversing children until one that does not match the specified critera is found.
        /// </summary>
        /// <param name="props">The properties to check on each of the elements</param>
        /// <param name="validators">Functions to return values for the current element, corresponding index->index with props, of each of the conditions</param>
        /// <param name="validate">A function to process the element and returns TRUE to continue descending, FALSE to stop</param>
        /// <returns>The last child of the initial element to match the specified conditions</returns>
        public virtual IAutomationHelper Descend(AutomationProperty[] props, Func<object>[] validators, Func<IAutomationHelper, bool> validate)
        {
            return Descend(props.Select((k, i) => new { k, v = validators[i] })
              .ToDictionary(x => x.k, x => x.v), validate);
        }
        /// <summary>
        /// Descend the children of the element by traversing children until one that does not match the specified critera is found.
        /// </summary>
        /// <param name="validators">A dictionary of Property->Values to use as the checking conditions on each of the children</param>
        /// <param name="validate">A function to process the element and returns TRUE to continue descending, FALSE to stop</param>
        /// <returns>The last child of the initial element to match the specified conditions</returns>
        public virtual IAutomationHelper Descend(Dictionary<AutomationProperty, Func<object>> validators, Func<IAutomationHelper, bool> validate)
        {
            IAutomationHelper el = this;
            while (true)
            {
                AutomationSelector selector;
                object[] vals = new object[validators.Count];
                for (int i = 0; i < vals.Length; i++)
                {
                    var k = validators.ElementAt(i);
                    vals[i] = k.Value();
                }
                selector = new AutomationSelector(validators.Keys.ToArray(), vals);
                IAutomationHelper tmp;
                if (vals.Length > 0)
                {
                    tmp = el.FirstChildWhere(selector);
                }
                else
                {
                    tmp = el.FirstChildWhere(Condition.TrueCondition);
                }
                if (tmp.Element != null)
                {
                    el = tmp;
                }
                else
                {
                    break;
                }
                if (!validate(tmp))
                    break;
            }
            return el;
        }

        /// <summary>
        /// Create a new wrapper for the specified element
        /// </summary>
        /// <param name="element">The element to wrap in a new automation helper</param>
        /// <returns>A new automation helper instance wrapping the specified element</returns>
        protected abstract BaseAutomationHelper _CreateInstance(AutomationElement element);

        /// <summary>
        /// Create a helper that implements the Selectable Item pattern
        /// </summary>
        /// <returns>A helper that implements the Selectable Item pattern</returns>
        public SelectableItemAutomationHelper AsSelectableItem()
        {
            return new SelectableItemAutomationHelper(this.element);
        }

        /// <summary>
        /// Create a helper that implements the ExpandContract pattern
        /// </summary>
        /// <returns>A helper that implements the ExpandContract pattern</returns>
        public ExpandableAutomationHelper AsExpandable()
        {
            return new ExpandableAutomationHelper(this.element);
        }
    }
}