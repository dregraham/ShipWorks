using System.Windows;

namespace ShipWorks.Shipping.UI.ValueConverters
{
    /// <summary>
    /// Convert a boolean to visibility
    /// </summary>
    /// <remarks>We're using this instead of the built in class because this will let us invert the result</remarks>
    public sealed class BooleanToVisibilityConverter : BooleanConverter<Visibility>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BooleanToVisibilityConverter() :
            base(Visibility.Visible, Visibility.Collapsed)
        { }
    }
}
