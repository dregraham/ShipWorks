using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using ShipWorks.OrderLookup.FieldManager;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Convert a field layout id to visibility
    /// </summary>
    public class SectionLayoutVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Convert a field layout id to visibility
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var fieldLayoutProvider = value as IOrderLookupFieldLayoutProvider;
            var fieldID = parameter as SectionLayoutFieldIDs?;

            if (fieldLayoutProvider == null)
            {
                return Visibility.Visible;
            }

            if (!fieldID.HasValue)
            {
                throw new InvalidOperationException("Must use a SectionLayoutFieldIDs value as converter parameter");
            }

            var fieldVisible = fieldLayoutProvider
                .Fetch()
                .SelectMany(x => x.SectionFields)
                .Where(x => x.Id.Equals(fieldID.Value))
                .Any(x => x.Selected);

            return fieldVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Convert back to source
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("We don't support converting back");
        }
    }
}
