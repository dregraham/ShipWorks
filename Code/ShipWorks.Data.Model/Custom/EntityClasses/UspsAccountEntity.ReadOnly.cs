using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Extra implementation of the UspsAccountEntity
    /// </summary>
    public partial class ReadOnlyUspsAccountEntity
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

        /// <summary>
        /// Get the address of the account
        /// </summary>
        public PersonAdapter Address { get; private set; }

        /// <summary>
        /// Gets the account description.
        /// </summary>
        public string AccountDescription => Description;

        /// <summary>
        /// Applies account to shipment
        /// </summary>
        public void ApplyTo(ShipmentEntity shipment) =>
            shipment.Postal.Usps.UspsAccountID = AccountId;

        /// <summary>
        /// Copy custom data
        /// </summary>
        partial void CopyCustomUspsAccountData(IUspsAccountEntity source)
        {
            Address = source.Address.CopyToNew();
        }
    }
}
