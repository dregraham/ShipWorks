using ShipWorks.Data.Model.Custom;
using ShipWorks.Shipping;

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
        public ShipmentTypeCode ShipmentType => UspsReseller == 1 ? ShipmentTypeCode.Express1Usps : ShipmentTypeCode.Usps;
    }
}
