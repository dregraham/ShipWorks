﻿using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Net;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Rating service for FedEx
    /// </summary>
    public class FedExRatingService : IRatingService
    {
        static readonly ILog log = LogManager.GetLogger(typeof(FedExRatingService));

        private readonly FedExAccountRepository fedExAccountRepository;
        private readonly FedExShipmentType fedExShipmentType;
        private readonly IFedExShippingClerkFactory shippingClerkFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExRatingService(FedExAccountRepository fedExAccountRepository,
            FedExShipmentType fedExShipmentType,
            IFedExShippingClerkFactory shippingClerkFactory)
        {
            this.fedExAccountRepository = fedExAccountRepository;
            this.fedExShipmentType = fedExShipmentType;
            this.shippingClerkFactory = shippingClerkFactory;
        }

        /// <summary>
        /// Get a list of rates for the FedEx shipment
        /// </summary>
        public RateGroup GetRates(ShipmentEntity shipment)
        {
            string originalHubID = shipment.FedEx.SmartPostHubID;

            try
            {
                // if there are no FedEx accounts call get rates using counter rates
                if (!fedExAccountRepository.Accounts.Any() && !fedExShipmentType.IsShipmentTypeRestricted)
                {
                    CounterRatesOriginAddressValidator.EnsureValidAddress(shipment);
                    return shippingClerkFactory.CreateForCounterRates(shipment)
                        .GetRates(shipment, new CertificateInspector(TangoCredentialStore.Instance.FedExCertificateVerificationData));
                }

                // there must be at least one FedEx account so we get rates using it
                return shippingClerkFactory.Create(shipment).GetRates(shipment, new TrustingCertificateInspector());
            }
            catch (FedExException ex)
            {
                log.Error("Translating FedEx exception to Shipping Exception", ex);
                throw new ShippingException(ex.Message, ex);
            }
            catch (CounterRatesOriginAddressException)
            {
                RateGroup errorRates = new RateGroup(new List<RateResult>());
                errorRates.AddFootnoteFactory(new CounterRatesInvalidStoreAddressFootnoteFactory(ShipmentTypeCode.FedEx));
                return errorRates;
            }
            finally
            {
                // Switch the settings repository back to the original now that we have counter rates
                shipment.FedEx.SmartPostHubID = originalHubID;
            }
        }
    }
}
