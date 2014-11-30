using System;

namespace StUtil.Extensions
{
    /// <summary>
    /// Extensions for IntPtrs
    /// </summary>
    public static class IntPtrExtensions
    {
        #region Methods: Arithmetics

        /// <summary>
        /// Decrements the specified pointer.
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static IntPtr Decrement(this IntPtr pointer, Int32 value)
        {
            return Increment(pointer, -value);
        }

        /// <summary>
        /// Decrements the specified pointer.
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static IntPtr Decrement(this IntPtr pointer, Int64 value)
        {
            return Increment(pointer, -value);
        }

        /// <summary>
        /// Decrements the specified pointer.
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static IntPtr Decrement(this IntPtr pointer, IntPtr value)
        {
            switch (IntPtr.Size)
            {
                case sizeof(Int32):
                    return (new IntPtr(pointer.ToInt32() - value.ToInt32()));

                default:
                    return (new IntPtr(pointer.ToInt64() - value.ToInt64()));
            }
        }

        /// <summary>
        /// Increments the specified pointer.
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static IntPtr Increment(this IntPtr pointer, Int32 value)
        {
            unchecked
            {
                switch (IntPtr.Size)
                {
                    case sizeof(Int32):
                        return (new IntPtr(pointer.ToInt32() + value));

                    default:
                        return (new IntPtr(pointer.ToInt64() + value));
                }
            }
        }

        /// <summary>
        /// Increments the specified pointer.
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static IntPtr Increment(this IntPtr pointer, Int64 value)
        {
            unchecked
            {
                switch (IntPtr.Size)
                {
                    case sizeof(Int32):
                        return (new IntPtr((Int32)(pointer.ToInt32() + value)));

                    default:
                        return (new IntPtr(pointer.ToInt64() + value));
                }
            }
        }

        /// <summary>
        /// Increments the specified pointer.
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static IntPtr Increment(this IntPtr pointer, IntPtr value)
        {
            unchecked
            {
                switch (IntPtr.Size)
                {
                    case sizeof(int):
                        return new IntPtr(pointer.ToInt32() + value.ToInt32());

                    default:
                        return new IntPtr(pointer.ToInt64() + value.ToInt64());
                }
            }
        }

        #endregion Methods: Arithmetics

        #region Methods: Comparison

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static Int32 CompareTo(this IntPtr left, Int32 right)
        {
            return left.CompareTo((UInt32)right);
        }

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static Int32 CompareTo(this IntPtr left, IntPtr right)
        {
            if (left.ToUInt64() > right.ToUInt64())
                return 1;

            if (left.ToUInt64() < right.ToUInt64())
                return -1;

            return 0;
        }

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static Int32 CompareTo(this IntPtr left, UInt32 right)
        {
            if (left.ToUInt64() > right)
                return 1;

            if (left.ToUInt64() < right)
                return -1;

            return 0;
        }

        #endregion Methods: Comparison

        #region Methods: Conversion

        /// <summary>
        /// Converts to a UInt32
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <returns></returns>
        public static UInt32 ToUInt32(this IntPtr pointer)
        {
            return (UInt32)pointer.ToInt32();
        }

        /// <summary>
        /// Converts to a UInt64
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <returns></returns>
        public static UInt64 ToUInt64(this IntPtr pointer)
        {
            return (UInt64)pointer.ToInt32();
        }

        #endregion Methods: Conversion

        #region Methods: Equality

        /// <summary>
        /// Checks if the two values are equal
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Boolean Equals(this IntPtr pointer, Int32 value)
        {
            return (pointer.ToInt32() == value);
        }

        /// <summary>
        /// Checks if the two values are equal
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Boolean Equals(this IntPtr pointer, Int64 value)
        {
            return (pointer.ToInt64() == value);
        }

        /// <summary>
        /// Checks if the two values are equal
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Boolean Equals(this IntPtr left, IntPtr ptr2)
        {
            return (left == ptr2);
        }

        /// <summary>
        /// Checks if the two values are equal
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Boolean Equals(this IntPtr pointer, UInt32 value)
        {
            return (pointer.ToUInt32() == value);
        }

        /// <summary>
        /// Checks if the two values are equal
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Boolean Equals(this IntPtr pointer, UInt64 value)
        {
            return (pointer.ToUInt64() == value);
        }

        /// <summary>
        /// Checks if the value is greater than or equal to the other value.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static Boolean GreaterThanOrEqualTo(this IntPtr left, IntPtr right)
        {
            return (left.CompareTo(right) >= 0);
        }

        /// <summary>
        /// Checks if the value is less than or equal to the other value.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static Boolean LessThanOrEqualTo(this IntPtr left, IntPtr right)
        {
            return (left.CompareTo(right) <= 0);
        }

        #endregion Methods: Equality

        #region Methods: Logic

        /// <summary>
        /// Bitwise ANDs the specified pointer.
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static IntPtr And(this IntPtr pointer, IntPtr value)
        {
            switch (IntPtr.Size)
            {
                case sizeof(Int32):
                    return (new IntPtr(pointer.ToInt32() & value.ToInt32()));

                default:
                    return (new IntPtr(pointer.ToInt64() & value.ToInt64()));
            }
        }

        /// <summary>
        /// Bitwise NOTs the specified pointer.
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static IntPtr Not(this IntPtr pointer)
        {
            switch (IntPtr.Size)
            {
                case sizeof(Int32):
                    return (new IntPtr(~pointer.ToInt32()));

                default:
                    return (new IntPtr(~pointer.ToInt64()));
            }
        }

        /// <summary>
        /// Bitwise ORs the specified pointer.
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static IntPtr Or(this IntPtr pointer, IntPtr value)
        {
            switch (IntPtr.Size)
            {
                case sizeof(Int32):
                    return (new IntPtr(pointer.ToInt32() | value.ToInt32()));

                default:
                    return (new IntPtr(pointer.ToInt64() | value.ToInt64()));
            }
        }

        /// <summary>
        /// Bitwise XORs the specified pointer.
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static IntPtr Xor(this IntPtr pointer, IntPtr value)
        {
            switch (IntPtr.Size)
            {
                case sizeof(Int32):
                    return (new IntPtr(pointer.ToInt32() ^ value.ToInt32()));

                default:
                    return (new IntPtr(pointer.ToInt64() ^ value.ToInt64()));
            }
        }

        #endregion Methods: Logic
    }
}