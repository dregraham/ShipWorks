using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;

namespace ShipWorks.Shipping.Carriers.UPS.BestRate
{
    public class UpsAccountRepository : ICarrierAccountRepository
    {
        /// <summary>
        /// Returns a list of accounts for the carrier.
        /// </summary>
        public IEnumerable<IEntity2> Accounts
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
        public IEntity2 GetAccount(long accountID)
        {
            return UpsAccountManager.GetAccount(accountID);
        }
    }
}
