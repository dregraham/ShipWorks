using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Represents the ShipmentTypeDataService
    /// </summary>
    public interface IShipmentTypeDataService
    {
        /// <summary>
        /// Load insurance data into the parent entity, or create if it doesn't exist.  If its already loaded and present
        /// it can be optionally refreshed.
        /// </summary>
        void LoadInsuranceData(ShipmentEntity shipment);
    }
}
