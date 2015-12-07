using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    public class FedExRatingService : IRatingService
    {
        private readonly FedExAccountRepository fedExAccountRepository;
        private readonly FedExShipmentType fedExShipmentType;
        private readonly FedExShippingClerkFactory shippingClerkFactory;

        public FedExRatingService(FedExAccountRepository fedExAccountRepository,
            ICachedRatesService cachedRatesService, 
            FedExShipmentType fedExShipmentType,
            FedExShippingClerkFactory shippingClerkFactory)
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
                    return shippingClerkFactory.CreateShippingClerk(shipment, true).GetRates(shipment);
                }

                // there must be at least one FedEx account so we get rates using it
                return shippingClerkFactory.CreateShippingClerk(shipment, false).GetRates(shipment);
            }
            catch (CounterRatesOriginAddressException)
            {
                RateGroup errorRates = new RateGroup(new List<RateResult>());
                errorRates.AddFootnoteFactory(new CounterRatesInvalidStoreAddressFootnoteFactory(fedExShipmentType));
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
