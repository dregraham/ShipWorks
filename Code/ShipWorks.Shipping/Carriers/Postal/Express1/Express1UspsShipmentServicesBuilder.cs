using ShipWorks.Shipping.Carriers.Postal.Usps.Express1;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Service type builder for Express1 Usps shipments
    /// </summary>
    public class Express1UspsShipmentServicesBuilder : PostalShipmentServicesBuilder
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Express1UspsShipmentServicesBuilder(Express1UspsShipmentType shipmentType, IExcludedServiceTypeRepository excludedServiceTypeRepository) :
            base(shipmentType, excludedServiceTypeRepository)
        {

        }
    }
}