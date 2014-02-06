using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Shipping.Carriers.BestRate.Setup
{
    public partial class CounterRateProcessingSetupWizard : WizardForm
    {
        private readonly RateResult absoluteBestRate;

        public CounterRateProcessingSetupWizard(RateGroup filteredRates, RateGroup allRates, IEnumerable<ShipmentEntity> shipmentsInBatch)
        {
            InitializeComponent();
            
            FilteredRates = filteredRates;
            AllRates = allRates;

            // Assume the user will opt to use an existing account, so we only have to 
            // change the value in in one spot (when signing up for an account)
            IgnoreAllCounterRates = true;

            // Make note of this otherwise we'd be running Rates.First() a number of times
            absoluteBestRate = filteredRates.Rates.First();

            // Swap out the "tokenized" provider text with that of the provider with the best rate
            LoadBestRateInfo();
            LoadExistingAccountRateInfo();

            if (shipmentsInBatch.Count() <= 1)
            {
                // There's only one shipment, so it doesn't make sense to show the UI 
                // regarding remaining shipments in the batch
                useExistingAccountsForRemainingLabel.Visible = false;
            }

            NextEnabled = false;
            NextVisible = false;
        }

        /// <summary>
        /// Gets all rates.
        /// </summary>
        public RateGroup AllRates
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the filtered rates.
        /// </summary>
        public RateGroup FilteredRates
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the selected rate.
        /// </summary>
        public RateResult SelectedRate
        {
            get
            {
                RateResult selectedRate = absoluteBestRate;

                if (IgnoreAllCounterRates)
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
        /// Gets or sets the selected shipment type based on the rate that
        /// the user selected.
        /// </summary>
        public ShipmentType SelectedShipmentType
        {
            get; 
            private set; 
        }

        /// <summary>
        /// Gets a value indicating whether the user chose to [ignore all counter rates] for
        /// the batch of shipments being processed.
        /// </summary>
        public bool IgnoreAllCounterRates
        {
            get;
            private set;
        }


        /// <summary>
        /// Loads the rate information for the best rate that was found.
        /// </summary>
        private void LoadBestRateInfo()
        {
            bestRateCarrierName.Text = EnumHelper.GetDescription(absoluteBestRate.ShipmentType);
            bestRateAmount.Text = string.Format("{0:C2}", absoluteBestRate.Amount);
            createAccountCarrierLogo.Image = EnumHelper.GetImage(absoluteBestRate.ShipmentType);
        }

        /// <summary>
        /// Loads the rate information for the "existing account" panels.
        /// </summary>
        private void LoadExistingAccountRateInfo()
        {
            // We're always only going to show one of the "existing account" panels, so adjust
            // the location to be on top of each other
            useExistingAccountPanel.Top = addExistingAccountPanel.Top;

            RateResult existingAccountRate = FilteredRates.Rates.First(r => !r.IsCounterRate);

            if (existingAccountRate != null)
            {
                // We have a rate that is not a counter rate, so we know the user has an
                // account setup, so we'll hide the panel for adding an account, adjust 
                // the location of the "use existing" panel, and resize the height of
                // the wizard accordingly
                addExistingAccountPanel.Visible = false;
                useExistingAccountPanel.BringToFront();
                Height = useExistingAccountPanel.Bottom + 135;

                useExistingCarrierLogo.Image = EnumHelper.GetImage(existingAccountRate.ShipmentType);
                useExistingAccountDescription.Text = useExistingAccountDescription.Text.Replace("{ProviderName}", EnumHelper.GetDescription(existingAccountRate.ShipmentType));
                //TODO: useExistingAccountDescription.Text = useExistingAccountDescription.Text.Replace({"{AccountDescription}", existingAccountRate.})

                // Show the actual amount and the difference between the best rate and 
                // the cheapest available rate
                existingAccountRateAmount.Text = string.Format("{0:C2}", existingAccountRate.Amount);

                decimal difference = existingAccountRate.Amount - absoluteBestRate.Amount;
                existingAccountRateDifference.Text = string.Format("({0:C2} more)", difference);
            }
            else
            {
                // There are only counter rates available, so we want to hide the panel that allows
                // the user to select a rate from an existing account
                useExistingAccountPanel.Visible = false;
                addExistingAccountPanel.BringToFront();
                Height = addExistingAccountPanel.Bottom + 90;

                LoadShippingProviders();
            }
        }

        /// <summary>
        /// Loads the shipping providers into the setup existing provider combo box.
        /// </summary>
        private void LoadShippingProviders()
        {
            EnumHelper.BindComboBox<ShipmentTypeCode>(setupExistingProvider);
        }


        /// <summary>
        /// Called when the sign up button is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnSignUp(object sender, System.EventArgs e)
        {
            SelectedShipmentType = ShipmentTypeManager.GetType(absoluteBestRate.ShipmentType);
            IgnoreAllCounterRates = false;

            using (WizardForm setupWizard = SelectedShipmentType.CreateSetupWizard())
            {
                // TODO: Make smooth transition to the setup wizard for the actual shipment type
                DialogResult = setupWizard.ShowDialog(this);
                Close();
            }
        }

        /// <summary>
        /// Called when the "Add my account" button is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnAddExistingAccount(object sender, System.EventArgs e)
        {
            SelectedShipmentType = ShipmentTypeManager.GetType((ShipmentTypeCode)setupExistingProvider.SelectedItem);
            IgnoreAllCounterRates = true;

            using (WizardForm setupWizard = SelectedShipmentType.CreateSetupWizard())
            {
                // TODO: Make smooth transition to the setup wizard for the actual shipment type
                DialogResult = setupWizard.ShowDialog(this);
                Close();
            }
        }

        /// <summary>
        ///  Called when the "Use my existing account" button is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnUseExistingAccount(object sender, System.EventArgs e)
        {
            RateResult existingAccountRate = FilteredRates.Rates.First(r => !r.IsCounterRate);

            SelectedShipmentType = ShipmentTypeManager.GetType(existingAccountRate.ShipmentType);
            IgnoreAllCounterRates = true;

            using (WizardForm setupWizard = SelectedShipmentType.CreateSetupWizard())
            {
                // TODO: Make smooth transition to the setup wizard for the actual shipment type
                DialogResult = setupWizard.ShowDialog(this);
                Close();
            }
        }
    }
}
