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
        private readonly ICounterRatesCredentialStore credentialStore;
        private readonly Lazy<List<EndiciaAccountEntity>> accounts; 

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="credentialStore"></param>
        public Express1EndiciaCounterAccountRepository(ICounterRatesCredentialStore credentialStore)
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
        /// Create a list of accounts from the credential store
        /// </summary>
        private List<EndiciaAccountEntity> ConvertCredentialsToEndiciaAccountEntities()
        {
            EndiciaAccountEntity account = new EndiciaAccountEntity
            {
                AccountNumber = credentialStore.Express1EndiciaAccountNumber,
                ApiUserPassword = credentialStore.Express1EndiciaPassPhrase,
                EndiciaReseller = (int) EndiciaReseller.Express1
            };

            return new List<EndiciaAccountEntity> { account };
        }
    }
}
