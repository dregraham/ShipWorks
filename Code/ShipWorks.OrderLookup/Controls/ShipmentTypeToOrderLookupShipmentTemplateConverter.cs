using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ShipWorks.Shipping;

namespace ShipWorks.OrderLookup.Controls
{
    /// <summary>
    /// Data template selector based on shipment type code
    /// </summary>
    public class ShipmentTypeToOrderLookupShipmentTemplateConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            FrameworkElement element = values[1] as FrameworkElement;
            if (element is null)
            {
                throw new InvalidOperationException("Invalid FrameworkElement in OrderLookupShipmentTypeToOrderLookupShipmentDetailsConverter");
            }
            
            if (values[0] is ShipmentTypeCode shipmentTypeCode )
            {
                switch (shipmentTypeCode)
                {
                    case ShipmentTypeCode.Usps:
                        return element.FindResource("usps") as DataTemplate;                        
                }
            }

            return element.FindResource("default") as DataTemplate;

            return null;
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
