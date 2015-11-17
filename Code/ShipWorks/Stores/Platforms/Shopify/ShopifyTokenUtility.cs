using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using log4net;
using System.ComponentModel;
using System.Xml.Linq;
using System.Xml;
using Interapptive.Shared;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// Utility class to help import and export ShopSite tokens
    /// </summary>
    public static class ShopifyTokenUtility
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ShopifyCreateTokenControl));

        /// <summary>
        /// Shows import token dialog, imports token, validates it.
        /// </summary>
        public static bool ImportTokenFile(ShopifyStoreEntity store, IWin32Window owner)
        {
            using (OpenFileDialog fileDlg = new OpenFileDialog())
            {
                fileDlg.Tag = Tuple.Create(store, owner);
                fileDlg.FileOk += new CancelEventHandler(OnOpenFileDialogOK);
                fileDlg.Filter = "Shopify Token File (*.tkn)|*.tkn";

                return fileDlg.ShowDialog(owner) == DialogResult.OK;
            }
        }

        /// <summary>
        /// User has clicked OK.  The OpenFileDialog is still open, and we can verify the valid file before closing.
        /// </summary>
        [NDependIgnoreLongMethod]
        private static void OnOpenFileDialogOK(object sender, CancelEventArgs e)
        {
            // Assume we'll fail
            e.Cancel = true;

            OpenFileDialog fileDlg = (OpenFileDialog) sender;

            var state = (Tuple<ShopifyStoreEntity, IWin32Window>) fileDlg.Tag;
            ShopifyStoreEntity store = state.Item1;
            IWin32Window owner = state.Item2;

            // load the file
            try
            {
                string contents = File.ReadAllText(fileDlg.FileName);

                string tokenXml = SecureText.Decrypt(contents, "token");
                if (string.IsNullOrWhiteSpace(tokenXml))
                {
                    throw new ShopifyException("The selected file is not a valid Shopify token.");
                }

                XElement xToken = XElement.Parse(tokenXml);
                string accessToken = (string) xToken.Element("AccessToken");
                string shopUrlName = (string) xToken.Element("ShopUrlName");

                store.SaveFields("");

                try
                {
                    store.ShopifyAccessToken = accessToken;
                    store.ShopifyShopUrlName = shopUrlName;

                    // Now get the store info
                    ShopifyWebClient webClient = new ShopifyWebClient(store, null);
                    webClient.RetrieveShopInformation();
                }
                catch
                {
                    store.RollbackFields("");

                    throw;
                }

                MessageHelper.ShowInformation(owner, "The token was successfully imported.");

                e.Cancel = false;
            }
            catch (ShopifyException ex)
            {
                log.Error(ex.Message, ex);
                MessageHelper.ShowError(owner, ex.Message);
            }
            catch (IOException ex)
            {
                string message = String.Format("Failure reading token file '{0}'", fileDlg.FileName);
                log.Error(message, ex);

                MessageHelper.ShowError(owner, String.Format("ShipWorks was unable to read the token file: {0}", fileDlg.FileName));
            }
            catch (XmlException ex)
            {
                string message = String.Format("Failure reading token file '{0}'", fileDlg.FileName);
                log.Error(message, ex);

                MessageHelper.ShowError(owner, String.Format("ShipWorks was unable to read the token file: {0}", fileDlg.FileName));
            }
            catch (UnauthorizedAccessException ex)
            {
                // No access
                string message = String.Format("Failure reading token file '{0}'", fileDlg.FileName);
                log.Error(message, ex);

                // show the error
                MessageHelper.ShowError(owner, String.Format("ShipWorks was unable to read the token file: {0}", fileDlg.FileName));
            }
        }

        /// <summary>
        /// Exports encrypted token to file.
        /// </summary>
        public static void ExportTokenFile(ShopifyStoreEntity store, IWin32Window owner)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            if (string.IsNullOrWhiteSpace(store.ShopifyAccessToken) || string.IsNullOrWhiteSpace(store.ShopifyShopUrlName))
            {
                MessageHelper.ShowError(owner, "You do not have an Shopify Login Token to export.");
                return;
            }

            using (SaveFileDialog saveDlg = new SaveFileDialog())
            {
                saveDlg.Filter = "Shopify Token File (*.tkn)|*.tkn";

                // present the Save dialog
                if (saveDlg.ShowDialog(owner) == DialogResult.OK)
                {
                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        XElement xToken = new XElement("ShopifyToken",
                            new XElement("AccessToken", store.ShopifyAccessToken),
                            new XElement("ShopUrlName", store.ShopifyShopUrlName));


                        using (StreamWriter writer = new StreamWriter(saveDlg.FileName))
                        {
                            writer.Write(SecureText.Encrypt(xToken.ToString(), "token"));
                        }

                        MessageHelper.ShowInformation(owner, "The token was successfully exported.");
                    }
                    catch (IOException ex)
                    {
                        string message = string.Format("Failure parsing export token file '{0}'", saveDlg.FileName);
                        log.Error(message, ex);

                        MessageHelper.ShowError(owner, string.Format("Unable to save token file: {0}", saveDlg.FileName));
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        string message = string.Format("Failure parsing export token file '{0}'", saveDlg.FileName);
                        log.Error(message, ex);

                        MessageHelper.ShowError(owner, string.Format("Unable to save token file: Access denied to '{0}'.", saveDlg.FileName));
                    }
                }
            }
        }
    }
}
