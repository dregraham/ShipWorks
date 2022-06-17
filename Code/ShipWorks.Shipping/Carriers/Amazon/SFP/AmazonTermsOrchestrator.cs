using System;
using System.Reactive;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using Newtonsoft.Json;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Shipping.Carriers.Amazon.SFP.DTO;
using ShipWorks.Shipping.Carriers.Amazon.SFP.Terms;
using ShipWorks.Users;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP
{
    /// <summary>
    /// Class for interacting with the user and AmazonTerms
    /// </summary>
    [Component]
    public class AmazonTermsOrchestrator : IAmazonTermsOrchestrator
    {
        private const string GetTermsUrl = "";
        private const string UpdateTermsUrl = "";

        private readonly IWarehouseRequestClient warehouseRequestClient;
        private readonly ICurrentUserSettings currentUserSettings;
        private readonly IAmazonSfpTermsViewModel amazonSfpTermsViewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonTermsOrchestrator(IWarehouseRequestClient warehouseRequestClient,
            ICurrentUserSettings currentUserSettings,
            IAmazonSfpTermsViewModel amazonSfpTermsViewModel)
        {
            this.warehouseRequestClient = warehouseRequestClient;
            this.currentUserSettings = currentUserSettings;
            this.amazonSfpTermsViewModel = amazonSfpTermsViewModel;
        }

        /// <summary>
        /// Do any work needed for Amazon terms acceptance
        /// </summary>
        public async Task<Unit> Handle()
        {
            var terms = await GetTerms().ConfigureAwait(false);

            if (terms == null)
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

        /// <summary>
        /// Make a call to get the latest Amazon terms
        /// </summary>
        private async Task<AmazonTermsVersion> GetTerms()
        {
            var request = new RestRequest(GetTermsUrl, Method.GET);

            // There's an issue with the deserialization and casting to an interface so we're casting manually
            var result = await warehouseRequestClient.MakeRequest(request, "GetAmazonTerms", ApiLogSource.Amazon).ConfigureAwait(false);

            // Check that the call returned valid information
            AmazonTermsVersion returnObject;
            if (string.IsNullOrWhiteSpace(result.Value?.Content))
            {
                returnObject = new AmazonTermsVersion();
            }
            else
            {
                returnObject = JsonConvert.DeserializeObject<AmazonTermsVersion>(result.Value.Content);
            }

            return returnObject;
        }
    }
}
