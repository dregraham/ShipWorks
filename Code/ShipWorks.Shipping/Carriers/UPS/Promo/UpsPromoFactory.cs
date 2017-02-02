using Autofac.Features.Indexed;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.UPS.Promo.Api;

namespace ShipWorks.Shipping.Carriers.UPS.Promo
{
    /// <summary>
    /// UpsPromoFactory
    /// </summary>
    public class UpsPromoFactory : IUpsPromoFactory
    {
        private readonly ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> accountRepository;
        private readonly IPromoClientFactory promoClientFactory;
        private readonly IUpsPromoPolicy upsPromoPolicy;
        readonly ICarrierSettingsRepository settingsRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsPromoFactory(IIndex<ShipmentTypeCode, ICarrierSettingsRepository> lookup, ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> accountRepository, IPromoClientFactory promoClientFactory, IUpsPromoPolicy upsPromoPolicy)
        {
            this.accountRepository = accountRepository;
            this.promoClientFactory = promoClientFactory;
            this.upsPromoPolicy = upsPromoPolicy;
            settingsRepository = lookup[ShipmentTypeCode.UpsOnLineTools];
        }

        /// <summary>
        /// Gets a UpsPromo
        /// </summary>
        public UpsPromo Get(UpsAccountEntity account)
        {
            return new UpsPromo(account, settingsRepository, accountRepository, promoClientFactory, upsPromoPolicy);
        }
    }
}
