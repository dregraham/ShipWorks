using Interapptive.Shared.Business;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Shipping;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Extra implementation of the DhlExpressAccountEntity
    /// </summary>
    public partial class DhlExpressAccountEntity : ICarrierAccount
    {
        /// <summary>
        /// Gets the account id in a generic way
        /// </summary>
        public long AccountId => this.DhlExpressAccountID;

        /// <summary>
        /// Get the shipment type to which this account applies
        /// </summary>
        public ShipmentTypeCode ShipmentType => ShipmentTypeCode.DhlExpress;

        /// <summary>
        /// Get the address of the account
        /// </summary>
        public PersonAdapter Address => new PersonAdapter(this, string.Empty);

        /// <summary>
        /// Gets the account description.
        /// </summary>
        public string AccountDescription => Description;

        /// <summary>
        /// Gets the shortened account description.
        /// </summary>
        public string ShortAccountDescription => UspsAccountId != null ? Description : AccountNumber.ToString();

        /// <summary>
        /// Applies account to shipment
        /// </summary>
        public void ApplyTo(ShipmentEntity shipment)
        {
            shipment.DhlExpress.DhlExpressAccountID = AccountId;
        }
    }
}
