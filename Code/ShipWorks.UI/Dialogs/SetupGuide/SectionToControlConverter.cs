using System;
using System.Globalization;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Data;

namespace ShipWorks.UI.Dialogs.SetupGuide
{
    /// <summary>
    /// Convert a selected section to a specific control
    /// </summary>
    [Obfuscation(Exclude = true, StripAfterObfuscation = false, ApplyToMembers = true)]
    public class SectionToControlConverter : IValueConverter
    {
        /// <summary>
        /// Convert from SelectedSection to Control
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SetupGuideSection? section = value as SetupGuideSection?;
            if (!section.HasValue)
            {
                return string.Empty;
            }

            return section.Value == SetupGuideSection.AddShippingAccount ?
                (UserControl) new ShippingOptions() :
                new MainOptions();
        }

        /// <summary>
        /// Don't support converting back
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
