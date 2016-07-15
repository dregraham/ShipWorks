namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Helper methods to create EnumResult objects
    /// </summary>
    /// <remarks>This is useful so that you don't have to create EnumResults using the constructor with
    /// type parameters</remarks>
    public static class EnumResultExtensions
    {
        /// <summary>
        /// Create an EnumResult object from the given enum with an empty message
        /// </summary>
        public static EnumResult<T> AsEnumResult<T>(this T value) where T : struct
        {
            return new EnumResult<T>(value);
        }

        /// <summary>
        /// Create an EnumResult object from the given enum with the specified message
        /// </summary>
        public static EnumResult<T> AsEnumResult<T>(this T value, string message) where T : struct
        {
            return new EnumResult<T>(value, message);
        }
    }
}
