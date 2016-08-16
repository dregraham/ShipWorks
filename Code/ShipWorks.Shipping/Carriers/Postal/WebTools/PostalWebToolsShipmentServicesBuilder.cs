using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Postal.WebTools
{
    /// <summary>
    /// Service type builder for Express1 Usps shipments
    /// </summary>
    public class PostalWebToolsShipmentServicesBuilder : PostalShipmentServicesBuilder
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PostalWebToolsShipmentServicesBuilder(PostalWebShipmentType shipmentType, IExcludedServiceTypeRepository excludedServiceTypeRepository) :
            base(shipmentType, excludedServiceTypeRepository)
        {

        }
    }
}