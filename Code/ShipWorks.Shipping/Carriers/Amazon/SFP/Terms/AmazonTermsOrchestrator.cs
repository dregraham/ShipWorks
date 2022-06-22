using System;
using System.Reactive;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using log4net;
using Newtonsoft.Json;
using ShipWorks.Shipping.Settings;
using ShipWorks.Users;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP.Terms
{
    /// <summary>
    /// Class for interacting with the user and AmazonTerms
    /// </summary>
    [Component]
    public class AmazonTermsOrchestrator : IAmazonTermsOrchestrator
    {
        private readonly ICurrentUserSettings currentUserSettings;
        private readonly IAmazonSfpTermsViewModel amazonSfpTermsViewModel;
        private readonly IAmazonTermsWebClient amazonTermsWebClient;
        private readonly IShippingSettings shippingSettings;
        private static readonly ILog log = LogManager.GetLogger(typeof(AmazonTermsOrchestrator));

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonTermsOrchestrator(IAmazonTermsWebClient amazonTermsWebClient,
            ICurrentUserSettings currentUserSettings,
            IAmazonSfpTermsViewModel amazonSfpTermsViewModel,
            IShippingSettings shippingSettings)
        {
            this.amazonTermsWebClient = amazonTermsWebClient;
            this.currentUserSettings = currentUserSettings;
            this.amazonSfpTermsViewModel = amazonSfpTermsViewModel;
            this.shippingSettings = shippingSettings;
        }

        /// <summary>
        /// Do any work needed for Amazon terms acceptance
        /// </summary>
        public async Task<Unit> Handle()
        {
            try
            {
                if (!shippingSettings.IsConfigured(ShipmentTypeCode.AmazonSFP))
                {
                    log.Info("Amazon Buy Shipping is not configured, so skipping additional terms checks.");
                    return Unit.Default;
                }

                var terms = await amazonTermsWebClient.GetTerms().ConfigureAwait(false);

                if (terms == null || string.IsNullOrWhiteSpace(terms.Version))
                {
                    log.Info("No Amazon Buy Shipping terms found from Hub, so skipping additional terms checks.");
                    // No terms to agree to, just return
                    return Unit.Default;
                }

                log.Info($"Terms received: {JsonConvert.SerializeObject(terms)}");

                var currentUserSettingsShouldShow = currentUserSettings.ShouldShowNotification(UserConditionalNotificationType.AmazonTermsAndConditions, DateTime.UtcNow);
                var deadlinePassed = DateTime.Parse(terms.DeadlineDate) <= DateTime.UtcNow;

                var shouldShow = currentUserSettingsShouldShow || deadlinePassed;

                log.Info($"currentUserSettingsShouldShow: {currentUserSettingsShouldShow}, deadlinePassed: {deadlinePassed}, shouldShow: {shouldShow}");

                if (shouldShow)
                {
                    log.Info("Show the Amazon Buy Shipping Terms.");
                    await amazonSfpTermsViewModel.Show(terms).ConfigureAwait(false);

                    TermsAccepted = amazonSfpTermsViewModel.TermsAccepted;

                    log.Info($"User accepted terms: {TermsAccepted}");
                }
            }
            catch (Exception ex)
            {
                // Just log and continue
                log.Error(ex.Message, ex);
            }

            return Unit.Default;
        }

        /// <summary>
        /// Have the terms been accepted
        /// </summary>
        public bool TermsAccepted { get; set; }
    }
}
