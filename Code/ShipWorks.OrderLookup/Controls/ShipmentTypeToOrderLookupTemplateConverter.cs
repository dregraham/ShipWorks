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
    
    public class ShipmentTypeToOrderLookupTemplateConverter : IMultiValueConverter
    {
        /// <summary>
        /// Looks for the template matching the name expected for the shipmentypecode. 
        /// </summary>
        /// <remarks>
        /// If not found or unknown shipmenttypecode or no shipmenttypecode, "default" template is returned.
        /// If not found, null is returned.
        /// </remarks>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            FrameworkElement element = values[1] as FrameworkElement;
            if (element is null)
            {
                throw new InvalidOperationException("Invalid FrameworkElement in OrderLookupShipmentTypeToOrderLookupShipmentDetailsConverter");
            }

            string templateToUse = "";
            if (values[0] is ShipmentTypeCode shipmentTypeCode)
            {
                templateToUse = Enum.GetName(typeof(ShipmentTypeCode), shipmentTypeCode).ToLower();
            }

            object template = null;
            if (!string.IsNullOrEmpty(templateToUse))
            {
                template = element.TryFindResource(templateToUse);
            }
            if (template == null)
            {
                template = element.TryFindResource("default");
            }

            return template as DataTemplate;
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
