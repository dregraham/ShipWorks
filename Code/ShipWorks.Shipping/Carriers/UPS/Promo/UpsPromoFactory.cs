using Autofac.Features.Indexed;
using Interapptive.Shared.Metrics;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.UPS.Promo.Api;
using ShipWorks.Shipping.Carriers.UPS.Promo.RateFootnotes;
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
        readonly ITrackedEvent telemetryEvent;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsPromoFactory(
            IIndex<ShipmentTypeCode, ICarrierSettingsRepository> lookup,
            ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> accountRepository,
            IPromoClientFactory promoClientFactory,
            IUpsPromoPolicy upsPromoPolicy,
            Func<string, ITrackedEvent> telemetryEvent)
        {
            this.telemetryEvent = telemetryEvent("Ups.Promo");
            this.accountRepository = accountRepository;
            this.promoClientFactory = promoClientFactory;
            this.upsPromoPolicy = upsPromoPolicy;
            settingsRepository = lookup[ShipmentTypeCode.UpsOnLineTools];
        }

        /// <summary>
        /// Gets a UpsPromo
        /// </summary>
        public IUpsPromo Get(UpsAccountEntity account)
        {
            return new TelemetricUpsPromo(telemetryEvent, GetUpsPromo(account));
        }

        /// <summary>
        /// Gets the footnote factory.
        /// </summary>
        public UpsPromoFootnoteFactory GetFootnoteFactory(UpsAccountEntity account)
        {
            IUpsPromo promo = GetUpsPromo(account);

            if (upsPromoPolicy.IsEligible(promo))
            {
                IUpsPromo telemetricPromo = new TelemetricUpsPromo(telemetryEvent, promo);

                // Create promo footnote factory
                UpsPromoFootnoteFactory promoFootNoteFactory = new UpsPromoFootnoteFactory(telemetricPromo, account);

                // Add factory to the final group rate group
                return promoFootNoteFactory;
            }

            return null;
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
