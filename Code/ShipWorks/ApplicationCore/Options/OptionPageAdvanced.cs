using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.Data;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.ShipSense;
using ShipWorks.Users;
using ShipWorks.Users.Audit;
using ShipWorks.Users.Logon;
using ShipWorks.Shipping.ShipSense.Settings;
using ShipWorks.Data.Connection;
using SD.LLBLGen.Pro.ORMSupportClasses;
using Interapptive.Shared.Data;
using ShipWorks.Shipping.ShipSense.Population;
using ShipWorks.Common.Threading;
using System.Threading.Tasks;

namespace ShipWorks.ApplicationCore.Options
{
    /// <summary>
    /// Advanced page of the Options window
    /// </summary>
    public partial class OptionPageAdvanced : OptionPageBase
    {
        ConfigurationEntity config;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public OptionPageAdvanced()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Do initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            ConfigurationData.CheckForChangesNeeded();
            config = ConfigurationData.Fetch();

            EnumHelper.BindComboBox<LogonMethod>(logOnMethod);
            logOnMethod.SelectedValue = (LogonMethod) config.LogOnMethod;

            addressCasing.Checked = config.AddressCasing;

            compareCustomerEmail.Checked = config.CustomerCompareEmail;
            compareCustomerAddress.Checked = config.CustomerCompareAddress;

            updateCustomerBilling.Checked = config.CustomerUpdateBilling;
            updateCustomerShipping.Checked = config.CustomerUpdateShipping;

            EnumHelper.BindComboBox<ModifiedOrderCustomerUpdateBehavior>(orderBillingAddressChanged);
            orderBillingAddressChanged.SelectedValue = (ModifiedOrderCustomerUpdateBehavior)config.CustomerUpdateModifiedBilling;

            EnumHelper.BindComboBox<ModifiedOrderCustomerUpdateBehavior>(orderShippingAddressChanged);
            orderShippingAddressChanged.SelectedValue = (ModifiedOrderCustomerUpdateBehavior)config.CustomerUpdateModifiedShipping;

            auditNewOrders.Checked = config.AuditNewOrders;
            auditDeletedOrders.Checked = config.AuditDeletedOrders;

            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            enableShipSense.Checked = settings.ShipSenseEnabled;
            clearKnowledgebase.Visible = UserSession.User.IsAdmin;

            autoCreateShipments.Checked = settings.AutoCreateShipments;
        }

        /// <summary>
        /// Save the settings
        /// </summary>
        public override void Save()
        {
            config.LogOnMethod = (int) logOnMethod.SelectedValue;
            config.AddressCasing = addressCasing.Checked;

            config.CustomerCompareEmail = compareCustomerEmail.Checked;
            config.CustomerCompareAddress = compareCustomerAddress.Checked;

            config.CustomerUpdateBilling = updateCustomerBilling.Checked;
            config.CustomerUpdateShipping = updateCustomerShipping.Checked;

            config.CustomerUpdateModifiedBilling = (int) orderBillingAddressChanged.SelectedValue;
            config.CustomerUpdateModifiedShipping = (int) orderShippingAddressChanged.SelectedValue;

            config.AuditNewOrders = auditNewOrders.Checked;
            config.AuditDeletedOrders = auditDeletedOrders.Checked;

            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            settings.ShipSenseEnabled = enableShipSense.Checked;
            settings.AutoCreateShipments = autoCreateShipments.Checked;
            ShippingSettings.Save(settings);

            ConfigurationData.Save(config);
        }

