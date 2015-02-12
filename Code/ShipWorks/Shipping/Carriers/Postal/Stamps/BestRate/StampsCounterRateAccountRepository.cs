using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.BestRate
{
    /// <summary>
    /// A repository for Stamps.com counter rate accounts
    /// </summary>
    public class StampsCounterRateAccountRepository : ICarrierAccountRepository<StampsAccountEntity>
    {
        private readonly ICounterRatesCredentialStore credentialStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="StampsCounterRateAccountRepository"/> class.
        /// </summary>
        /// <param name="credentialStore">The credential store.</param>
        public StampsCounterRateAccountRepository(ICounterRatesCredentialStore credentialStore)
        {
            this.credentialStore = credentialStore;
        }

        /// <summary>
        /// Gets the accounts for the carrier.
        /// </summary>
        public IEnumerable<StampsAccountEntity> Accounts
        {
            get 
            { 
                List<StampsAccountEntity> accounts = new List<StampsAccountEntity>();
                try
                {
                    StampsAccountEntity account = new StampsAccountEntity()
                    {
                        Username = credentialStore.StampsUsername,
                        Password = credentialStore.StampsPassword,
                        PostalCode = "63102",
                        CountryCode = "US",
                        StampsAccountID = -1052,
                        CreatedDate = DateTime.Now.AddDays(-30)
                    };
                    
                    accounts.Add(account);
                }
                catch (MissingCounterRatesCredentialException)
                {
                    // Eat this exception, and carry on as if there was not an account
                }
                return accounts;
            }
        }

        /// <summary>
        /// Returns a carrier counter rate account.
        /// </summary>
        /// <returns>Returns the first counter rate.</returns>
        public StampsAccountEntity GetAccount(long accountID)
        {
            return Accounts.First();
        }

        /// <summary>
        /// Gets the default profile account.
        /// </summary>
        public StampsAccountEntity DefaultProfileAccount
        {
            get { return Accounts.First(); }
        }

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        /// <param name="account">The account.</param>
        public void Save(StampsAccountEntity account)
        {
            // Nothing to save. This is a counter rate account.
        }
    }
}
