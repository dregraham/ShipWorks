using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Interapptive.Shared.Utility;
using ShipWorks.OrderLookup.FieldManager;

namespace ShipWorks.UI.ValueConverters
{
    /// <summary>
    /// Return Visibility based on field being allowed
    /// </summary>
    public class VisibilityWhenFieldAllowedConverter : IMultiValueConverter
    {
        /// <summary>
        /// Return Visibility based on field visibility
        /// </summary>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            IOrderLookupFieldLayoutRepository repo = values[0] as IOrderLookupFieldLayoutRepository;
            SectionLayoutFieldIDs? fieldID = EnumHelper.TryParseEnum<SectionLayoutFieldIDs>(values[1].ToString(), true);
            Visibility visibility = Visibility.Visible;

            if (repo != null && fieldID.HasValue)
            {
                var sectionFields = repo.Fetch()?.SelectMany(l => l.SectionFields);
                var sectionField = sectionFields?.FirstOrDefault(l => l.Id.Equals(fieldID.Value));

                visibility = sectionField?.Selected == true ? visibility : Visibility.Collapsed;
            }

            return visibility;
        }

        /// <summary>
        /// Convert back
        /// </summary>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
