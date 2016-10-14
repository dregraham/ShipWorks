using Interapptive.Shared.Enums;
using Interapptive.Shared.Security;
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
        private const string ProductionUrl = "https://ws.fedex.com:443/web-services/";

        private const string TestingUrl = "https://wsbeta.fedex.com:443/web-services/";

        private readonly ICarrierSettingsRepository settingsRepository;

        private readonly ShippingSettingsEntity shippingSettings;

        private const string FimsMailViewShipUrl = "http://www.fimsform.com/pkgFedex/pkgFormService";
        private const string FimsMailViewTrackUrlFormat = "http://mailviewrecipient.fedex.com/recip_package_summary.aspx?PostalID={0}";

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
        public string CspCredentialKey => "olaPdFVk3aMvbfNA";

        /// <summary>
        /// Gets the CSP credential password.
        /// </summary>
        /// <remarks>
        /// Password - SAISQtME8lAOPurbWNQD2Ft96
        /// </remarks>
        public string CspCredentialPassword => SecureText.Decrypt("5SbhB/whSJsVfHFUoLlKA4/B2c5p2/PUgmo8d1uGaZo=", "apptive");

        /// <summary>
        /// Gets the user credentials key.
        /// </summary>
        /// <value>The user credentials key.</value>
        public string UserCredentialsKey => shippingSettings.FedExUsername;

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
        /// <remarks>
        /// This is the first part of the 2016 VersionCaptureId: ITSW6828
        /// </remarks>
        public string ClientProductId => "ITSW";

        /// <summary>
        /// Gets the client product version.
        /// </summary>
        /// <remarks>
        /// This is the last part of the 2016 VersionCaptureId: ITSW6828
        /// </remarks>
        public string ClientProductVersion => "6828";

        /// <summary>
        /// Gets the CSP solution ID given to ShipWorks from FedEx.
        /// </summary>
        /// <value>The solution ID.</value>
        public string CspSolutionId => "086";

        /// <summary>
        /// Get the FedEx endpoint URL to use
        /// </summary>
        public string EndpointUrl => settingsRepository.UseTestServer ? TestingUrl : ProductionUrl;

        /// <summary>
        /// Get the FedEx FIMS tracking endpoint URL to use
        /// </summary>
        public string FimsTrackEndpointUrlFormat => FimsMailViewTrackUrlFormat;

        /// <summary>
        /// Get the FedEx FIMS Ship endpoint URL to use
        /// </summary>
        public string FimsShipEndpointUrl => FimsMailViewShipUrl;

        /// <summary>
        /// Get the FedEx FIMS username to use
        /// </summary>
        public string FimsUsername => shippingSettings.FedExFimsUsername;

        /// <summary>
        /// Get the FedEx FIMS password to use
        /// </summary>
        public string FimsPassword => shippingSettings.FedExFimsPassword;

        /// <summary>
        /// Gets the ship version number.
        /// </summary>
        public const string ShipVersionNumber = "19";

        /// <summary>
        /// Gets the open ship version number.
        /// </summary>
        public const string OpenShipVersionNumber = "11";

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