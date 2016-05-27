using ShipWorks.Shipping.Carriers.Postal.Usps.Express1;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Package type builder for Express1 USPS shipments
    /// </summary>
    public class Express1UspsShipmentPackageTypesBuilder : PostalShipmentPackageTypesBuilder
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Express1UspsShipmentPackageTypesBuilder(Express1UspsShipmentType shipmentType, IExcludedPackageTypeRepository excludedPackageTypeRepository) :
            base(shipmentType, excludedPackageTypeRepository)
        {

        }
    }
}