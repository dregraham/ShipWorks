using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Autofac;
using Autofac.Features.OwnedInstances;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Services.Builders;
using ShipWorks.UI.Controls.Design;

namespace ShipWorks.Shipping.UI.ValueConverters
{
    /// <summary>
    /// Converts a ServiceAdapter into a list of available Postal confirmation Types
    /// </summary>
    public class ShipmentAdapterToPackagingTypesConverter : IValueConverter
    {
        private readonly IShipmentPackageTypesBuilderFactory shipmentPackageTypesBuilderFactory;

        /// <summary>
        /// Constructor (should only be used by WPF)
        /// </summary>
        public ShipmentAdapterToPackagingTypesConverter() : this(null)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentAdapterToPackagingTypesConverter(IShipmentPackageTypesBuilderFactory shipmentPackageTypesBuilderFactory)
        {
            this.shipmentPackageTypesBuilderFactory = shipmentPackageTypesBuilderFactory ?? GetShipmentPackageTypesBuilderFactory();
        }

        /// <summary>
        /// Convert a shipment adapter to a list of postal confirmation types
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // only null if in designer
            if (shipmentPackageTypesBuilderFactory == null)
            {
                return Enumerable.Empty<KeyValuePair<string, long>>();
            }

            if (!(value is ICarrierShipmentAdapter shipmentAdapter))
            {
                return Enumerable.Empty<KeyValuePair<string, long>>();
            }

            return shipmentPackageTypesBuilderFactory.Get(shipmentAdapter.ShipmentTypeCode)
                .BuildPackageTypeDictionary(new[] { shipmentAdapter.Shipment });
        }

        /// <summary>
        /// Do not support converting back
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("Does not support converting back");
        }

        /// <summary>
        /// Get a ShipmentPackageTypesBuilder Factory
        /// </summary>
        private IShipmentPackageTypesBuilderFactory GetShipmentPackageTypesBuilderFactory() =>
            DesignModeDetector.IsDesignerHosted() ? null : IoC.UnsafeGlobalLifetimeScope.Resolve<Owned<IShipmentPackageTypesBuilderFactory>>().Value;

    }
}
