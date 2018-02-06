namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Convert a boolean to an integer
    /// </summary>
    public class BooleanToIntConverter : BooleanConverter<int>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="trueValue">Integer that represents true</param>
        /// <param name="falseValue">Integer that represents false</param>
        public BooleanToIntConverter() : base(1, 0)
        {
        }
    }
}
