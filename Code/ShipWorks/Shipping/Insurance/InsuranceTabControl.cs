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

namespace ShipWorks.Shipping.Insurance
{
    /// <summary>
    /// UI control for displaying insurance information
    /// </summary>
    public partial class InsuranceTabControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public InsuranceTabControl()
        {
            InitializeComponent();

            messageLabel.Location = new Point(4, 8);
            insuranceViewClaimControl.Location = new Point(0, 4);
        }

        /// <summary>
        /// Load claim data into the control.  If multiple shipments are passed, an message is displayed informing
        /// the user to select a single shipment to see insurance info.
        /// </summary>
        /// <param name="shipments"></param>
        public void LoadClaim(List<ShipmentEntity> shipments)
        {
            if (shipments.Count > 1)
            {
                messageLabel.Text = "Multiple shipments are selected. Select a single shipment to view insurance information.";
                messageLabel.Visible = true;
                insuranceViewClaimControl.Visible = false;
                return;
            }
            
            ShipmentEntity shipment = shipments.First();

            if (!shipment.Processed)
            {
                messageLabel.Text = "The selected shipment has not been processed.";
                messageLabel.Visible = true;
                insuranceViewClaimControl.Visible = false;
                return;
            }

            if (shipment.Voided)
            {
                messageLabel.Text = "This shipment has been voided and has no insurance claims.";
                messageLabel.Visible = true;
                insuranceViewClaimControl.Visible = false;
                return;
            }

            if (!shipment.Insurance)
            {
                messageLabel.Text = "This shipment has not been insured.";
                messageLabel.Visible = true;
                insuranceViewClaimControl.Visible = false;
                return;
            }

            messageLabel.Visible = false;
            
            ShipmentTypeDataService.LoadInsuranceData(shipment);

            if (shipment.InsurancePolicy != null)
            {
                ShowViewClaim(shipment);
            }
            else
            {
                ShowEditClaim(shipment);
            }
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
        }
    }
}
