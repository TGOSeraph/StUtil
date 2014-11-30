using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Generic
{
    /// <summary>
    /// Generic event args with a single field
    /// </summary>
    public class EventArgs<T> : EventArgs
    {
        /// <summary>
        /// The value
        /// </summary>
        private T _value;

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public T Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventArgs{T}" /> class.
        /// </summary>
        public EventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventArgs{T}" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public EventArgs(T value)
        {
            _value = value;
        }
    }

    /// <summary>
    /// Generic event args with two fields
    /// </summary>
    public class EventArgs<T, U> : EventArgs
    {
        /// <summary>
        /// The first value
        /// </summary>
        private T _value1;

        /// <summary>
        /// The second value
        /// </summary>
        private U _value2;

        /// <summary>
        /// Gets or sets the first value.
        /// </summary>
        /// <value>The first value.</value>
        public T Value1
        {
            get { return _value1; }
            set { _value1 = value; }
        }

        /// <summary>
        /// Gets or sets the second value.
        /// </summary>
        /// <value>The second value.</value>
        public U Value2
        {
            get { return _value2; }
            set { _value2 = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventArgs{T, U}" /> class.
        /// </summary>
        public EventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventArgs{T, U}" /> class.
        /// </summary>
        /// <param name="value1">The first value.</param>
        public EventArgs(T value1)
        {
            _value1 = value1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventArgs{T, U}" /> class.
        /// </summary>
        /// <param name="value2">The second value.</param>
        public EventArgs(U value2)
        {
            _value2 = value2;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventArgs{T, U}" /> class.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        public EventArgs(T value1, U value2)
        {
            _value1 = value1;
            _value2 = value2;
        }
    }

    /// <summary>
    /// Generic event args with three fields
    /// </summary>
    public class EventArgs<T, U, V> : EventArgs
    {
        /// <summary>
        /// The first value
        /// </summary>
        private T _value1;

        /// <summary>
        /// The second value
        /// </summary>
        private U _value2;

        /// <summary>
        /// The third value
        /// </summary>
        private V _value3;

        /// <summary>
        /// Gets or sets the first value.
        /// </summary>
        /// <value>The first value.</value>
        public T Value1
        {
            get { return _value1; }
            set { _value1 = value; }
        }

        /// <summary>
        /// Gets or sets the second value.
        /// </summary>
        /// <value>The second value.</value>
        public U Value2
        {
            get { return _value2; }
            set { _value2 = value; }
        }

        /// <summary>
        /// Gets or sets the third value.
        /// </summary>
        /// <value>The third value.</value>
        public V Value3
        {
            get { return _value3; }
            set { _value3 = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventArgs{T, U, V}" /> class.
        /// </summary>
        public EventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventArgs{T, U, V}" /> class.
        /// </summary>
        /// <param name="value1">The first value.</param>
        public EventArgs(T value1)
        {
            _value1 = value1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventArgs{T, U, V}" /> class.
        /// </summary>
        /// <param name="value2">The second value.</param>
        public EventArgs(U value2)
        {
            _value2 = value2;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventArgs{T, U, V}" /> class.
        /// </summary>
        /// <param name="value3">The third value.</param>
        public EventArgs(V value3)
        {
            _value3 = value3;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventArgs{T, U, V}" /> class.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        public EventArgs(T value1, U value2)
        {
            _value1 = value1;
            _value2 = value2;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventArgs{T, U, V}" /> class.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <param name="value3">The third value.</param>
        public EventArgs(T value1, U value2, V value3)
        {
            _value1 = value1;
            _value2 = value2;
            _value3 = value3;
        }
    }
}