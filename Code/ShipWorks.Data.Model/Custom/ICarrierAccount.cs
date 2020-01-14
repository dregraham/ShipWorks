﻿using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;

namespace ShipWorks.Data.Model.Custom
{
    /// <summary>
    /// Allow carrier accounts to be used interchangeably
    /// </summary>
    public interface ICarrierAccount
    {
        /// <summary>
        /// Get the id of the account
        /// </summary>
        long AccountId { get; }

        /// <summary>
        /// Get the shipment type to which this account applies
        /// </summary>
        ShipmentTypeCode ShipmentType { get; }

        /// <summary>
        /// Get the address of the account
        /// </summary>
        PersonAdapter Address { get; }

        /// <summary>
        /// Gets the account description.
        /// </summary>
        string AccountDescription { get; }

        /// <summary>
        /// Gets the shortened account description.
        /// </summary>
        string ShortAccountDescription { get; }

        /// <summary>
        /// Apply this account to the shipment.
        /// </summary>
        void ApplyTo(ShipmentEntity shipment);
    }
}