        /// <summary>
        /// Called when the button to clear the knowledge base is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnClearKnowledgebase(object sender, EventArgs e)
        {
            const string ConfirmationText = @"You are about to delete all of the information that ShipWorks " +
                "uses to create shipments based on your shipment history.";

            bool isReloadRequested = false;
            DialogResult result = DialogResult.No;

            using (ShipSenseConfirmationDlg confirmationDialog = new ShipSenseConfirmationDlg(ConfirmationText))
            {
                // Make note of whether we need to reload the knowledge base here, so we can
                // dispose of the confirmation dialog and show the progress dialog while 
                // the loader is running.
                result = confirmationDialog.ShowDialog(this);
                isReloadRequested = confirmationDialog.IsReloadRequested;
            }

            if (result == DialogResult.Yes)
            {
                // Setup dependencies for the progress dialog
                ProgressItem progressItem = new ProgressItem("Clear ShipSense");

                ProgressProvider progressProvider = new ProgressProvider();
                progressProvider.ProgressItems.Add(progressItem);

                using (ProgressDlg progressDialog = new ProgressDlg(progressProvider))
                {
                    progressDialog.Title = "Reload ShipSense";
                    progressDialog.Description = "ShipSense is learning from your shipment history.";

                    progressDialog.AutoCloseWhenComplete = false;
                    progressDialog.AllowCloseWhenRunning = false;

                    progressDialog.ActionColumnHeaderText = "ShipSense";
                    progressDialog.CloseTextWhenComplete = "Close";


                    // The reset could take a few seconds depending on the size of the database, so 
                    // reset the knowledge base on a separate thread
                    Task resetTask = new Knowledgebase().ResetAsync(UserSession.User, progressItem);
                    Task reloadTask = null;

                    if (isReloadRequested)
                    {
                        // The reset has completed, setup the reload if it was requested
                        reloadTask = CreateReloadKnowledgebaseTask(progressProvider);
                    }

                    resetTask.ContinueWith((t) =>
                    {
                        reloadTask?.Start();
                    });

                    // Start the reset/reload work and show the progress dialog
                    resetTask.Start();

                    progressDialog.ShowDialog(this);
                }
            }
        }

        /// <summary>
        /// Called when the button to edit ShipSense settings is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnEditShipSenseClick(object sender, EventArgs e)
        {
            using (ShipSenseUniquenessSettingsDlg dialog = new ShipSenseUniquenessSettingsDlg())
            {
                dialog.ShowDialog(this);
            }
        }

        /// <summary>
        /// Called when the button to reload the ShipSense knowledge base is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnReloadKnowledgebase(object sender, EventArgs e)
        {
            const string ConfirmationText = "Reloading the knowledge base will overwrite all existing ShipSense data and result in orders older than 30 days " +
                                            "not being immediately recognized by ShipSense.\n\n" +
                                            "Do you wish to continue?";

            DialogResult result = MessageHelper.ShowQuestion(this, MessageBoxIcon.Question, MessageBoxButtons.YesNo, ConfirmationText);

            if (result == DialogResult.Yes)
            {
                ReloadKnowledgebase();
            }
        }

        /// <summary>
        /// Reloads the ShipSense knowledge base with the latest shipment history.
        /// </summary>
        private void ReloadKnowledgebase()
        {
            ProgressProvider progressProvider = new ProgressProvider();
            using (ProgressDlg progressDialog = new ProgressDlg(progressProvider))
            {
                progressDialog.Title = "Reload ShipSense";
                progressDialog.Description = "ShipSense is learning from your shipment history.";

                progressDialog.AutoCloseWhenComplete = false;
                progressDialog.AllowCloseWhenRunning = false;

                progressDialog.ActionColumnHeaderText = "ShipSense";
                progressDialog.CloseTextWhenComplete = "Close";

                CreateReloadKnowledgebaseTask(progressProvider).Start();

                // Show the progress dialog
                progressDialog.ShowDialog(this);
            }
        }

        /// <summary>
        /// Creates a task to reload the ShipSense knowledge base with the latest shipment history. 
        /// This overloaded version allows the reload process to attach a progress item to an existing 
        /// progress provider.
        /// </summary>
        /// <remarks>
        /// Task will be added to progressProvider. Still needs to be started.
        /// </remarks>
        private Task CreateReloadKnowledgebaseTask(ProgressProvider progressProvider)
        {
            // Record an entry in the audit log that the KB reload was started
            AuditUtility.Audit(AuditActionType.ReloadShipSenseStarted);

            // Setup dependencies for the progress dialog
            ProgressItem progressItem = new ProgressItem("Reloading ShipSense");
            progressProvider.ProgressItems.Add(progressItem);

            ShipSenseLoader loader = new ShipSenseLoader(progressItem);

            // Indicate that we want to reset the hash keys and prepare the environment for the 
            // load process to begin
            loader.ResetOrderHashKeys = true;
            loader.PrepareForLoading();

            // Start the load asynchronously now that everything should be ready to load
            // We MUST ContinueWith and dispose the loader so that the sql connection
            Task reloadKnowledgebaTask = new Task(loader.LoadData);
            reloadKnowledgebaTask.ContinueWith(t => loader.Dispose());

            return reloadKnowledgebaTask;
        }
    }
}
