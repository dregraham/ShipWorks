using ShipWorks.Data.Model.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Business;
using ShipWorks.Shipping;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Extra implementation of the ShipEngineAccountEntity
    /// </summary>
    public partial class ShipEngineAccountEntity : ICarrierAccount
    {
        /// <summary>
        /// Gets the account id in a generic way
        /// </summary>
        public long AccountId => ShipEngineAccountID;

        /// <summary>
        /// Get the shipment type to which this account applies
        /// </summary>
        public ShipmentTypeCode ShipmentType => (ShipmentTypeCode) ShipmentTypeCode;

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
        public void ApplyTo(ShipmentEntity shipment)
        {
            shipment.DhlExpress.ShipEngineAccountID = AccountId;
        }
    }
}
