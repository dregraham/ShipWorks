using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.BestRate
{
    /// <summary>
    /// A repository for USPS counter rate accounts
    /// </summary>
    public class UspsCounterRateAccountRepository :
        ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>, ICarrierAccountRetriever
    {
        private readonly ICredentialStore credentialStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="UspsCounterRateAccountRepository"/> class.
        /// </summary>
        /// <param name="credentialStore">The credential store.</param>
        public UspsCounterRateAccountRepository(ICredentialStore credentialStore)
        {
            this.credentialStore = credentialStore;
        }

        /// <summary>
        /// Gets the accounts for the carrier.
        /// </summary>
        public IEnumerable<UspsAccountEntity> Accounts
        {
            get
            {
                List<UspsAccountEntity> accounts = new List<UspsAccountEntity>();
                try
                {
                    UspsAccountEntity account = new UspsAccountEntity()
                    {
                        Username = credentialStore.UspsUsername,
                        Password = credentialStore.UspsPassword,
                        PostalCode = "63102",
                        CountryCode = "US",
                        UspsAccountID = -1052,
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
        /// Gets the accounts for the carrier.
        /// </summary>
        public IEnumerable<IUspsAccountEntity> AccountsReadOnly => Accounts;

        /// <summary>
        /// Force a check for changes
        /// </summary>
        public void CheckForChangesNeeded()
        {
            // Since this is for counter rates, we don't need to worry about this
        }

        /// <summary>
        /// Returns a carrier counter rate account.
        /// </summary>
        /// <returns>Returns the first counter rate.</returns>
        public UspsAccountEntity GetAccount(long accountID) => Accounts.First();

        /// <summary>
        /// Returns a carrier counter rate account.
        /// </summary>
        public UspsAccountEntity GetAccount(IShipmentEntity shipment) => Accounts.First();

        /// <summary>
        /// Returns a carrier counter rate account.
        /// </summary>
        /// <returns>Returns the first counter rate.</returns>
        public IUspsAccountEntity GetAccountReadOnly(long accountID) => AccountsReadOnly.First();

        /// <summary>
        /// Returns a carrier account associated with the specified shipment
        /// </summary>
        public IUspsAccountEntity GetAccountReadOnly(IShipmentEntity shipment) => AccountsReadOnly.First();

        /// <summary>
        /// Gets the default profile account.
        /// </summary>
        public UspsAccountEntity DefaultProfileAccount
        {
            get { return Accounts.First(); }
        }

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        /// <param name="account">The account.</param>
        public void Save(UspsAccountEntity account)
        {
            // Nothing to save. This is a counter rate account.
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
