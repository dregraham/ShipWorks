using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Express1
{
    /// <summary>
    /// Account repository for Express1 Endicia accounts
    /// </summary>
    public class Express1EndiciaAccountRepository : ICarrierAccountRepository<EndiciaAccountEntity>
    {
        /// <summary>
        /// Gets all the Express1 Endicia accounts in the system
        /// </summary>
        public IEnumerable<EndiciaAccountEntity> Accounts
        {
            get
            {
                return EndiciaAccountManager.Express1Accounts;
            }
        }

        /// <summary>
        /// Gets the Endicia account with the specified id
        /// </summary>
        /// <param name="accountID">Id of the account to retrieve</param>
        public EndiciaAccountEntity GetAccount(long accountID)
        {
            return EndiciaAccountManager.GetAccount(accountID);
        }

        public EndiciaAccountEntity DefaultProfileAccount
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
