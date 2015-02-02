using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Postal rate selection that is aware of accounts
    /// </summary>
    public class UspsPostalRateSelection : PostalRateSelection
    {
        public UspsPostalRateSelection(PostalServiceType serviceType, PostalConfirmationType confirmationType, StampsAccountEntity account)
            : base(serviceType, confirmationType)
        {
            Accounts = new List<StampsAccountEntity> { account };
        }

        /// <summary>
        /// Accounts associated with this rate
        /// </summary>
        public List<StampsAccountEntity> Accounts { get; private set; }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/>, is equal to this instance.
        /// </summary>
        public override bool Equals(object obj)
        {
            UspsPostalRateSelection postalRateSelection = obj as UspsPostalRateSelection;

            return postalRateSelection != null &&
                postalRateSelection.Accounts.Count == Accounts.Count &&
                postalRateSelection.Accounts.Select(account => account.StampsAccountID)
                    .All(x => Accounts.Select(account => account.StampsAccountID).Contains(x)) &&
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
    }
}
