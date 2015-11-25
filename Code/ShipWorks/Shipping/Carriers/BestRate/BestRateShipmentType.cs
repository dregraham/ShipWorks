using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.Business;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Filters.Content.Conditions.Shipments;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Carriers.BestRate.RateGroupFiltering;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Shipping.ShipSense.Packaging;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using Autofac;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// Best rate implementation of ShipmentType
    /// </summary>
    public class BestRateShipmentType : ShipmentType
    {
        private readonly ILog log;
        private readonly IBestRateShippingBrokerFactory brokerFactory;
        private readonly IRateGroupFilterFactory filterFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="BestRateShipmentType"/> class. This
        /// version of the constructor will use the "live" implementation of the
        /// IBestRateShippingBrokerFactory interface.
        /// </summary>
        public BestRateShipmentType()
            : this(new BestRateShippingBrokerFactory(), new BestRateFilterFactory(), LogManager.GetLogger(typeof(BestRateShipmentType)))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BestRateShipmentType" /> class. This version of
        /// the constructor is primarily for testing purposes.
        /// </summary>
        /// <param name="brokerFactory">The broker factory.</param>
        /// <param name="filterFactory">The filter factory.</param>
        /// <param name="log">The log.</param>
        public BestRateShipmentType(IBestRateShippingBrokerFactory brokerFactory, IRateGroupFilterFactory filterFactory, ILog log)
        {
            this.brokerFactory = brokerFactory;
            this.filterFactory = filterFactory;
            this.log = log;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BestRateShipmentType"/> class.
        /// </summary>
        public BestRateShipmentType(BestRateShippingBrokerFactory bestRateShippingBrokerFactory)
            : this(bestRateShippingBrokerFactory, new BestRateFilterFactory(), LogManager.GetLogger(typeof(BestRateShipmentType)))
        { }

        /// <summary>
        /// The ShipmentTypeCode represented by this ShipmentType
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode
        {
            get { return ShipmentTypeCode.BestRate; }
        }

        /// <summary>
        /// Indicates if the shipment service type supports getting rates
        /// </summary>
        public override bool SupportsGetRates
        {
            get { return true; }
        }

        /// <summary>
        /// Indicates that this shipment type supports shipping from an account address
        /// </summary>
        public override bool SupportsAccountAsOrigin
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Apply the specified shipment profile to the given shipment.
        /// </summary>
        public override void ApplyProfile(ShipmentEntity shipment, ShippingProfileEntity profile)
        {
            base.ApplyProfile(shipment, profile);

            BestRateShipmentEntity bestRateShipment = shipment.BestRate;
            BestRateProfileEntity bestRateProfile = profile.BestRate;

            ShippingProfileUtility.ApplyProfileValue(bestRateProfile.DimsProfileID, bestRateShipment, BestRateShipmentFields.DimsProfileID);
            ShippingProfileUtility.ApplyProfileValue(bestRateProfile.DimsWeight, bestRateShipment, BestRateShipmentFields.DimsWeight);
            ShippingProfileUtility.ApplyProfileValue(bestRateProfile.DimsLength, bestRateShipment, BestRateShipmentFields.DimsLength);
            ShippingProfileUtility.ApplyProfileValue(bestRateProfile.DimsHeight, bestRateShipment, BestRateShipmentFields.DimsHeight);
            ShippingProfileUtility.ApplyProfileValue(bestRateProfile.DimsWidth, bestRateShipment, BestRateShipmentFields.DimsWidth);
            ShippingProfileUtility.ApplyProfileValue(bestRateProfile.DimsAddWeight, bestRateShipment, BestRateShipmentFields.DimsAddWeight);

            ShippingProfileUtility.ApplyProfileValue(bestRateProfile.ServiceLevel, bestRateShipment, BestRateShipmentFields.ServiceLevel);

            if (bestRateProfile.Weight.HasValue && bestRateProfile.Weight.Value != 0)
            {
                ShippingProfileUtility.ApplyProfileValue(bestRateProfile.Weight, shipment, ShipmentFields.ContentWeight);
            }
        }

        /// <summary>
        /// Update the origin address based on the given originID value.  If the shipment has already been processed, nothing is done.  If
        /// the originID is no longer valid and the address could not be updated, false is returned.
        /// </summary>
        public override bool UpdatePersonAddress(ShipmentEntity shipment, PersonAdapter person, long originID)
        {
            if (shipment.Processed)
            {
                return true;
            }

            if (originID == (int)ShipmentOriginSource.Account)
            {
                // Copy an empty person since the account address used will depend on each carrier
                PersonAdapter.Copy(new PersonAdapter { OriginID = (int)ShipmentOriginSource.Account }, person);
                return true;
            }

            return base.UpdatePersonAddress(shipment, person, originID);
        }

        /// <summary>
        /// Create the UserControl used to handle best rate shipments
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        protected override ServiceControlBase InternalCreateServiceControl(RateControl rateControl)
        {
            return new BestRateServiceControl(ShipmentTypeCode, rateControl);
        }

        /// <summary>
        /// Create the UserControl that is used to edit a profile for the service
        /// </summary>
        protected override ShippingProfileControlBase CreateProfileControl()
        {
            return new BestRateProfileControl();
        }

        /// <summary>
        /// Gets the package adapter for the shipment.
        /// </summary>
        public override IEnumerable<IPackageAdapter> GetPackageAdapters(ShipmentEntity shipment)
        {
            if (shipment.BestRate == null)
            {
                ShippingManager.EnsureShipmentLoaded(shipment);
            }

            return new List<IPackageAdapter>()
            {
                new BestRatePackageAdapter(shipment)
            };
        }

        /// <summary>
        /// Allows bases classes to apply the default settings to the given profile
        /// </summary>
        protected override void ConfigurePrimaryProfile(ShippingProfileEntity profile)
        {
            log.Warn("ConfigurePrimaryProfile called for BestRateShipmentType.");
            Debug.Assert(false, "ConfigurePrimaryProfile maybe shouldn't be called for BestRateShipmentType.");
        }

        /// <summary>
        /// Ensures that the carrier specific data for the shipment, such as the FedEx data, are loaded for the shipment.  If the data
        /// already exists, nothing is done: it is not refreshed.  This method can throw SqlForeignKeyException if the root shipment
        /// or order has been deleted, ORMConcurrencyException if the shipment had been edited elsewhere, and ObjectDeletedException if the shipment
        /// had been deleted.
        /// </summary>
        public override void LoadShipmentData(ShipmentEntity shipment, bool refreshIfPresent)
        {
            ShipmentTypeDataService.LoadShipmentData(this, shipment, shipment, "BestRate", typeof(BestRateShipmentEntity), refreshIfPresent);
        }

        /// <summary>
        /// Get the carrier specific description of the shipping service used. The carrier specific data must already exist
        /// when this method is called.
        /// </summary>
        public override string GetServiceDescription(ShipmentEntity shipment)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get the insurance data that describes what type of insurance is being used and on what parcels.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">shipment</exception>
        public override ShipmentParcel GetParcelDetail(ShipmentEntity shipment, int parcelIndex)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            return new ShipmentParcel(shipment, null,
                new InsuranceChoice(shipment, shipment, shipment.BestRate, null),
                new DimensionsAdapter(shipment.BestRate));
        }

        /// <summary>
        /// Called to get the latest rates for the shipment. This implementation will accumulate the
        /// best shipping rate for all of the individual carrier-accounts within ShipWorks.
        /// </summary>
        public override RateGroup GetRates(ShipmentEntity shipment)
        {
            try
            {
                AddBestRateEvent(shipment, BestRateEventTypes.RatesCompared);

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
                    rateGroup.AddFootnoteFactory(new BrokerExceptionsRateFootnoteFactory(this, distinctExceptions));
                }

                return rateGroup;
            }
            catch (BestRateException ex)
            {
                // A problem occurred that is germane to the BestRateShipmentType (and not within any
                // brokers or shipment types); this is most likely there aren't any providers/accounts
                // setup to use with best rate, so we'll just return a rate group communicating the
                // problem to the user
                return new InvalidRateGroup(this, ex);
            }
        }

        /// <summary>
        /// Called to get the latest rates for the shipment. This implementation will accumulate the
        /// best shipping rate for all of the individual carrier-accounts within ShipWorks.
        /// </summary>
        private IEnumerable<RateGroup> GetRates(ShipmentEntity shipment, List<BrokerException> exceptionHandler)
        {
            List<IBestRateShippingBroker> bestRateShippingBrokers = brokerFactory.CreateBrokers(shipment, true).ToList();

            if (!bestRateShippingBrokers.Any())
            {
                string message = string.Format("No accounts are configured to use with best rate.{0}Check the shipping settings to ensure " +
                                               "your shipping accounts have been setup for the shipping providers being used with best rate.", Environment.NewLine);

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
        /// Create a single, filtered rate group from a collection of rate groups
        /// </summary>
        private RateGroup CompileBestRates(ShipmentEntity shipment, IEnumerable<RateGroup> rateGroups)
        {
            RateGroup compiledRateGroup = new RateGroup(rateGroups.SelectMany(x => x.Rates));

            // Add the footnotes from all returned RateGroups into the new compiled RateGroup
            foreach (IRateFootnoteFactory footnoteFactory in rateGroups.SelectMany(x => x.FootnoteFactories))
            {
                compiledRateGroup.AddFootnoteFactory(footnoteFactory);
            }

            // Filter out any rates as necessary
            foreach (IRateGroupFilter rateGroupFilter in filterFactory.CreateFilters(shipment))
            {
                compiledRateGroup = rateGroupFilter.Filter(compiledRateGroup);
            }

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
            return Task<RateGroup>.Factory.StartNew(() =>
                {
                    return broker.GetBestRates(shipment, brokerExceptions);
                });
        }

        /// <summary>
        /// Ensures that the carrier specific data for the given profile exists and is loaded
        /// </summary>
        public override void LoadProfileData(ShippingProfileEntity profile, bool refreshIfPresent)
        {
            ShipmentTypeDataService.LoadProfileData(profile, "BestRate", typeof(BestRateProfileEntity), refreshIfPresent);
        }

        /// <summary>
        /// Process the shipment
        /// </summary>
        public override void ProcessShipment(ShipmentEntity shipment)
        {
            // This is by design. The best rate shipment type should never actually
            // process a shipment due to the pre-process functionality
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Gets whether the specified settings tab should be hidden in the UI
        /// </summary>
        public override bool IsSettingsTabHidden(ShipmentTypeSettingsControl.Page tab)
        {
            return tab == ShipmentTypeSettingsControl.Page.Actions || tab == ShipmentTypeSettingsControl.Page.Printing;
        }

        /// <summary>
        /// Gets an instance to the best rate shipping broker for a provider based on the shipment configuration.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>An instance of a NullShippingBroker since this is not applicable to the best rate shipment type.</returns>
        public override IBestRateShippingBroker GetShippingBroker(ShipmentEntity shipment)
        {
            return new NullShippingBroker();
        }

        /// <summary>
        /// Creates the UserControl that is used to edit the defaults\settings for the service
        /// </summary>
        protected override SettingsControlBase CreateSettingsControl()
        {
            return new BestRateSettingsControl();
        }

        /// <summary>
        /// Gets the processing synchronizer to be used during the PreProcessing of a shipment.
        /// </summary>
        protected override IShipmentProcessingSynchronizer GetProcessingSynchronizer()
        {
            // The synchronizer isn't used in overridden PreProcess method for this shipment type
            return null;
        }

        /// <summary>
        /// Gets rates and converts shipment to the found best rate type.
        /// </summary>
        /// <returns>This will return the shipping type of the best rate found.</returns>
        public override List<ShipmentEntity> PreProcess(ShipmentEntity shipment, Func<CounterRatesProcessingArgs, DialogResult> counterRatesProcessing, RateResult selectedRate, ILifetimeScope lifetimeScope)
        {
            AddBestRateEvent(shipment, BestRateEventTypes.RateAutoSelectedAndProcessed);

            ShippingManager.EnsureShipmentLoaded(shipment);
            IEnumerable<RateGroup> rateGroups = GetRatesForPreProcessing(shipment);

            // We want all the rates here, so we can pass them back to the coutner rate processing if needed
            RateGroup filteredRates = CompileBestRates(shipment, rateGroups);
            if (!filteredRates.Rates.Any())
            {
                throw new ShippingException("ShipWorks could not find any rates.");
            }

            List<RateResult> ratesToApplyToReturnedShipments = new List<RateResult>();

            if (selectedRate != null)
            {
                ratesToApplyToReturnedShipments = HandleSelectedRate(shipment, counterRatesProcessing, selectedRate, rateGroups, filteredRates);
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

                // If the best rate is a counter rate, raise an event that will let the user sign up for the service
                if (bestRate.IsCounterRate)
                {
                    ratesToApplyToReturnedShipments = HandleCounterRate(shipment, rateGroups.ToList(), filteredRates, counterRatesProcessing);
                    if (ratesToApplyToReturnedShipments == null)
                    {
                        // This would mean the user canceled; stop processing
                        return null;
                    }
                }
                else
                {
                    // The cheapest rate is not a counter rate, so compile a list of the possible
                    // rates to apply to the shipment during processing. This is basically a fail
                    // over mechanism in case the processing with the first rate fails
                    ratesToApplyToReturnedShipments = rateGroups
                        .SelectMany(x => x.Rates)
                        .Where(r => !r.IsCounterRate && r.Amount == bestRate.Amount)
                        .ToList();
                }
            }

            List<ShipmentEntity> shipmentsToReturn = new List<ShipmentEntity>();
            foreach (RateResult rateToApply in ratesToApplyToReturnedShipments)
            {
                // Apply the selected rate to the shipment, so it's configured
                // for processing
                ApplySelectedShipmentRate(shipment, rateToApply);
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
                return GetRates(shipment, new List<BrokerException>());
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
        /// <param name="shipment">The shipment.</param>
        /// <param name="counterRatesProcessing">The counter rates processing.</param>
        /// <param name="selectedRate">The selected rate.</param>
        /// <param name="rateGroups">The rate groups.</param>
        /// <param name="filteredRates">The filtered rates.</param>
        /// <returns></returns>
        /// <exception cref="ShippingException">The rate that was selected is out of date or could not be found. Please select another rate.</exception>
        private List<RateResult> HandleSelectedRate(ShipmentEntity shipment, Func<CounterRatesProcessingArgs, DialogResult> counterRatesProcessing, RateResult selectedRate, IEnumerable<RateGroup> rateGroups, RateGroup filteredRates)
        {
            // We want to try to process with the selected rate that was provided. Build
            // up our list of fail over candidates in case the processing the shipment with
            // the first rate fails
            List<RateResult> ratesToApplyToReturnedShipments = rateGroups
                .ToList()
                .SelectMany(x => x.Rates)
                .Where(r => r.Amount == selectedRate.Amount && r.OriginalTag == selectedRate.OriginalTag)
                .ToList();

            if (selectedRate.IsCounterRate)
            {
                ratesToApplyToReturnedShipments = HandleCounterRate(shipment, rateGroups.ToList(), filteredRates, counterRatesProcessing);
            }
            else
            {
                // The rate was not a counter rate, but it was not found
                if (!ratesToApplyToReturnedShipments.Any())
                {
                    throw new ShippingException("The rate that was selected is out of date or could not be found. Please select another rate.");
                }
            }

            return ratesToApplyToReturnedShipments;
        }

        /// <summary>
        /// Handles the counter rate.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <param name="originalRateGroups">The original rate groups.</param>
        /// <param name="filteredRates">The filtered rates.</param>
        /// <param name="counterRatesProcessing">The counter rates processing.</param>
        /// <returns></returns>
        /// <exception cref="ShippingException">ShipWorks could not find any rates.</exception>
        private List<RateResult> HandleCounterRate(ShipmentEntity shipment, IEnumerable<RateGroup> originalRateGroups, RateGroup filteredRates, Func<CounterRatesProcessingArgs, DialogResult> counterRatesProcessing)
        {
            List<RateResult> ratesToApplyToReturnedShipments = null;

            // Get all rates that meet the specified service level ordered by amount
            BestRateServiceLevelFilter filter = new BestRateServiceLevelFilter((ServiceLevelType)shipment.BestRate.ServiceLevel);
            RateGroup allRates = filter.Filter(new RateGroup(originalRateGroups.SelectMany(x => x.Rates)));

            CounterRatesProcessingArgs eventArgs = new CounterRatesProcessingArgs(allRates, filteredRates, shipment);

            if (counterRatesProcessing != null)
            {
                // Invoke the callback for handling the case where a counter rate is the
                // best rate available (e.g. sign up for an account with the best rate provider,
                // choose to use an existing account instead, etc.)
                counterRatesProcessing(eventArgs);
                ShippingSettings.CheckForChangesNeeded();
            }

            if (eventArgs.SelectedShipmentType != null)
            {
                // Get the rates for the newly created account
                IBestRateShippingBroker broker = eventArgs.SelectedShipmentType.GetShippingBroker(shipment);
                RateGroup rateGroup = broker.GetBestRates(shipment, new List<BrokerException>());

                if (eventArgs.SelectedRate == null)
                {
                    // A shipment type was selected, but a rate wasn't selected meaning this is the case where
                    // an existing account was ADDED to ShipWorks, so just process with the first rate that we
                    // get back for this account

                    // Compiling the best rates will give us a list of rates from the broker that is sorted by rate
                    // and filtered by service level
                    RateResult selectedRate = CompileBestRates(shipment, new List<RateGroup> { rateGroup }).Rates.FirstOrDefault();

                    // Ensure that the results of the dialog return an actual rate of some kind
                    if (selectedRate == null)
                    {
                        throw new ShippingException("ShipWorks could not find any rates.");
                    }

                    // We're going to process the shipment with the selected rate
                    ratesToApplyToReturnedShipments = new List<RateResult> { selectedRate };
                }
                else
                {
                    // The event args indicate a rate was selected, so we'll use it
                    // to find the corresponding rate in the new list of results
                    RateResult selectedRate = rateGroup.Rates.FirstOrDefault(r => r.OriginalTag.Equals(eventArgs.SelectedRate.OriginalTag));

                    if (selectedRate == null)
                    {
                        throw new ShippingException("The service identified as the best rate is not available with the account you just created. You'll have to configure this shipment manually by clicking on the \"Configure\" link in the rate grid.");
                    }

                    ratesToApplyToReturnedShipments = new List<RateResult> { selectedRate };
                }
            }

            return ratesToApplyToReturnedShipments;
        }

        /// <summary>
        /// Indicates if customs forms may be required to ship the shipment based on the
        /// shipping address and any store specific logic that may impact whether customs
        /// is required (i.e. eBay GSP).
        /// </summary>
        public override bool IsCustomsRequired(ShipmentEntity shipment)
        {
            // Make sure the best rate shipment data is loaded (in the event that we're
            // coming from somewhere other than the shipping screen)
            LoadShipmentData(shipment, false);

            IEnumerable<IBestRateShippingBroker> brokers = brokerFactory.CreateBrokers(shipment, false);
            return brokers.Any(b => b.IsCustomsRequired(shipment));
        }

        /// <summary>
        /// Handles exceptions generated during the pre-process phase
        /// </summary>
        /// <param name="exception">Exception that was generated</param>
        private static void PreProcessExceptionHandler(BrokerException exception)
        {
            if (exception.SeverityLevel == BrokerExceptionSeverityLevel.Error)
            {
                // Throw the inner exception since the actual shipping exception we're interested
                // in (and the application is expecting to handle) is here
                throw exception.InnerException;
            }
        }

        /// <summary>
        /// Applies the selected rate to the specified shipment
        /// </summary>
        /// <param name="shipment">Shipment that will have the rate applied</param>
        /// <param name="bestRate">Rate that should be applied to the shipment</param>
        public void ApplySelectedShipmentRate(ShipmentEntity shipment, RateResult bestRate)
        {
            AddBestRateEvent(shipment, BestRateEventTypes.RateSelected);
            BestRateEventTypes originalEventTypes = (BestRateEventTypes)shipment.BestRateEvents;

            BestRateResultTag bestRateResultTag = ((BestRateResultTag)bestRate.Tag);

            bestRateResultTag.RateSelectionDelegate(shipment);

            // Reset the event types after the the selected shipment has been applied to
            // avoid losing them during the transition to the targeted shipment type
            shipment.BestRateEvents = (byte)originalEventTypes;
        }

        /// <summary>
        /// Apply the configured defaults and profile rule settings to the given shipment
        /// </summary>
        public override void ConfigureNewShipment(ShipmentEntity shipment)
        {
            base.ConfigureNewShipment(shipment);

            shipment.BestRate.InsuranceValue = 0;
            shipment.BestRate.RequestedLabelFormat = (int) LabelFormatType.Standard;
        }

        /// <summary>
        /// Update any data that could have changed dynamically or externally
        /// </summary>
        public override void UpdateDynamicShipmentData(ShipmentEntity shipment)
        {
            base.UpdateDynamicShipmentData(shipment);

            InsuranceProvider shipmentInsuranceProvider = GetShipmentInsuranceProvider(shipment);

            shipment.InsuranceProvider = (int)shipmentInsuranceProvider;

            shipment.RequestedLabelFormat = shipment.BestRate.RequestedLabelFormat;
        }

        public override RatingFields RatingFields
        {
            get
            {
                if (ratingField != null)
                {
                    return ratingField;
                }

                ratingField = base.RatingFields;
                ratingField.ShipmentFields.Add(BestRateShipmentFields.DimsAddWeight);
                ratingField.ShipmentFields.Add(BestRateShipmentFields.DimsHeight);
                ratingField.ShipmentFields.Add(BestRateShipmentFields.DimsLength);
                ratingField.ShipmentFields.Add(BestRateShipmentFields.DimsWidth);
                ratingField.ShipmentFields.Add(BestRateShipmentFields.DimsWeight);

                return ratingField;
            }
        }

        /// <summary>
        /// Gets the shipment insurance provider based on carriers selected.
        /// </summary>
        /// <returns></returns>
        public InsuranceProvider GetShipmentInsuranceProvider(ShipmentEntity shipment)
        {
            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            IEnumerable<IBestRateShippingBroker> brokersWithAccounts = brokerFactory.CreateBrokers(shipment, false).Where(b => b.HasAccounts).ToList();

            // Default shipmentInsuranceProvider is ShipWorks
            InsuranceProvider shipmentInsuranceProvider;

            if (brokersWithAccounts.Count() == 1)
            {
                // If 1 carrier, use that carrier's insurance provider
                shipmentInsuranceProvider = brokersWithAccounts.First().GetInsuranceProvider(settings);
            }
            else if (brokersWithAccounts.Any())
            {
                // If more than 1 carrier, if any of the carrier's are not ShipWorks insurance, set to invalid.
                if (brokersWithAccounts.Any(b => b.GetInsuranceProvider(settings) != InsuranceProvider.ShipWorks))
                {
                    shipmentInsuranceProvider = InsuranceProvider.Invalid;
                }
                else
                {
                    shipmentInsuranceProvider = InsuranceProvider.ShipWorks;
                }
            }
            else
            {
                // No brokersWithAccounts
                shipmentInsuranceProvider = InsuranceProvider.Invalid;
            }

            return shipmentInsuranceProvider;
        }

        /// <summary>
        /// Adds the best rate event.
        /// </summary>
        private static void AddBestRateEvent(ShipmentEntity shipment, BestRateEventTypes eventType)
        {
            if ((shipment.BestRateEvents & (byte) BestRateEventTypes.RateAutoSelectedAndProcessed) != (byte) BestRateEventTypes.RateAutoSelectedAndProcessed)
            {
                // User already processed it, don't give credit for getting rates which happens during process...
                shipment.BestRateEvents |= (byte)eventType;
            }
        }

        /// <summary>
        /// Saves the requested label format to the child shipment
        /// </summary>
        public override void SaveRequestedLabelFormat(ThermalLanguage requestedLabelFormat, ShipmentEntity shipment)
        {
            if (shipment.BestRate != null)
            {
                shipment.BestRate.RequestedLabelFormat = (int)requestedLabelFormat;
            }
        }

        /// <summary>
        /// Update the label format of carrier specific unprocessed shipments
        /// </summary>
        public override void UpdateLabelFormatOfUnprocessedShipments(SqlAdapter adapter, int newLabelFormat, RelationPredicateBucket bucket)
        {
            bucket.Relations.Add(ShipmentEntity.Relations.BestRateShipmentEntityUsingShipmentID);

            adapter.UpdateEntitiesDirectly(new BestRateShipmentEntity { RequestedLabelFormat = newLabelFormat }, bucket);
        }
    }
}
