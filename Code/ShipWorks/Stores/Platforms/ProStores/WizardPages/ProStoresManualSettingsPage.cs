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
using Interapptive.Shared.UI;
using System.Xml;
using ShipWorks.Stores.Management;

namespace ShipWorks.Stores.Platforms.ProStores.WizardPages
{
    /// <summary>
    /// Wizard page for allowing a user to manual enter the details of their prostores installation
    /// </summary>
    public partial class ProStoresManualSettingsPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ProStoresManualSettingsPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Stepping into the page
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            // Don't skip it going back.  If they choose ApiToken login method, and then come back, we still want to show this page.
            if (e.StepReason == WizardStepReason.StepForward)
            {
                e.Skip = (GetStore<ProStoresStoreEntity>().LoginMethod == (int) ProStoresLoginMethod.ApiToken);
            }
        }

        /// <summary>
        /// Selected radio button changed
        /// </summary>
        private void OnRadioChanged(object sender, EventArgs e)
        {
            apiEntryPoint.Enabled = radioExists.Checked;
        }

        /// <summary>
        /// Stepping next
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            ProStoresStoreEntity store = GetStore<ProStoresStoreEntity>();

            if (radioExists.Checked)
            {
                string url = apiEntryPoint.Text.Trim();

                if (url.Length == 0)
                {
                    MessageHelper.ShowInformation(this, "Enter the API Entry Point of your ProStores store.");
                    e.NextPage = this;
                    return;
                }

                Cursor.Current = Cursors.WaitCursor;

                try
                {
                    XmlDocument apiInfoResponse = ProStoresWebClient.GetStoreApiInfo(url);

                    ((ProStoresStoreType) StoreTypeManager.GetType(store)).LoadApiInfo(url, apiInfoResponse);
                }
                catch (ProStoresException ex)
                {
                    MessageHelper.ShowError(this, "An error occurred while accessing the API Entry Point:\n\n" + ex.Message);
                    e.NextPage = this;

                    return;
                }
            }
        }
    }
}
