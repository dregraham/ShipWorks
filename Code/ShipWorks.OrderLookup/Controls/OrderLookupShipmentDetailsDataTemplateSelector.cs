using System.Windows;
using System.Windows.Controls;
using ShipWorks.Shipping;

namespace ShipWorks.OrderLookup.Controls
{
    /// <summary>
    /// Data template selector based on shipment type code
    /// </summary>
    public class OrderLookupShipmentDetailsDataTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// select the datatemplate based on shipment type code
        /// </summary>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;

            if (element != null && item != null && item is OrderLookupShipmentDetailsViewModel viewModel)
            {
                switch (viewModel.MessageBus?.ShipmentAdapter?.ShipmentTypeCode)
                {
                    case ShipmentTypeCode.Usps:
                        return element.FindResource("usps") as DataTemplate;
                    default:
                        return element.FindResource("default") as DataTemplate;
                }
            }

            return null;
        }
    }
}
