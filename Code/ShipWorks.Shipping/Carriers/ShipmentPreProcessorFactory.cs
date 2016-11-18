using Autofac.Features.Indexed;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.ComponentRegistration;

namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// Create a shipment preprocessor factory
    /// </summary>
    [Component]
    public class ShipmentPreProcessorFactory : Factory<ShipmentTypeCode, IShipmentPreProcessor>, IShipmentPreProcessorFactory
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentPreProcessorFactory(IIndex<ShipmentTypeCode, IShipmentPreProcessor> lookup) : base(lookup)
        {
        }
    }
}
