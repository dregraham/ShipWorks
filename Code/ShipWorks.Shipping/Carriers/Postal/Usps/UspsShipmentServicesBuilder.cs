using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Service type builder for Usps shipments
    /// </summary>
    public class UspsShipmentServicesBuilder : PostalShipmentServicesBuilder
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UspsShipmentServicesBuilder(UspsShipmentType shipmentType, IExcludedServiceTypeRepository excludedServiceTypeRepository) :
            base(shipmentType, excludedServiceTypeRepository)
        {

        }
    }
}