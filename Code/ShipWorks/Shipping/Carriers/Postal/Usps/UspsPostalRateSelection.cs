using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Postal rate selection that is aware of accounts
    /// </summary>
    public class UspsPostalRateSelection : PostalRateSelection, IUspsPostalRateSelection
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UspsPostalRateSelection(PostalServiceType serviceType, UspsAccountEntity account)
            : base(serviceType)
        {
            Accounts = new List<IUspsAccountEntity> { account };
        }

        /// <summary>
        /// Accounts associated with this rate
        /// </summary>
        public List<IUspsAccountEntity> Accounts { get; }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/>, is equal to this instance.
        /// </summary>
        public override bool Equals(object obj)
        {
            UspsPostalRateSelection postalRateSelection = obj as UspsPostalRateSelection;

            return postalRateSelection != null &&
                postalRateSelection.Accounts.Count == Accounts.Count &&
                postalRateSelection.Accounts.Select(account => account.UspsAccountID)
                    .All(x => Accounts.Select(account => account.UspsAccountID).Contains(x)) &&
                base.Equals(obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode() + Accounts.GetHashCode();
        }

        /// <summary>
        /// Is the rate compatible with the specified shipment
        /// </summary>
        public bool IsRateFor(IShipmentEntity shipment)
        {
            if (shipment?.Postal == null)
            {
                throw new ArgumentNullException("shipment");
            }

            return ServiceType == (PostalServiceType) shipment.Postal.Service;
        }
    }
}
