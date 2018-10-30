using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Common.Threading;
using ShipWorks.Core.Messaging;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.ShipSense;
using ShipWorks.Shipping.ShipSense.Population;
using ShipWorks.Shipping.ShipSense.Settings;
using ShipWorks.Users;
using ShipWorks.Users.Audit;
using ShipWorks.Users.Logon;

namespace ShipWorks.ApplicationCore.Options
{
    /// <summary>
    /// Advanced page of the Options window
    /// </summary>
    public partial class OptionPageAdvanced : OptionPageBase
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(OptionPageAdvanced));
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
            orderBillingAddressChanged.SelectedValue = (ModifiedOrderCustomerUpdateBehavior) config.CustomerUpdateModifiedBilling;

            EnumHelper.BindComboBox<ModifiedOrderCustomerUpdateBehavior>(orderShippingAddressChanged);
            orderShippingAddressChanged.SelectedValue = (ModifiedOrderCustomerUpdateBehavior) config.CustomerUpdateModifiedShipping;

            auditEnabled.Checked = config.AuditEnabled;
            auditNewOrders.Checked = config.AuditNewOrders;
            auditNewOrders.Enabled = auditEnabled.Checked;
            auditDeletedOrders.Checked = config.AuditDeletedOrders;
            auditDeletedOrders.Enabled = auditEnabled.Checked;

            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            enableShipSense.Checked = settings.ShipSenseEnabled;
            clearKnowledgebase.Visible = UserSession.User.IsAdmin;
            editShipSenseSettings.Enabled = enableShipSense.Checked;
            relearnShipSense.Enabled = enableShipSense.Checked;

            autoCreateShipments.Checked = settings.AutoCreateShipments;

            useParallelActionProcessing.Checked = config.UseParallelActionQueue;

            SetupShipmentEditLimit(settings);
        }

        /// <summary>
        /// Setup the shipment edit limit dropdown
        /// </summary>
        private void SetupShipmentEditLimit(ShippingSettingsEntity settings)
        {
            shipmentEditLimit.DataSource = ShipmentsLoaderConstants.MaxAllowedOrderOptions
                .ToDictionary(x => x.ToString("#,###"), x => x)
                .ToList();
            shipmentEditLimit.DisplayMember = "Key";
            shipmentEditLimit.ValueMember = "Value";
            shipmentEditLimit.SelectedValue =
                ShipmentsLoaderConstants.MaxAllowedOrderOptions.Contains(settings.ShipmentEditLimit) ?
                    settings.ShipmentEditLimit :
                    ShipmentsLoaderConstants.DefaultMaxAllowedOrders;
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

            SaveAuditSettings();

            config.UseParallelActionQueue = useParallelActionProcessing.Checked;

            ShippingSettingsWrapper shippingSettingsWrapper = new ShippingSettingsWrapper(Messenger.Current);
            ShippingSettingsEntity settings = shippingSettingsWrapper.Fetch();
            settings.ShipSenseEnabled = enableShipSense.Checked;
            settings.AutoCreateShipments = autoCreateShipments.Checked;
            settings.ShipmentEditLimit = (int) shipmentEditLimit.SelectedValue;
            shippingSettingsWrapper.Save(settings);

            ConfigurationData.Save(config);
        }

        /// <summary>
        /// Save audit settings
        /// </summary>
        private void SaveAuditSettings()
        {
            if (config.AuditEnabled != auditEnabled.Checked)
            {
                log.Info($"AuditEnabled changed from {config.AuditEnabled} to {auditEnabled.Checked}");

                using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                {
                    IAuditUtility auditUtility = lifetimeScope.Resolve<IAuditUtility>();
                    string from = config.AuditEnabled ? "Enabled" : "Disabled";
                    string to = auditEnabled.Checked ? "Enabled" : "Disabled";
                    AuditReason auditReason = new AuditReason(AuditReasonType.AuditStateChanged, 
                        $"Auditing state changed from {from} to {to}");

                    auditUtility.AuditAsync(null, AuditActionType.AuditStateChanged, auditReason, SqlAdapter.Default);
                }
            }

            config.AuditEnabled = auditEnabled.Checked;
            config.AuditNewOrders = auditNewOrders.Checked;
            config.AuditDeletedOrders = auditDeletedOrders.Checked;
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
                    Task resetTask = Knowledgebase.ResetAsync(UserSession.User, progressItem);
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

        /// <summary>
        /// Update visibility of ShipSense buttons based on ShipSense enabled/disabled.
        /// </summary>
        private void OnEnableShipSenseCheckedChanged(object sender, EventArgs e)
        {
            editShipSenseSettings.Enabled = enableShipSense.Checked;
            relearnShipSense.Enabled = enableShipSense.Checked;
        }

        /// <summary>
        /// Update visibility of auditing buttons based on audit enabled/disabled.
        /// </summary>
        private void OnAuditEnableChanged(object sender, EventArgs e)
        {
            auditNewOrders.Enabled = auditEnabled.Checked;
            auditDeletedOrders.Enabled = auditEnabled.Checked;
        }

        /// <summary>
        /// Open a browser to the auditing help article.
        /// </summary>
        private void OnAuditInfoClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            WebHelper.OpenUrl("http://support.shipworks.com/support/solutions/articles/4000125442-audit-records-in-shipworks", this);
        }
    }
}
