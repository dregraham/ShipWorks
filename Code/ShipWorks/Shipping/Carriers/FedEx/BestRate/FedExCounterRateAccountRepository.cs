using System;
using System.Collections.Generic;
using System.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;

namespace ShipWorks.Shipping.Carriers.FedEx.BestRate
{
    /// <summary>
    /// A repository for FedEx counter rate accounts
    /// </summary>
    public class FedExCounterRateAccountRepository : FedExSettingsRepository, ICarrierAccountRepository<FedExAccountEntity>
    {
        private readonly ICredentialStore credentialStore;
        private readonly Lazy<List<FedExAccountEntity>> lazyAccounts;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="credentialStore">Credential store that contains FedEx counter rate credentials.</param>
        public FedExCounterRateAccountRepository(ICredentialStore credentialStore)
        {
            this.credentialStore = credentialStore;
            lazyAccounts = new Lazy<List<FedExAccountEntity>>(ConvertTangoCredentialsToFedExAccountEntities);
        }

        /// <summary>
        /// Gets the accounts for the carrier.
        /// </summary>
        public IEnumerable<FedExAccountEntity> Accounts => lazyAccounts.Value;

        /// <summary>
        /// Force a check for changes
        /// </summary>
        public void CheckForChangesNeeded()
        {
            // We don't need to check for changes for counter rates
        }

        /// <summary>
        /// Converts Tango FedEx credentials into a FedExAccountEntity
        /// </summary>
        private List<FedExAccountEntity> ConvertTangoCredentialsToFedExAccountEntities()
        {
            List<FedExAccountEntity> accounts = new List<FedExAccountEntity>();

            try
            {
                FedExAccountEntity fedExAccountEntity = new FedExAccountEntity
                {
                    AccountNumber = credentialStore.FedExAccountNumber,
                    MeterNumber = credentialStore.FedExMeterNumber,
                    PostalCode = "63102",
                    CountryCode = "US",
                    SmartPostHubList = string.Empty,
                    FedExAccountID = -1055
                };

                accounts.Add(fedExAccountEntity);
            }
            catch (MissingCounterRatesCredentialException)
            {
                // Eat this exception, and carry on as if there was not an account
            }

            return accounts;
        }

        /// <summary>
        /// Returns a carrier counter rate account.
        /// </summary>
        /// <returns>Returns the first counter rate.</returns>
        public FedExAccountEntity GetAccount(long accountID)
        {
            return Accounts.First();
        }

        /// <summary>
        /// Gets the default profile account. This will always return the same account that is
        /// used to get counter rates.
        /// </summary>
        public FedExAccountEntity DefaultProfileAccount
        {
            get
            {
                return Accounts.First();
            }
        }

        /// <summary>
        /// Gets shipping settings with the counter version of the FedEx credentials
        /// </summary>
        /// <returns></returns>
        public override ShippingSettingsEntity GetShippingSettings()
        {
            ShippingSettingsEntity settings = base.GetShippingSettings();
            settings.FedExUsername = credentialStore.FedExUsername;
            settings.FedExPassword = credentialStore.FedExPassword;
            return settings;
        }

        /// <summary>
        /// Gets the FedEx account that should be used for counter rates.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A FedExAccountEntity object.</returns>
        public override IEntity2 GetAccount(ShipmentEntity shipment)
        {
            return Accounts.First();
        }

        /// <summary>
        /// Gets a list of the FedEx account that should be used for counter rates
        /// </summary>
        public override IEnumerable<IEntity2> GetAccounts()
        {
            return lazyAccounts.Value;
        }

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        /// <param name="account">The account.</param>
        public void Save(FedExAccountEntity account)
        {
            // Nothing to save. This is a counter rate account.
        }
    }
}
