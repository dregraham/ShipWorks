using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Business;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.BestRate.Footnote;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Settings.Origin;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// Best rate implementation of ShipmentType
    /// </summary>
    public class BestRateShipmentType : ShipmentType
    {
        private readonly ILog log;
        private readonly IBestRateShippingBrokerFactory brokerFactory;
        private IRateGroupFilter rateGroupFilter;

        /// <summary>
        /// Initializes a new instance of the <see cref="BestRateShipmentType"/> class. This
        /// version of the constructor will use the "live" implementation of the 
        /// IBestRateShippingBrokerFactory interface.
        /// </summary>
        public BestRateShipmentType()
            : this(new BestRateShippingBrokerFactory(), LogManager.GetLogger(typeof(BestRateShipmentType)))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BestRateShipmentType" /> class. This version of
        /// the constructor is primarily for testing purposes.
        /// </summary>
        /// <param name="brokerFactory">The broker factory.</param>
        /// <param name="log">The log.</param>
        public BestRateShipmentType(IBestRateShippingBrokerFactory brokerFactory, ILog log)
        {
            this.brokerFactory = brokerFactory;
            this.log = log;
        }


        /// <summary>
        /// IRateGroupFilter that returns a new list of the filtered rate results.
        /// </summary>
        public IRateGroupFilter RateGroupFilter
        {
            get
            {
                if (rateGroupFilter == null)
                {
                    rateGroupFilter = new RateGroupFilter();
                }

                return rateGroupFilter;
            }
            set
            {
                rateGroupFilter = value; 
            }
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
        public override ServiceControlBase CreateServiceControl()
        {
            return new BestRateServiceControl(ShipmentTypeCode);
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
            RateGroup rateGroup = GetRates(shipment, ex =>
            {
                // Accumulate all of the broker exceptions for later use
                log.WarnFormat("Received an error while obtaining rates from a carrier. {0}", ex.Message);
                brokerExceptions.Add(ex);
            });

            // Get a list of distinct exceptions based on the message text ordered by the severity level (highest to lowest)
            IEnumerable<BrokerException> distinctExceptions = brokerExceptions.OrderBy(ex => ex.SeverityLevel, new BrokerExceptionSeverityLevelComparer())
                                                                                .GroupBy(e => e.Message)
                                                                                .Select(m => m.First()).ToList();
            if (distinctExceptions.Any())
            {
                rateGroup.AddFootnoteCreator(() => new BrokerExceptionsRateFootnoteControl(distinctExceptions));  
            }

            return rateGroup;
        }

        /// <summary>
        /// Called to get the latest rates for the shipment. This implementation will accumulate the 
        /// best shipping rate for all of the individual carrier-accounts within ShipWorks.
        /// </summary>
        private RateGroup GetRates(ShipmentEntity shipment, Action<BrokerException> exceptionHandler)
        {
            IEnumerable<IBestRateShippingBroker> bestRateShippingBrokers = brokerFactory.CreateBrokers(shipment);
            
            if (!bestRateShippingBrokers.Any())
            {
                string message = string.Format("There are not any accounts configured to use with best rate.{0}Check the shipping settings to ensure " +
                                               "your shipping accounts have been setup for the shipping providers being used with best rate.", Environment.NewLine);

                throw new ShippingException(message);
            }

            // Start getting rates from each enabled carrier
            Task<RateGroup>[] tasks = bestRateShippingBrokers
                .Select(broker => StartGetRatesTask(broker, shipment, exceptionHandler))
                .ToArray();

            Task.WaitAll(tasks);

            List<RateGroup> rateGroups = tasks.Select(x => x.Result).ToList();
            IEnumerable<RateResult> allRates = rateGroups.SelectMany(x => x.Rates);

            RateGroup compiledRateGroup = new RateGroup(allRates);
            SetFootnote(rateGroups, compiledRateGroup);

            compiledRateGroup.Carrier = ShipmentTypeCode.BestRate;

            // Filter out any rates as necessary
            compiledRateGroup = RateGroupFilter.FilterRates(compiledRateGroup, (ServiceLevelType)shipment.BestRate.ServiceLevel);

            // Allow each rate result the chance to mask its description if needed based on the 
            // other rate results in the list. This is for UPS that does not want its named-rates
            // intermingled with rates from other carriers
            compiledRateGroup.Rates.ForEach(x => x.MaskDescription(compiledRateGroup.Rates));
            
            return compiledRateGroup;
        }

        /// <summary>
        /// Sets the footnote.
        /// </summary>
        /// <param name="allRateGroups">The rate groups.</param>
        /// <param name="compiledRateGroup">The compiled rate group.</param>
        public static void SetFootnote(IEnumerable<RateGroup> allRateGroups, RateGroup compiledRateGroup)
        {
            // Only add footnotes from rate groups from which rates will actually be displayed
            IEnumerable<Func<RateFootnoteControl>> includedRateGroups = allRateGroups
                                                                            .Where(rg => rg.FootnoteCreators.Any())
                                                                            .Where(rg => rg.Rates
                                                                                                                // where the rate doesn't have an image or the associated rate footer doesn't need an image
                                                                                .Intersect(compiledRateGroup.Rates.Where(r => r.AmountFootnote != null || !rg.FootnoteCreators.First().Invoke().AssociatedWithAmountFooter))
                                                                                .Any())
                                                                            .SelectMany(outerGroup => outerGroup.FootnoteCreators);

            foreach (Func<RateFootnoteControl> creator in includedRateGroups)
            {
                compiledRateGroup.AddFootnoteCreator(creator);
            }
        }

        /// <summary>
        /// Starts getting rates for a broker
        /// </summary>
        /// <param name="broker">Broker for which to start getting rates</param>
        /// <param name="shipment">Shipment for which to get rates</param>
        /// <param name="exceptionHandler">Handler for exceptions generated while getting rates</param>
        /// <returns>A task that will contain the results</returns>
        private static Task<RateGroup> StartGetRatesTask(IBestRateShippingBroker broker, ShipmentEntity shipment, Action<BrokerException> exceptionHandler)
        {
            return Task<RateGroup>.Factory.StartNew(() => broker.GetBestRates(shipment, exceptionHandler));
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
        public override ShipmentType PreProcess(ShipmentEntity shipment)
        {
            AddBestRateEvent(shipment, BestRateEventTypes.RateAutoSelectedAndProcessed);

            ShippingManager.EnsureShipmentLoaded(shipment);
            RateGroup rateGroup;

            try
            {
                rateGroup = GetRates(shipment, PreProcessExceptionHandler);
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
            
            RateResult bestRate = rateGroup.Rates.FirstOrDefault();

            if (bestRate == null)
            {
                throw new ShippingException("ShipWorks could not find any rates.");
            }

            ApplySelectedShipmentRate(shipment, bestRate);

            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.SaveAndRefetch(shipment);
            }

            return ShipmentTypeManager.GetType(shipment);
        }

        /// <summary>
        /// Indicates if customs forms may be required to ship the shipment based on the
        /// shipping address and any store specific logic that may impact whether customs
        /// is required (i.e. eBay GSP).
        /// </summary>
        public override bool IsCustomsRequired(ShipmentEntity shipment)
        {
            IEnumerable<IBestRateShippingBroker> brokers = brokerFactory.CreateBrokers(shipment);
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
        public static void ApplySelectedShipmentRate(ShipmentEntity shipment, RateResult bestRate)
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
            IEnumerable<IBestRateShippingBroker> brokersWithAccounts = brokerFactory.CreateBrokers(shipment).Where(b => b.HasAccounts).ToList();

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
