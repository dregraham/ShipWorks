using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance.InsureShip;

namespace ShipWorks.Shipping.Insurance
{
    /// <summary>
    /// UI control for displaying insurance information
    /// </summary>
    public partial class InsuranceTabControl : UserControl
    {
        private ShipmentEntity loadedShipment;

        /// <summary>
        /// Constructor
        /// </summary>
        public InsuranceTabControl()
        {
            InitializeComponent();

            messageLabel.Location = new Point(8, 7);
            insuranceViewClaimControl.Location = new Point(0, 4);
            insuranceSubmitClaimControl.Location = new Point(0, 4);
        }

        /// <summary>
        /// Load claim data into the control.  If multiple shipments are passed, a message is displayed informing
        /// the user to select a single shipment to see insurance info.
        /// </summary>
        /// <param name="shipments"></param>
        public void LoadClaim(List<ShipmentEntity> shipments)
        {
            insuranceViewClaimControl.Visible = false;
            insuranceSubmitClaimControl.Visible = false;
            messageLabel.Visible = false;

            if (!IsValid(shipments))
            {
                loadedShipment = null;
                return;
            }

            loadedShipment = shipments.First();

            if (loadedShipment.InsurancePolicy == null)
            {
                // Only load insurance data if it is not already loaded so that in-memory changes are not overwritten
                ShipmentTypeDataService.LoadInsuranceData(loadedShipment);   
            }

            if (loadedShipment.InsurancePolicy != null)
            {
                ShowViewClaim(loadedShipment);
                ShowEditClaim(loadedShipment);
            }
        }

        /// <summary>
        /// Determines if the shipment(s) is valid.  Sets UI message text appropriately.
        /// </summary>
        /// <param name="shipments">The list of shipments</param>
        /// <returns>True if the the view or edit control can be shown.  False otherwise.</returns>
        private bool IsValid(List<ShipmentEntity> shipments)
        {
            if (!shipments.Any())
            {
                messageLabel.Text = "No shipments are selected. Select a single shipment to view insurance information.";
                messageLabel.Visible = true;
                return false; 
            }

            if (shipments.Count > 1)
            {
                messageLabel.Text = "Multiple shipments are selected. Select a single shipment to view insurance information.";
                messageLabel.Visible = true;
                return false;
            }

            ShipmentEntity shipment = shipments.First();

            if (!shipment.Processed)
            {
                messageLabel.Text = "The selected shipment has not been processed.";
                messageLabel.Visible = true;
                return false;
            }

            if (shipment.Voided)
            {
                messageLabel.Text = "This shipment has been voided and has no insurance claims.";
                messageLabel.Visible = true;
                return false;
            }

            if (!shipment.Insurance)
            {
                messageLabel.Text = "This shipment has not been insured.";
                messageLabel.Visible = true;
                return false;
            }

            if (shipment.InsuranceProvider != (int) InsuranceProvider.ShipWorks)
            {
                messageLabel.Text = "This shipment was not insured with ShipWorks insurance. ";
                AppendPotentialSavingsAmount(shipment);
                messageLabel.Visible = true;

                return false;
            }

            InsureShipSettings insureShipSettings = new InsureShipSettings();
            DateTime allowedSubmitClaimDateUtc = shipment.ShipDate + insureShipSettings.ClaimSubmissionWaitingPeriod;

            if (DateTime.UtcNow < allowedSubmitClaimDateUtc)
            {
                messageLabel.Text = string.Format("You may submit a claim on or after {0}.", allowedSubmitClaimDateUtc.ToLocalTime());
                messageLabel.Visible = true;
                insuranceViewClaimControl.Visible = false;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Show the readonly view of the claim
        /// </summary>
        private void ShowViewClaim(ShipmentEntity shipment)
        {
            insuranceViewClaimControl.LoadClaim(shipment);
        }

        /// <summary>
        /// Show the readonly editable of the claim
        /// </summary>
        private void ShowEditClaim(ShipmentEntity shipment)
        {
            insuranceSubmitClaimControl.LoadShipment(shipment);
            insuranceSubmitClaimControl.ClaimSubmitted = () => LoadClaim(new List<ShipmentEntity>() {shipment});
        }

        /// <summary>
        /// Appends the potential savings amount to the message label if ShipWorks insurance was not used and is 
        /// cheaper than the carrier insurance.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        private void AppendPotentialSavingsAmount(ShipmentEntity shipment)
        {
            if (shipment.InsuranceProvider != (int) InsuranceProvider.ShipWorks)
            {
                // Get the cost 
                ShipmentType shipmentType = ShipmentTypeManager.GetType(shipment);

                InsuranceChoice insuranceChoice = shipmentType.GetParcelDetail(shipment, 0).Insurance;
                InsuranceCost cost = InsuranceUtility.GetInsuranceCost(shipment, insuranceChoice.InsuranceValue);

                if (cost.ShipWorks > 0 && cost.Carrier.HasValue && cost.Carrier > cost.ShipWorks)
                {
                    // Show the amount of savings if ShipWorks insurance was used.
                    messageLabel.Text += string.Format("You could have saved ${0:0.00} using ShipWorks insurance.", cost.Carrier - cost.ShipWorks);
                }
            }
        }

        /// <summary>
        /// Save any changes to the shipment
        /// </summary>
        public void SaveToShipments()
        {
            if (insuranceSubmitClaimControl.Visible && loadedShipment != null)
            {
                insuranceSubmitClaimControl.SaveToPolicy(loadedShipment.InsurancePolicy);
            }
        }
    }
}
