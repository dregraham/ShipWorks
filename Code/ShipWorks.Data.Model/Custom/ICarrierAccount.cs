using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;

namespace ShipWorks.Data.Model.Custom
{
    /// <summary>
    /// Allow carrier accounts to be used interchangably
    /// </summary>
    public interface ICarrierAccount : IEntity2
    {
        /// <summary>
        /// Get the id of the account
        /// </summary>
        long AccountId { get; }

        /// <summary>
        /// Get a description of the account
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Get the shipment type to which this account applies
        /// </summary>
        ShipmentTypeCode ShipmentType { get; }

        /// <summary>
        /// Apply this account to the shipment.
        /// </summary>
        void ApplyTo(ShipmentEntity shipment);
    }
}
