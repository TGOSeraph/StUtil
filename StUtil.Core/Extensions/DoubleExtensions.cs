namespace StUtil.Extensions
{
    /// <summary>
    /// Extensions for Doubles
    /// </summary>
    public static class DoubleExtensions
    {
        /// <summary>
        /// Clamps a value in the allowed range of a byte
        /// </summary>
        /// <param name="value">The value to clamp to byte range</param>
        /// <returns>
        /// A value between 0 and 255, if value gt 255 then 255, or if value lt 0, else value
        /// </returns>
        public static byte ClampByte(this double value)
        {
            if (value > 255)
                return 255;
            else if (value < 0)
                return 0;
            else
                return (byte)value;
        }
    }
}