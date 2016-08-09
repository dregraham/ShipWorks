using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Extra implementation of the FedExAccountEntity
    /// </summary>
    public partial class ReadOnlyFedExAccountEntity
    {
        /// <summary>
        /// Gets the account id in a generic way
        /// </summary>
        public long AccountId => FedExAccountID;

        /// <summary>
        /// Get the shipment type to which this account applies
        /// </summary>
        public ShipmentTypeCode ShipmentType => ShipmentTypeCode.FedEx;

        /// <summary>
        /// Get the address of the account
        /// </summary>
        public PersonAdapter Address { get; private set; }

        /// <summary>
        /// Applies account to shipment
        /// </summary>
        public void ApplyTo(ShipmentEntity shipment) => shipment.FedEx.FedExAccountID = AccountId;

        /// <summary>
        /// Copy custom data
        /// </summary>
        partial void CopyCustomFedExAccountData(IFedExAccountEntity source)
        {
            Address = source.Address.CopyToNew();
        }
    }
}
