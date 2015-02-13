using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    /// <summary>
    /// Basic repository for retrieving Stamps accounts
    /// </summary>
    public class StampsAccountRepository : CarrierAccountRepositoryBase<UspsAccountEntity>, ICarrierAccountRepository<UspsAccountEntity>
    {
        /// <summary>
        /// Returns a list of Stamps accounts.
        /// </summary>
        public override IEnumerable<UspsAccountEntity> Accounts
        {
            get
            {
                return StampsAccountManager.StampsAccounts.ToList();
            }
        }

        /// <summary>
        /// Returns a Stamps account for the provided accountID.
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
                long? accountID = new StampsShipmentType().GetPrimaryProfile().Postal.Usps.UspsAccountID;

                return GetProfileAccount(ShipmentTypeCode.Stamps, accountID);
            }
        }

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        /// <param name="account">The account.</param>
        public override void Save(UspsAccountEntity account)
        {
            StampsAccountManager.SaveAccount(account);
        }
    }
}
