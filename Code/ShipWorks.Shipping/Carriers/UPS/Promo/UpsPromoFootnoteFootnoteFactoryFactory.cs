using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac.Features.Indexed;
using ShipWorks.Data.Administration.Indexing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.UPS.Promo.API;
using ShipWorks.Shipping.Carriers.UPS.Promo.RateFootnotes;

namespace ShipWorks.Shipping.Carriers.UPS.Promo
{
    public class UpsPromoFootnoteFootnoteFactoryFactory : IUpsPromoFootnoteFactoryFactory
    {
        private readonly IUpsPromoPolicy promoPolicy;
        private readonly ICarrierAccountRepository<UpsAccountEntity> upsAccountRepository;
        private readonly IUpsPromoFactory promoFactory;

        public UpsPromoFootnoteFootnoteFactoryFactory(IUpsPromoPolicy promoPolicy, ICarrierAccountRepository<UpsAccountEntity> upsAccountRepository, IUpsPromoFactory promoFactory)
        {
            this.promoPolicy = promoPolicy;
            this.upsAccountRepository = upsAccountRepository;
            this.promoFactory = promoFactory;
        }

        /// <summary>
        /// Returns a UpsPromoFootnoteFactory if applicable - else null
        /// </summary>
        public UpsPromoFootnoteFactory Get(ShipmentEntity shipment)
        {
            UpsAccountEntity account = upsAccountRepository.GetAccount(shipment.Ups.UpsAccountID);
            UpsPromo upsPromo = promoFactory.Get(account);

            if (!promoPolicy.IsEligible(upsPromo))
            {
                return null;
            }

            // Create promofootnote factory
            UpsPromoFootnoteFactory promoFootNoteFactory = new UpsPromoFootnoteFactory(upsPromo);

            // Add factgory too the final group rate group
            return promoFootNoteFactory;
        }
    }
}
