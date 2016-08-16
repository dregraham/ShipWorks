using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// Package type builder for Endicia shipments
    /// </summary>
    public class EndiciaShipmentPackageTypesBuilder : PostalShipmentPackageTypesBuilder
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EndiciaShipmentPackageTypesBuilder(EndiciaShipmentType shipmentType, IExcludedPackageTypeRepository excludedPackageTypeRepository) :
            base(shipmentType, excludedPackageTypeRepository)
        {

        }
    }
}