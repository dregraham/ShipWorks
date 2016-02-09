using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Carriers.BestRate.RateGroupFiltering;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// Rating service for the BestRate carrier
    /// </summary>
    public class BestRateRatingService : IRatingService, IBestRateBrokerRatingService
    {
        private readonly IIndex<ShipmentTypeCode, ShipmentType> shipmentTypeManager;
        private readonly IBestRateShippingBrokerFactory brokerFactory;
        private readonly IRateGroupFilterFactory filterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public BestRateRatingService(IIndex<ShipmentTypeCode, ShipmentType> shipmentTypeManager, IBestRateShippingBrokerFactory brokerFactory, IRateGroupFilterFactory filterFactory)
        {
            this.shipmentTypeManager = shipmentTypeManager;
            this.brokerFactory = brokerFactory;
            this.filterFactory = filterFactory;
        }

        /// <summary>
        /// Called to get the latest rates for the shipment. This implementation will accumulate the
        /// best shipping rate for all of the individual carrier-accounts within ShipWorks.
        /// </summary>
        /// <param name="shipment">Shipment for which to get rates</param>
        /// <returns>RateGroup of best rates for all carriers enabled for best rate</returns>
        public RateGroup GetRates(ShipmentEntity shipment)
        {
            try
            {
                BestRateShipmentType.AddBestRateEvent(shipment, BestRateEventTypes.RatesCompared);

                List<BrokerException> brokerExceptions = new List<BrokerException>();
                IEnumerable<RateGroup> rateGroups = GetRates(shipment, brokerExceptions);

                RateGroup rateGroup = CompileBestRates(shipment, rateGroups);

                // Get a list of distinct exceptions based on the message text ordered by the severity level (highest to lowest)
                IEnumerable<BrokerException> distinctExceptions = brokerExceptions
                    .Where(ex => ex != null)
                    // I got an exception because this was null. I wasn't able to reproduce. this is here just in case. I don't like it.
                    .OrderBy(ex => ex.SeverityLevel, new BrokerExceptionSeverityLevelComparer())
                    .GroupBy(e => e.Message + e.ShipmentType.ToString())
                    .Select(m => m.First()).ToList();

                if (distinctExceptions.Any())
                {
                    rateGroup.AddFootnoteFactory(new BrokerExceptionsRateFootnoteFactory(ShipmentTypeCode.BestRate, distinctExceptions));
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
        /// <param name="shipment">Shipment for which to get rates</param>
        /// <param name="exceptionHandler">Handler for exceptions generated while getting rates</param>
        /// <returns>IEnumerable of RateGroup for each carrier enabled for best rate</returns>
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
        /// Is the rate for the specified shipment
        /// </summary>
        public bool IsRateSelectedByShipment(RateResult rateResult, ICarrierShipmentAdapter shipmentAdapter)
        {
            throw new NotImplementedException("Best Rates is not yet supported");
        }

        /// <summary>
        /// Create a single, filtered rate group from a collection of rate groups
        /// </summary>
        public RateGroup CompileBestRates(ShipmentEntity shipment, IEnumerable<RateGroup> rateGroups)
        {
            RateGroup compiledRateGroup = new RateGroup(rateGroups.SelectMany(x => x.Rates));

            // Add the footnotes from all returned RateGroups into the new compiled RateGroup
            foreach (IRateFootnoteFactory footnoteFactory in rateGroups.SelectMany(x => x.FootnoteFactories))
            {
                compiledRateGroup.AddFootnoteFactory(footnoteFactory);
            }

            // Filter out any rates as necessary
            compiledRateGroup = filterFactory.CreateFilters(shipment)
                .Aggregate(compiledRateGroup, (current, rateGroupFilter) => rateGroupFilter.Filter(current));

            // Allow each rate result the chance to mask its description if needed based on the
            // other rate results in the list. This is for UPS that does not want its named-rates
            // intermingled with rates from other carriers
            compiledRateGroup.Rates.ForEach(x => x.MaskDescription(compiledRateGroup.Rates));
            compiledRateGroup.Carrier = ShipmentTypeCode.BestRate;

            return compiledRateGroup;
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