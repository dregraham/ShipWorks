using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Postal.WebTools
{
    /// <summary>
    /// Package type builder for postal with out postage shipments
    /// </summary>
    public class PostalWebToolsShipmentPackageTypesBuilder : PostalShipmentPackageTypesBuilder
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PostalWebToolsShipmentPackageTypesBuilder(PostalWebShipmentType shipmentType, IExcludedPackageTypeRepository excludedPackageTypeRepository) :
            base(shipmentType, excludedPackageTypeRepository)
        {

        }
    }
}