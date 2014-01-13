using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.iParcel.BestRate
{
    public class iParcelAccountRepository : ICarrierAccountRepository<IParcelAccountEntity>
    {
        /// <summary>
        /// Gets the accounts for the carrier.
        /// </summary>
        public IEnumerable<IParcelAccountEntity> Accounts
        {
            get
            {
                return iParcelAccountManager.Accounts;
            }
        }

        /// <summary>
        /// Returns a carrier account for the provided accountID.
        /// </summary>
        /// <param name="accountID">The account ID for which to return an account.</param>
        /// <returns>The matching account as IEntity2.</returns>
        public IParcelAccountEntity GetAccount(long accountID)
        {
            return iParcelAccountManager.GetAccount(accountID);
        }
    }
}
