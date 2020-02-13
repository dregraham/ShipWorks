using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Wrap the static ShipmentTypeDataService
    /// </summary>
    [Component]
    public class ShipmentTypeDataServiceWrapper : IShipmentTypeDataService
    {
        /// <summary>
        /// Load insurance data into the parent entity, or create if it doesn't exist.  If its already loaded and present
        /// it can be optionally refreshed.
        /// </summary>
        public void LoadInsuranceData(ShipmentEntity shipment) =>
            ShipmentTypeDataService.LoadInsuranceData(shipment);
    }
}
