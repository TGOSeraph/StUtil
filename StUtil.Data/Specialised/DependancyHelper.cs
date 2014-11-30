using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Data.Specialised
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">The type of the dependancy</typeparam>
    public class DependancyHelper<T>
    {
        /// <summary>
        /// The dependancies for this helper
        /// </summary>
        private Dictionary<T, Dependacy<T>> dependancies = new Dictionary<T, Dependacy<T>>();

        /// <summary>
        /// Creates the dependancy between the two items.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="dependsOn">The depends on.</param>
        public void CreateDependancy(T item, T dependsOn)
        {
            if (!dependancies.ContainsKey(item))
            {
                dependancies.Add(item, new Dependacy<T>(item));
            }
            dependancies[item].DependsOn.Add(dependsOn);
        }
            
        /// <summary>
        /// Gets the dependancies for this helper
        /// </summary>
        /// <value>
        /// The dependancies.
        /// </value>
        public Dictionary<T, Dependacy<T>> Dependancies
        {
            get
            {
                return dependancies;
            }
        }

        /// <summary>
        /// Gets the items that are not completed but have all dependancies satisified.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="completed">The completed.</param>
        /// <returns></returns>
        public IEnumerable<T> GetAvailable(IEnumerable<T> items, IEnumerable<T> completed)
        {
            items = items.Except(completed);
            List<T> available = new List<T>();
            foreach (T item in items)
            {
                if (!dependancies.ContainsKey(item))
                {
                    available.Add(item);
                }
                else if (dependancies[item].DependsOn.All(d => completed.Contains(d)))
                {
                    available.Add(item);
                }
            }
            return available;
        }

        /// <summary>
        /// Gets the items ordered by their dependancies.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">There is no path through the items that satisfies the dependancies;items</exception>
        public IEnumerable<T> GetOrdered(IEnumerable<T> items)
        {
            List<T> done = new List<T>();
            List<T> remaining = items.ToList();

            while (remaining.Count > 0)
            {
                IEnumerable<T> can = GetAvailable(items, done);
                if (can.Count() == 0)
                {
                    throw new ArgumentException("There is no path through the items that satisfies the dependancies", "items");
                }
                done.AddRange(can);
                foreach (T item in can)
                {
                    remaining.Remove(item);
                }
            }

            return done;
        }
    }

}
