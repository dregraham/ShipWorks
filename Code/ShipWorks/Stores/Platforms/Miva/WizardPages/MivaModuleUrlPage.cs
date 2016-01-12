using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using System.Net;
using Interapptive.Shared;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using log4net;
using ShipWorks.Stores.Management;
using Interapptive.Shared.Net;

namespace ShipWorks.Stores.Platforms.Miva.WizardPages
{
    /// <summary>
    /// Wizard page for determining a user's Miva Merchant module URL
    /// </summary>
    public partial class MivaModuleUrlPage : AddStoreWizardPage
    {
        static readonly ILog log = LogManager.GetLogger(typeof(MivaModuleUrlPage));

        /// <summary>
        /// Constructor
        /// </summary>
        public MivaModuleUrlPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Choice of finding or specifying has changed
        /// </summary>
        private void OnChoiceChanged(object sender, EventArgs e)
        {
            panelFindUrl.Enabled = radioFindUrl.Checked;
            panelSpecifyUrl.Enabled = radioSpecifyUrl.Checked;
        }
        
        /// <summary>
        /// Stepping next
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            if (radioFindUrl.Checked)
            {
                if (!FindModule())
                {
                    e.NextPage = this;
                }
            }
            else
            {
                if (!ValidateModule())
                {
                    e.NextPage = this;
                }
            }
        }

        /// <summary>
        /// Attempt to find the module using the information entered by the user.
        /// </summary>
        [NDependIgnoreLongMethod]
        private bool FindModule()
        {
            string baseUrl = website.Text;

            if (baseUrl.Trim().Length == 0)
            {
                MessageHelper.ShowMessage(this, "Please enter the website URL of your online store.");
                return false;
            }

            decimal version;
            if (!decimal.TryParse(mivaVersion.Text.Trim(), out version) || version < 1)
            {
                MessageHelper.ShowMessage(this, "Please enter a valid number for the Miva Merchant version.");
                return false;
            }

            Cursor.Current = Cursors.WaitCursor;

            log.InfoFormat("Locating miva module using '{0}', version '{1}'", baseUrl, version);

            Uri uri;
            string uriScheme = (findUseSecure.Checked ? "https://" : "http://");

            try
            {

                if (baseUrl.IndexOf(Uri.SchemeDelimiter) == -1)
                {
                    baseUrl = uriScheme + baseUrl;
                }

                uri = new Uri(baseUrl);

                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(uriScheme + uri.Host);
                request.Timeout = 10000; // 10 seconds

                using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                {
                    // See if we got a valid response
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new WebException(response.StatusDescription);
                    }
                }
            }
            catch (UriFormatException)
            {
                MessageHelper.ShowError(this, "The URL entered is not a valid website.");
                return false;
            }
            catch (NotSupportedException)
            {
                MessageHelper.ShowError(this, "The URL entered is not a valid website.");
                return false;
            }
            catch (Exception ex)
            {
                if (WebHelper.IsWebException(ex))
                {
                    MessageHelper.ShowError(this, ex.Message);
                    return false;
                }

                throw;
            }

            string prefix = "";
            string module = "";

            // Look for the 5.0 module
            if (version >= 5.00m)
            {
                prefix = "mm5";
                module = "shipworks3.mvc";
            }
            else
            {
                if (version >= 4.14m)
                {
                    module = "shipworks3.mvc";
                }
                else
                {
                    module = "shipworks3.mv";
                }

                prefix = "Merchant2";
            }

            MivaStoreEntity mivaStore = GetStore<MivaStoreEntity>();

            for (decimal versionAttempt = version + .05m; versionAttempt >= Math.Floor(version); versionAttempt -= .01m)
            {
                string url = uriScheme + string.Format("{0}/{1}/{2}/modules/util/{3}",
                    uri.Host, prefix, versionAttempt, module);

                if (CheckUrl(url))
                {
                    // Fill the info into the Store
                    mivaStore.ModuleUrl = url;

                    return true;
                }
            }

            MessageHelper.ShowInformation(this,
                "ShipWorks was unable to locate the ShipWorks Miva Module.\n\n" +
                "This could be because your store uses a shared secure certificate, the\n" +
                "module is not installed, or the module is in a non-standard location.\n\n" +
                "For further assistance, please see the ShipWorks Help system or visit our\n" +
                "support forum at http://www.interapptive.com/support.");

            return false;
        }

        /// <summary>
        /// Determine if the following url exists.
        /// </summary>
        private bool CheckUrl(string url)
        {
            log.InfoFormat("Checking url '{0}'", url);

            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
                request.Timeout = 4000; // 4 seconds

                using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                {
                    return (response.StatusCode == HttpStatusCode.OK);
                }
            }
            catch (UriFormatException ex)
            {
                log.Error("Check failed", ex);
                return false;
            }
            catch (Exception ex)
            {
                if (WebHelper.IsWebException(ex))
                {
                    log.Error("Check failed", ex);
                    return false;
                }

                throw;
            }
        }

        /// <summary>
        /// Validate the manually entered module information.
        /// </summary>
        [NDependIgnoreLongMethod]
        private bool ValidateModule()
        {
            string url = moduleUrl.Text.Trim();

            // Error if its empty
            if (url.Length == 0)
            {
                MessageHelper.ShowMessage(this, "Please enter the URL of the ShipWorks module.");
                return false;
            }

            string requiredScheme = specifiedUseSecure.Checked ? "https://" : "http://";

            // See if the scheme is included
            if (url.IndexOf(Uri.SchemeDelimiter) == -1)
            {
                url = requiredScheme + url;
            }

            Uri uri;

            try
            {
                // Create the Uri object from the url
                uri = new Uri(url);
            }
            catch (UriFormatException)
            {
                MessageHelper.ShowMessage(this, "The specified module URL is not a valid web address.");
                return false;
            }

            // Validate the secure scheme
            if (specifiedUseSecure.Checked && uri.Scheme != Uri.UriSchemeHttps)
            {
                MessageHelper.ShowMessage(this,
                    "The protocol you entered is (" + uri.Scheme + "://).  For your security,\n" +
                    "you are required to use the (https://) protocol.");

                return false;
            }

            // Validate the unsecure scheme
            if (!specifiedUseSecure.Checked && uri.Scheme != Uri.UriSchemeHttp)
            {
                MessageHelper.ShowMessage(this,
                    "The protocol you entered is (" + uri.Scheme + "://).  To connect unsecure,\n" +
                    "you are required to use the (http://) protocol.");

                return false;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(uri);
                request.Timeout = 10000; // 10 seconds

                using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                {
                    // See if we got a valid response
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new WebException(response.StatusDescription);
                    }
                }
            }
            catch (Exception ex)
            {
                if (WebHelper.IsWebException(ex))
                {
                    MessageHelper.ShowError(this,
                        "ShipWorks could not connect to the specified URL.\n\nDetails: " + ex.Message);

                    return false;
                }

                throw;
            }

            MivaStoreEntity mivaStore = (MivaStoreEntity) ((AddStoreWizard) Wizard).Store;

            // Fill the info into the Store
            mivaStore.ModuleUrl = url;

            return true;
        }
    }
}
