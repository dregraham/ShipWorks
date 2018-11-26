namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Convert a boolean to a string
    /// </summary>
    public class BooleanToStringConverter : BooleanConverter<string>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="trueValue">String that represents true</param>
        /// <param name="falseValue">String that represents false</param>
        public BooleanToStringConverter() : base("Yes", "No")
        {
        }
    }
}
