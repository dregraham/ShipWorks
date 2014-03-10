using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.BestRate
{
    /// <summary>
    /// Basic repository for retrieving Stamps accounts
    /// </summary>
    public class StampsAccountRepository : ICarrierAccountRepository<StampsAccountEntity>
    {
        /// <summary>
        /// Returns a list of Stamps accounts.
        /// </summary>
        public IEnumerable<StampsAccountEntity> Accounts
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
        public StampsAccountEntity GetAccount(long accountID)
        {
            return StampsAccountManager.GetAccount(accountID);
        }

        public StampsAccountEntity DefaultProfileAccount
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
