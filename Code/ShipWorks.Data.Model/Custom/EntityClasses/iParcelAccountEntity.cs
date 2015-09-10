﻿using ShipWorks.Data.Model.Custom;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Extra implementation of the IParcelAccountEntity
    /// </summary>
    public partial class IParcelAccountEntity : ICarrierAccount
    {
        /// <summary>
        /// Gets the account id in a generic way
        /// </summary>
        public long AccountId => IParcelAccountID;

        /// <summary>
        /// Get the shipment type to which this account applies
        /// </summary>
        public int ShipmentType => 12;
    }
}
