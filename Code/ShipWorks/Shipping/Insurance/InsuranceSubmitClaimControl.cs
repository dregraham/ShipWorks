using System;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore;
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
        private static readonly ILog log = LogManager.GetLogger(typeof(InsuranceSubmitClaimControl));

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
            issueDate.Value = DateTime.Now;
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

            Cursor.Current = Cursors.WaitCursor;

            // Disable the button to provide some feedback to the user that something is happening.
            // Maybe change this to use a progress provider if it takes more than a few seconds.
            submitClaim.Enabled = false;

            using (var lifetimeScope = IoC.BeginLifetimeScope())
            {
                var claim = lifetimeScope.Resolve<IInsureShipClaim>();

                EnsureStoreExists(lifetimeScope.Resolve<IStoreManager>())
                    .Bind(() => claim.Submit((InsureShipClaimType) claimType.SelectedValue, shipment, UpdateInsurancePolicy))
                    .Do(() => CompleteClaimSubmission(lifetimeScope))
                    .OnFailure(ex =>
                    {
                        log.Error(ex);
                        lifetimeScope.Resolve<IMessageHelper>().ShowError(ex.Message);
                    });
            }

            // Re-enable the button in the event an error occurred
            submitClaim.Enabled = true;
        }

        /// <summary>
        /// Update the given policy with data from customer
        /// </summary>
        private void UpdateInsurancePolicy(InsurancePolicyEntity policy)
        {
            policy.ItemName = itemName.Text;
            policy.DamageValue = damageValue.Amount;
            policy.SubmissionDate = DateTime.UtcNow;
            policy.Description = description.Text;
            policy.EmailAddress = email.Text;
            policy.DateOfIssue = issueDate.Value;
        }

        /// <summary>
        /// Ensure the store for the shipment exists
        /// </summary>
        private Result EnsureStoreExists(IStoreManager storeManager)
        {
            var storeEntity = storeManager.GetStoreReadOnly(shipment.Order.StoreID);
            if (storeEntity == null)
            {
                return new InsureShipException("The store the shipment was in has been deleted.");
            }

            return Result.FromSuccess();
        }

        /// <summary>
        /// Complete the claim submission
        /// </summary>
        private void CompleteClaimSubmission(ILifetimeScope lifetimeScope)
        {
            LogClaimToTango(lifetimeScope.Resolve<ITangoWebClient>(), shipment);

            var sqlAdapterFactory = lifetimeScope.Resolve<ISqlAdapterFactory>();

            using (var adapter = sqlAdapterFactory.CreateTransacted())
            {
                adapter.SaveAndRefetch(shipment);
                adapter.Commit();
            }

            ClaimSubmitted.Invoke();
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
        public void LogClaimToTango(ITangoWebClient tangoWebClient, ShipmentEntity shipment)
        {
            try
            {
                tangoWebClient.LogSubmitInsuranceClaim(shipment);
            }
            catch (InsureShipException ex)
            {
                log.Error("While attempting to log the insurance claim with Tango, an error occurred.", ex);
            }
        }
    }
}
