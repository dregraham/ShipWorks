using System;
using System.Windows.Forms;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance.InsureShip;
using ShipWorks.Stores;

namespace ShipWorks.Shipping.Insurance
{
    /// <summary>
    /// Control to submit a claim to InsureShip
    /// </summary>
    public partial class InsuranceSubmitClaimControl : UserControl
    {
        private ShipmentEntity shipment;
        static readonly ILog log = LogManager.GetLogger(typeof(InsuranceSubmitClaimControl));

        /// <summary>
        /// Initializes a new instance of the <see cref="InsuranceSubmitClaimControl"/> class.
        /// </summary>
        public InsuranceSubmitClaimControl()
        {
            InitializeComponent();

            EnumHelper.BindComboBox<InsureShipClaimType>(claimType);
        }

        /// <summary>
        /// Gets or sets the claim submitted.
        /// </summary>
        public Action ClaimSubmitted { get; set; }

        /// <summary>
        /// Loads the shipment.
        /// </summary>
        public void LoadShipment(ShipmentEntity shipment)
        {
            this.shipment = shipment;

            // Make sure that controls are reset even if the data is empty
            claimType.SelectedValue = (InsureShipClaimType) shipment.InsurancePolicy.ClaimType.GetValueOrDefault((int) InsureShipClaimType.Damage);
            damageValue.Amount = shipment.InsurancePolicy.DamageValue.GetValueOrDefault(InsuranceUtility.GetInsuredValue(shipment));
            itemName.Text = shipment.InsurancePolicy.ItemName;
            description.Text = shipment.InsurancePolicy.Description;
            email.Text = shipment.InsurancePolicy.EmailAddress ?? shipment.OriginEmail;
        }

        /// <summary>
        /// Called when [submit claim click].
        /// </summary>
        private void OnSubmitClaimClick(object sender, EventArgs e)
        {
            if (damageValue.Amount <= 0)
            {
                MessageHelper.ShowError(this, "A value greater than $0.00 must be supplied for damages when submitting a claim.");
                return;
            }

            if (string.IsNullOrWhiteSpace(itemName.Text))
            {
                MessageHelper.ShowError(this, "An item name must be provided when submitting a claim.");
                return;
            }

            if (string.IsNullOrWhiteSpace(description.Text))
            {
                MessageHelper.ShowError(this, "A description must be provided when submitting a claim.");
                return;
            }

            if (string.IsNullOrWhiteSpace(email.Text))
            {
                MessageHelper.ShowError(this, "An email address must be provided when submitting a claim.");
                return;
            }

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                // Disable the button to provide some feedback to the user that something is happening.
                // Maybe change this to use a progress provider if it takes more than a few seconds.
                submitClaim.Enabled = false;

                StoreEntity storeEntity = StoreManager.GetStore(shipment.Order.StoreID);
                if (storeEntity == null)
                {
                    throw new InsureShipException("The store the shipment was in has been deleted.");
                }

                InsureShipAffiliate insureShipAffiliate = TangoWebClient.GetInsureShipAffiliate(storeEntity);
                InsureShipClaim claim = new InsureShipClaim(shipment, insureShipAffiliate);

                claim.Submit((InsureShipClaimType) claimType.SelectedValue, itemName.Text, description.Text, damageValue.Amount, email.Text);
                LogClaimToTango();

                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    adapter.SaveAndRefetch(shipment);
                    adapter.Commit();
                }

                ClaimSubmitted.Invoke();
            }
            catch (InsureShipException ex)
            {
                log.Error(ex);
                MessageHelper.ShowError(this, ex.Message);
            }
            finally
            {
                // Renable the button in the event an error occurred
                submitClaim.Enabled = true;
            }
        }

        /// <summary>
        /// Save any changes to the specified policy
        /// </summary>
        public void SaveToPolicy(InsurancePolicyEntity insurancePolicy)
        {
            insurancePolicy.ClaimType = (int) claimType.SelectedValue;
            insurancePolicy.DamageValue = damageValue.Amount;
            insurancePolicy.ItemName = itemName.Text;
            insurancePolicy.Description = description.Text;
            insurancePolicy.EmailAddress = email.Text;
        }

        /// <summary>
        /// Logs the claim to tango.
        /// </summary>
        public void LogClaimToTango()
        {
            try
            {
                TangoWebClient.LogSubmitInsuranceClaim(shipment);
            }
            catch (InsureShipException ex)
            {
                log.Error("While attempting to log the insurance claim with Tango, an error occured.", ex);
            }
        }

        /// <summary>
        /// Called when [submit claim link clicked].
        /// </summary>
        private void OnSubmitClaimLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            StoreEntity store = StoreManager.GetStore(shipment.Order.StoreID);

            InsureShipAffiliate insureShipAffiliate = TangoWebClient.GetInsureShipAffiliate(store);

            string onlineStoreID = insureShipAffiliate.InsureShipStoreID;

            string url = string.Format(
                "https://www.interapptive.com/account/insuranceclaim.php?id={0}&shipment={1}",
                onlineStoreID,
                shipment.OnlineShipmentID);

            WebHelper.OpenUrl(url, this);
        }
    }
}
