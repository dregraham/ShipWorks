﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac.Features.Indexed;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.Contracts;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.RateFootnotes.Promotion;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    public class Express1UspsRatingService : UspsRatingService
    {
        public Express1UspsRatingService(ICachedRatesService cachedRatesService,
            IIndex<ShipmentTypeCode, ShipmentType> shipmentTypeFactory, 
            Express1UspsAccountRepository accountRepository,
            IIndex<ShipmentTypeCode, IRatingService> ratingServiceFactory) : 
            base(cachedRatesService, ratingServiceFactory, shipmentTypeFactory, accountRepository)
        {
        }

        /// <summary>
        /// Get postal rates for the given shipment
        /// </summary>
        /// <param name="shipment">Shipment for which to retrieve rates</param>
        protected override RateGroup GetRatesInternal(ShipmentEntity shipment)
        {
            // Overridden here otherwise relying on the UspsShipmentType to get rates
            // would result in infinite recursion when using auto-routing since the UspsShipmentType 
            // is just calling GetRatesInternal on an Express1UspsShipmentType which then creates a new
            // Express1UspsShipmentType and gets rates, and on and on...

            return GetRatesFromApi(shipment);
        }

        /// <summary>
        /// Gets the rates from the Exprss1 API.
        /// </summary>
        private RateGroup GetRatesFromApi(ShipmentEntity shipment)
        {
            List<RateResult> rateResults = CreateWebClient().GetRates(shipment);
            RateGroup rateGroup = new RateGroup(FilterRatesByExcludedServices(shipment, rateResults));

            if (UspsAccountManager.UspsAccounts.All(a => a.ContractType != (int)UspsAccountContractType.Reseller))
            {
                rateGroup.AddFootnoteFactory(new UspsRatePromotionFootnoteFactory(shipmentTypeFactory[ShipmentTypeCode.Express1Usps], shipment, true));
            }

            return rateGroup;
        }

        /// <summary>
        /// Gets counter rates for a postal shipment
        /// </summary>
        /// <param name="shipment">Shipment for which to retrieve rates</param>
        protected override RateGroup GetCounterRates(ShipmentEntity shipment)
        {
            throw new ShippingException("An account is required to view Express1 rates.");
        }

        /// <summary>
        /// Returns web client
        /// </summary>
        protected override IUspsWebClient CreateWebClient()
        {
            return new Express1UspsWebClient(accountRepository, new LogEntryFactory(), CertificateInspector());
        }

        /// <summary>
        /// Gets the filtered rates based on any excluded services configured for this postal shipment type.
        /// </summary>
        protected override List<RateResult> FilterRatesByExcludedServices(ShipmentEntity shipment, List<RateResult> rates)
        {
            List<PostalServiceType> availableServiceTypes = shipmentTypeFactory[ShipmentTypeCode.Express1Usps].GetAvailableServiceTypes().Select(s => (PostalServiceType)s).ToList(); ;

            if (shipment.ShipmentType == (int) ShipmentTypeCode.Express1Usps)
            {
                availableServiceTypes.Add((PostalServiceType)shipment.Postal.Service);
            }

            List<RateResult> rateResults = rates.Where(r => r.Tag is PostalRateSelection && availableServiceTypes.Contains(((PostalRateSelection)r.Tag).ServiceType)).ToList();

            rateResults.ForEach(r => r.ShipmentType = ShipmentTypeCode.Express1Usps);

            return rateResults;
        }
    }
}
