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
        private ProgressDlg progressDialog;
        
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

            auditNewOrders.Checked = config.AuditNewOrders;
            auditDeletedOrders.Checked = config.AuditDeletedOrders;

            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            enableShipSense.Checked = settings.ShipSenseEnabled;
            clearKnowledgebase.Visible = UserSession.User.IsAdmin;
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

            config.AuditNewOrders = auditNewOrders.Checked;
            config.AuditDeletedOrders = auditDeletedOrders.Checked;

            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            settings.ShipSenseEnabled = enableShipSense.Checked;
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
            const string ConfirmationText = @"Clearing the knowledge base will delete all of the information that ShipWorks " +
                "uses to create shipments based on your shipment history.\n\n" +
                "Do you wish to continue?";

            DialogResult result = MessageHelper.ShowQuestion(this, MessageBoxIcon.Question, MessageBoxButtons.YesNo, ConfirmationText);

            if (result == DialogResult.Yes)
            {
                new Knowledgebase().Reset(UserSession.User);
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
            ProgressItem progressItem = new ProgressItem("Reloading ShipSense");

            ProgressProvider progressProvider = new ProgressProvider();
            progressProvider.ProgressItems.Add(progressItem);

            UpdateShipmentRange();
            ShipSenseLoader.LoadDataAsync(progressItem, true);

            progressDialog = new ProgressDlg(progressProvider)
            {
                Title = "Reload ShipSense",
                Description = "Your shipment history is being used to reload the ShipSense knowledge base.",

                AutoCloseWhenComplete = true,
                AllowCloseWhenRunning = false,

                ActionColumnHeaderText = "ShipSense",

                CloseTextWhenComplete = "Close"
            };

            progressDialog.ShowDialog(this);
            progressDialog.Dispose();

        }

        /// <summary>
        /// Updates the shipment range of the shipping settings that will be used when rebuilding 
        /// the ShipSense knowledge base.
        /// </summary>
        private void UpdateShipmentRange()
        {
            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            settings.ShipSenseProcessedShipmentID = GetStartingShipmentID();
            settings.ShipSenseEndShipmentID = GetEndingShipmentID();

            ShippingSettings.Save(settings);
        }


        /// <summary>
        /// Gets the shipment ID to start from when rebuilding the ShipSense knowledge base.
        /// </summary>
        private static long GetStartingShipmentID()
        {
            long startingShipmentID = 0;

            using (SqlConnection connection = SqlSession.Current.OpenConnection())
            {
                SqlCommand command = SqlCommandProvider.Create(connection);
                command.CommandText = @"
                                        DECLARE @ShipSenseProcessedShipmentID BIGINT
                                        WITH Shipments AS                                        (	                                        SELECT TOP 25000 ShipmentID FROM Shipment WITH (NOLOCK) WHERE Processed = 1 ORDER BY ShipmentID DESC                                        )                                        SELECT MIN(ShipmentID) FROM Shipments";

                using (SqlDataReader reader = SqlCommandProvider.ExecuteReader(command))
                {
                    if (reader.Read())
                    {
                        startingShipmentID = reader.GetInt64(0);
                    }
                }
            }

            return startingShipmentID;
        }

        /// <summary>
        /// Gets the maximum shipment ID of all the processed shipments.
        /// </summary>
        private static long GetEndingShipmentID()
        {
            long endingShipmentID = 0;

            using (SqlConnection connection = SqlSession.Current.OpenConnection())
            {
                SqlCommand command = SqlCommandProvider.Create(connection);
                command.CommandText = @"SELECT MAX(ShipmentID) FROM SHIPMENT WITH (NOLOCK) WHERE Processed = 1";

                using (SqlDataReader reader = SqlCommandProvider.ExecuteReader(command))
                {
                    if (reader.Read())
                    {
                        endingShipmentID = reader.GetInt64(0);
                    }
                }
            }

            return endingShipmentID;
        }
    }
}
