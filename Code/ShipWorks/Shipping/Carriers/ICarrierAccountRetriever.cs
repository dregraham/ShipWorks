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
        /// Gets the accounts as ICarrierAccounts.
        /// </summary>
        IEnumerable<T> Accounts { get; }
    }
}