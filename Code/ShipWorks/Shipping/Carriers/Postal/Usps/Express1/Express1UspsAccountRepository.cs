using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Express1;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Express1
{
    /// <summary>
    /// Account repository for Express1 Usps accounts
    /// </summary>
    public class Express1UspsAccountRepository : CarrierAccountRepositoryBase<UspsAccountEntity>
    {
        /// <summary>
        /// Gets all the Express1 Usps accounts in the system
        /// </summary>
        public override IEnumerable<UspsAccountEntity> Accounts
        {
            get
            {
                return UspsAccountManager.Express1Accounts;
            }
        }

        /// <summary>
        /// Gets the Express1 Usps account with the specified id
        /// </summary>
        /// <param name="accountID">Id of the account to retrieve</param>
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
                long? accountID = new Express1StampsShipmentType().GetPrimaryProfile().Postal.Usps.UspsAccountID;
                return GetProfileAccount(ShipmentTypeCode.Express1Stamps, accountID);
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
