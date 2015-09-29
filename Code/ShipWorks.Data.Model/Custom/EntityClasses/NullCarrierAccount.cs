using System;
using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;

namespace ShipWorks.Data.Model.Custom.EntityClasses
{
    public class NullCarrierAccount : NullEntity, ICarrierAccount
    {
        /// <summary>
        /// Get the made up account id
        /// </summary>
        public long AccountId => -1;

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
        /// Applies account to shipment
        /// </summary>
        public void ApplyTo(ShipmentEntity shipment)
        {
            throw new System.NotImplementedException();
        }
    }
}