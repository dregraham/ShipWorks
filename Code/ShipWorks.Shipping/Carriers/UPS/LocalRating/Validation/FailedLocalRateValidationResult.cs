﻿using System;
using System.Linq;
using Interapptive.Shared.UI;
using ShipWorks.Shipping.Services.ProcessShipmentsWorkflow;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation
{
    /// <summary>
    /// Local Rate Validation Result
    /// </summary>
    public class FailedLocalRateValidationResult : ILocalRateValidationResult
    {
        private readonly int totalShipmentsValidated;
        private readonly int shipmentsWithRateDiscrepancies;
        private readonly IDialog upsLocalRateDiscrepancyDialog;
        private readonly IUpsLocalRateDiscrepancyViewModel discrepancyViewModel;
        private readonly Uri helpArticleUrl = new Uri("http://support.shipworks.com/support/solutions/articles/4000103270-ups-local-rating");

        /// <summary>
        /// Constructor
        /// </summary>
        public FailedLocalRateValidationResult(int totalShipmentsValidated, 
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
        /// Gets the validation message.
        /// </summary>
        private string GetMessage()
        {
            string startOfMessage = totalShipmentsValidated > 1
                ? $"{shipmentsWithRateDiscrepancies} of {totalShipmentsValidated} UPS shipments"
                : "The UPS shipment";
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