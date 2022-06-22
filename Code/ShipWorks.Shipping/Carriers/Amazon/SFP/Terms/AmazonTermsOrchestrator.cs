using System;
using System.Reactive;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
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

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonTermsOrchestrator(IAmazonTermsWebClient amazonTermsWebClient,
            ICurrentUserSettings currentUserSettings,
            IAmazonSfpTermsViewModel amazonSfpTermsViewModel)
        {
            this.amazonTermsWebClient = amazonTermsWebClient;
            this.currentUserSettings = currentUserSettings;
            this.amazonSfpTermsViewModel = amazonSfpTermsViewModel;
        }

        /// <summary>
        /// Do any work needed for Amazon terms acceptance
        /// </summary>
        public async Task<Unit> Handle()
        {
            var terms = await amazonTermsWebClient.GetTerms().ConfigureAwait(false);

            if (terms == null || string.IsNullOrWhiteSpace(terms.Version))
            {
                // No terms to agree to, just return
                return Unit.Default;
            }
            
            var  shouldShow = currentUserSettings.ShouldShowNotification(UserConditionalNotificationType.AmazonTermsAndConditions, DateTime.UtcNow) ||
                              DateTime.Parse(terms.DeadlineDate) <= DateTime.UtcNow;
            
            if (shouldShow)
            {
                await amazonSfpTermsViewModel.Show(terms).ConfigureAwait(false);
                TermsAccepted = amazonSfpTermsViewModel.TermsAccepted;
            }

            return Unit.Default;
        }

        /// <summary>
        /// Have the terms been accepted
        /// </summary>
        public bool TermsAccepted { get; set; }
    }
}
