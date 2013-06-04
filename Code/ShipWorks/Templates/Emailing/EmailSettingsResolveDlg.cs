using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Stores;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Templates.Emailing
{
    /// <summary>
    /// Window for resolving which set of email settings to use based on a given template and input set
    /// </summary>
    public partial class EmailSettingsResolveDlg : Form
    {
        EmailSettingsResolveEventArgs args;

        const string customerInfo = 
            "A customer has orders from more than one store.  " + 
            "The email settings for the stores are different, and you must choose which settings to use.";

        const string reportLabelInfo = 
            "The selection used for the {0} has orders from more than one store.  " +
            "The email settings for the stores are different, and you must choose which settings to use.";

        /// <summary>
        /// Constructor
        /// </summary>
        public EmailSettingsResolveDlg(EmailSettingsResolveEventArgs args)
        {
            InitializeComponent();

            this.args = args;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            List<StoreEntity> stores = args.Stores.Select(id => StoreManager.GetStore(id)).ToList();

            // Load the store choices
            storeCombo.DisplayMember = "StoreName";
            storeCombo.ValueMember = "StoreID";
            storeCombo.DataSource = stores.Select(s => new { StoreName = s.StoreName, StoreID = s.StoreID }).ToList();
            storeCombo.SelectedIndex = 0;

            if (args.Customer != null)
            {
                PrepareCustomerUI();
            }
            else
            {
                PrepareReportLabelUI();
            }

            UpdateRadioChoiceUI();
        }

        /// <summary>
        /// Prepare the UI for the case where there is a selected customer who has multiple orders
        /// from different stores.
        /// </summary>
        private void PrepareCustomerUI()
        {
            labelInfo.Text = customerInfo;

            useMostRecentOrder.Checked = true;

            labelStoreSpecific.Visible = false;
        }

        /// <summary>
        /// Prepare the UI for the case where a report or label template has content from multiple stores.
        /// </summary>
        private void PrepareReportLabelUI()
        {
            labelInfo.Text = string.Format(reportLabelInfo, (args.TemplateType == TemplateType.Report) ? "report" : "label sheet");

            useSpecificStore.Checked = true;

            useMostRecentOrder.Visible = false;
            useMostRecentOrderAlways.Visible = false;
            useSpecificStore.Visible = false;
            labelStoreSpecific.Visible = true;

            labelStoreSpecific.Location = useMostRecentOrder.Location;
            storeCombo.Location = useMostRecentOrderAlways.Location;

            Height -= 50;
        }

        /// <summary>
        /// The radio selection choice has changed
        /// </summary>
        private void OnChoiceChanged(object sender, EventArgs e)
        {
            UpdateRadioChoiceUI();
        }

        /// <summary>
        /// Update the UI that changes based on the current radio selection
        /// </summary>
        private void UpdateRadioChoiceUI()
        {
            useMostRecentOrderAlways.Enabled = useMostRecentOrder.Checked;
            storeCombo.Enabled = useSpecificStore.Checked;
        }

        /// <summary>
        /// Accepting the selected results
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            if (useMostRecentOrder.Checked)
            {
                args.UseMostRecentOrder = true;
                args.UseMostRecentOrderForRest = useMostRecentOrderAlways.Checked;
            }
            else
            {
                args.UseSpecificStoreID = (long) storeCombo.SelectedValue;
            }

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Canceling the email operation completely
        /// </summary>
        private void OnCancel(object sender, EventArgs e)
        {
            args.Cancel = true;
        }
    }
}
