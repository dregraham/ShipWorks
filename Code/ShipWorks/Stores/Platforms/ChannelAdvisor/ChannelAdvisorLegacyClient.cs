using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.ApplicationCore.Logging;
using System.Web;
using Interapptive.Shared.Net;
using System.Net;
using log4net;
using System.Xml;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor
{
    /// <summary>
    /// Responsible for communicating with ChannelAdvisor
    /// </summary>
    static class ChannelAdvisorLegacyClient
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(ChannelAdvisorLegacyClient));

        /// <summary>
        /// Tests connectivity to ensure the profileid and credentials are correct.
        /// This is done by downloading a checkout report
        /// </summary>
        public static bool TestConnection(int profileId, string username, string password)
        {
            HttpVariableRequestSubmitter submitter = new HttpVariableRequestSubmitter();
            submitter.Uri = new Uri("https://merchant.channeladvisor.com/Export/CFExport.dll?CheckoutReport?");
            submitter.Variables.Add("pid", profileId.ToString());
            submitter.Variables.Add("mn", username);
            submitter.Variables.Add("pw", password);
            submitter.Variables.Add("ver", "2");

            // it's a GET request
            submitter.Verb = HttpVerb.Get;

            // log the request
            ApiLogEntry logEntry = new ApiLogEntry(ApiLogSource.ChannelAdvisor, "TestConnectivity");
            logEntry.LogRequest(submitter);

            // execute the request
            try
            {
                using (IHttpResponseReader responseReader = submitter.GetResponse())
                {
                    if (responseReader.HttpWebResponse.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        return false;
                    }

                    // get the response data out
                    string response = responseReader.ReadResult();
                    logEntry.LogResponse(response);

                    response = XmlUtility.StripInvalidXmlCharacters(response);

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(response);

                    XmlNode errorNode = doc.SelectSingleNode("//Error");
                    if (errorNode != null)
                    {
                        log.ErrorFormat("Failed testing CA Profile ID for connectivity: {0}", errorNode.InnerText);

                        return false;
                    }

                    return true;
                }
            }
            catch (XmlException ex)
            {
                log.Error("Failed testing CA Profile ID for connectivity.", ex);
                return false;
            }
            catch (Exception ex)
            {
                if (WebHelper.IsWebException(ex))
                {
                    log.Error("Failed testing CA Profile ID for connectivity.", ex);
                    return false;
                }

                throw;
            }
        }
    }
}
