using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Extra implementation of the UpsAccountEntity
    /// </summary>
    public partial class ReadOnlyUpsAccountEntity
    {
        /// <summary>
        /// Gets the account id in a generic way
        /// </summary>
        public long AccountId => UpsAccountID;

        /// <summary>
        /// Get the shipment type to which this account applies
        /// </summary>
        public ShipmentTypeCode ShipmentType => ShipmentTypeCode.UpsOnLineTools;

        /// <summary>
        /// Get the address of the account
        /// </summary>
        public PersonAdapter Address { get; private set; }

        /// <summary>
        /// Applies account to shipment
        /// </summary>
        public void ApplyTo(ShipmentEntity shipment) =>
            shipment.Ups.UpsAccountID = AccountId;

        /// <summary>
        /// Copy custom data
        /// </summary>
        partial void CopyCustomUpsAccountData(IUpsAccountEntity source)
        {
            Address = source.Address.CopyToNew();
        }
    }
}
