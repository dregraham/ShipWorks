using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using ShipWorks.Shipping.Editing;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    public partial class CounterRateProcessingWizardPage : WizardPage
    {
        private readonly IEnumerable<ShipmentEntity> shipmentsInBatch;
        private bool usingAvailableRate;

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

            this.shipmentsInBatch = shipmentsInBatch;
            usingAvailableRate = false;

            if (this.shipmentsInBatch.Count() > 1)
            {
                // There's only one shipment, so it doesn't make sense to show the UI 
                // for the user to ignore counter rates for the rest of the batch
                ignoreCounterRates.Visible = false;
            }

            if (AllRates.Rates.All(r => r.IsCounterRate))
            {
                // There aren't any rates for an existing account, so remove 
                // the UI for the user choose to use an available rate
                availableRatePanel.Visible = false;
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
                RateResult selectedRate = FilteredRates.Rates.First();

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
        /// Called when [use available rate link clicked].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="LinkLabelLinkClickedEventArgs"/> instance containing the event data.</param>
        private void OnUseAvailableRateLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Note that the user chose to use the available rate and close the wizard
            // hosting this page.
            usingAvailableRate = true;            
            Wizard.Close();
        }
    }
}
