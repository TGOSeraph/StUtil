namespace StUtil.Extensions
{
    /// <summary>
    /// Extension methods for the char datatype
    /// </summary>
    public static class CharExtensions
    {
        /// <summary>
        /// Determines whether the specified character is hexadecimal.
        /// </summary>
        /// <param name="c">The character</param>
        /// <returns></returns>
        public static bool IsHexadecimal(this char c)
        {
            return (c >= '0' && c <= '9') ||
                     (c >= 'a' && c <= 'f') ||
                     (c >= 'A' && c <= 'F');
        }
    }
}