using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI.Wizard;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.Newegg.WizardPages
{
    /// <summary>
    /// Newegg add store wizard page for configuring the store's download criteria.
    /// </summary>
    public partial class NeweggDownloadCriteriaPage : AddStoreWizardPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NeweggDownloadCriteriaPage" /> class.
        /// </summary>
        public NeweggDownloadCriteriaPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called when stepping into the download criteria page.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="WizardSteppingIntoEventArgs" /> instance containing the event data.</param>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            NeweggStoreEntity neweggStore = GetStore<NeweggStoreEntity>();
            excludeFulfilledByNewegg.Checked = neweggStore.ExcludeFulfilledByNewegg;
        }


        /// <summary>
        /// Called when stepping into the next page.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="WizardStepEventArgs" /> instance containing the event data.</param>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            NeweggStoreEntity store = GetStore<NeweggStoreEntity>();
            store.ExcludeFulfilledByNewegg = excludeFulfilledByNewegg.Checked;
        }
    }
}
