namespace ShipWorks.Installer.ValueConverters
{
    /// <summary>
    /// Convert a boolean to a string
    /// </summary>
    public class BooleanToStringConverter : BooleanConverter<string>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BooleanToStringConverter() : base("Yes", "No")
        {
        }
    }
}
