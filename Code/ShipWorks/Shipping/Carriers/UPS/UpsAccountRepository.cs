using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.OpenAccount;

namespace ShipWorks.Shipping.Carriers.UPS
{
    public class UpsAccountRepository : ICarrierAccountRepository<UpsAccountEntity>, IUpsOpenAccountRepository
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

        public UpsAccountEntity DefaultProfileAccount
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Saves the given UPS account entity to the underlying data source.
        /// </summary>
        /// <param name="account">The account.</param>
        public void Save(UpsAccountEntity account)
        {
            if (account == null)
            {
                throw new ArgumentNullException("account");
            }

            UpsAccountManager.SaveAccount(account);
        }
    }
}
