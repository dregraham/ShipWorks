using System.Windows;

namespace ShipWorks.Installer.ValueConverters
{
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
