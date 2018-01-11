﻿using Interapptive.Shared.Business;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Shipping;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Extra implementation of the UpsAccountEntity
    /// </summary>
    public partial class UpsAccountEntity : ICarrierAccount
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
        public PersonAdapter Address => new PersonAdapter(this, string.Empty);

        /// <summary>
        /// Gets the account description.
        /// </summary>
        public string AccountDescription => Description;

        /// <summary>
        /// Applies account to shipment
        /// </summary>
        public void ApplyTo(ShipmentEntity shipment) =>
            shipment.Ups.UpsAccountID = AccountId;
    }
}
