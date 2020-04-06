using Autofac.Features.Indexed;
using Interapptive.Shared.Metrics;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.UPS.Promo.Api;
using System;

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
        readonly Func<string, ITrackedEvent> telemetryEventFunc;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsPromoFactory(
            IIndex<ShipmentTypeCode, ICarrierSettingsRepository> lookup,
            ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> accountRepository,
            IPromoClientFactory promoClientFactory,
            IUpsPromoPolicy upsPromoPolicy,
            Func<string, ITrackedEvent> telemetryEventFunc)
        {
            this.telemetryEventFunc = telemetryEventFunc;
            this.accountRepository = accountRepository;
            this.promoClientFactory = promoClientFactory;
            this.upsPromoPolicy = upsPromoPolicy;
            settingsRepository = lookup[ShipmentTypeCode.UpsOnLineTools];
        }

        /// <summary>
        /// Gets a UpsPromo
        /// </summary>
        public IUpsPromo Get(UpsAccountEntity account, UpsPromoSource source, UpsPromoAccountType accountType)
        {
            return new TelemetricUpsPromo(telemetryEventFunc, GetUpsPromo(account), source, accountType);
        }

        /// <summary>
        /// Get a UpsPromo
        /// </summary>
        private IUpsPromo GetUpsPromo(UpsAccountEntity account)
        {
            return new UpsPromo(account, settingsRepository, accountRepository, promoClientFactory, upsPromoPolicy);
        }
    }
}
