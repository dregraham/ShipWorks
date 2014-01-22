using System;
using System.Collections.Generic;
using System.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Shipping.Carriers.UPS.BestRate
{
    /// <summary>
    /// A repository for UPS counter rate accounts
    /// </summary>
    public class UpsCounterRateAccountRepository : ICarrierAccountRepository<UpsAccountEntity>
    {
        private readonly ICounterRatesCredentialStore counterRatesCredentialStore;
        private readonly Lazy<List<UpsAccountEntity>> lazyAccounts;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="counterRatesCredentialStore"></param>
        public UpsCounterRateAccountRepository(ICounterRatesCredentialStore counterRatesCredentialStore)
        {
            this.counterRatesCredentialStore = counterRatesCredentialStore;

            lazyAccounts = new Lazy<List<UpsAccountEntity>>(ConvertTangoCredentialsToFedExAccountEntities);
        }

        /// <summary>
        /// Gets the accounts for the carrier.
        /// </summary>
        public IEnumerable<UpsAccountEntity> Accounts
        {
            get
            {
                return lazyAccounts.Value;
            }
        }

        /// <summary>
        /// Returns a carrier counter rate account.
        /// </summary>
        /// <returns>Returns the first counter rate.</returns>
        public UpsAccountEntity GetAccount(long accountID)
        {
            return Accounts.First();
        }

        /// <summary>
        /// Gets the FedEx account that should be used for counter rates.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A FedExAccountEntity object.</returns>
        public IEntity2 GetAccount(ShipmentEntity shipment)
        {
            return Accounts.First();
        }

        /// <summary>
        /// Converts Tango Ups credentials into a FedExAccountEntity
        /// </summary>
        private List<UpsAccountEntity> ConvertTangoCredentialsToFedExAccountEntities()
        {
            UpsAccountEntity fedExAccountEntity = new UpsAccountEntity
            {
                UserID = counterRatesCredentialStore.UpsUserId,
                Password = counterRatesCredentialStore.UpsPassword,
                PostalCode = "63102",
                CountryCode = "US",
                RateType = (int)UpsRateType.Retail
            };

            return new List<UpsAccountEntity> { fedExAccountEntity };
        }
    }
}
