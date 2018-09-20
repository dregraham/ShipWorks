using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Autofac;
using Autofac.Features.OwnedInstances;
using ShipWorks.ApplicationCore;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;
using ShipWorks.UI.Controls.Design;

namespace ShipWorks.Shipping.UI.ValueConverters
{
    /// <summary>
    /// Converts a Service Adapter into a list of available Service Types
    /// </summary>
    public class ShipmentAdapterToServiceTypesConverter : IValueConverter
    {
        private readonly IShipmentServicesBuilderFactory shipmentServicesBuilderFactory;

        /// <summary>
        /// Constructor (should only be used by WPF)
        /// </summary>
        public ShipmentAdapterToServiceTypesConverter() : this(null)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentAdapterToServiceTypesConverter(IShipmentServicesBuilderFactory shipmentServicesBuilderFactory)
        {
            this.shipmentServicesBuilderFactory = shipmentServicesBuilderFactory ?? GetShipmentServicesBuilderFactory();
        }

        /// <summary>
        /// Convert a shipment adapter to list of service types
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ICarrierShipmentAdapter shipmentAdapter))
            {
                return Enumerable.Empty<KeyValuePair<string, long>>();
            }

            Dictionary<int, string> updatedServices = new Dictionary<int, string>();

            try
            {
                updatedServices = shipmentServicesBuilderFactory.Get(shipmentAdapter.ShipmentTypeCode)
                    .BuildServiceTypeDictionary(new[] { shipmentAdapter.Shipment });
            }
            catch (InvalidRateGroupShippingException)
            {
                updatedServices.Add(shipmentAdapter.ServiceType, "Error getting service types.");
            }

            // If no service types are returned, the carrier doesn't support service types,
            // so just return.
            if (!updatedServices.Any())
            {
                return new List<KeyValuePair<int, string>>();
            }

            // Get the new list
            return new List<KeyValuePair<int, string>>(updatedServices);
        }

        /// <summary>
        /// Do not support converting back
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("Does not support converting back");
        }


        private IShipmentServicesBuilderFactory GetShipmentServicesBuilderFactory() =>
            DesignModeDetector.IsDesignerHosted() ? null : IoC.UnsafeGlobalLifetimeScope.Resolve<Owned<IShipmentServicesBuilderFactory>>().Value;

    }
}
