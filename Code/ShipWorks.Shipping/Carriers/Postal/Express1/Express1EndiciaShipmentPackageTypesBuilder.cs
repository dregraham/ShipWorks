using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Package type builder for Express1 Endicia shipments
    /// </summary>
    public class Express1EndiciaShipmentPackageTypesBuilder : PostalShipmentPackageTypesBuilder
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Express1EndiciaShipmentPackageTypesBuilder(Express1EndiciaShipmentType shipmentType, IExcludedPackageTypeRepository excludedPackageTypeRepository) :
            base(shipmentType, excludedPackageTypeRepository)
        {

        }
    }
}