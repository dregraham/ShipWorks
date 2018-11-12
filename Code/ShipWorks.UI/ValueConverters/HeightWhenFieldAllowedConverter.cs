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
    /// Converter to return height based on field being allowed
    /// </summary>
    public class HeightWhenFieldAllowedConverter : IMultiValueConverter
    {
        /// <summary>
        /// Convert height based on field visibility
        /// </summary>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            IOrderLookupFieldLayoutProvider repo = values[0] as IOrderLookupFieldLayoutProvider;
            SectionLayoutFieldIDs? fieldID = EnumHelper.TryParseEnum<SectionLayoutFieldIDs>(values[1].ToString(), true);
            GridLength height = (GridLength) values[2];

            if (repo != null && fieldID.HasValue)
            {
                var sectionFields = repo.Fetch()?.SelectMany(l => l.SectionFields);
                var sectionField = sectionFields?.FirstOrDefault(l => l.Id.Equals(fieldID.Value));

                height = sectionField?.Selected == true ? height : new GridLength(0);
            }

            return height;
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
