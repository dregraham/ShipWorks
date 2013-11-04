using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Shipping.Carriers
{
    public interface ICarrierAccountRepository
    {
        /// <summary>
        /// Returns a list of accounts for the carrier.
        /// </summary>
        IEnumerable<IEntity2> Accounts { get; }

        /// <summary>
        /// Returns a carrier account for the provided accountID.
        /// </summary>
        /// <param name="accountID">The account ID for which to return an account.</param>
        /// <returns>The matching account as IEntity2.</returns>
        IEntity2 GetAccount(long accountID);
    }
}
