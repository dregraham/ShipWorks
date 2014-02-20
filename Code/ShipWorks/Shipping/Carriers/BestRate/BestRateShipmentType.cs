using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.Business;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Carriers.BestRate.RateGroupFiltering;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Settings.Origin;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Stores.Platforms.Amazon.WebServices.Associates;
using ShipWorks.Data;

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

        public event EventHandler SignUpForProviderAccountCompleted;

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
        public override ServiceControlBase CreateServiceControl(RateControl rateControl)
        {
            return new BestRateServiceControl(ShipmentTypeCode, rateControl);
        }

        /// <summary>
        /// Create the UserControl that is used to edit a profile for the service
        /// </summary>
        public override ShippingProfileControlBase CreateProfileControl()
        {
            return new BestRateProfileControl();
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
            AddBestRateEvent(shipment, BestRateEventTypes.RatesCompared);

            List<BrokerException> brokerExceptions = new List<BrokerException>();
            IEnumerable<RateGroup> rateGroups = GetRates(shipment, brokerExceptions);
            
            RateGroup rateGroup = CompileBestRates(shipment, rateGroups);

            // Get a list of distinct exceptions based on the message text ordered by the severity level (highest to lowest)
            IEnumerable<BrokerException> distinctExceptions = brokerExceptions.OrderBy(ex => ex.SeverityLevel, new BrokerExceptionSeverityLevelComparer())
                                                                                .GroupBy(e => e.Message)
                                                                                .Select(m => m.First()).ToList();
            if (distinctExceptions.Any())
            {
                rateGroup.AddFootnoteFactory(new BrokerExceptionsRateFootnoteFactory(this, distinctExceptions));
            }

            return rateGroup;
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

                throw new ShippingException(message);
            }

            // Start getting rates from each enabled carrier
            List<Task<RateGroup>> tasks = bestRateShippingBrokers
                .Select(broker => StartGetRatesTask(broker, shipment, exceptionHandler))
                .ToList();
            
            tasks.ForEach(t => t.Wait());
            
            return tasks.Select(x => x.Result);
        }

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
        /// <param name="exceptionHandler">Handler for exceptions generated while getting rates</param>
        /// <returns>A task that will contain the results</returns>
        private static Task<RateGroup> StartGetRatesTask(IBestRateShippingBroker broker, ShipmentEntity shipment, List<BrokerException> brokerExceptions)
        {
            return Task<RateGroup>.Factory.StartNew(() => broker.GetBestRates(shipment, brokerExceptions));
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
        public override SettingsControlBase CreateSettingsControl()
        {
            return new BestRateSettingsControl();
        }
        
        /// <summary>
        /// Gets rates and converts shipment to the found best rate type.
        /// </summary>
        /// <returns>This will return the shipping type of the best rate found.</returns>
        public override List<ShipmentEntity> PreProcess(ShipmentEntity shipment, Func<CounterRatesProcessingArgs, DialogResult> counterRatesProcessing)
        {
            AddBestRateEvent(shipment, BestRateEventTypes.RateAutoSelectedAndProcessed);

            ShippingManager.EnsureShipmentLoaded(shipment);
            IEnumerable<RateGroup> rateGroups;

            try
            {
                rateGroups = GetRates(shipment, new List<BrokerException>());
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

            RateGroup filteredRates = CompileBestRates(shipment, rateGroups);
            RateResult bestRate = filteredRates.Rates.FirstOrDefault();

            if (bestRate == null)
            {
                throw new ShippingException("ShipWorks could not find any rates.");
            }

            List<RateResult> ratesToApplyToReturnedShipments;

            // If the best rate is a counter rate, raise an event that will let the user sign up for the service
            if (bestRate.IsCounterRate)
            {
                // Get all rates that meet the specified service level ordered by amount
                BestRateServiceLevelFilter filter = new BestRateServiceLevelFilter((ServiceLevelType)shipment.BestRate.ServiceLevel);
                RateGroup allRates = filter.Filter(new RateGroup(rateGroups.SelectMany(x => x.Rates)));

                // Determine what the actual shipment type should be for the selected best rate
                // (i.e. use Endicia if a postal type was selected)
                //ShipmentTypeCode shipmentTypeCode = bestRate.ShipmentType;

                //ShipmentType setupShipmentType = DetermineCounterRateShipmentTypeForCounterRateSetupWizard(shipmentTypeCode);
                CounterRatesProcessingArgs eventArgs = new CounterRatesProcessingArgs(allRates, filteredRates, shipment.ShipmentID);

                if (counterRatesProcessing != null)
                {
                    counterRatesProcessing(eventArgs);
                    ShippingSettings.CheckForChangesNeeded();
                }

               if (eventArgs.SelectedShipmentType != null)
                {
                    // Get the best rates for the newly created account
                    IBestRateShippingBroker broker = eventArgs.SelectedShipmentType.GetShippingBroker(shipment);
                    RateGroup bestRateGroup = broker.GetBestRates(shipment, new List<BrokerException>());

                    // Compiling the best rates will give us a list of rates from the broker that is sorted by rate
                    // and filtered by service level
                    bestRate = CompileBestRates(shipment, new List<RateGroup> { bestRateGroup }).Rates.FirstOrDefault();

                    ratesToApplyToReturnedShipments = new List<RateResult> { bestRate };
                }
                else
                {
                    // This would mean the user canceled 
                    return null;
                }

                // Ensure that the results of the dialog return an actual rate of some kind
                if (bestRate == null)
                {
                    throw new ShippingException("ShipWorks could not find any rates.");
                }
            }
            else
            {
                ratesToApplyToReturnedShipments = rateGroups
                    .SelectMany(x => x.Rates)
                    .Where(r => !r.IsCounterRate && r.Amount == bestRate.Amount)
                    .ToList();
            }

            List<ShipmentEntity> shipmentsToReturn = new List<ShipmentEntity>();
            foreach (RateResult rateToApply in ratesToApplyToReturnedShipments)
            {
                ApplySelectedShipmentRate(shipment, rateToApply);   
                shipmentsToReturn.Add(shipment);
            }
            
            return shipmentsToReturn;
        }
        
        /// <summary>
        /// Indicates if customs forms may be required to ship the shipment based on the
        /// shipping address and any store specific logic that may impact whether customs
        /// is required (i.e. eBay GSP).
        /// </summary>
        public override bool IsCustomsRequired(ShipmentEntity shipment)
        {
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
            bool selectRate = true;

            if (bestRateResultTag.SignUpAction != null)
            {
                // Capture the result of the sign up action to determine if the rate
                // should be selected (i.e. the setup wizard completed)
                bool signedUpForAccount = bestRateResultTag.SignUpAction();
                selectRate = signedUpForAccount;
                
                if (signedUpForAccount && SignUpForProviderAccountCompleted != null)
                {
                    // They didn't cancel out of the wizard, so signal that the 
                    // user signed up for an account
                    SignUpForProviderAccountCompleted(this, EventArgs.Empty);
                }
            }
            
            if (selectRate)
            {
                // We only want to select the rate if the sign up action finished    
                bestRateResultTag.RateSelectionDelegate(shipment);

                // Reset the event types after the the selected shipment has been applied to 
                // avoid losing them during the transition to the targeted shipment type
                shipment.BestRateEvents = (byte)originalEventTypes;
            }
        }

        /// <summary>
        /// Apply the configured defaults and profile rule settings to the given shipment
        /// </summary>
        public override void ConfigureNewShipment(ShipmentEntity shipment)
        {
            base.ConfigureNewShipment(shipment);

            shipment.BestRate.InsuranceValue = 0;
        }

        /// <summary>
        /// Update any data that could have changed dynamically or externally
        /// </summary>
        public override void UpdateDynamicShipmentData(ShipmentEntity shipment)
        {
            base.UpdateDynamicShipmentData(shipment);

            InsuranceProvider shipmentInsuranceProvider = GetShipmentInsuranceProvider(shipment);

            shipment.InsuranceProvider = (int)shipmentInsuranceProvider;
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
    }
}
