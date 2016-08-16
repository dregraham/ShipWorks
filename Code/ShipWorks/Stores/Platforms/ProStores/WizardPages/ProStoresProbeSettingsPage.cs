using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using Interapptive.Shared.UI;
using System.Net;
using ShipWorks.Data.Model.EntityClasses;
using System.Xml;
using Interapptive.Shared;
using ShipWorks.Stores.Management;
using Interapptive.Shared.Net;

namespace ShipWorks.Stores.Platforms.ProStores.WizardPages
{
    /// <summary>
    /// Wizard page for trying to probe the settings of a ProStore installation
    /// </summary>
    public partial class ProStoresProbeSettingsPage : AddStoreWizardPage
    {
        bool importedToken = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProStoresProbeSettingsPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Stepping next
        /// </summary>
        [NDependIgnoreLongMethod]
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            string url = storeUrl.Text.Trim();

            if (url.Length == 0)
            {
                if (!importedToken)
                {
                    MessageHelper.ShowInformation(this, "Enter the URL to your ProStores store.");
                    e.NextPage = this;
                }

                return;
            }

            if (!url.StartsWith("http"))
            {
                url = "http://" + url;
            }

            ProStoresStoreEntity store = GetStore<ProStoresStoreEntity>();

            store.ApiEntryPoint = "";
            store.LoginMethod = (int) ProStoresLoginMethod.LegacyUserPass;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                WebRequest request = WebRequest.Create(url);
                using (WebResponse response = request.GetResponse())
                {
                    string apiEntryPoint = response.Headers["X-ProStores-StoreApiEntryPoint"];

                    if (!string.IsNullOrEmpty(apiEntryPoint))
                    {
                        XmlDocument apiInfoResponse = ProStoresWebClient.GetStoreApiInfo(apiEntryPoint);

                        ((ProStoresStoreType) StoreTypeManager.GetType(store)).LoadApiInfo(apiEntryPoint, apiInfoResponse);
                    }
                }
            }
            catch (ProStoresException ex)
            {
                MessageHelper.ShowError(this, "ProStores returned an error while accessing your store:\n\n" + ex.Message);
                e.NextPage = this;

                return;
            }
            catch (UriFormatException)
            {
                MessageHelper.ShowError(this, "The URL you entered is not a valid URL.");
                e.NextPage = this;

                return;
            }
            catch (Exception ex)
            {
                if (WebHelper.IsWebException(ex))
                {
                    MessageHelper.ShowError(this, "ShipWorks could not connect to the URL provided.\n\nDetails: " + ex.Message);
                    e.NextPage = this;

                    return;
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Import a token file
        /// </summary>
        private void OnImportTokenFile(object sender, EventArgs e)
        {
            if (ProStoresTokenManageControl.ImportTokenFile(GetStore<ProStoresStoreEntity>(), this))
            {
                importedToken = true;
                MessageHelper.ShowInformation(this, "The token file has been imported.");

                Wizard.MoveNext();
            }
        }
    }
}
