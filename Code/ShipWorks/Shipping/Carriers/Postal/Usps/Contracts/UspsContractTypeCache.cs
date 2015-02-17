using System;
using System.Globalization;
using System.Runtime.Caching;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Contracts
{
    /// <summary>
    /// An in-memory cache for the contract types associated with a Stamps.com account. This is intended
    /// to be used in a manner that reduces the number of calls to the Stamps.com API while also keeping
    /// the contract type of the stamps account up to date in ShipWorks.
    /// </summary>
    public static class UspsContractTypeCache
    {
        private static readonly ObjectCache cache = MemoryCache.Default;
        private static readonly object syncLock = new object();
        
        /// <summary>
        /// Sets an item in the cache using the account ID as the key and the contract type as the value. 
        /// Entries remain in the cache for 30 minutes from being set.
        /// </summary>
        /// <param name="accountId">The account ID.</param>
        /// <param name="contractType">Type of the contract.</param>
        public static void Set(long accountId, UspsAccountContractType contractType)
        {
            lock (syncLock)
            {
                string key = GetKey(accountId);
                cache.Set(key, contractType, DateTime.UtcNow.AddMinutes(30));
            }
        }

        /// <summary>
        /// Gets the type of the contract.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <returns>The StampsAccountContractType associated with the given account ID; null is returned if the item is not in the cache.</returns>
        public static UspsAccountContractType GetContractType(long accountId)
        {
            lock (syncLock)
            {
                string key = GetKey(accountId);
                return (UspsAccountContractType)cache[key];
            }
        }

        /// <summary>
        /// Determines whether the cache [contains] an entry for [the specified account ID].
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <returns>A Boolean indicating whether an entry exists for the given account ID.</returns>
        public static bool Contains(long accountId)
        {
            lock (syncLock)
            {
                string key = GetKey(accountId);
                return cache[key] != null;
            }
        }

        /// <summary>
        /// A helper method to build the string representation of the account ID.
        /// </summary>
        private static string GetKey(long accountId)
        {
            return accountId.ToString(CultureInfo.InvariantCulture);
        }
    }
}
