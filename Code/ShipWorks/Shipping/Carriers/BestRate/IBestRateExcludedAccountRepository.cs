using System.Collections.Generic;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    public interface IBestRateExcludedAccountRepository
    {
        /// <summary>
        /// Initialize the repository
        /// </summary>
        void InitializeForCurrentSession();

        /// <summary>
        /// Save the given accountIDs to the repository
        /// </summary>
        void Save(IEnumerable<long> accountIDs);

        /// <summary>
        /// Gets all the excluded account IDs from the repository
        /// </summary>
        IEnumerable<long> GetAll();
    }
}
