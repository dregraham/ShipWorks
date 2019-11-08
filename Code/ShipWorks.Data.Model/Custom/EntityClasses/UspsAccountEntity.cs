﻿using Interapptive.Shared.Business;
using ShipWorks.Shipping;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Extra implementation of the UspsAccountEntity
    /// </summary>
    public partial class UspsAccountEntity
    {
        /// <summary>
        /// Gets the account id in a generic way
        /// </summary>
        public long AccountId => UspsAccountID;

        /// <summary>
        /// Get the shipment type to which this account applies
        /// </summary>
        /// <remarks>Assume Usps unless explicitly marked as Express1</remarks>
        public ShipmentTypeCode ShipmentType => UspsReseller == 1 ? ShipmentTypeCode.Express1Usps : ShipmentTypeCode.Usps;

        /// <summary>
        /// Get the address of the account
        /// </summary>
        public PersonAdapter Address
        {
            get { return new PersonAdapter(this, string.Empty); }
            set { PersonAdapter.Copy(value, Address); }
        }

        /// <summary>
        /// Gets the account description.
        /// </summary>
        public string AccountDescription => Description;

        /// <summary>
        /// Applies account to shipment
        /// </summary>
        public void ApplyTo(ShipmentEntity shipment) =>
            shipment.Postal.Usps.UspsAccountID = AccountId;

        /// <summary>
        /// Gets the shortened account description.
        /// </summary>
        public string ShortAccountDescription => Username;
    }
}
