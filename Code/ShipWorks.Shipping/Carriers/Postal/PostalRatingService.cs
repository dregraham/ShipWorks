﻿using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Carriers.Postal.WebTools;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal
{
    public abstract class PostalRatingService : IRatingService
    {
        private readonly Func<ShipmentTypeCode, ShipmentType> shipmentTypeFactory;
        private readonly Func<ShipmentTypeCode, CarrierAccountRepositoryBase<EndiciaAccountEntity>> accountRepository;

        protected PostalRatingService(Func<ShipmentTypeCode, ShipmentType> shipmentTypeFactory, Func<ShipmentTypeCode, CarrierAccountRepositoryBase<EndiciaAccountEntity>> accountRepository)
        {
            this.shipmentTypeFactory = shipmentTypeFactory;
            this.accountRepository = accountRepository;
        }

        public abstract RateGroup GetRates(ShipmentEntity shipment);

        /// <summary>
        /// Gets counter rates for a postal shipment
        /// </summary>
        /// <param name="shipment">Shipment for which to retrieve rates</param>
        protected virtual RateGroup GetCounterRates(ShipmentEntity shipment)
        {
            try
            {
                CounterRatesOriginAddressValidator.EnsureValidAddress(shipment);
            }
            catch (CounterRatesOriginAddressException)
            {
                RateGroup errorRates = new RateGroup(new List<RateResult>());
                errorRates.AddFootnoteFactory(new CounterRatesInvalidStoreAddressFootnoteFactory(shipmentTypeFactory((ShipmentTypeCode)shipment.ShipmentType)));
                return errorRates;
            }

            RateGroup rates = new RateGroup(new List<RateResult>());

            if (!shipmentTypeFactory((ShipmentTypeCode)shipment.ShipmentType).IsShipmentTypeRestricted)
            {
                // Only get counter rates if the shipment type has not been restricted
                rates = new PostalWebShipmentType().GetRates(shipment);
                rates.Rates.ForEach(x =>
                {
                    if (x.ProviderLogo != null)
                    {
                        // Only change existing logos; don't set logos for rates that don't have them
                        x.ProviderLogo = EnumHelper.GetImage((ShipmentTypeCode)shipment.ShipmentType);
                    }
                });
            }

            return rates;
        }

        /// <summary>
        /// Gets the filtered rates based on any excluded services configured for this postal shipment type.
        /// </summary>
        protected virtual List<RateResult> FilterRatesByExcludedServices(ShipmentEntity shipment, List<RateResult> rates)
        {
            List<PostalServiceType> availableServiceTypes = shipmentTypeFactory((ShipmentTypeCode)shipment.ShipmentType).GetAvailableServiceTypes().Select(s => (PostalServiceType)s).Union(new List<PostalServiceType> { (PostalServiceType)shipment.Postal.Service }).ToList();
            return rates.Where(r => r.Tag is PostalRateSelection && availableServiceTypes.Contains(((PostalRateSelection)r.Tag).ServiceType)).ToList();
        }
    }
}