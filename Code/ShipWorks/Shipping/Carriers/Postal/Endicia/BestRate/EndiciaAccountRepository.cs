using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.BestRate
{
    /// <summary>
    /// Basic repository for retrieving Endicia accounts
    /// </summary>
    public class EndiciaAccountRepository : ICarrierAccountRepository<EndiciaAccountEntity>
    {
        /// <summary>
        /// Returns a list of Endicia accounts.
        /// </summary>
        public IEnumerable<EndiciaAccountEntity> Accounts
        {
            get
            {
                return EndiciaAccountManager.EndiciaAccounts.ToList();
            }
        }

        /// <summary>
        /// Returns a Endicia account for the provided accountID.
        /// </summary>
        /// <param name="accountID">The account ID for which to return an account.</param>
        /// <returns>The matching account.</returns>
        public EndiciaAccountEntity GetAccount(long accountID)
        {
            return EndiciaAccountManager.GetAccount(accountID);
        }
    }
}
