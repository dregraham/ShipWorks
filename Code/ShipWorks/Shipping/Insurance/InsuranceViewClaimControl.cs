using System;
using System.Globalization;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance.InsureShip;
using ShipWorks.Stores;

namespace ShipWorks.Shipping.Insurance
{
    /// <summary>
    /// UI for displaying a readonly view of an Insurance Claim
    /// </summary>
    public partial class InsuranceViewClaimControl : UserControl
    {
        private ShipmentEntity shipment;

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
            this.shipment = shipment;
            InsurancePolicyEntity insurancePolicy = shipment.InsurancePolicy;

            if (insurancePolicy == null)
            {
                return;
            }

            claimType.Text = EnumHelper.GetDescription((InsureShipClaimType) insurancePolicy.ClaimType);
            
            itemName.Text = insurancePolicy.ItemName;
            description.Text = insurancePolicy.Description;
            email.Text = insurancePolicy.EmailAddress;

            damageValue.Text = insurancePolicy.DamageValue.GetValueOrDefault().ToString("C");
            submittedOn.Text = insurancePolicy.SubmissionDate.GetValueOrDefault().ToLocalTime().ToString("g");
            
            claimID.Text = insurancePolicy.ClaimID.GetValueOrDefault().ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Get the claim status for the shipment's insurance policy
        /// </summary>
        private string FetchClaimStatus()
        {
            StoreEntity storeEntity = StoreManager.GetStore(shipment.Order.StoreID);
            if (storeEntity == null)
            {
                throw new InsureShipException("The store the shipment was in has been deleted.");
            }

            InsureShipAffiliate insureShipAffiliate = TangoWebClient.GetInsureShipAffiliate(storeEntity);

            InsureShipClaim claim = new InsureShipClaim(shipment, insureShipAffiliate);
            return claim.CheckStatus();
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
            description.Text = string.Empty;
            claimID.Text = string.Empty;
            claimStatus.Text = string.Empty;
        }

        /// <summary>
        /// Called when the check status button is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnStatusButtonClick(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                claimStatus.Text = FetchClaimStatus();
            }
            catch (InsureShipException)
            {
                MessageHelper.ShowError(this, "ShipWorks was unable to check the status of your claim. Please try again later or contact InsureShip to check your claim status.");
            }
        }
    }
}
