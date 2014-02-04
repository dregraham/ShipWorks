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
        private readonly WizardForm host;
        private readonly IEnumerable<ShipmentEntity> shipmentsInBatch;

        /// <summary>
        /// Initializes a new instance of the <see cref="CounterRateProcessingWizardPage" /> class.
        /// </summary>
        /// <param name="filteredRates">The filtered rates.</param>
        /// <param name="allRates">All rates.</param>
        /// <param name="host">The Wizard form that is hosting this page.</param>
        /// <param name="shipmentsInBatch">All of the shipments being processed in the current batch.</param>
        public CounterRateProcessingWizardPage(RateGroup filteredRates, RateGroup allRates, WizardForm host, IEnumerable<ShipmentEntity> shipmentsInBatch)
        {
            InitializeComponent();
            
            FilteredRates = filteredRates;
            AllRates = allRates;

            this.host = host;
            this.shipmentsInBatch = shipmentsInBatch;

            if (this.shipmentsInBatch.Count() > 1)
            {
                // TODO: remove the UI for the user to ignore counter rates for the rest of the batch
            }

            if (AllRates.Rates.All(r => r.IsCounterRate))
            {
                // TODO: remove the UI for the user choose to use an available rate
            }

            this.host.FormClosing += FinalizeValues;
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
        public RateResult SelectedRate { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the user chose to [ignore all counter rates] for
        /// the batch of shipments being processed.
        /// </summary>
        public bool IgnoreAllCounterRates { get; private set; }

        /// <summary>
        /// Finalizes the values.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>
        private void FinalizeValues(object sender, FormClosingEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
