using ShipWorks.Data.Model.Custom;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Extra implementation of the UspsAccountEntity
    /// </summary>
    public partial class UspsAccountEntity : ICarrierAccount
    {
        /// <summary>
        /// Gets the account id in a generic way
        /// </summary>
        public long AccountId => UspsAccountID;

        /// <summary>
        /// Get the shipment type to which this account applies
        /// </summary>
        /// <remarks>Assume Usps unless explicitly marked as Express1</remarks>
        public int ShipmentType => UspsReseller == 1 ? 13 : 15;
    }
}
