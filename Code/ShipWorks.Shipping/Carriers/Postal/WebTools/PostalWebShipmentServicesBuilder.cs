using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Postal.WebTools
{
    /// <summary>
    /// Service type builder for Express1 Usps shipments
    /// </summary>
    public class PostalWebShipmentServicesBuilder : PostalShipmentServicesBuilder
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PostalWebShipmentServicesBuilder(PostalWebShipmentType shipmentType, IExcludedServiceTypeRepository excludedServiceTypeRepository) :
            base(shipmentType, excludedServiceTypeRepository)
        {

        }
    }
}