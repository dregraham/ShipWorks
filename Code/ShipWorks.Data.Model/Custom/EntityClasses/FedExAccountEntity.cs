using ShipWorks.Data.Model.Custom;
using ShipWorks.Shipping;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Partial class for builtin FedExAccountEntity
    /// </summary>
    public partial class FedExAccountEntity : ICarrierAccount
    {
        /// <summary>
        /// Indicates if this account was migrated from ShipWorks2, and has not yet been updated for the new API
        /// </summary>
        public bool Is2xMigrationPending => string.IsNullOrEmpty(MeterNumber);

        /// <summary>
        /// Gets the account id in a generic way
        /// </summary>
        public long AccountId => FedExAccountID;

        /// <summary>
        /// Get the shipment type to which this account applies
        /// </summary>
        public ShipmentTypeCode ShipmentType => ShipmentTypeCode.FedEx;
    }
}
