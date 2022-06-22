using System;
using System.Reactive;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using log4net;
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
                    return Unit.Default;
                }

                var terms = await amazonTermsWebClient.GetTerms().ConfigureAwait(false);

                if (terms == null || string.IsNullOrWhiteSpace(terms.Version))
                {
                    // No terms to agree to, just return
                    return Unit.Default;
                }
                
                var shouldShow = currentUserSettings.ShouldShowNotification(UserConditionalNotificationType.AmazonTermsAndConditions, DateTime.UtcNow) ||
                                 DateTime.Parse(terms.DeadlineDate) <= DateTime.UtcNow;

                if (shouldShow)
                {
                    await amazonSfpTermsViewModel.Show(terms).ConfigureAwait(false);
                    TermsAccepted = amazonSfpTermsViewModel.TermsAccepted;
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
