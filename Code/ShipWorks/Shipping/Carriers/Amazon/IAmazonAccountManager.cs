using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Manager for working with Amazon accounts
    /// </summary>
    public interface IAmazonAccountManager
    {
        /// <summary>
        /// Initialize Account manager
        /// </summary>
        void InitializeForCurrentSession();

        /// <summary>
        /// Return the active list of Amazon accounts
        /// </summary>
        IEnumerable<AmazonAccountEntity> Accounts { get; }

        /// <summary>
        /// Save the given Amazon account
        /// </summary>
        void SaveAccount(AmazonAccountEntity account);

        /// <summary>
        /// Get the account with the specified ID, or null if not found.
        /// </summary>
        AmazonAccountEntity GetAccount(long accountID);

        /// <summary>
        /// Delete the given Amazon account
        /// </summary>
        void DeleteAccount(AmazonAccountEntity account);

        /// <summary>
        /// Get the default description to use for the given account
        /// </summary>
        string GetDefaultDescription(AmazonAccountEntity account);
    }
}