using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace StUtil.Extensions
{
    /// <summary>
    /// Extensions for Enumerable objects
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// holds specific comparers for each enum type, to make comparison faster
        /// </summary>
        private readonly static Dictionary<TypeCode, Func<Enum, Enum, bool>> Comparers =
           new Dictionary<TypeCode, Func<Enum, Enum, bool>>()
           {
              {TypeCode.Byte,   (e1, e2) => (Convert.ToByte(e1)   & Convert.ToByte(e2))   != 0},
              {TypeCode.SByte,  (e1, e2) => (Convert.ToSByte(e1)  & Convert.ToSByte(e2))  != 0},
              {TypeCode.Int16,  (e1, e2) => (Convert.ToInt16(e1)  & Convert.ToInt16(e2))  != 0},
              {TypeCode.Int32,  (e1, e2) => (Convert.ToInt32(e1)  & Convert.ToInt32(e2))  != 0},
              {TypeCode.Int64,  (e1, e2) => (Convert.ToInt64(e1)  & Convert.ToInt64(e2))  != 0},
              {TypeCode.UInt16, (e1, e2) => (Convert.ToUInt16(e1) & Convert.ToUInt16(e2)) != 0},
              {TypeCode.UInt32, (e1, e2) => (Convert.ToUInt32(e1) & Convert.ToUInt32(e2)) != 0},
              {TypeCode.UInt64, (e1, e2) => (Convert.ToUInt64(e1) & Convert.ToUInt64(e2)) != 0},
        };

        /// <summary>
        /// check if <see cref="@this" /> contains all of enums in flags
        /// </summary>
        /// <param name="this">enum to check</param>
        /// <param name="flags">list of flags to look for</param>
        /// <returns>true if all of the flags contained in @this; otherwise, false</returns>
        public static bool All(this Enum @this, params Enum[] flags)
        {
            return flags.All(@enum => Contains(@this, @enum));
        }

        /// <summary> check if <see cref="@this"/> contains any of enums in flags <remarks>instead
        /// of using:<code>(enum1 & enum2) != 0</code></remarks> </summary> <param name="this">enum
        /// to check</param> <param name="flags">list of flags to look for</param> <returns>true if
        /// at least one of the flags contained in @this; otherwise, false</returns>
        public static bool Any(this Enum @this, params Enum[] flags)
        {
            return flags.Any(@enum => Contains(@this, @enum));
        }

        /// <summary>
        /// Get the base-type of <see cref="@this" /> as in Enum declaration
        /// </summary>
        /// <param name="this">enum to check</param>
        /// <returns>the underlying type of th enum</returns>
        public static Type BaseType(this Enum @this)
        {
            Type res = null;
            switch (@this.GetTypeCode())
            {
                case TypeCode.SByte:
                    res = typeof(sbyte);
                    break;

                case TypeCode.Byte:
                    res = typeof(byte);
                    break;

                case TypeCode.Int16:
                    res = typeof(Int16);
                    break;

                case TypeCode.UInt16:
                    res = typeof(UInt16);
                    break;

                case TypeCode.Int32:
                    res = typeof(Int32);
                    break;

                case TypeCode.UInt32:
                    res = typeof(UInt32);
                    break;

                case TypeCode.Int64:
                    res = typeof(Int16);
                    break;

                case TypeCode.UInt64:
                    res = typeof(UInt16);
                    break;

                default:
                    throw new ArgumentOutOfRangeException("this", @this, "This cannot accor");
            }
            return res;
        }

        /// <summary>
        /// Get an attribute from an enum
        /// </summary>
        /// <typeparam name="T">The type of the object to get the attribute from</typeparam>
        /// <typeparam name="U">The type of attribute to get</typeparam>
        /// <param name="source">The object to get the attribute from</param>
        /// <returns>The attribute of the specified type on the source object</returns>
        public static U GetAttribute<T, U>(this T source)
            where U : Attribute
            where T : struct, IConvertible
        {
            return GetAttributes<T, U>(source).FirstOrDefault();
        }

        /// <summary>
        /// Get an array attributes from an enum
        /// </summary>
        /// <typeparam name="T">The type of the object to get the attributes from</typeparam>
        /// <typeparam name="U">The type of attributes to get</typeparam>
        /// <param name="source">The object to get the attributes from</param>
        /// <returns>An array of attributes matching the specified type on the object</returns>
        public static U[] GetAttributes<T, U>(this T source)
            where U : Attribute
            where T : struct, IConvertible
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());

            U[] attributes = (U[])fi.GetCustomAttributes(typeof(U), false);

            if (attributes != null && attributes.Length > 0)
            {
                return attributes;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the Description attribute from an enum
        /// </summary>
        /// <typeparam name="T">The type of object to get the description of</typeparam>
        /// <param name="source">The object to get the description of</param>
        /// <returns>The description of the object</returns>
        public static string GetDescription<T>(this T source)
            where T : struct, IConvertible
        {
            return GetAttribute<T, DescriptionAttribute>(source).Description;
        }

        /// <summary>
        /// Gets the next enum in an enumeration
        /// </summary>
        /// <typeparam name="T">The type of enumeration</typeparam>
        /// <param name="value">The value to get the following element from</param>
        /// <returns>The value int the enum following the specified value</returns>
        public static T? GetNext<T>(this T value)
            where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }
            bool next = false;
            foreach (T item in Enum.GetValues(typeof(T)))
            {
                if (next)
                {
                    return item;
                }
                if (item.Equals(value))
                {
                    next = true;
                }
            }
            return null;
        }

        /// <summary>
        /// Checks if <see cref="@this" /> is the same as <see cref="other" />
        /// </summary>
        /// <param name="this">enum to check</param>
        /// <param name="other">another enum to check</param>
        /// <returns>true, if @this and other are of the same type</returns>
        public static bool IsSameTypeAs(this Enum @this, Enum other)
        {
            return @this.GetType() == other.GetType();
        }

        /// <summary>
        /// Yield all defined flags that is contained both in <see cref="@this" /> and in <see
        /// cref="other" />
        /// </summary>
        /// <param name="this">enum to iterate</param>
        /// <param name="other">enum to join</param>
        /// <returns>enumerator that iterates each flag @this and also other contains</returns>
        public static IEnumerable<Enum> Join(this Enum @this, Enum other)
        {
            Func<Enum, Enum, bool> comparer = Comparers[@this.GetTypeCode()];
            return @this.ToFlags().Where(flag => comparer(other, flag));
        }

        /// <summary>
        /// Yield all defined flags that is contained in <see cref="@this" /> and not in <see
        /// cref="other" />
        /// </summary>
        /// <param name="this">enum to include</param>
        /// <param name="other">enum to exclude</param>
        /// <returns>
        /// enumarator that itterates each flag @this contains, but not containd in other
        /// </returns>
        public static IEnumerable<Enum> LeftOuterJoin(this Enum @this, Enum other)
        {
            Func<Enum, Enum, bool> comparer = Comparers[@this.GetTypeCode()];
            return @this.ToFlags().Where(flag => !comparer(other, flag));
        }

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more
        /// enumerated constants to an equivalent enumerated object. A parameter specifies whether
        /// the operation is case-sensitive.
        /// </summary>
        /// <typeparam name="T">An enumeration type</typeparam>
        /// <param name="value">A string containing the name or value to convert</param>
        /// <param name="ignoreCase">true to ignore case; false to regard case</param>
        /// <returns>An object of type enumType whose value is represented by value</returns>
        public static T Parse<T>(this string value, bool ignoreCase = false) where T : struct
        {
            Type enumType = typeof(T);
            if (!enumType.IsEnum)
            {
                throw new ArgumentException(string.Format("T ({0}) is not an Enum", enumType.Name), "T");
            }
            return (T)Enum.Parse(enumType, value, ignoreCase);
        }

        /// <summary>
        /// Yield all defined flags that is contained in <see cref="other" /> and not in <see
        /// cref="@this" />
        /// </summary>
        /// <param name="this">enum to exclude</param>
        /// <param name="other">enum to include</param>
        /// <returns>
        /// enumarator that itterates each flag other contains, but not containd in @this
        /// </returns>
        public static IEnumerable<Enum> RightOuterJoin(this Enum @this, Enum other)
        {
            return other.LeftOuterJoin(@this);
        }

        /// <summary>
        /// Yield all defined flags that is contained in <see cref="@this" />
        /// </summary>
        /// <param name="this">enum to itrate</param>
        /// <returns>enumerator that iterates each flag @this contains</returns>
        public static IEnumerable<Enum> ToFlags(this Enum @this)
        {
            Func<Enum, Enum, bool> comparer = Comparers[@this.GetTypeCode()];

            foreach (Enum flag in Enum.GetValues(@this.GetType()))
            {
                if (comparer(flag, @this))
                {
                    yield return flag;
                }
            }
        }

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more
        /// enumerated constants to an equivalent enumerated object. A parameter specifies whether
        /// the operation is case-sensitive. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="T">The enumeration type to which to convert value</typeparam>
        /// <param name="value">
        /// The string representation of the enumeration name or underlying value to convert
        /// </param>
        /// <param name="result">
        /// When this method returns, contains an object of type T whose value is represented by
        /// value. This parameter is passed uninitialized
        /// </param>
        /// <param name="default">Default value to save in result, if parse is failed</param>
        /// <param name="ignoreCase">true to ignore case; false to consider case</param>
        /// <returns>true if the value parameter was converted successfully; otherwise, false</returns>
        public static bool TryParse<T>(this string value, out T result, T @default = default(T), bool ignoreCase = false) where T : struct
        {
            bool res;
            try
            {
                result = value.Parse<T>(ignoreCase);
                res = true;
            }
            catch (Exception)
            {
                result = @default;
                res = false;
            }
            return res;
        }

        /// <summary>
        /// check if <see cref="@this" /> contains the <see cref="flag" />
        /// </summary>
        /// <param name="this">enum to check</param>
        /// <param name="flag">flag to look for</param>
        /// <returns>true if @this contains flag</returns>
        private static bool Contains(Enum @this, Enum flag)
        {
            return Comparers[@this.GetTypeCode()](@this, flag);
        }
    }
}