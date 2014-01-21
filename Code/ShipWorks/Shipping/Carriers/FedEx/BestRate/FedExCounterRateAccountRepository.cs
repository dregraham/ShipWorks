using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.FedEx.BestRate
{
    /// <summary>
    /// A repository for FedEx counter rate accounts
    /// </summary>
    public class FedExCounterRateAccountRepository : ICarrierAccountRepository<FedExAccountEntity>
    {
        private readonly ICounterRatesCredentialStore counterRatesCredentialStore;
        private readonly Lazy<List<FedExAccountEntity>> lazyAccounts;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="counterRatesCredentialStore">Credential store that contains FedEx counter rate credentials.</param>
        public FedExCounterRateAccountRepository(ICounterRatesCredentialStore counterRatesCredentialStore)
        {
            this.counterRatesCredentialStore = counterRatesCredentialStore;
            lazyAccounts = new Lazy<List<FedExAccountEntity>>(ConvertTangoCredentialsToFedExAccountEntities);
        }

        /// <summary>
        /// Gets the accounts for the carrier.
        /// </summary>
        public IEnumerable<FedExAccountEntity> Accounts 
        {
            get
            {
                return lazyAccounts.Value;
            }
        }

        /// <summary>
        /// Converts Tango FedEx credentials into a FedExAccountEntity
        /// </summary>
        private List<FedExAccountEntity> ConvertTangoCredentialsToFedExAccountEntities()
        {
            FedExAccountEntity fedExAccountEntity = new FedExAccountEntity
            {
                AccountNumber = counterRatesCredentialStore.FedExAccountNumber,
                MeterNumber = counterRatesCredentialStore.FedExMeterNumber,
                FedExAccountID = -1055
            };

            return new List<FedExAccountEntity>() {fedExAccountEntity};
        }

        /// <summary>
        /// Returns a carrier counter rate account.
        /// </summary>
        /// <returns>Returns the first counter rate.</returns>
        public FedExAccountEntity GetAccount(long accountID)
        {
            return Accounts.First();
        }
    }
}
