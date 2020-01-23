using System;
using System.Globalization;
using System.Windows.Data;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.UI.ValueConverters
{
    /// <summary>
    /// Converter for getting an image from a shipment type code
    /// </summary>
    public class ShipmentTypeToImageConverter : IValueConverter
    {
        /// <summary>
        /// Converts a shipment type to an image
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ShipmentTypeCode shipmentTypeCode))
            {
                return null;
            }

            return EnumHelper.GetWpfImageSource(shipmentTypeCode);
        }

        /// <summary>
        /// Convert back not implemented
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
