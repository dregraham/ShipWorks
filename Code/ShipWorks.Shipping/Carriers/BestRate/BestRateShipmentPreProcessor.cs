using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// Preprocessor for best rate shipments
    /// </summary>
    [KeyedComponent(typeof(IShipmentPreProcessor), ShipmentTypeCode.BestRate)]
    public class BestRateShipmentPreProcessor : IShipmentPreProcessor
    {
        private readonly IBestRateBrokerRatingService brokerRatingService;
        private readonly IShippingManager shippingManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public BestRateShipmentPreProcessor(IShippingManager shippingManager,
            IBestRateBrokerRatingService brokerRatingService)
        {
            this.shippingManager = shippingManager;
            this.brokerRatingService = brokerRatingService;
        }

        /// <summary>
        /// Uses the synchronizer to check whether an account exists and call the counterRatesProcessing callback
        /// provided when trying to process a shipment without any accounts for this shipment type in ShipWorks,
        /// otherwise the shipment is unchanged.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="selectedRate">The selected rate.</param>
        /// <param name="configurationCallback">Callback to execute when a shipment type is configured.</param>
        /// <returns></returns>
        /// <exception cref="ShippingException">An account must be created to process this shipment.</exception>
        public virtual IEnumerable<ShipmentEntity> Run(ShipmentEntity shipment, RateResult selectedRate, Action configurationCallback)
        {
            BestRateShipmentType.AddBestRateEvent(shipment, BestRateEventTypes.RateAutoSelectedAndProcessed);

            shippingManager.EnsureShipmentLoaded(shipment);
            IEnumerable<RateGroup> rateGroups = GetRatesForPreProcessing(shipment);

            // We want all the rates here, so we can pass them back to the counter rate processing if needed
            RateGroup filteredRates = brokerRatingService.CompileBestRates(shipment, rateGroups);
            if (!filteredRates.Rates.Any())
            {
                throw new ShippingException("ShipWorks could not find any rates.");
            }

            List<RateResult> ratesToApplyToReturnedShipments = new List<RateResult>();

            if (selectedRate != null)
            {
                ratesToApplyToReturnedShipments = HandleSelectedRate(selectedRate, filteredRates);
                if (ratesToApplyToReturnedShipments == null)
                {
                    // This would mean the user canceled; stop processing
                    return null;
                }
            }

            if (!ratesToApplyToReturnedShipments.Any())
            {
                // A rate was not selected in the grid, so we need to treat this as if the user selected
                // the first rate in the list (the best rate); this case could occur if multiple shipments
                // are being processed in a batch or if the rates have not been fetched and displayed in
                // the grid yet.
                RateResult bestRate = filteredRates.Rates.FirstOrDefault();

                // The cheapest rate is not a counter rate, so compile a list of the possible
                // rates to apply to the shipment during processing. This is basically a fail
                // over mechanism in case the processing with the first rate fails
                ratesToApplyToReturnedShipments = rateGroups
                    .SelectMany(x => x.Rates)
                    .Where(r => r.AmountOrDefault == bestRate.AmountOrDefault)
                    .ToList();
            }

            List<ShipmentEntity> shipmentsToReturn = new List<ShipmentEntity>();
            foreach (RateResult rateToApply in ratesToApplyToReturnedShipments)
            {
                // Apply the selected rate to the shipment, so it's configured
                // for processing
                BestRateShipmentType.ApplySelectedShipmentRate(shipment, rateToApply);
                shipmentsToReturn.Add(shipment);
            }

            return shipmentsToReturn;
        }

        /// <summary>
        /// Get a list of rates for preprocessing
        /// </summary>
        private IEnumerable<RateGroup> GetRatesForPreProcessing(ShipmentEntity shipment)
        {
            try
            {
                // Important to get rates here again because this will ensure that the rates
                // are current with the configuration of the shipment; this will come into
                // play when comparing the selected rate with the rates in the rate groups
                return brokerRatingService.GetRates(shipment, new List<BrokerException>());
            }
            catch (AggregateException ex)
            {
                // Inspect the aggregate exception for the details of the first underlying exception
                if (ex.InnerException is AggregateException)
                {
                    // The inner exception is also an aggregate exception (in the case that a multi-threaded
                    // broker also threw an aggregate exception), so dive into the details of it to grab the
                    // first meaningful exception
                    AggregateException innerAggregate = ex.InnerException as AggregateException;
                    throw innerAggregate.InnerExceptions.First();
                }

                // The inner exception is not an aggregate exception, so we can just throw the
                // first inner exception
                throw ex.InnerExceptions.First();
            }
        }

        /// <summary>
        /// Handles the selected rate.
        /// </summary>
        /// <param name="selectedRate">The selected rate.</param>
        /// <param name="filteredRates">The filtered rates.</param>
        /// <returns></returns>
        /// <exception cref="ShippingException">The rate that was selected is out of date or could not be found. Please select another rate.</exception>
        private List<RateResult> HandleSelectedRate(RateResult selectedRate, RateGroup filteredRates)
        {
            // We want to try to process with the selected rate that was provided. Build
            // up our list of fail over candidates in case the processing the shipment with
            // the first rate fails
            List<RateResult> ratesToApplyToReturnedShipments = filteredRates.Rates
                .Where(r => r.AmountOrDefault == selectedRate.AmountOrDefault &&
                    r.OriginalTag != null &&
                    selectedRate.OriginalTag != null &&
                    r.OriginalTag.Equals(selectedRate.OriginalTag))
                .ToList();

            // The rate was not found
            if (!ratesToApplyToReturnedShipments.Any())
            {
                throw new ShippingException("The rate that was selected is out of date or could not be found. Please select another rate.");
            }

            return ratesToApplyToReturnedShipments;
        }
    }
}
