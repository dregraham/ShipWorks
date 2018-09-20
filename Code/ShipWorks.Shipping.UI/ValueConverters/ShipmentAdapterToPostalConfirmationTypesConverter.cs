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
using ShipWorks.UI.Controls.Design;

namespace ShipWorks.Shipping.UI.ValueConverters
{
    /// <summary>
    /// Converts a ServiceAdapter into a list of available Postal confirmation Types
    /// </summary>
    public class ShipmentAdapterToPostalConfirmationTypesConverter : IValueConverter
    {
        private readonly IShipmentTypeManager shipmentTypeManager;

        /// <summary>
        /// Constructor (should only be used by WPF)
        /// </summary>
        public ShipmentAdapterToPostalConfirmationTypesConverter() : this(null)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentAdapterToPostalConfirmationTypesConverter(IShipmentTypeManager shipmentTypeManager)
        {
            this.shipmentTypeManager = shipmentTypeManager ?? GetShipmentTypeManager();
        }

        /// <summary>
        /// Convert a shipment adapter to a list of postal confirmation types
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // only null if in designer
            if (shipmentTypeManager == null)
            {
                return Enumerable.Empty<KeyValuePair<string, long>>();
            }

            // Check to see if object is a postal shipment adapter
            if (!(value is ICarrierShipmentAdapter shipmentAdapter && 
                PostalUtility.IsPostalShipmentType(shipmentAdapter.ShipmentTypeCode)))
            {
                return Enumerable.Empty<KeyValuePair<string, long>>();
            }

            PostalShipmentType postalShipmentType = ((PostalShipmentType) shipmentTypeManager.Get(shipmentAdapter.ShipmentTypeCode));
            PostalServiceType postalServiceType = (PostalServiceType) shipmentAdapter.ServiceType;

            // See if all have confirmation as an option or not
            PostalPackagingType packagingType = (PostalPackagingType) shipmentAdapter.Shipment.Postal.PackagingType;
            return postalShipmentType
                .GetAvailableConfirmationTypes(shipmentAdapter.Shipment.ShipCountryCode, postalServiceType, packagingType)
                .ToDictionary(serviceType => (int) serviceType, serviceType => EnumHelper.GetDescription(serviceType));

        }

        /// <summary>
        /// Do not support converting back
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("Does not support converting back");
        }

        /// <summary>
        /// Get a shipment type factory
        /// </summary>
        private IShipmentTypeManager GetShipmentTypeManager() =>
            DesignModeDetector.IsDesignerHosted() ? null : IoC.UnsafeGlobalLifetimeScope.Resolve<Owned<IShipmentTypeManager>>().Value;
    }
}
