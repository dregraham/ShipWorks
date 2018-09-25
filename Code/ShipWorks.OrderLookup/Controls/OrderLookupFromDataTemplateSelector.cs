using System.Windows;
using System.Windows.Controls;
using ShipWorks.Shipping;

namespace ShipWorks.OrderLookup.Controls
{
    /// <summary>
    /// Data template selector based on shipment type code
    /// </summary>
    public class OrderLookupFromDataTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// select the datatemplate based on shipment type code
        /// </summary>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;

            if (element != null && item != null && item is OrderLookupFromViewModel)
            {
                OrderLookupFromViewModel viewModel = item as OrderLookupFromViewModel;

                switch (viewModel.MessageBus?.ShipmentAdapter?.ShipmentTypeCode)
                {
               

                }
            }

            return null;
        }
    }
}
