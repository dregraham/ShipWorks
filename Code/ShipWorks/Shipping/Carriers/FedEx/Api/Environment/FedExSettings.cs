using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Environment
{
    /// <summary>
    /// Encapsulates the settings that are common to FedEx services (credentials, client production ID, etc.). The repository
    /// is used for fetching persisted settings such as whether to use a test server.
    /// </summary>
    public class FedExSettings
    {
        // TODO: add the production URL when we receive it from FedEx
        private const string ProductionUrl = "https://ws.fedex.com:443/web-services/";

        private const string TestingUrl = "https://wsbeta.fedex.com:443/web-services/";

        private readonly ICarrierSettingsRepository settingsRepository;

        private readonly ShippingSettingsEntity shippingSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExSettings" /> class.
        /// </summary>
        public FedExSettings()
            : this(new FedExSettingsRepository())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExSettings" /> class.
        /// </summary>
        /// <param name="settingsRepository">The settings repository.</param>
        public FedExSettings(ICarrierSettingsRepository settingsRepository)
        {
            this.settingsRepository = settingsRepository;

            // Fetch the shipping settings and save them so we don't have to make repeated reqeusts
            shippingSettings = settingsRepository.GetShippingSettings();
        }

        /// <summary>
        /// Gets the CSP credential key.
        /// </summary>
        /// <value>
        /// The CSP credential key.
        /// </value>
        public string CspCredentialKey
        {
            get
            {
                return "fOilEsjTTKLzuwNi";
            }
        }

        /// <summary>
        /// Gets the CSP credential password.
        /// </summary>
        /// <value>The CSP credential password.</value>
        public string CspCredentialPassword
        {
            get
            {
                return SecureText.Decrypt("ED92wCRD8FOLC7+3XBElFwZiMTc3af91XMaxrxLLF4g=", "apptive");
            }
        }

        /// <summary>
        /// Gets the user credentials key.
        /// </summary>
        /// <value>The user credentials key.</value>
        public string UserCredentialsKey
        {
            get
            {
                return shippingSettings.FedExUsername;
            }
        }

        /// <summary>
        /// Gets the user credentials password.
        /// </summary>
        /// <value>The user credentials password.</value>
        public string UserCredentialsPassword
        {
            get { return shippingSettings.FedExPassword != null ? SecureText.Decrypt(shippingSettings.FedExPassword, "FedEx") : null; }
        }

        /// <summary>
        /// Gets the client product ID.
        /// </summary>
        /// <value>
        /// The client product ID.
        /// </value>
        public string ClientProductId
        {
            get
            {
                return "ITSW";
            }
        }

        /// <summary>
        /// Gets the client product version.
        /// </summary>
        /// <value>
        /// The client product version.
        /// </value>
        public string ClientProductVersion
        {
            get
            {
                return "5236";
            }
        }

        /// <summary>
        /// Gets the CSP solution ID given to ShipWorks from FedEx.
        /// </summary>
        /// <value>The solution ID.</value>
        public string CspSolutionId
        {
            get
            {
                return "086";
            }
        }

        /// <summary>
        /// Get the FedEx endpoint URL to use
        /// </summary>
        public string EndpointUrl
        {
            get
            {
                return settingsRepository.UseTestServer ? TestingUrl : ProductionUrl;
            }
        }

        /// <summary>
        /// Gets the ship version number.
        /// </summary>
        public const string ShipVersionNumber = "15";

        /// <summary>
        /// Gets the open ship version number.
        /// </summary>
        public const string OpenShipVersionNumber = "9";

        /// <summary>
        /// Gets the type of the currency.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns></returns>
        public CurrencyType GetCurrencyType(ShipmentEntity shipment)
        {
            FedExAccountEntity account = (FedExAccountEntity)settingsRepository.GetAccount(shipment);
            
            if (account == null)
            {
                throw new CarrierException("Shipment not associated with a FedEx account.");
            }

            return ShipmentType.GetCurrencyForCountryCode(account.CountryCode);
        }
    }
}