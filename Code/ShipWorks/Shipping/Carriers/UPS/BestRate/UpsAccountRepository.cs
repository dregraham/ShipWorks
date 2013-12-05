using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.UPS.BestRate
{
    public class UpsAccountRepository : ICarrierAccountRepository<UpsAccountEntity>
    {
        /// <summary>
        /// Returns a list of accounts for the carrier.
        /// </summary>
        public IEnumerable<UpsAccountEntity> Accounts
        {
            get
            {
                return UpsAccountManager.Accounts;
            }
        }

        /// <summary>
        /// Returns a carrier account for the provided accountID.
        /// </summary>
        /// <param name="accountID">The account ID for which to return an account.</param>
        /// <returns>The matching account as IEntity2.</returns>
        public UpsAccountEntity GetAccount(long accountID)
        {
            return UpsAccountManager.GetAccount(accountID);
        }
    }
}
