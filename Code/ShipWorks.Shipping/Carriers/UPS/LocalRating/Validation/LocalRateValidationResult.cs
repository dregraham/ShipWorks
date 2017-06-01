using System;
using Interapptive.Shared.UI;
using ShipWorks.Shipping.Services.ProcessShipmentsWorkflow;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation
{
    /// <summary>
    /// Local Rate Validation Result
    /// </summary>
    public class LocalRateValidationResult : ILocalRateValidationResult
    {
        private readonly int totalShipmentsValidated;
        private readonly int shipmentsWithRateDiscrepancies;
        private readonly IDialog upsLocalRateDiscrepancyDialog;
        private readonly IUpsLocalRateDiscrepancyViewModel discrepancyViewModel;
        private readonly Uri helpArticleUrl = new Uri("http://support.shipworks.com/support/solutions/articles/4000103270-ups-local-rating");

        /// <summary>
        /// Constructor
        /// </summary>
        public LocalRateValidationResult(int totalShipmentsValidated, 
            int shipmentsWithRateDiscrepancies,
            IDialog upsLocalRateDiscrepancyDialog,
            IUpsLocalRateDiscrepancyViewModel discrepancyViewModel)
        {
            this.totalShipmentsValidated = totalShipmentsValidated;
            this.shipmentsWithRateDiscrepancies = shipmentsWithRateDiscrepancies;
            this.upsLocalRateDiscrepancyDialog = upsLocalRateDiscrepancyDialog;
            this.discrepancyViewModel = discrepancyViewModel;
        }

        /// <summary>
        /// Gets the validation message.
        /// </summary>
        public string Message
        {
            get
            {
                if (ValidationFailed)
                {
                    string startOfMessage = totalShipmentsValidated > 1
                        ? $"{shipmentsWithRateDiscrepancies} of {totalShipmentsValidated} UPS shipments"
                        : "The Ups shipment";
                    string endOfMessage =
                        "had local rates that did not match the rates on your UPS account. Please review and update your local rates.";

                    return $"{startOfMessage} {endOfMessage}";
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Prepends the message to workflow result error message list.
        /// </summary>
        public void PrependMessageToWorkflowResultErrors(IProcessShipmentsWorkflowResult result)
        {
            result.NewErrors.Insert(0, Message);
        }

        /// <summary>
        /// Shows the validation message.
        /// </summary>
        public void ShowMessage()
        {
            if (ValidationFailed)
            {
                discrepancyViewModel.Load(Message, helpArticleUrl);
                upsLocalRateDiscrepancyDialog.DataContext = discrepancyViewModel;
                upsLocalRateDiscrepancyDialog.ShowDialog();
            }
        }

        /// <summary>
        /// Validation fails if any rate discrepancies
        /// </summary>
        private bool ValidationFailed => shipmentsWithRateDiscrepancies != 0;
    }
}