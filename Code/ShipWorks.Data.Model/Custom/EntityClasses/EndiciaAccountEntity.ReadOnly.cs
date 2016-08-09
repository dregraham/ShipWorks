using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Extra implementation of the EndiciaAccountEntity
    /// </summary>
    public partial class ReadOnlyEndiciaAccountEntity
    {
        /// <summary>
        /// Gets the account id in a generic way
        /// </summary>
        public long AccountId => EndiciaAccountID;

        /// <summary>
        /// Get the shipment type to which this account applies
        /// </summary>
        /// <remarks>Assume Endicia unless explicitly marked as Express1</remarks>
        public ShipmentTypeCode ShipmentType => EndiciaReseller == 1 ? ShipmentTypeCode.Express1Endicia : ShipmentTypeCode.Endicia;

        /// <summary>
        /// Get the address of the account
        /// </summary>
        public PersonAdapter Address { get; private set; }

        /// <summary>
        /// Applies account to shipment
        /// </summary>
        public void ApplyTo(ShipmentEntity shipment) =>
            shipment.Postal.Endicia.EndiciaAccountID = AccountId;

        /// <summary>
        /// Copy custom data
        /// </summary>
        partial void CopyCustomEndiciaAccountData(IEndiciaAccountEntity source)
        {
            Address = source.Address.CopyToNew();
        }
    }
}
