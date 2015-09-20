using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// Service type builder for Endicia shipments
    /// </summary>
    public class EndiciaShipmentServicesBuilder : PostalShipmentServicesBuilder
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EndiciaShipmentServicesBuilder(EndiciaShipmentType shipmentType, IExcludedServiceTypeRepository excludedServiceTypeRepository) :
            base(shipmentType, excludedServiceTypeRepository)
        {

        }
    }
}