using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.UI.Wizard;
using ShipWorks.Shipping.Editing;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    public partial class CounterRateProcessingWizardPage : WizardPage
    {
        private bool usingAvailableRate;
        private readonly RateResult absoluteBestRate;

        /// <summary>
        /// Initializes a new instance of the <see cref="CounterRateProcessingWizardPage" /> class.
        /// </summary>
        /// <param name="filteredRates">The filtered rates.</param>
        /// <param name="allRates">All rates.</param>
        /// <param name="shipmentsInBatch">All of the shipments being processed in the current batch.</param>
        public CounterRateProcessingWizardPage(RateGroup filteredRates, RateGroup allRates, IEnumerable<ShipmentEntity> shipmentsInBatch)
        {
            InitializeComponent();
            
            FilteredRates = filteredRates;
            AllRates = allRates;
            
            usingAvailableRate = false;

            // Make note of this otherwise we'd be running Rates.First() a number of times
            absoluteBestRate = filteredRates.Rates.First();
            
            // Swap out the "tokenized" provider text with that of the provider with the best rate
            bestRateDescription.Text = ReplaceTokensInDescription(bestRateDescription.Text, absoluteBestRate.ShipmentType, absoluteBestRate.Amount);
            Description = ReplaceTokensInDescription(Description, absoluteBestRate.ShipmentType, absoluteBestRate.Amount);

            if (shipmentsInBatch.Count() <= 1)
            {
                // There's only one shipment, so it doesn't make sense to show the UI 
                // for the user to ignore counter rates for the rest of the batch
                ignoreCounterRates.Visible = false;
            }

            if (AllRates.Rates.All(r => r.IsCounterRate))
            {
                // There aren't any rates for an existing account, so remove the UI 
                // for the user choose to use an available rate and change the verbiage
                // of the title so it doesn't read as though we're comparing the counter
                // rate to a rate for an account that is already setup
                availableRatePanel.Visible = false;
                Title = @"It looks like you need to add a shipping account to ShipWorks";
            }
            else
            {
                // Find the difference between the available rate and the cheapest rate to 
                // display for comparison purposes
                RateResult bestAvailableRate = AllRates.Rates.First(r => !r.IsCounterRate);

                // We need to swap out the tokenized values in the more expensive rate text
                decimal difference = bestAvailableRate.Amount - absoluteBestRate.Amount;
                moreExpensiveAvailableRate.Text = ReplaceTokensInDescription(moreExpensiveAvailableRate.Text, bestAvailableRate.ShipmentType, difference);
            }

            
        }

        /// <summary>
        /// Gets all rates.
        /// </summary>
        public RateGroup AllRates { get; private set; }

        /// <summary>
        /// Gets the filtered rates.
        /// </summary>
        public RateGroup FilteredRates { get; private set; }

        /// <summary>
        /// Gets the selected rate.
        /// </summary>
        public RateResult SelectedRate
        {
            get
            {
                RateResult selectedRate = absoluteBestRate;

                if (usingAvailableRate)
                {
                    // The user opted to use a rate from an existing account, so grab the first 
                    // non-counter rate from the list containing all of the rates. All rates is 
                    // used as the source, so any service types that were filtered out based on 
                    // the cheapest counter rate still applied appropriately
                    selectedRate = AllRates.Rates.First(r => !r.IsCounterRate);
                }

                return selectedRate;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the user chose to [ignore all counter rates] for
        /// the batch of shipments being processed.
        /// </summary>
        public bool IgnoreAllCounterRates
        {
            get
            {
                // Set the property to ignore all counter rates to false if the user did not 
                // choose the option to use an available rate in addition to checking the box.
                // This avoids the incorrect value being returned when the user has the ignore
                // option checked, but continued to setup an account anyway
                return usingAvailableRate && ignoreCounterRates.Checked;
            }
        }

        /// <summary>
        /// A helper method that replaces the {ProviderName} and {Amount} tokens in the source
        /// string with the corresponding values from the given rate.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="rate">The rate.</param>
        /// <returns>The formatted string with the actual provider name and rate amount.</returns>
        private string ReplaceTokensInDescription(string source, ShipmentTypeCode shipmentTypeCode, decimal amount)
        {
            // Swap out the generic provider text with that of the provider with 
            // the values from the rate
            string providerName = EnumHelper.GetDescription(shipmentTypeCode);
            
            string result = source.Replace("{ProviderName}", providerName);
            result = result.Replace("{Amount}", string.Format("{0:C2}", amount));

            return result;
        }

        /// <summary>
        /// Called when [use available rate link clicked].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="LinkLabelLinkClickedEventArgs"/> instance containing the event data.</param>
        private void OnUseAvailableRateLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Note that the user chose to use the available rate and close the wizard
            // hosting this page.
            usingAvailableRate = true;

            // Need to explicitly set the dialog result to indicate that the user
            // actually chose an option rather than canceling
            Wizard.DialogResult = DialogResult.OK;            
            Wizard.Close();
        }
    }
}
