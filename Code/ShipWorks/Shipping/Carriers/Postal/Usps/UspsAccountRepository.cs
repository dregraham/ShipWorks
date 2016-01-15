using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Basic repository for retrieving USPS accounts
    /// </summary>
    public class UspsAccountRepository : CarrierAccountRepositoryBase<UspsAccountEntity>
    {
        /// <summary>
        /// Returns a list of USPS accounts.
        /// </summary>
        public override IEnumerable<UspsAccountEntity> Accounts => UspsAccountManager.UspsAccounts.ToList();

        /// <summary>
        /// Force a check for changes
        /// </summary>
        public override void CheckForChangesNeeded() => UspsAccountManager.CheckForChangesNeeded();

        /// <summary>
        /// Returns a USPS account for the provided accountID.
        /// </summary>
        /// <param name="accountID">The account ID for which to return an account.</param>
        /// <returns>The matching account.</returns>
        public override UspsAccountEntity GetAccount(long accountID)
        {
            return Accounts.ToList().FirstOrDefault(a => a.UspsAccountID == accountID);
        }

        /// <summary>
        /// Gets the default profile account.
        /// </summary>
        /// <value>
        /// The default profile account.
        /// </value>
        public override UspsAccountEntity DefaultProfileAccount
        {
            get
            {
                long? accountID = GetPrimaryProfile(ShipmentTypeCode.Usps).Postal.Usps.UspsAccountID;
                return GetProfileAccount(ShipmentTypeCode.Usps, accountID);
            }
        }

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        /// <param name="account">The account.</param>
        public override void Save(UspsAccountEntity account)
        {
            UspsAccountManager.SaveAccount(account);
        }
    }
}
