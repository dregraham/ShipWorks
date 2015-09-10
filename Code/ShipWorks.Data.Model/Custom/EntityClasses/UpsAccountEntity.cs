using ShipWorks.Data.Model.Custom;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Extra implementation of the UpsAccountEntity
    /// </summary>
    public partial class UpsAccountEntity : ICarrierAccount
    {
        /// <summary>
        /// Gets the account id in a generic way
        /// </summary>
        public long AccountId => UpsAccountID;

        /// <summary>
        /// Get the shipment type to which this account applies
        /// </summary>
        public int ShipmentType => 0;
    }
}
