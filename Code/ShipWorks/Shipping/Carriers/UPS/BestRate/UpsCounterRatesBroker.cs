using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.UpsEnvironment;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.UPS.BestRate
{
    public class UpsCounterRatesBroker : UpsBestRateBroker
    {


        /// <summary>
        /// Initializes a new instance of the <see cref="UpsCounterRatesBroker"/> class.
        /// </summary>
        /// <remarks>
        /// This is designed to be used within ShipWorks
        /// </remarks>
        public UpsCounterRatesBroker()
            : this(new UpsOltShipmentType(), new UpsCounterRateAccountRepository(TangoCounterRatesCredentialStore.Instance), new UpsCounterRateSettingsRepository(TangoCounterRatesCredentialStore.Instance))
        {}


        /// <summary>
        /// Initializes a new instance of the <see cref="UpsCounterRatesBroker"/> class.
        /// </summary>
        public UpsCounterRatesBroker(UpsShipmentType upsShipmentType, ICarrierAccountRepository<UpsAccountEntity> accountRepository, ICarrierSettingsRepository settingsRepository) 
            : base(upsShipmentType, accountRepository, settingsRepository)
        {

        }

        /// <summary>
        /// Gets a list of UPS rates
        /// </summary>
        /// <param name="shipment">Shipment for which rates should be retrieved</param>
        /// <param name="exceptionHandler">Action that performs exception handling</param>
        /// <returns>A RateGroup containing the counter rates for a generic UPS account.</returns>
        public override RateGroup GetBestRates(ShipmentEntity shipment, Action<BrokerException> exceptionHandler)
        {
            ((UpsShipmentType)ShipmentType).SettingsRepository = SettingsRepository;
            ((UpsShipmentType)ShipmentType).AccountRepository = AccountRepository;

            // We're passing in our own exception handler to accumulate broker exceptions otherwise
            // any SurePost exceptions generated will display as having a Warning severity level; later
            // on we're going to make sure any broker exceptions have the severity level set to 
            // information and then call the exception handler provided.
            List<BrokerException> brokerExceptions = new List<BrokerException>();
            RateGroup rates = base.GetBestRates(shipment, brokerExceptions.Add);

            foreach (BestRateResultTag bestRateResultTag in rates.Rates.Select(rate => (BestRateResultTag)rate.Tag))
            {
                // We want the UPS account setup wizard to show when a rate is selected so the user 
                // can create their own UPS account since these rates are just counter rates 
                // using a ShipWorks account.
                bestRateResultTag.SignUpAction = new Func<bool>(DisplaySetupWizard);
            }

            // Since we sent our own exception handler to the GetBestRates method, we need to 
            // call the exception handler that was provided with any broker exceptions
            foreach (BrokerException brokerException in brokerExceptions)
            {
                if (brokerException.SeverityLevel != BrokerExceptionSeverityLevel.Information)
                {
                    // Translate the broker exception to an informational severity level before
                    // sending it to the exception handler
                    exceptionHandler(new BrokerException(brokerException, BrokerExceptionSeverityLevel.Information, brokerException.ShipmentType));
                }
                else
                {
                    exceptionHandler(brokerException);
                }
            }
            
            return rates;
        }

        /// <summary>
        /// Displays the UPS setup wizard.
        /// </summary>
        private bool DisplaySetupWizard()
        {
            using (Form setupWizard = ShipmentType.CreateSetupWizard())
            {
                DialogResult result = setupWizard.ShowDialog();

                if (result == DialogResult.OK)
                {
                    ShippingSettings.MarkAsConfigured(ShipmentTypeCode.UpsOnLineTools);
                }

                return result == DialogResult.OK;
            }
        }
        
        /// <summary>
        /// Gets the first account from the repository and returns the ID.
        /// </summary>
        protected override long GetAccountID(UpsAccountEntity account)
        {
            return account.UpsAccountID;
        }

        /// <summary>
        /// Wraps the shipping exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>A new BrokerException with a severity level of Information.</returns>
        protected override BrokerException WrapShippingException(ShippingException ex)
        {
            // Since this is just getting counter rates, we want to have the severity level
            // as information for all shipping exceptions
            return new BrokerException(ex, BrokerExceptionSeverityLevel.Information, ShipmentType);
        }

        /// <summary>
        /// Gets a value indicating whether [is counter rate].
        /// </summary>
        /// <value>Always returns <c>true</c>.</value>
        public override bool IsCounterRate
        {
            get { return true; }
        }
    }
}