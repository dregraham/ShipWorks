using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;

namespace ShipWorks.Shipping.Carriers.UPS.UpsEnvironment
{
    /// <summary>
    /// Encapsulates the settings that are common to UPS services (credentials, client production ID, etc.). The repository
    /// is used for fetching persisted settings such as whether to use a test server.
    /// </summary>
    public class UpsSettings
    {
        // TODO: verify production URL when we receive it from Ups
        private const string ProductionUrl = "https://onlinetools.ups.com/";
        private const string TestingUrl = "https://wwwcie.ups.com";

        private readonly ICarrierSettingsRepository settingsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsSettings" /> class.
        /// </summary>
        /// <param name="settingsRepository">The settings repository.</param>
        public UpsSettings(ICarrierSettingsRepository settingsRepository)
        {
            this.settingsRepository = settingsRepository;
        }

        /// <summary>
        /// Get the UpsOpenAccount endpoint URL to use
        /// </summary>
        public string EndpointUrl
        {
            get
            {
                return settingsRepository.UseTestServer ? TestingUrl : ProductionUrl;
            }
        }
    }
}
