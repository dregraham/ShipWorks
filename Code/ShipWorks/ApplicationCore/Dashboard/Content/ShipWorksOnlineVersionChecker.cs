using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using log4net;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Users;

namespace ShipWorks.ApplicationCore.Dashboard.Content
{
    /// <summary>
    /// Utility class for checking the current version of ShipWorks
    /// </summary>
    public static class ShipWorksOnlineVersionChecker
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(ShipWorksOnlineVersionChecker));

        /// <summary>
        /// Process the request
        /// </summary>
        public static ShipWorksOnlineVersion CheckOnlineVersion()
        {
            Uri requestUri = new Uri("http://www.interapptive.com/shipworks/version.php");

            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(requestUri);
            request.UserAgent = "shipworks";

            // Only wait up to X seconds for a response
            request.Timeout = (int) TimeSpan.FromSeconds(15).TotalMilliseconds;

            HttpWebResponse response = (HttpWebResponse) request.GetResponse();

            // See if we got a valid response
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new WebException(response.StatusDescription);
            }

            string result = "";
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                result = reader.ReadToEnd().Trim();
            }

            try
            {
                XDocument xDocument = XDocument.Parse(result);

                ShipWorksOnlineVersion onlineVersion = new ShipWorksOnlineVersion(
                    new Version((string) xDocument.Descendants("Version").First()),
                    (string) xDocument.Descendants("DownloadUrl").First(),
                    (string) xDocument.Descendants("WhatsNewUrl").First());

                // We also have to extract out insurance versioning information
                InsuranceUtility.ConfigureInsurance(
                    (int) xDocument.XPathSelectElement("//Insurance/ShipWorksRateVersion"),
                    (int) xDocument.XPathSelectElement("//Insurance/CarrierRateVersion"),
                    (int) xDocument.XPathSelectElement("//Insurance/CountryVersion"),
                    (int) xDocument.XPathSelectElement("//Insurance/InfoBannerVersion"));

                log.InfoFormat("Online version found to be {0}.", onlineVersion.Version);
                return onlineVersion;
            }
            catch (XmlException ex)
            {
                log.Error("Online version response could not be parsed: " + ex.Message);

                throw new WebException(ex.Message, ex);
            }
            catch (InvalidOperationException ex)
            {
                log.Error("Online version could not be retrieved: " + ex.Message);

                throw new WebException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Check the version signed off on in the database by the current user in the current computer.
        /// </summary>
        public static Version CheckSignedOffVersion()
        {
            VersionSignoffCollection signoff = VersionSignoffCollection.Fetch(SqlAdapter.Default,
                VersionSignoffFields.ComputerID == UserSession.Computer.ComputerID &
                VersionSignoffFields.UserID == UserSession.User.UserID);

            if (signoff.Count == 0)
            {
                return new Version(0, 0);
            }

            Debug.Assert(signoff.Count == 1);

            string version = signoff[0].Version;

            log.InfoFormat("Signoff version found to be {0}.", version);
            return new Version(version);
        }

        /// <summary>
        /// Signoff for the current user on the current computer having seen this version
        /// </summary>
        public static void Signoff(Version version)
        {
            VersionSignoffEntity signoff;

            VersionSignoffCollection signoffs = VersionSignoffCollection.Fetch(SqlAdapter.Default,
                VersionSignoffFields.ComputerID == UserSession.Computer.ComputerID &
                VersionSignoffFields.UserID == UserSession.User.UserID);

            if (signoffs.Count == 0)
            {
                signoff = new VersionSignoffEntity();
                signoff.ComputerID = UserSession.Computer.ComputerID;
                signoff.UserID = UserSession.User.UserID;
            }
            else
            {
                signoff = signoffs[0];
            }

            signoff.Version = version.ToString();

            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.SaveEntity(signoff);
            }
        }
    }
}
