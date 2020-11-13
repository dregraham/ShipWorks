using System.Windows;

namespace ShipWorks.Installer.ValueConverters
{
    /// <summary>
    /// Class to inversely convert boolean to visibility
    /// </summary>
    public class InverseBooleanToVisibilityConverter : BooleanConverter<Visibility>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public InverseBooleanToVisibilityConverter() :
            base(Visibility.Collapsed, Visibility.Visible)
        { }
    }
}
