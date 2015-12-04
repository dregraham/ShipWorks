using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.UI;
using System.IO;
using Interapptive.Shared.Utility;
using System.Xml.Linq;
using System.Xml;
using Interapptive.Shared;

namespace ShipWorks.Stores.Platforms.ProStores
{
    /// <summary>
    /// UserControl for managing and renewing a proStores token
    /// </summary>
    public partial class ProStoresTokenManageControl : UserControl
    {
        ProStoresStoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProStoresTokenManageControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the control for the given store
        /// </summary>
        public void InitializeForStore(ProStoresStoreEntity store)
        {
            this.store = store;

            UpdateTokenDisplay();
        }

        /// <summary>
        /// Update the display of the token
        /// </summary>
        private void UpdateTokenDisplay()
        {
            if (store.ApiToken.Length == 0)
            {
                tokenBox.Text = "None";
            }
            else
            {
                tokenBox.Text = string.Format("User '{0}', {1}", store.Username, store.ApiStorefrontUrl);
            }
        }

        /// <summary>
        /// Change the current logon token for the store
        /// </summary>
        private void OnChangeToken(object sender, EventArgs e)
        {
            using (ProStoresTokenWizard wizard = new ProStoresTokenWizard(store))
            {
                if (wizard.ShowDialog(this) == DialogResult.OK)
                {
                    UpdateTokenDisplay();
                }
            }
        }

        /// <summary>
        /// Import a token from a token file
        /// </summary>
        private void OnImportTokenFile(object sender, EventArgs e)
        {
            if (ImportTokenFile(store, this))
            {
                UpdateTokenDisplay();

                MessageHelper.ShowInformation(this, "The token file has been imported.");
            }
        }

        /// <summary>
        /// Import a token file for the given store.  Any errors are displayed using the given owner.
        /// </summary>
        [NDependIgnoreLongMethod]
        public static bool ImportTokenFile(ProStoresStoreEntity store, IWin32Window owner)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "ProStores Token File (*.tkn)|*.tkn";

                if (dlg.ShowDialog(owner) == DialogResult.OK)
                {
                    // load the file
                    try
                    {
                        string contents = File.ReadAllText(dlg.FileName);
                        string tokenXml = SecureText.Decrypt(contents, "token");

                        XmlDocument tokenDoc = new XmlDocument();
                        tokenDoc.LoadXml(tokenXml);

                        store.LoginMethod = (int) ProStoresLoginMethod.ApiToken;
                        store.ShortName = tokenDoc.SelectSingleNode("//ShortName").InnerText;
                        store.Username = tokenDoc.SelectSingleNode("//Username").InnerText;
                        store.ApiEntryPoint = tokenDoc.SelectSingleNode("//EntryPoint").InnerText;
                        store.ApiToken = tokenDoc.SelectSingleNode("//Token").InnerText;
                        store.ApiStorefrontUrl = tokenDoc.SelectSingleNode("//StorefrontUrl").InnerText;
                        store.ApiTokenLogonUrl = tokenDoc.SelectSingleNode("//TokenLogonUrl").InnerText;
                        store.ApiXteUrl = tokenDoc.SelectSingleNode("//XteUrl").InnerText;
                        store.ApiRestSecureUrl = tokenDoc.SelectSingleNode("//RestSecureUrl").InnerText;
                        store.ApiRestNonSecureUrl = tokenDoc.SelectSingleNode("//RestNonSecureUrl").InnerText;
                        store.ApiRestScriptSuffix = tokenDoc.SelectSingleNode("//RestScriptSuffix").InnerText;

                        Cursor.Current = Cursors.WaitCursor;

                        ProStoresWebClient.TestXteConnection(store);

                        return true;
                    }
                    catch (ProStoresException ex)
                    {
                        MessageHelper.ShowError(owner, "There was an error validated the ProStores token:\n\n" + ex.Message);
                    }
                    catch (NullReferenceException)
                    {
                        // This would be thrown if SelectSingleNode returned null
                        MessageHelper.ShowError(owner, "The selected file is not a valid ProStores token file.");
                    }
                    catch (IOException ex)
                    {
                        MessageHelper.ShowError(owner, String.Format("ShipWorks was unable to read the token file: {0}", ex.Message));
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        MessageHelper.ShowError(owner, String.Format("ShipWorks was unable to read the token file: {0}", ex.Message));
                    }
                    catch (XmlException)
                    {
                        MessageHelper.ShowError(owner, "Unable to import token file, invalid format.");
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Export a token to a file
        /// </summary>
        private void OnExportTokenFile(object sender, EventArgs e)
        {
            if (store.ApiToken.Length == 0)
            {
                MessageHelper.ShowError(this, "You do not have an eBay Login Token to export.");
                return;
            }

            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Filter = "ProStores Token File (*.tkn)|*.tkn";

                // present the Save dialgo
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    // Build the token as a small XML file
                    XElement saveToken =
                        new XElement("ProStoresToken",
                            new XElement("ShortName", store.ShortName),
                            new XElement("Username", store.Username),
                            new XElement("EntryPoint", store.ApiEntryPoint),
                            new XElement("Token", store.ApiToken),
                            new XElement("StorefrontUrl", store.ApiStorefrontUrl),
                            new XElement("TokenLogonUrl", store.ApiTokenLogonUrl),
                            new XElement("XteUrl", store.ApiXteUrl),
                            new XElement("RestSecureUrl", store.ApiRestSecureUrl),
                            new XElement("RestNonSecureUrl", store.ApiRestNonSecureUrl),
                            new XElement("RestScriptSuffix", store.ApiRestScriptSuffix));

                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        using (StreamWriter writer = new StreamWriter(dlg.FileName))
                        {
                            writer.Write(SecureText.Encrypt(saveToken.ToString(), "token"));
                        }

                        MessageHelper.ShowInformation(this, "The token was successfully exported.");
                    }
                    catch (IOException ex)
                    {
                        MessageHelper.ShowError(this, String.Format("Unable to save token file: {0}", ex.Message));
                    }
                    catch (UnauthorizedAccessException)
                    {
                        MessageHelper.ShowError(this, String.Format("Unable to save token file: Access denied to '{0}'.", dlg.FileName));
                    }
                }
            }
        }
    }
}
