using ShipWorks.Data.Model.Custom;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Extra implementation of the OnTracAccountEntity
    /// </summary>
    public partial class OnTracAccountEntity : ICarrierAccount
    {
        /// <summary>
        /// Gets the account id in a generic way
        /// </summary>
        public long AccountId => OnTracAccountID;

        /// <summary>
        /// Get the shipment type to which this account applies
        /// </summary>
        public int ShipmentType => 11;
    }
}
