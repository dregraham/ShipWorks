using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace ShipWorks.UI.Dialogs.NewUserExperience
{
    /// <summary>
    /// Convert a selected section to a specific control
    /// </summary>
    public class SectionToControlConverter : IValueConverter
    {
        /// <summary>
        /// Convert from SelectedSection to Control
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            NewUserExperienceSection? section = value as NewUserExperienceSection?;
            if (!section.HasValue)
            {
                return string.Empty;
            }

            return section.Value == NewUserExperienceSection.AddShippingAccount ?
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
