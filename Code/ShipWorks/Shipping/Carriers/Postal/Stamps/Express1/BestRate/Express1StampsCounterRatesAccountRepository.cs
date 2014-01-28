using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Express1.BestRate
{
    /// <summary>
    /// Gets account data for Express1 Stamps counter rates
    /// </summary>
    public class Express1StampsCounterRatesAccountRepository : ICarrierAccountRepository<StampsAccountEntity>
    {
        private readonly ICounterRatesCredentialStore credentialStore;
        private readonly Lazy<List<StampsAccountEntity>> accounts;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="credentialStore"></param>
        public Express1StampsCounterRatesAccountRepository(ICounterRatesCredentialStore credentialStore)
        {
            this.credentialStore = credentialStore;
            accounts = new Lazy<List<StampsAccountEntity>>(ConvertCredentialsToStampsAccountEntities);
        }

        /// <summary>
        /// Gets a list of counter rate accounts
        /// </summary>
        public IEnumerable<StampsAccountEntity> Accounts
        {
            get
            {
                return accounts.Value;
            }
        }

        /// <summary>
        /// Gets a counter rate account
        /// </summary>
        public StampsAccountEntity GetAccount(long accountID)
        {
            return accounts.Value.First();
        }

        /// <summary>
        /// Create a list of accounts from the credential store
        /// </summary>
        private List<StampsAccountEntity> ConvertCredentialsToStampsAccountEntities()
        {
            StampsAccountEntity account = new StampsAccountEntity
            {
                Username = credentialStore.Express1StampUsername,
                Password = credentialStore.Express1StampsPassword,
                IsExpress1 = true
            };

            return new List<StampsAccountEntity> { account };
        }
    }
}
