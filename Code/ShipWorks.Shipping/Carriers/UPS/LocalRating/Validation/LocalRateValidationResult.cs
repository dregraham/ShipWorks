using System;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation
{
    /// <summary>
    /// Local Rate Validation Result
    /// </summary>
    public class LocalRateValidationResult : ILocalRateValidationResult
    {
        private readonly int shipmentCount;
        private readonly int discrepancyCount;
        private readonly IDialog upsLocalRateDiscrepancyDialog;
        private readonly IUpsLocalRateDiscrepancyViewModel discrepancyViewModel;
        private readonly Uri helpArticleUrl = new Uri("http://support.shipworks.com/support/solutions/articles/4000103270-ups-local-rating");

        /// <summary>
        /// Constructor
        /// </summary>
        public LocalRateValidationResult(int shipmentCount,
            int discrepancyCount,
            IDialog upsLocalRateDiscrepancyDialog,
            IUpsLocalRateDiscrepancyViewModel discrepancyViewModel)
        {
            this.shipmentCount = shipmentCount;
            this.discrepancyCount = discrepancyCount;
            this.upsLocalRateDiscrepancyDialog = upsLocalRateDiscrepancyDialog;
            this.discrepancyViewModel = discrepancyViewModel;

            Message = GetMessage();
        }

        /// <summary>
        /// LocalRate validated against shipment cost
        /// </summary>
        public bool IsValid => discrepancyCount == 0;

        /// <summary>
        /// Gets the validation message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Shows the validation message.
        /// </summary>
        public void ShowMessage()
        {
            if (!Message.IsNullOrWhiteSpace())
            {
                discrepancyViewModel.Load(Message, helpArticleUrl);
                upsLocalRateDiscrepancyDialog.DataContext = discrepancyViewModel;
                upsLocalRateDiscrepancyDialog.ShowDialog();
            }
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        private string GetMessage()
        {
            if (discrepancyCount > 0)
            {
                string startOfMessage = shipmentCount > 1 ?
                    $"{discrepancyCount} of {shipmentCount} UPS shipments" :
                    "The Ups shipment";
                string endOfMessage = "had local rates that did not match the rates on your UPS account. Please review and update your local rates.";

                return $"{startOfMessage} {endOfMessage}";
            }

            return string.Empty;
        }
    }
}