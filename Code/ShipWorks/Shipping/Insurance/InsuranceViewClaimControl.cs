using System;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance.InsureShip;

namespace ShipWorks.Shipping.Insurance
{
    /// <summary>
    /// UI for displaying a readonly view of an Insurance Claim
    /// </summary>
    public partial class InsuranceViewClaimControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public InsuranceViewClaimControl()
        {
            InitializeComponent();

            ClearValues();
        }

        /// <summary>
        /// Populate the form fields 
        /// </summary>
        public void LoadClaim(ShipmentEntity shipment)
        {
            if (shipment.Voided || shipment.InsurancePolicy == null || !shipment.InsurancePolicy.ClaimID.HasValue)
            {
                Visible = false;
                return;
            }

            InsurancePolicyEntity insurancePolicy = shipment.InsurancePolicy;

            claimType.Text = EnumHelper.GetDescription((InsureShipClaimType) insurancePolicy.ClaimType);
            itemName.Text = insurancePolicy.ItemName;
            damageValue.Text = insurancePolicy.DamageValue.Value.ToString("C");
            submittedOn.Text = insurancePolicy.SubmissionDate.Value.ToLocalTime().ToString("g");

            Visible = true;
        }

        /// <summary>
        /// Clear current values to get ready for next policy
        /// </summary>
        private void ClearValues()
        {
            claimType.Text = string.Empty;
            itemName.Text = string.Empty;
            damageValue.Text = string.Empty;
            submittedOn.Text = string.Empty;
        }
    }
}
