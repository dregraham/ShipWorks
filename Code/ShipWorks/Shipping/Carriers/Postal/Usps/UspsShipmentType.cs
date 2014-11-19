﻿using System.Collections.Generic;
using System.Linq;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Shipping.Carriers.Postal.Stamps.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Registration;
using ShipWorks.Shipping.Carriers.Postal.Usps.BestRate;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// A shipment type for the generic USPS shipment type in ShipWorks. This is actually a
    /// Stamps.com Expedited shipment type (which is why it derives from the StampsShipmentType)
    /// that gets presented as USPS to the end user.
    /// </summary>
    public class UspsShipmentType : StampsShipmentType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UspsShipmentType"/> class.
        /// </summary>
        public UspsShipmentType()
        {
            ShouldRetrieveExpress1Rates = false;

            // Use the "live" versions by default
            AccountRepository = new UspsAccountRepository();
            LogEntryFactory = new LogEntryFactory();
        }

        /// <summary>
        /// The ShipmentTypeCode enumeration value
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode
        {
            get { return ShipmentTypeCode.Usps; }
        }

        /// <summary>
        /// Create the settings control for stamps.com
        /// </summary>
        public override SettingsControlBase CreateSettingsControl()
        {
            return new StampsSettingsControl(ShipmentTypeCode);
        }

        /// <summary>
        /// Create the Form used to do the setup for the Stamps.com API
        /// </summary>
        public override ShipmentTypeSetupWizardForm CreateSetupWizard()
        {
            IRegistrationPromotion promotion = new RegistrationPromotionFactory().CreateRegistrationPromotion();
            return new UspsSetupWizard(promotion, true);
        }

        /// <summary>
        /// Get postal rates for the given shipment
        /// </summary>
        /// <param name="shipment">Shipment for which to retrieve rates</param>
        protected override RateGroup GetRatesFromApi(ShipmentEntity shipment)
        {
            List<RateResult> stampsRates = new StampsApiSession(AccountRepository, LogEntryFactory, CertificateInspector).GetRates(shipment);
            
            RateGroup rateGroup = new RateGroup(stampsRates);
            AddUspsRatePromotionFootnote(shipment, rateGroup);
            
            return rateGroup;
        }

        /// <summary>
        /// Creates the Express1/Stamps service control.
        /// </summary>
        /// <param name="rateControl">A handle to the rate control so the selected rate can be updated when
        /// a change to the shipment, such as changing the service type, matches a rate in the control</param>
        protected override ServiceControlBase InternalCreateServiceControl(RateControl rateControl)
        {
            // Just use the stamps service control, but configured it for USPS (Stamps.com Expedited)
            return new StampsServiceControl(ShipmentTypeCode, StampsResellerType.StampsExpedited, rateControl);
        }

        /// <summary>
        /// Allows the shipment type to run any pre-processing work that may need to be performed prior to
        /// actually processing the shipment. In most cases this is checking to see if an account exists
        /// and will call the counterRatesProcessing callback provided when trying to process a shipment
        /// without any accounts for this shipment type in ShipWorks, otherwise the shipment is unchanged.
        /// </summary>
        /// <param name="shipment"></param>
        /// <param name="counterRatesProcessing"></param>
        /// <param name="selectedRate"></param>
        /// <returns>
        /// The updates shipment (or shipments) that is ready to be processed. A null value may
        /// be returned to indicate that processing should be halted completely.
        /// </returns>
        public override List<ShipmentEntity> PreProcess(ShipmentEntity shipment, System.Func<CounterRatesProcessingArgs, System.Windows.Forms.DialogResult> counterRatesProcessing, RateResult selectedRate)
        {
            // We want to perform the processing of the base ShipmentType and not that of the Stamps.com shipment type
            IShipmentProcessingSynchronizer synchronizer = GetProcessingSynchronizer();
            ShipmentTypePreProcessor preProcessor = new ShipmentTypePreProcessor();

            return preProcessor.Run(synchronizer, shipment, counterRatesProcessing, selectedRate);
        }

        /// <summary>
        /// Process the shipment. Overridden here, so overhead of Express1 can be removed.
        /// </summary>
        public override void ProcessShipment(ShipmentEntity shipment)
        {
            ValidateShipment(shipment);

            try
            {
                new StampsApiSession(AccountRepository, LogEntryFactory, CertificateInspector).ProcessShipment(shipment);
            }
            catch (StampsException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Gets an instance to the best rate shipping broker for the USPS (Stamps.com Expedited) 
        /// shipment type based on the shipment configuration.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>An instance of a StampsBestRateBroker.</returns>
        public override IBestRateShippingBroker GetShippingBroker(ShipmentEntity shipment)
        {
            if (AccountRepository.Accounts.Any())
            {
                // We have an account, so use the normal broker
                return new UspsBestRateBroker(this, AccountRepository);
            }
            else
            {
                // No accounts, so use the counter rates broker to allow the user to
                // sign up for the account. We can use the StampsCounterRateAccountRepository 
                // here because the underlying accounts being used are the same.
                return new UspsCounterRatesBroker(new StampsCounterRateAccountRepository(TangoCounterRatesCredentialStore.Instance));
            }
        }
    }
}
