﻿using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;

namespace ShipWorks.Data.Model.Custom.EntityClasses
{
    public class NullCarrierAccount : NullEntity, ICarrierAccount
    {
        /// <summary>
        /// Get the made up account id
        /// </summary>
        /// <remarks>This was changed to zero from -1 because it is used in the shipping panel to denote
        /// no accounts.  When the selected account is saved back to the shipment, it was saving -1 even though
        /// the shipping dialog saved zero.  This caused a lot of unnecessary flip-flopping.</remarks>
        public long AccountId => 0;

        /// <summary>
        /// Get the made up description
        /// </summary>
        public string Description => "No accounts";

        /// <summary>
        /// Get the made up shipment type (none)
        /// </summary>
        public ShipmentTypeCode ShipmentType => ShipmentTypeCode.None;

        /// <summary>
        /// Get the address
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
            // Since this is used for carriers that don't have accounts, this method will just do nothing since
            // throwing a NotImplementedException meant that we couldn't call this method without worrying about
            // the actual type.
        }
    }
}