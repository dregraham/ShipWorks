using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    public class BestRateRatingService : IRatingService, IBestRateBrokerRatingService
    {
        private readonly BestRateShipmentType bestRateShipmentType;
        private readonly IBestRateShippingBrokerFactory brokerFactory;

        public BestRateRatingService(BestRateShipmentType bestRateShipmentType, IBestRateShippingBrokerFactory brokerFactory)
        {
            this.bestRateShipmentType = bestRateShipmentType;
            this.brokerFactory = brokerFactory;
        }

        /// <summary>
        /// Called to get the latest rates for the shipment. This implementation will accumulate the
        /// best shipping rate for all of the individual carrier-accounts within ShipWorks.
        /// </summary>
        public RateGroup GetRates(ShipmentEntity shipment)
        {
            try
            {
                BestRateShipmentType.AddBestRateEvent(shipment, BestRateEventTypes.RatesCompared);

                List<BrokerException> brokerExceptions = new List<BrokerException>();
                IEnumerable<RateGroup> rateGroups = GetRates(shipment, brokerExceptions);

                RateGroup rateGroup = bestRateShipmentType.CompileBestRates(shipment, rateGroups);

                // Get a list of distinct exceptions based on the message text ordered by the severity level (highest to lowest)
                IEnumerable<BrokerException> distinctExceptions = brokerExceptions
                    .Where(ex => ex != null)
                    // I got an exception because this was null. I wasn't able to reproduce. this is here just in case. I don't like it.
                    .OrderBy(ex => ex.SeverityLevel, new BrokerExceptionSeverityLevelComparer())
                    .GroupBy(e => e.Message + e.ShipmentType.ToString())
                    .Select(m => m.First()).ToList();

                if (distinctExceptions.Any())
                {
                    rateGroup.AddFootnoteFactory(new BrokerExceptionsRateFootnoteFactory(bestRateShipmentType, distinctExceptions));
                }

                return rateGroup;
            }
            catch (BestRateException ex)
            {
                // A problem occurred that is germane to the BestRateShipmentType (and not within any
                // brokers or shipment types); this is most likely there aren't any providers/accounts
                // setup to use with best rate, so we'll just return a rate group communicating the
                // problem to the user
                return new InvalidRateGroup(ShipmentTypeCode.BestRate, ex);
            }
        }

        /// <summary>
        /// Called to get the latest rates for the shipment. This implementation will accumulate the
        /// best shipping rate for all of the individual carrier-accounts within ShipWorks.
        /// </summary>
        public IEnumerable<RateGroup> GetRates(ShipmentEntity shipment, List<BrokerException> exceptionHandler)
        {
            List<IBestRateShippingBroker> bestRateShippingBrokers = brokerFactory.CreateBrokers(shipment, true).ToList();

            if (!bestRateShippingBrokers.Any())
            {
                string message =
                    $"No accounts are configured to use with best rate.{Environment.NewLine}Check the shipping settings to ensure " +
                    "your shipping accounts have been setup for the shipping providers being used with best rate.";

                throw new BestRateException(message);
            }

            // Start getting rates from each enabled carrier
            List<Task<RateGroup>> tasks = bestRateShippingBrokers
                .Select(broker => StartGetRatesTask(broker, shipment, exceptionHandler))
                .ToList();

            tasks.ForEach(t => t.Wait());

            return tasks.Select(x => x.Result);
        }


        /// <summary>
        /// Starts getting rates for a broker
        /// </summary>
        /// <param name="broker">Broker for which to start getting rates</param>
        /// <param name="shipment">Shipment for which to get rates</param>
        /// <param name="brokerExceptions">Handler for exceptions generated while getting rates</param>
        /// <returns>A task that will contain the results</returns>
        private static Task<RateGroup> StartGetRatesTask(IBestRateShippingBroker broker, ShipmentEntity shipment, List<BrokerException> brokerExceptions)
        {
            return Task<RateGroup>.Factory.StartNew(() => broker.GetBestRates(shipment, brokerExceptions));
        }
    }
}