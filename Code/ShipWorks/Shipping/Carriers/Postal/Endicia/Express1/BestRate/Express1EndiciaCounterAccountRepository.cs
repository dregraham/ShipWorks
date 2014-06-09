using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Express1.BestRate
{
    /// <summary>
    /// Gets account data for Express1 Endicia counter rates
    /// </summary>
    public class Express1EndiciaCounterAccountRepository : ICarrierAccountRepository<EndiciaAccountEntity>
    {
        private readonly ICredentialStore credentialStore;
        private readonly Lazy<List<EndiciaAccountEntity>> accounts; 

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="credentialStore"></param>
        public Express1EndiciaCounterAccountRepository(ICredentialStore credentialStore)
        {
            this.credentialStore = credentialStore;
            accounts = new Lazy<List<EndiciaAccountEntity>>(ConvertCredentialsToEndiciaAccountEntities);
        }

        /// <summary>
        /// Gets a list of counter rate accounts
        /// </summary>
        public IEnumerable<EndiciaAccountEntity> Accounts
        {
            get
            {
                return accounts.Value;
            }
        }

        /// <summary>
        /// Gets a counter rate account
        /// </summary>
        public EndiciaAccountEntity GetAccount(long accountID)
        {
            return accounts.Value.First();
        }

        /// <summary>
        /// Gets the default profile account. This will always return the same account that is 
        /// used to get counter rates.
        /// </summary>
        public EndiciaAccountEntity DefaultProfileAccount
        {
            get
            {
                return Accounts.First();
            }
        }

        /// <summary>
        /// Create a list of accounts from the credential store
        /// </summary>
        private List<EndiciaAccountEntity> ConvertCredentialsToEndiciaAccountEntities()
        {
            List<EndiciaAccountEntity> counterRatesAccounts = new List<EndiciaAccountEntity>();

            try
            {
                EndiciaAccountEntity account = new EndiciaAccountEntity
                {
                    AccountNumber = credentialStore.Express1EndiciaAccountNumber,
                    ApiUserPassword = credentialStore.Express1EndiciaPassPhrase,
                    EndiciaReseller = (int)EndiciaReseller.Express1,
                    CountryCode = "US"
                };

                counterRatesAccounts.Add(account);
            }
            catch (MissingCounterRatesCredentialException)
            {
                // Eat this exception, and carry on as if there was not an account
            }

            return counterRatesAccounts;
        }
    }
}
