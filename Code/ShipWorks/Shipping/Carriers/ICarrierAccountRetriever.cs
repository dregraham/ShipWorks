using System.Collections.Generic;
using ShipWorks.Data.Model.Custom;

namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// Non generic implementation of ICarrierAccountRepository
    /// </summary>
    public interface ICarrierAccountRetriever<out T> where T : ICarrierAccount
    {
        /// <summary>
        /// Returns a carrier account for the provided accountID.
        /// </summary>
        /// <param name="accountID">The account ID for which to return an account.</param>
        /// <returns>The matching account as IEntity2.</returns>
        T GetAccount(long accountID);

        /// <summary>
        /// Gets the accounts as ICarrierAccounts.
        /// </summary>
        IEnumerable<T> Accounts { get; }
    }
}