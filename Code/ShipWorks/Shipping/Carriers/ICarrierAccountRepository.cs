using System.Collections.Generic;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Shipping.Carriers
{
    public interface ICarrierAccountRepository<out T> where T : IEntity2
    {
        /// <summary>
        /// Returns a list of accounts for the carrier.
        /// </summary>
        IEnumerable<T> Accounts { get; }

        /// <summary>
        /// Returns a carrier account for the provided accountID.
        /// </summary>
        /// <param name="accountID">The account ID for which to return an account.</param>
        /// <returns>The matching account as IEntity2.</returns>
        T GetAccount(long accountID);

        /// <summary>
        /// Returns the default account as defined by the primary profile
        /// </summary>
        T DefaultProfileAccount { get; }
    }
}
