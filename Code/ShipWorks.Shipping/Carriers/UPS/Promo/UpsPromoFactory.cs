using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac.Features.Indexed;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.UPS.Promo.API;

namespace ShipWorks.Shipping.Carriers.UPS.Promo
{
    /// <summary>
    /// UpsPromoFactory
    /// </summary>
    public class UpsPromoFactory : IUpsPromoFactory
    {
        private readonly ICarrierAccountRepository<UpsAccountEntity> accountRepository;
        private readonly IPromoClientFactory promoClientFactory;
        readonly ICarrierSettingsRepository settingsRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsPromoFactory(IIndex<ShipmentTypeCode, ICarrierSettingsRepository> lookup, ICarrierAccountRepository<UpsAccountEntity> accountRepository, IPromoClientFactory promoClientFactory)
        {
            this.accountRepository = accountRepository;
            this.promoClientFactory = promoClientFactory;
            settingsRepository = lookup[ShipmentTypeCode.UpsOnLineTools];
        }

        /// <summary>
        /// Gets a UpsPromo
        /// </summary>
        public UpsPromo Get(UpsAccountEntity account)
        {
            return new UpsPromo(account, settingsRepository, accountRepository, promoClientFactory);
        }
    }
}
