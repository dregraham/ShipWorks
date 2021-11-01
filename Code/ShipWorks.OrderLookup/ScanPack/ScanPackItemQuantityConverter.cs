using System;
using System.Globalization;
using System.Windows.Data;

namespace ShipWorks.OrderLookup.ScanPack
{
    public class ScanPackItemQuantityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ScanPackItem scanPackItem))
            {
                throw new InvalidOperationException("ScanPackItemQuantityConverter value was not a ScanPackItem");
            }

            if (scanPackItem.IsBundle && !scanPackItem.IsBundleComplete)
            {
                return $"(Incomplete) x{scanPackItem.Quantity}";
            }

            return $"x{scanPackItem.Quantity}";

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}