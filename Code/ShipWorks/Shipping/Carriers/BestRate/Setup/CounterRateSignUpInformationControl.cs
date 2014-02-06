using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Shipping.Editing;
using Interapptive.Shared.Utility;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Shipping.Carriers.BestRate.Setup
{
    public partial class CounterRateSignUpInformationControl : UserControl
    {
        private readonly RateResult bestRate;

        /// <summary>
        /// Initializes a new instance of the <see cref="CounterRateSignUpInformationControl"/> class.
        /// </summary>
        /// <param name="bestRate">The best rate.</param>
        public CounterRateSignUpInformationControl(RateResult bestRate)
        {
            InitializeComponent();

            this.bestRate = bestRate;
        }

        /// <summary>
        /// Called when the control is loaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnLoad(object sender, EventArgs e)
        {
            carrierLogo.Image = EnumHelper.GetImage(bestRate.ShipmentType);
            carrierName.Text = EnumHelper.GetDescription(bestRate.ShipmentType);
            rateAmount.Text = string.Format("{0:C2}", bestRate.Amount);
        }

        /// <summary>
        /// Called when the sign up button is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnSignUp(object sender, EventArgs e)
        {
            ShipmentType shipmentType = ShipmentTypeManager.GetType(bestRate.ShipmentType);

            WizardForm hostWizard = ParentForm as WizardForm;
            if (hostWizard != null)
            {
                // Transition to the setup wizard for the actual shipment type
                //using (WizardForm setupWizard = shipmentType.CreateSetupWizard())
                //{
                //}
            }
            else
            {
                using (WizardForm setupWizard = shipmentType.CreateSetupWizard())
                {
                    setupWizard.ShowDialog(this);
                }
            }
        }
    }
}
