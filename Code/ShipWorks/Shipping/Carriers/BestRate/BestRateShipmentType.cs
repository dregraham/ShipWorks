using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Editing.Enums;
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
        /// The ShipmentTypeCode represented by this ShipmentType
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode
        {
            get { return ShipmentTypeCode.BestRate; }
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
        /// Create the UserControl used to handle best rate shipments
        /// </summary>
        public override ServiceControlBase CreateServiceControl()
        {
            return new BestRateServiceControl(ShipmentTypeCode);
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
        public override InsuranceChoice GetParcelInsuranceChoice(ShipmentEntity shipment, int parcelIndex)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            return new InsuranceChoice(shipment, shipment, shipment.BestRate, shipment.BestRate);
        }

        /// <summary>
        /// Called to get the latest rates for the shipment. This implementation will accumulate the 
        /// best shipping rate for all of the individual carrier-accounts within ShipWorks.
        /// </summary>
        public override RateGroup GetRates(ShipmentEntity shipment)
        {
            // TODO: Handle errors so they can be displayed. This should be done during the "Notify users when best rate could not be found" story
            return GetRates(shipment, ex => { } );
        }

        /// <summary>
        /// Called to get the latest rates for the shipment. This implementation will accumulate the 
        /// best shipping rate for all of the individual carrier-accounts within ShipWorks.
        /// </summary>
        public RateGroup GetRates(ShipmentEntity shipment, Action<ShippingException> exceptionHandler)
        {
            List<RateResult> rates = new List<RateResult>();

            IEnumerable<IBestRateShippingBroker> bestRateShippingBrokers = brokerFactory.CreateBrokers();
            
            if (!bestRateShippingBrokers.Any())
            {
                throw new ShippingException("No Carrier with an account has been selected.");
            }

            foreach (IBestRateShippingBroker broker in bestRateShippingBrokers)
            {
                // Use the broker to get the best rates for each shipping provider
                rates.AddRange(broker.GetBestRates(shipment, exceptionHandler));
            }

            if (shipment.BestRate.ServiceLevel != (int)ServiceLevelType.Anytime)
            {
                DateTime? maxDeliveryDate = rates
                    .Where(x => x.ServiceLevel != ServiceLevelType.Anytime)
                    .Where(x => (int)x.ServiceLevel <= shipment.BestRate.ServiceLevel)
                    .Max(x => x.ExpectedDeliveryDate);

                rates = rates.Where(x => x.ExpectedDeliveryDate <= maxDeliveryDate).ToList();
            }
            

            // We want the cheapest rates to appear first, and any ties to be ordered by service level
            // and return the top 5
            IEnumerable<RateResult> orderedRates = rates.OrderBy(r => r.Amount);

            var orderedRatesList = orderedRates.Take(5).ToList();
            orderedRatesList.ForEach(x => x.MaskDescription(orderedRatesList));

            return new RateGroup(orderedRatesList);
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets whether the specified settings tab should be hidden in the UI
        /// </summary>
        public override bool IsSettingsTabHidden(ShipmentTypeSettingsControl.Page tab)
        {
            return tab == ShipmentTypeSettingsControl.Page.Actions || tab == ShipmentTypeSettingsControl.Page.Printing;
        }

        /// <summary>
        /// Gets an instance to the best rate shipping broker for the best rate shipment type.
        /// </summary>
        /// <returns>An instance of a NullShippingBroker since this is not applicable to the best rate shipment type.</returns>
        public override IBestRateShippingBroker GetShippingBroker()
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
        /// Indicates if the shipment service type supports getting rates
        /// </summary>
        public override bool SupportsGetRates
        {
            get { return true; }
        }

        /// <summary>
        /// Gets rates and converts shipment to the found best rate type.
        /// </summary>
        /// <returns>This will return the shipping type of the best rate found.</returns>
        public override ShipmentType PreProcess(ShipmentEntity shipment)
        {
            RateGroup rateGroup = GetRates(shipment, ex => { throw ex; });
            RateResult bestRate = rateGroup.Rates.FirstOrDefault();

            if (bestRate == null)
            {
                throw new ShippingException("ShipWorks could not find any rates.");
            }

            Action<ShipmentEntity> action = ((Action<ShipmentEntity>)bestRate.Tag);
            action(shipment);

            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.SaveAndRefetch(shipment);
            }

            return ShipmentTypeManager.GetType(shipment);
        }
    }
}
