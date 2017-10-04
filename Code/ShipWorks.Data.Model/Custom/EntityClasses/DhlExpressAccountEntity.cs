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
        /// Applies account number to DhlExpress Shipment
        /// </summary>
        /// <param name="shipment"></param>
        public void ApplyTo(ShipmentEntity shipment)
        {
            throw new NotImplementedException();
        }
    }
}
