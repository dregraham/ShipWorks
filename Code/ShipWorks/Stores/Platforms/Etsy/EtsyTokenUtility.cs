using System;
using Interapptive.Shared.UI;
using System.Windows.Forms;
using System.IO;
using ShipWorks.Data.Model.EntityClasses;
using log4net;
using System.Xml.Linq;
using System.Xml;
using Interapptive.Shared;
using Interapptive.Shared.Security;

namespace ShipWorks.Stores.Platforms.Etsy
{
    /// <summary>
    /// Class to manage token
    /// </summary>
    public class EtsyTokenUtility
    {
        readonly EtsyStoreEntity store;
        static readonly ILog log = LogManager.GetLogger(typeof(EtsyTokenManageControl));

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="etsyStore"></param>
        public EtsyTokenUtility(EtsyStoreEntity etsyStore)
        {
            store = etsyStore;
        }

        /// <summary>
        /// Shows import token dialog, imports token, validates it.
        /// </summary>
        [NDependIgnoreLongMethod]
        public bool ImportToken(IWin32Window parentWindow, EtsyWebClient webClient)
        {
            if (webClient==null)
            {
                throw new ArgumentNullException("webClient");
            }
            if (parentWindow == null)
            {
                throw new ArgumentNullException("parentWindow");
            }

            bool isTokenValid = false;

            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Etsy Token File (*.tkn)|*.tkn";

                if (dlg.ShowDialog(parentWindow) == DialogResult.OK)
                {
                    // load the file
                    try
                    {
                        string contents = File.ReadAllText(dlg.FileName);

                        string tokenXml = SecureText.Decrypt(contents, "token");
                        if (string.IsNullOrWhiteSpace(tokenXml))
                        {
                            throw new EtsyException("There was an error reading the selected token file");
                        }

                        XElement xToken = XElement.Parse(tokenXml);
                        string oAuthToken = (string) xToken.Element("OAuthToken");
                        string oAuthSecret = (string) xToken.Element("OAuthTokenSecret");

                        Cursor.Current = Cursors.WaitCursor;

                        try
                        {
                            store.SaveFields("Checkpoint");

                            store.OAuthToken = oAuthToken;
                            store.OAuthTokenSecret = oAuthSecret;

                            webClient.RetrieveTokenShopDetails();
                        }
                        catch
                        {
                            store.RollbackFields("Checkpoint");

                            throw;
                        }

                        isTokenValid = true;
                    }
                    catch (EtsyException ex)
                    {
                        log.Error(ex.Message, ex);

                        MessageHelper.ShowError(parentWindow, ex.Message);
                    }
                    catch (IOException ex)
                    {
                        // device failure most likely
                        string message = String.Format("Failure reading token file '{0}'", dlg.FileName);
                        log.Error(message, ex);

                        //show the error
                        MessageHelper.ShowError(parentWindow, String.Format("ShipWorks was unable to read the token file: {0}", ex.Message));
                    }
                    catch (XmlException ex)
                    {
                        //wrong file type
                        string message = String.Format("Failure parsing token file '{0}'", dlg.FileName);
                        log.Error(message, ex);

                        //show the error
                        MessageHelper.ShowError(parentWindow, String.Format("ShipWorks was unable to read the token file: {0}", ex.Message));
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        // No access
                        string message = String.Format("Failure reading token file '{0}'", dlg.FileName);
                        log.Error(message, ex);

                        // show the error
                        MessageHelper.ShowError(parentWindow, String.Format("ShipWorks was unable to access the token file: {0}", ex.Message));
                    }
                }
            }

            return isTokenValid;
        }

        /// <summary>
        /// Exports encrypted token to file.
        /// </summary>
        /// <param name="parentWindow"></param>
        public void ExportToken(IWin32Window parentWindow)
        {
            if (parentWindow == null)
            {
                throw new ArgumentNullException("parentWindow");
            }

            if (store.OAuthToken.Length == 0)
            {
                MessageHelper.ShowError(parentWindow, "You do not have an Etsy Login Token to export.");
                return;
            }

            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Filter = "Etsy Token File (*.tkn)|*.tkn";

                // present the Save dialgo
                if (dlg.ShowDialog(parentWindow) == DialogResult.OK)
                {
                    XElement xToken = new XElement("EtsyToken",
                        new XElement("OAuthToken", store.OAuthToken),
                        new XElement("OAuthTokenSecret", store.OAuthTokenSecret));

                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        using (StreamWriter writer = new StreamWriter(dlg.FileName))
                        {
                            writer.Write(SecureText.Encrypt(xToken.ToString(), "token"));
                        }

                        MessageHelper.ShowInformation(parentWindow, "The token was successfully exported.");
                    }
                    catch (IOException ex)
                    {
                        MessageHelper.ShowError(parentWindow, String.Format("ShipWorks was unable to save token file: {0}", ex.Message));
                    }
                    catch (UnauthorizedAccessException)
                    {
                        MessageHelper.ShowError(parentWindow, String.Format("ShipWorks was unable to save token file: Access denied to '{0}'.", dlg.FileName));
                    }
                }
            }
        }
    }
}