using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.UPS.Promo.Api
{
    /// <summary>
    /// An implementation of the IPromoClientFactory that creates the "live" implementation
    /// of the IUpsApiPromoClient.
    /// </summary>
    public class UpsPromoWebClientFactory : IPromoClientFactory
    {
        private readonly ILogEntryFactory logEntryFactory;
        private readonly ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> accountRepository;
        private readonly IUpsUtility upsUtility;
        private readonly IEncryptionProviderFactory encryptionProviderFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsPromoWebClientFactory"/> class.
        /// </summary>
        public UpsPromoWebClientFactory(ILogEntryFactory logEntryFactory, ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> accountRepository, IUpsUtility upsUtility, IEncryptionProviderFactory encryptionProviderFactory)
        {
            this.logEntryFactory = logEntryFactory;
            this.accountRepository = accountRepository;
            this.upsUtility = upsUtility;
            this.encryptionProviderFactory = encryptionProviderFactory;
        }

        /// <summary>
        /// Uses the UpsPromo provided to create a web client for communicating with 
        /// the UPS Promo API.
        /// </summary>
        /// <param name="promo">The UPS promo that the client is being created for.</param>
        /// <returns>A concrete UpsApiPromoClient instance.</returns>
        public IUpsApiPromoClient CreatePromoClient(UpsPromo promo)
        {
            return new UpsApiPromoClient(promo, logEntryFactory, accountRepository, upsUtility, encryptionProviderFactory);
        }
    }
}
