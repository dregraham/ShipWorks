using ShipWorks.Data.Model.Custom;
using Interapptive.Shared.Business;
using ShipWorks.Shipping;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Extra implementation of the DhlExpressAccountEntity
    /// </summary>
    public partial class AmazonSWAAccountEntity : ICarrierAccount
    {
        /// <summary>
        /// Gets the account id in a generic way
        /// </summary>
        public long AccountId => AmazonSWAAccountID;

        /// <summary>
        /// Get the shipment type to which this account applies
        /// </summary>
        public ShipmentTypeCode ShipmentType => ShipmentTypeCode.AmazonSWA;

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
        public string ShortAccountDescription => Description;

        /// <summary>
        /// Applies account to shipment
        /// </summary>
        public void ApplyTo(ShipmentEntity shipment)
        {
            shipment.AmazonSWA.AmazonSWAAccountID = AccountId;
        }
    }
}
