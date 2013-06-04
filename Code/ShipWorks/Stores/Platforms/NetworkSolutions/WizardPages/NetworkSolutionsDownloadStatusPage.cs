using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Stores.Management;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.UI;

namespace ShipWorks.Stores.Platforms.NetworkSolutions.WizardPages
{
    /// <summary>
    /// Wizard page for configuring which order statuses to download.
    /// </summary>
    public partial class NetworkSolutionsDownloadStatusPage : AddStoreWizardPage
    {
        /// <summary>
        /// state object for checkedlistbox
        /// </summary>
        class StatusCheckItem
        {
            public string Name;
            public long Code;

            public override string ToString()
            {
                return Name;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public NetworkSolutionsDownloadStatusPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// User is coming to this page, load the UI
        /// </summary>
        private void OnSteppingInto(object sender, ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs e)
        {
            NetworkSolutionsStoreEntity nsStore = GetStore<NetworkSolutionsStoreEntity>();
            if (nsStore == null)
            {
                throw new InvalidOperationException("A non NetworkSolutions store was passed to the Network Solutions store settings control.");
            }

            // wipe out any old values
            statuses.Items.Clear();

            // get the collection of currently chosen codes to be downloaded
            List<string> selectedCodes = nsStore.DownloadOrderStatuses.Split(',').ToList();
            selectedCodes.RemoveAll( s => s.Length == 0);

            NetworkSolutionsStatusCodeProvider codeProvider = new NetworkSolutionsStatusCodeProvider(nsStore);

            // add a default value
            if (selectedCodes.Count == 0)
            {
                // add "Order Received" and "Payment Received" if this is the first time through here
                if (codeProvider.CodeNames.Contains("Order Received"))
                {
                    selectedCodes.Add(codeProvider.GetCodeValue("Order Received").ToString());
                }

                if (codeProvider.CodeNames.Contains("Payment Received"))
                {
                    selectedCodes.Add(codeProvider.GetCodeValue("Payment Received").ToString());
                }
            }

            foreach (long code in codeProvider.CodeValues)
            {
                StatusCheckItem newItem = new StatusCheckItem
                {
                    Name = codeProvider.GetCodeName(code),
                    Code = code
                };

                bool chosen = selectedCodes.Contains(code.ToString());

                statuses.Items.Add(newItem, chosen);
            }
        }

        /// <summary>
        /// User is navigating to the next page
        /// </summary>
        private void OnStepNext(object sender, ShipWorks.UI.Wizard.WizardStepEventArgs e)
        {
            NetworkSolutionsStoreEntity nsStore = GetStore<NetworkSolutionsStoreEntity>();
            if (nsStore == null)
            {
                throw new InvalidOperationException("A non NetworkSolutions store was passed to the NetworkSolutionsDownloadStatusPage.");
            }

            if (statuses.CheckedItems.Count == 0)
            {
                MessageHelper.ShowError(this, "At least one order status must be selected for download.");
                e.NextPage = this;
            }

            List<long> chosenStatuses = new List<long>();
            foreach (StatusCheckItem selectedItem in statuses.CheckedItems)
            {
                chosenStatuses.Add(selectedItem.Code);
            }

            // save the selected statuses as CSV
            nsStore.DownloadOrderStatuses = string.Join(",", chosenStatuses.ConvertAll<string>(l => l.ToString()).ToArray());
        }
    }
}
