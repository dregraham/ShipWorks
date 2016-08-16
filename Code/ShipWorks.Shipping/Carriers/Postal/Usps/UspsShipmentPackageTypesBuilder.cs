using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Package type builder for USPS (Stamps) shipments
    /// </summary>
    public class UspsShipmentPackageTypesBuilder : PostalShipmentPackageTypesBuilder
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UspsShipmentPackageTypesBuilder(UspsShipmentType shipmentType, IExcludedPackageTypeRepository excludedPackageTypeRepository) :
            base(shipmentType, excludedPackageTypeRepository)
        {

        }
    }
}