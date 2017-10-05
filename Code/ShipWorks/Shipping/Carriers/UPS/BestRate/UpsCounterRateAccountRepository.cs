using System;
using System.Collections.Generic;
using System.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.UpsEnvironment;

namespace ShipWorks.Shipping.Carriers.UPS.BestRate
{
    /// <summary>
    /// A repository for UPS counter rate accounts
    /// </summary>
    public class UpsCounterRateAccountRepository : UpsSettingsRepository,
        ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity>,
        ICarrierAccountRetriever
    {
        private readonly ICredentialStore credentialStore;
        private readonly Lazy<List<UpsAccountEntity>> lazyAccounts;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="credentialStore"></param>
        public UpsCounterRateAccountRepository(ICredentialStore credentialStore)
        {
            this.credentialStore = credentialStore;

            lazyAccounts = new Lazy<List<UpsAccountEntity>>(ConvertTangoCredentialsToUpsAccountEntities);
        }

        /// <summary>
        /// Gets the accounts for the carrier.
        /// </summary>
        public IEnumerable<UpsAccountEntity> Accounts => lazyAccounts.Value;

        /// <summary>
        /// Gets the accounts for the carrier.
        /// </summary>
        public IEnumerable<IUpsAccountEntity> AccountsReadOnly => lazyAccounts.Value;


        /// <summary>
        /// Saves the specified account.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="account">The account.</param>
        /// v
        public void Save<T>(T account) => Save(account as UpsAccountEntity);

        /// <summary>
        /// Deletes the account.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="account">The account.</param>
        public void DeleteAccount<T>(T account) => DeleteAccount(account as UpsAccountEntity);

        /// <summary>
        /// Force a check for changes
        /// </summary>
        public void CheckForChangesNeeded()
        {
            // We don't need to check for changes for counter rates
        }

        /// <summary>
        /// Returns a carrier counter rate account.
        /// </summary>
        /// <returns>Returns the first counter rate.</returns>
        public UpsAccountEntity GetAccount(long accountID) => Accounts.First();

        /// <summary>
        /// Returns a carrier counter rate account.
        /// </summary>
        /// <returns>Returns the first counter rate.</returns>
        public IUpsAccountEntity GetAccountReadOnly(long accountID) => AccountsReadOnly.First();

        /// <summary>
        /// Returns a carrier account associated with the specified shipment
        /// </summary>
        public UpsAccountEntity GetAccount(IShipmentEntity shipment) => Accounts.First();

        /// <summary>
        /// Returns a carrier account associated with the specified shipment
        /// </summary>
        public IUpsAccountEntity GetAccountReadOnly(IShipmentEntity shipment) => AccountsReadOnly.First();

        /// <summary>
        /// Gets the default profile account. This will always return the same account that is
        /// used to get counter rates.
        /// </summary>
        public UpsAccountEntity DefaultProfileAccount
        {
            get
            {
                return Accounts.First();
            }
        }

        /// <summary>
        /// Gets the Ups account that should be used for counter rates.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A UpsAccountEntity object.</returns>
        public override IEntity2 GetAccount(ShipmentEntity shipment)
        {
            return Accounts.First();
        }

        /// <summary>
        /// Converts Tango Ups credentials into a UpsAccountEntity
        /// </summary>
        private List<UpsAccountEntity> ConvertTangoCredentialsToUpsAccountEntities()
        {
            List<UpsAccountEntity> accounts = new List<UpsAccountEntity>();

            try
            {
                // Set the account number to an empty string because the edition check assumes it will not be null.
                // This will prevent any other consumers from having to worry about calling ToLower or anything else on a null string.
                UpsAccountEntity upsAccountEntity = new UpsAccountEntity
                {
                    UserID = credentialStore.UpsUserId,
                    Password = credentialStore.UpsPassword,
                    PostalCode = "63102",
                    CountryCode = "US",
                    RateType = (int) UpsRateType.Retail,
                    UpsAccountID = -1056,
                    AccountNumber = string.Empty
                };

                accounts.Add(upsAccountEntity);
            }
            catch (MissingCounterRatesCredentialException)
            {
                // Eat this exception, and carry on as if there was not an account
            }

            return accounts;
        }

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        /// <param name="account">The account.</param>
        public void Save(UpsAccountEntity account)
        {
            // Nothing to save. This is a counter rate account.
        }

        /// <summary>
        /// Deletes the account.
        /// </summary>
        /// <param name="account">The account.</param>
        public void DeleteAccount(UpsAccountEntity account)
        {
            //Nothing to delete for counter rate account.
        }

        /// <summary>
        /// Returns a carrier account for the provided accountID.
        /// </summary>
        ICarrierAccount ICarrierAccountRetriever.GetAccountReadOnly(long accountID) =>
            GetAccountReadOnly(accountID);

        /// <summary>
        /// Returns a carrier account for the provided accountID.
        /// </summary>
        ICarrierAccount ICarrierAccountRetriever.GetAccountReadOnly(IShipmentEntity shipment) =>
            GetAccountReadOnly(shipment);

        /// <summary>
        /// Returns a list of accounts for the carrier.
        /// </summary>
        IEnumerable<ICarrierAccount> ICarrierAccountRetriever.AccountsReadOnly =>
            AccountsReadOnly.OfType<ICarrierAccount>();
    }
}
