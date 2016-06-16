using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Interapptive.Shared.Collections;

namespace ShipWorks.Shipping.UI.ShippingPanel.ValueConverters
{
    /// <summary>
    /// Merge a list of addressess with a selected address
    /// </summary>
    public class OriginAddressListConverter : IMultiValueConverter
    {
        /// <summary>
        /// Perform the conversions
        /// </summary>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            List<object> objectValues = (values ?? Enumerable.Empty<object>()).ToList();

            List<KeyValuePair<string, long>> addresses = objectValues
                .OfType<IEnumerable<KeyValuePair<string, long>>>()
                .SelectMany(x => x)
                .ToList();
            long? selectedAddress = objectValues.OfType<long?>().FirstOrDefault();

            if (selectedAddress.HasValue && addresses.None(x => x.Value == selectedAddress))
            {
                addresses.Add(new KeyValuePair<string, long>("(deleted)", selectedAddress.Value));
            }

            return addresses;
        }

        /// <summary>
        /// Don't support converting
        /// </summary>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
