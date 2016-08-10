using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Extra implementation of the OnTracAccountEntity
    /// </summary>
    public partial class ReadOnlyOnTracAccountEntity
    {
        /// <summary>
        /// Gets the account id in a generic way
        /// </summary>
        public long AccountId => OnTracAccountID;

        /// <summary>
        /// Get the shipment type to which this account applies
        /// </summary>
        public ShipmentTypeCode ShipmentType => ShipmentTypeCode.OnTrac;

        /// <summary>
        /// Get the address of the account
        /// </summary>
        public PersonAdapter Address { get; private set; }

        /// <summary>
        /// Applies account to shipment
        /// </summary>
        public void ApplyTo(ShipmentEntity shipment) => shipment.OnTrac.OnTracAccountID = AccountId;

        /// <summary>
        /// Copy custom data
        /// </summary>
        partial void CopyCustomOnTracAccountData(IOnTracAccountEntity source)
        {
            Address = source.Address.CopyToNew();
        }
    }
}
