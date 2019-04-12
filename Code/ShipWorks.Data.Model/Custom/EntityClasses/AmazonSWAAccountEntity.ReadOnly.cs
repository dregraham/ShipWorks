using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using System;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Extra implementation of the AmazonSWAAccountEntity
    /// </summary>
    public partial class ReadOnlyAmazonSWAAccountEntity
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
        public PersonAdapter Address { get; private set; }

        /// <summary>
        /// Gets the account description.
        /// </summary>
        public string AccountDescription => Description;

        /// <summary>
        /// Applies account to shipment
        /// </summary>
        public void ApplyTo(ShipmentEntity shipment)
        {
            shipment.AmazonSWA.AmazonSWAAccountID = AccountId;
        }

        /// <summary>
        /// Copy custom data
        /// </summary>
        partial void CopyCustomAmazonSWAAccountData(IAmazonSWAAccountEntity source)
        {
            Address = source.Address.CopyToNew();
        }
    }
}
