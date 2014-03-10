using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx;

namespace ShipWorks.Shipping.Carriers.OnTrac.BestRate
{
    class OnTracAccountRepository : ICarrierAccountRepository<OnTracAccountEntity>
    {
        /// <summary>
        /// Gets the accounts for the carrier.
        /// </summary>
        public IEnumerable<OnTracAccountEntity> Accounts
        {
            get
            {
                return OnTracAccountManager.Accounts;
            }
        }

        /// <summary>
        /// Returns a carrier account for the provided accountID.
        /// </summary>
        /// <param name="accountID">The account ID for which to return an account.</param>
        /// <returns>The matching account as IEntity2.</returns>
        public OnTracAccountEntity GetAccount(long accountID)
        {
            return OnTracAccountManager.GetAccount(accountID);
        }

        public OnTracAccountEntity DefaultProfileAccount
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
