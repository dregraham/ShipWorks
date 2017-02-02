using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Provides functionality to determine residential address information for a shipment
    /// </summary>
    public interface IResidentialDeterminationService
    {
        /// <summary>
        /// Uses the address and shipment configuration to determine what the residential status flag should be set to.
        /// </summary>
        bool IsResidentialAddress(ShipmentEntity shipment);
    }
}
