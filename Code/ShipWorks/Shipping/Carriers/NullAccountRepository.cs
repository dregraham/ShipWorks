using System.Collections.Generic;
using ShipWorks.Data.Model.Custom.EntityClasses;

namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// Basic repository for retrieving postal (w/o postage) accounts
    /// </summary>
    public class NullAccountRepository : ICarrierAccountRepository<NullCarrierAccount>
    {
        /// <summary>
        /// Returns a list of postal (w/o postage) accounts.
        /// </summary>
        public IEnumerable<NullCarrierAccount> Accounts => new List<NullCarrierAccount> { new NullCarrierAccount() };

        /// <summary>
        /// Force a check for changes
        /// </summary>
        public void CheckForChangesNeeded()
        {
            // Since we have no accounts, this doesn't need to do anything
        }

        /// <summary>
        /// Returns a postal (w/o postage) account for the provided accountID.
        /// </summary>
        /// <param name="accountID">The account ID for which to return an account.</param>
        /// <returns>Null entity.</returns>
        public NullCarrierAccount GetAccount(long accountID) => new NullCarrierAccount();

        /// <summary>
        /// Gets the default profile account.
        /// </summary>
        public NullCarrierAccount DefaultProfileAccount => new NullCarrierAccount();

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        /// <param name="account">The account.</param>
        public void Save(NullCarrierAccount account)
        {
            // Nothing to save
        }
    }
}
