using System.Reflection;

namespace ShipWorks.Installer.ValueConverters
{
    /// <summary>
    /// Convert a boolean value to a double
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class BooleanToDoubleConverter : BooleanConverter<double>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BooleanToDoubleConverter() : base(1, 0)
        {
        }
    }
}
