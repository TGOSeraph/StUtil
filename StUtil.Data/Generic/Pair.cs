namespace StUtil.Data.Generic
{
    /// <summary>
    /// A pairing of two objects
    /// </summary>
    /// <typeparam name="TFirst">The type of the first object</typeparam>
    /// <typeparam name="TSecond">The type of the second object</typeparam>
    public class Pair<TFirst, TSecond>
    {
        /// <summary>
        /// The first object
        /// </summary>
        public TFirst First { get; set; }

        /// <summary>
        /// The second object
        /// </summary>
        public TSecond Second { get; set; }

        /// <summary>
        /// Initializes a new instance of the class
        /// </summary>
        /// <param name="first">The first object to store</param>
        /// <param name="second">The second objec to store</param>
        public Pair(TFirst first, TSecond second)
        {
            this.First = first;
            this.Second = second;
        }
    }
}