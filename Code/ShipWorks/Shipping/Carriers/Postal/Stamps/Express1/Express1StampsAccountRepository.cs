using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Express1
{
    /// <summary>
    /// Account repository for Express1 Stamps accounts
    /// </summary>
    public class Express1StampsAccountRepository : ICarrierAccountRepository<StampsAccountEntity>
    {
        /// <summary>
        /// Gets all the Express1 Stamps accounts in the system
        /// </summary>
        public IEnumerable<StampsAccountEntity> Accounts
        {
            get
            {
                return StampsAccountManager.Express1Accounts;
            }
        }

        /// <summary>
        /// Gets the Stamps account with the specified id
        /// </summary>
        /// <param name="accountID">Id of the account to retrieve</param>
        public StampsAccountEntity GetAccount(long accountID)
        {
            return StampsAccountManager.GetAccount(accountID);
        }
    }
}
