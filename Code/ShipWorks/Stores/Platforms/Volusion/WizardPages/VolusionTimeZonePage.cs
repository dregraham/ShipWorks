using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Volusion.WizardPages
{
    /// <summary>
    /// Page for configuring what the online store's TimeZone setting is
    /// </summary>
    public partial class VolusionTimeZonePage : VolusionAddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public VolusionTimeZonePage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Moving to the next wizard page
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            VolusionStoreEntity store = GetStore<VolusionStoreEntity>();

            store.ServerTimeZone = timeZoneControl.SelectedTimeZone.Id;

            List<string> chosenStatuses = new List<string>();
            foreach (string selectedItem in statuses.CheckedItems)
            {
                chosenStatuses.Add(selectedItem);
            }

            // save the selected statuses as CSV
            store.DownloadOrderStatuses = string.Join(",", chosenStatuses);

        }

        /// <summary>
        /// Load the TimeZone information from the store entity
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            VolusionStoreEntity store = GetStore<VolusionStoreEntity>();

            timeZoneControl.SelectedTimeZone = TimeZoneInfo.FindSystemTimeZoneById(store.ServerTimeZone);

            if (store == null)
            {
                throw new InvalidOperationException("A non Volusion store was passed to the Volusion store settings control.");
            }

            statuses.Items.Clear();

            StoreType storeType = StoreTypeManager.GetType(store);

            // get the collection of currently chosen codes to be downloaded
            List<string> selectedStatuses = store.DownloadOrderStatuses.Split(',').ToList();

            foreach (string status in storeType.GetOnlineStatusChoices())
            {
                // check the ones that are selected
                bool chosen = selectedStatuses.Contains(status);
                statuses.Items.Add(status, chosen);
            }
        }
    }
}
