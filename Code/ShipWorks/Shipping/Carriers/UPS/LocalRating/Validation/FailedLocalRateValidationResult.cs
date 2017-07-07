using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Dashboard;
using ShipWorks.ApplicationCore.Dashboard.Content;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services.ProcessShipmentsWorkflow;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation
{
    /// <summary>
    /// Local Rate Validation Result
    /// </summary>
    public class FailedLocalRateValidationResult : ILocalRateValidationResult
    {
        private readonly IDialog upsLocalRateDiscrepancyDialog;
        private readonly IUpsLocalRateDiscrepancyViewModel discrepancyViewModel;
        private readonly Uri helpArticleUrl = new Uri("http://support.shipworks.com/support/solutions/articles/4000103804-ups-local-rating-troubleshooting-guide");

        /// <summary>
        /// Constructor
        /// </summary>
        public FailedLocalRateValidationResult(IEnumerable<UpsLocalRateDiscrepancy> rateDiscrepancies,
            IEnumerable<ShipmentEntity> shipments,
            IDialog upsLocalRateDiscrepancyDialog,
            IUpsLocalRateDiscrepancyViewModel discrepancyViewModel)
        {
            RateDiscrepancies = rateDiscrepancies;

            Shipments = shipments;
            this.upsLocalRateDiscrepancyDialog = upsLocalRateDiscrepancyDialog;
            this.discrepancyViewModel = discrepancyViewModel;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FailedLocalRateValidationResult"/> class.
        /// </summary>
        /// <param name="rateDiscrepancies">The rate discrepancies.</param>
        /// <param name="shipments"></param>
        public FailedLocalRateValidationResult(IEnumerable<UpsLocalRateDiscrepancy> rateDiscrepancies, IEnumerable<ShipmentEntity> shipments)
        {
            Shipments = shipments;
            RateDiscrepancies = rateDiscrepancies;
        }

        public IEnumerable<ShipmentEntity> Shipments { get; }

        /// <summary>
        /// Gets the local rates that we're validated to get this result
        /// </summary>
        public IEnumerable<UpsLocalRateDiscrepancy> RateDiscrepancies { get; }

        /// <summary>
        /// Handles a validation failure.
        /// </summary>
        public void HandleValidationFailure(IProcessShipmentsWorkflowResult workflowResult)
        {
            if (workflowResult.NewErrors.Any())
            {
                workflowResult.NewErrors.Insert(0, GetMessage());
            }
            else
            {
                ShowMessage();
            }
        }

        /// <summary>
        /// Create a dashboard message to show the failure
        /// </summary>
        public DashboardItem CreateDashboardMessage()
        {
            return new DashboardLocalMessageItem("FailedLocalRatingValidation", DashboardMessageImageType.Warning,
                "UPS rates may have changed. Please review and update your local rates.", string.Empty,
                new DashboardActionUrl("[link]View Details[/link]", helpArticleUrl));
        }

        /// <summary>
        /// Gets the user friendly message.
        /// </summary>
        public string GetUserFriendlyMessage()
        {
            return string.Join(Environment.NewLine,
                RateDiscrepancies.Select(rateDiscrepancy => rateDiscrepancy.GetUserMessage()).ToList());
        }

        /// <summary>
        /// Gets the validation message.
        /// </summary>
        private string GetMessage()
        {
            int shipmentCount = Shipments.Count();

            string startOfMessage = shipmentCount > 1 ?
                $"{RateDiscrepancies.Count()} of the {shipmentCount} successfully processed UPS shipments" :
                "The UPS shipment";
            string endOfMessage =
                "had local rates that did not match the rates on your UPS account. Please review and update your local rates.";

            return $"{startOfMessage} {endOfMessage}";
        }

        /// <summary>
        /// Shows the validation message.
        /// </summary>
        private void ShowMessage()
        {
            discrepancyViewModel.Load(GetMessage(), helpArticleUrl);
            upsLocalRateDiscrepancyDialog.DataContext = discrepancyViewModel;
            upsLocalRateDiscrepancyDialog.ShowDialog();
        }
    }
}