using System;

namespace StUtil.Generic
{
    /// <summary>
    /// Generic event args with a single field
    /// </summary>
    /// <remarks>
    /// 2013-06-26  - Initial version
    /// </remarks>
    public class EventArgs<T> : EventArgs
    {
        private T _value;

        public T Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public EventArgs()
        {
        }

        public EventArgs(T value)
        {
            _value = value;
        }
    }

    /// <summary>
    /// Generic event args with two fields
    /// </summary>
    /// <remarks>
    /// 2013-06-26  - Initial version
    /// </remarks>
    public class EventArgs<T, U> : EventArgs
    {
        private T _value1;

        public T Value1
        {
            get { return _value1; }
            set { _value1 = value; }
        }

        private U _value2;

        public U Value2
        {
            get { return _value2; }
            set { _value2 = value; }
        }

        public EventArgs()
        {
        }

        public EventArgs(T value1)
        {
            _value1 = value1;
        }

        public EventArgs(U value2)
        {
            _value2 = value2;
        }

        public EventArgs(T value1, U value2)
        {
            _value1 = value1;
            _value2 = value2;
        }
    }

    /// <summary>
    /// Generic event args with three fields
    /// </summary>
    /// <remarks>
    /// 2013-06-26  - Initial version
    /// </remarks>
    public class EventArgs<T, U, V> : EventArgs
    {
        private T _value1;

        public T Value1
        {
            get { return _value1; }
            set { _value1 = value; }
        }

        private U _value2;

        public U Value2
        {
            get { return _value2; }
            set { _value2 = value; }
        }

        private V _value3;

        public V Value3
        {
            get { return _value3; }
            set { _value3 = value; }
        }

        public EventArgs()
        {
        }

        public EventArgs(T value1)
        {
            _value1 = value1;
        }

        public EventArgs(U value2)
        {
            _value2 = value2;
        }

        public EventArgs(V value3)
        {
            _value3 = value3;
        }

        public EventArgs(T value1, U value2)
        {
            _value1 = value1;
            _value2 = value2;
        }

        public EventArgs(T value1, U value2, V value3)
        {
            _value1 = value1;
            _value2 = value2;
            _value3 = value3;
        }
    }
}
