using Interapptive.Shared.Collections;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace ShipWorks.Shipping.UI.Views.ShippingPanel.ValueConverters
{
    public class OriginAddressListConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            List<KeyValuePair<string, long>> addresses = values.OfType<IEnumerable<KeyValuePair<string, long>>>().SelectMany(x => x).ToList();
            long selectedAddress = values.OfType<long>().FirstOrDefault();

            if (addresses.None(x => x.Value == selectedAddress))
            {
                addresses.Add(new KeyValuePair<string, long>("(deleted)", selectedAddress));
            }

            return addresses;

            //return Enumerable.Empty<KeyValuePair<string, long>>();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
