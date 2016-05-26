using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using log4net;
using System.Web;
using Interapptive.Shared;
using Interapptive.Shared.Net;

namespace ShipWorks.Stores.Platforms.Volusion
{
    /// <summary>
    /// Mimics a web browser for screen-scraping information from
    /// Volusion's website since some functionality isn't available via
    /// any api.
    ///
    /// Functionality provided:
    ///     Shipping Method downloads
    ///     Payment Method downloads
    ///     Retreiving the user's EncryptedPassword
    /// </summary>
    public class VolusionWebSession
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(VolusionWebSession));

        // Types of reports/queries to define and run
        enum ReportType
        {
            ShippingMethods,
            PaymentMethods
        }

        // Regular expression for pulling out the volusion submission token - needed when posting values to their web app
        private string postTokenRegex = "id=\"NOSAVE___Form_Submission_Token\".*value=\"(?<token>[A-F0-9]{15,})\"";

        // logged in or not
        private bool loggedIn = false;

        // store url
        private string storeUrl;

        // cookie container for session conversation
        private CookieContainer cookieContainer = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public VolusionWebSession(string storeUrl)
        {
            if (storeUrl == null || storeUrl.Length == 0)
            {
                throw new ArgumentException("storeUrl must be specified.");
            }

            this.storeUrl = storeUrl;
        }

        /// <summary>
        /// Performa  login on the Volusion website
        /// </summary>
        public bool LogOn(string username, string password)
        {
            if (loggedIn)
            {
                loggedIn = false;
            }

            // perform login and return success and/or failure
            try
            {
                log.Info("Preparing to LogOn to the Volusion Store website");

                // prepare the cookie container
                cookieContainer = new CookieContainer();

                // setup the two urls to be hit
                string loginUrl = $"https://my.volusion.com/TransferLogin.aspx?HostName={GetStoreHostName().Replace(".", "%2E")}&PageName=login.asp";
                string adminUrl = GetUrl("admin/");

                HttpWebRequest request = CreateWebRequest(adminUrl, "GET");

                // just access the login page to initiate any server-side session
                HttpWebResponse response;
                using (response = (HttpWebResponse) request.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new VolusionException("Unable to access store login page.");
                    }
                }

                loggedIn = LogInToVolusion(username, password, loginUrl);

                return loggedIn;
            }
            catch (UriFormatException ex)
            {
                log.Error("Unable to simulate a website logon.", ex);
                return false;
            }
            catch (Exception ex)
            {
                if (WebHelper.IsWebException(ex))
                {
                    log.Error("Unable to simulate a website logon.", ex);
                    return false;
                }

                throw;
            }
        }

        private bool LogInToVolusion(string username, string password, string loginUrl)
        {
            NetworkUtility networkUtility = new NetworkUtility();

            // now post the user-supplied credentials to the login page to complete the login
            string content = MakeParameter("CustomerNewOld", "old") + MakeParameter("email", username) + MakeParameter("password", password) + MakeParameter("IP_ADDRESS", networkUtility.GetPublicIPAddress());
            if (content.StartsWith("&", StringComparison.OrdinalIgnoreCase))
            {
                content = content.Substring(1);
            }

            // Get the request as bytes to be posted to the server
            byte[] contentBytes = Encoding.ASCII.GetBytes(content);

            // due to Volusion's use of HttpOnly (protected) cookies, we can't just set AllowAutoRedirect on a request
            // and let .NET handle the numerous login redirects they do.  This results in a very important cookie being
            // ommitted along the way and the login fails.
            using (HttpWebResponse response = ExecuteLoginWithRedirects(loginUrl, contentBytes))
            {
                log.Info("LogOn Request response code = " + response.StatusCode);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    // if we were told to go back to the login page, we didn't get logged in
                    if (Regex.IsMatch(response.ResponseUri.AbsolutePath, "login.asp", RegexOptions.IgnoreCase))
                    {
                        log.Info("Unable to locate text text login.asp in the response, assuming a failed logon.");
                        return false;
                    }
                    log.Info("Logon success.");
                    return true;
                }
                log.Error("Failed request");
                return false;
            }
        }

        /// <summary>
        /// Perform a Volusion login, manually handling the redirects they use.
        /// </summary>
        private HttpWebResponse ExecuteLoginWithRedirects(string loginUrl, byte[] contentBytes)
        {
            int maxRedirects = 10;

            // Kick off the login process with a POST to the login url
            HttpWebRequest request = CreateWebRequest(loginUrl, "POST");
            request.ContentType = "application/x-www-form-urlencoded";
            request.AllowAutoRedirect = false;
            request.ContentLength = contentBytes.Length;

            // write the request bytes
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(contentBytes, 0, contentBytes.Length);
            }

            // execute the request
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            // "Slt" cookie we will be manually creating at some point
            Cookie sltCookie = null;

            // perform automatic redirecting in a loop
            while (response.StatusCode == HttpStatusCode.Found)
            {
                // see if we're being told to redirect, via the Location header
                string location = response.Headers["location"];
                if (String.IsNullOrEmpty(location))
                {
                    break;
                }

                // prevent an infinite request loop
                maxRedirects--;
                if (maxRedirects == 0)
                {
                    throw new VolusionException("Maximum number of login redirects reached.");
                }

                // close the previous request
                response.Close();

                // configure a GET request
                request = CreateWebRequest(location, "GET");
                request.AllowAutoRedirect = false;

                // add the myserious "slt" cookie
                if (sltCookie != null)
                {
                    cookieContainer.Add(sltCookie);
                }

                // execute the redirect
                response = (HttpWebResponse)request.GetResponse();

                // search for the mysterious "slt" cookie
                sltCookie = ReadSltCookie(response);
            }

            // it is up to the caller to close this
            return response;
        }

        /// <summary>
        /// Parses the headers for a cookie named "slt".  Volusion sets this has
        /// an HTTPOnly cookie, which .NET doesn't include in the cookie collection.
        /// </summary>
        private Cookie ReadSltCookie(HttpWebResponse response)
        {
            Cookie sltCookie = null;

            // get the raw header for setting cookie values
            string setCookieHeader = response.Headers["set-cookie"];
            if (!String.IsNullOrEmpty(setCookieHeader))
            {
                // look for a cookie value named "slt"
                string sltValue = Regex.Match(setCookieHeader, "slt=([^;]+)", RegexOptions.IgnoreCase).Groups[1].Value;
                if (!String.IsNullOrEmpty(sltValue))
                {
                    sltCookie = new Cookie("slt", sltValue, "/", GetStoreHostName());
                }
            }

            return sltCookie;
        }

        /// <summary>
        /// Logs out of the volusion website
        /// </summary>
        public void LogOff()
        {
            log.Info("Logging off of the Volusion website");
            if (!loggedIn)
            {
                return;
            }

            string logoutUrl = GetUrl("login.asp?logout=yes");

            HttpWebRequest request = CreateWebRequest(logoutUrl, "GET");
            using (request.GetResponse())
            {

            }
        }

        #region Utility Methods

        /// <summary>
        /// Takes the name\value pair and makes it into a URL parameter
        /// </summary>
        private string MakeParameter(string name, string val)
        {
            if (name.Length == 0)
                return "";

            return "&" + HttpUtility.UrlEncode(name) + "=" + HttpUtility.UrlEncode(val);
        }

        /// <summary>
        /// Gets the hostname for the user's store
        /// </summary>
        private string GetStoreHostName()
        {
            Uri storeUri = new Uri(storeUrl);
            return storeUri.Host;
        }

        /// <summary>
        /// Append relative path to the store root url
        /// </summary>
        private string GetUrl(string relativePath)
        {
            string url = storeUrl;
            if (!url.EndsWith("/"))
            {
                if (!relativePath.StartsWith("/"))
                {
                    url += "/";
                }
            }
            else
            {
                if (relativePath.StartsWith("/"))
                {
                    // need to remove the trailing slash
                    url = url.Substring(0, url.Length - 1);
                }
            }

            url += relativePath;

            return url;
        }

        /// <summary>
        /// Creates a web request and sets its necessary properties
        /// </summary>
        private HttpWebRequest CreateWebRequest(string url, string method)
        {
            if (url.StartsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                if (storeUrl.EndsWith("/", StringComparison.OrdinalIgnoreCase))
                {
                    url = url.Substring(1);
                }
                url = storeUrl + url;
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = cookieContainer;
            request.Method = method;
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0;)";

            return request;
        }

        #endregion

        /// <summary>
        /// Accesses the page at the specified url and parses the html for the post token
        /// </summary>
        private string GetFormSubmissionToken(string url)
        {
            log.Info("Retreiving form submission token");
            HttpWebRequest request = CreateWebRequest(url, "GET");
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string data = reader.ReadToEnd();

                    Regex tokenRegex = new Regex(postTokenRegex);
                    Match match = tokenRegex.Match(data);
                    if (match.Success)
                    {
                        log.Info("Token = " + match.Groups["token"].Value);
                        return match.Groups["token"].Value;
                    }
                    else
                    {
                        throw new VolusionException("Unable to locate the Form Submission Token.");
                    }
                }
            }
        }

        /// <summary>
        /// Creates a Volusion QueryBank saved query and returns the title so it can be exported
        /// </summary>
        [NDependIgnoreLongMethod]
        private string CreateQueryBankQuery(ReportType reportType)
        {
            if (!loggedIn)
            {
                throw new InvalidOperationException("Must be logged in to perform this operation.");
            }

            string table = "";
            string query = "";
            string reportTitle = "";
            if (reportType == ReportType.ShippingMethods)
            {
                table = "shippingmethods";
                query = "usp_PagedItems 1, 500, '', '', '', 'SELECT ShippingMethods.ShippingMethodID, ShippingMethods.ShippingMethodName, ShippingMethods.ServiceCode, ShippingMethods.Gateway FROM shippingmethods WITH (NOLOCK) ORDER BY ShippingMethods.ShippingOrderBy'";
                reportTitle = "Shipworks_shipping_methods_" + Guid.NewGuid().ToString().Substring(0, 8);
            }
            else if (reportType == ReportType.PaymentMethods)
            {
                table = "paymentmethods";
                query = "usp_PagedItems 1, 500, '', '', '', 'SELECT PaymentMethods.PaymentMethodID, PaymentMethods.PaymentMethodType, PaymentMethods.PaymentMethod FROM paymentmethods WITH (NOLOCK) ORDER BY PaymentMethods.PaymentMethodID'";
                reportTitle = "Shipworks_payment_methods_" + Guid.NewGuid().ToString().Substring(0, 8);
            }

            string formSubmissionToken = GetFormSubmissionToken(GetUrl("/admin/TableViewer.asp?table=" + table));
            string tableViewerUrl = GetUrl("/admin/TableViewer.asp?table=shippingmethods");

            string postValues = "NOSAVE___Form_Submission_Token=" + formSubmissionToken;
            postValues += "&QB_Export_Title=" + reportTitle;
            postValues += "&QB_Export_Gobal=Y";
            postValues += "&QB_Global=Y";
            postValues += "&table=" + table;
            postValues += "&BUpdates_Table=" + table;
            postValues += "&Unique_TableViewer_SQL_" + table + "=" + HttpUtility.UrlEncode(query);
            postValues += "&Page=1";
            postValues += "&RowsPerPage=500";
            postValues += "&submit.export=" + HttpUtility.UrlEncode("Export these results");

            log.Info("Creating QueryBankQuery by POSTing " + postValues);

            HttpWebRequest request = CreateWebRequest(tableViewerUrl, "POST");
            request.ContentType = "application/x-www-form-urlencoded";

            // get the request bytes and set the request length before writing
            byte[] requestBytes = System.Text.Encoding.ASCII.GetBytes(postValues);
            request.ContentLength = requestBytes.Length;

            // write the request
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(requestBytes, 0, requestBytes.Length);
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    // return the generated report title
                    log.Info("Generated report title = " + reportTitle);
                    return reportTitle;
                }
                else
                {
                    throw new VolusionException("Unable to create the requested QueryBank query.");
                }
            }
        }

        /// <summary>
        /// Returns the report id (QB_ID) and submission token for the
        /// provided saved query name
        /// </summary>
        private string[] GetReportDetail(string queryName)
        {
            log.InfoFormat("GetReportDetail for query '{0}'", queryName);
            if (!loggedIn)
            {
                throw new InvalidOperationException("Must be logged in to perform this operation.");
            }

            HttpWebRequest request = CreateWebRequest(GetUrl("/admin/db_export.asp?QueryBank=Y&QB_ID="), "GET");
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string reportID = "";
                        string postToken = "";

                        string page = reader.ReadToEnd();

                        // pull out the form submission token
                        postToken = Regex.Match(page, postTokenRegex).Groups["token"].Value;

                        // pull out the qb_id
                        reportID = Regex.Match(page, @"name=""QB_ID"".*value=""(?<value>[0-9]+)"".*" + queryName + @".*\<", RegexOptions.Singleline).Groups["value"].Value;

                        // log data
                        log.InfoFormat("Found report detail: reportID = {0}, postToken = {1}", reportID, postToken);

                        return new string[] { reportID, postToken };
                    }
                }

                throw new VolusionException("Unable to retrieve report details.");
            }
        }

        /// <summary>
        /// Returns export/report data for a particular QueryBank ID
        /// </summary>
        private string GetReportData(string reportID, string postToken)
        {
            log.InfoFormat("GetReportData for reportID = {0}, postToken = {1}", reportID, postToken);

            string url = GetUrl("/admin/db_export.asp");

            HttpWebRequest request = CreateWebRequest(url, "POST");
            request.ContentType = "application/x-www-form-urlencoded";

            string postData = "NOSAVE___Form_Submission_Token=" + postToken;
            postData += "&QB_ID=" + reportID;
            postData += "&QueryBank=Y";
            postData += "&FileType=CSV";

            log.InfoFormat("postData = {0}", postData);

            byte[] requestBytes = System.Text.Encoding.ASCII.GetBytes(postData);
            request.ContentLength = requestBytes.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(requestBytes, 0, requestBytes.Length);
            }

            string path = "";
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string page = reader.ReadToEnd();

                        // locate path to the file to download with the actual data! Almost there!
                        path = Regex.Match(page, @"(?<file>[^""]*SAVED_EXPORT_.*\.CSV)", RegexOptions.IgnoreCase).Groups["file"].Value;
                        log.InfoFormat("Export Url = {0}", path);
                    }
                }
            }

            if (path.Length == 0)
            {
                throw new VolusionException("Unable to locate Export download path.");
            }

            // go get the real, exported data
            return DownloadText(GetUrl(path));
        }

        /// <summary>
        /// Returns the text/html from the url specialized
        /// </summary>
        private string DownloadText(string url)
        {
            log.InfoFormat("Download report text content from {0}", url);

            HttpWebRequest request = CreateWebRequest(url, "GET");

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string data = reader.ReadToEnd();
                        log.InfoFormat("Downloaded Text = {0}", data);

                        return data;
                    }
                }
                else
                {
                    throw new VolusionException("Unable to download export file from " + url);
                }
            }
        }

        /// <summary>
        /// Get payment methods from the Volusion store admin site
        /// </summary>
        public string RetrievePaymentMethods()
        {
            log.Info("Retrieving payment methods.");
            if (!loggedIn)
            {
                throw new InvalidOperationException("Must be logged in to perform this operation.");
            }

            try
            {
                string reportName = CreateQueryBankQuery(ReportType.PaymentMethods);
                string[] reportDetail = GetReportDetail(reportName);

                if (reportDetail[0].Length == 0 || reportDetail[1].Length == 0)
                {
                    throw new VolusionException("Unable to retrieve payment method export details.");
                }

                string reportID = reportDetail[0];
                string postToken = reportDetail[1];

                return GetReportData(reportID, postToken);
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(VolusionException));
            }
        }

        /// <summary>
        /// Retrieve the shipping methed export
        /// </summary>
        public string RetrieveShippingMethods()
        {
            log.Info("Retrieving shipping methods.");
            if (!loggedIn)
            {
                throw new InvalidOperationException("Must be logged in to perform this operation.");
            }

            try
            {
                string reportName = CreateQueryBankQuery(ReportType.ShippingMethods);
                string[] reportDetail = GetReportDetail(reportName);

                if (reportDetail[0].Length == 0 || reportDetail[1].Length == 0)
                {
                    throw new VolusionException("Unable to retrieve shipping method export details.");
                }

                string reportID = reportDetail[0];
                string postToken = reportDetail[1];

                return GetReportData(reportID, postToken);
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(VolusionException));
            }
        }

        /// <summary>
        /// Retrieve the "Help" page to see if we can locate the EncryptedPassword without requiring users to go through
        /// the many steps to find it themselves
        /// </summary>
        public string RetrieveEncryptedPassword()
        {
            if (!loggedIn)
            {
                throw new InvalidOperationException("Must be logged in to perform this operation.");
            }

            try
            {
                string transferUrl = GetUrl("xFerToNet.asp?URL=%2E%2E%2Fnet%2FWebForm%F5Import%F5Export%F5Help%2Easpx");

                // request a transfer to the asp.net side of their website
                HttpWebRequest request = CreateWebRequest(transferUrl, "GET");
                request.AllowAutoRedirect = false; //.NET isn't handling the redirect from their servers, must handle it ourselves

                string sessionID = "";
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream responseStream = response.GetResponseStream();
                    if (response.StatusCode == HttpStatusCode.Found || response.StatusCode == HttpStatusCode.Redirect)
                    {
                        // pull the session id out of the redirect response
                        sessionID = Regex.Match(response.Headers["location"], @"SessionID=(\d+)", RegexOptions.IgnoreCase).Groups[1].Value;

                        // close the previous response, and continue with the next request
                        response.Close();
                    }
                    else if (response.StatusCode == HttpStatusCode.OK)
                    {
                        // start the screen scraping (this never happened during testing, but just incase we get OK , start parsing)
                        return ParseHelpPageForPassword(responseStream);
                    }
                }

                if (sessionID.Length > 0)
                {
                    // now request the actual target page we want, including the newly obtained Session ID
                    string helpUrl = GetUrl("net/WebForm_Import_Export_Help.aspx");
                    helpUrl += "?SessionID=" + sessionID;

                    request = CreateWebRequest(helpUrl, "GET");
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            return ParseHelpPageForPassword(response.GetResponseStream());
                        }
                    }
                }

                return "";
            }
            catch (Exception ex)
            {
                if (WebHelper.IsWebException(ex))
                {
                    log.Error("Failed to retrieve the encrypted password for Volusion.", ex);

                    // didn't get the password
                    return "";
                }

                throw;
            }
        }

        /// <summary>
        /// Parses the help page for the Encrypted Password that needs to be used when calling
        /// the api
        /// </summary>
        private string ParseHelpPageForPassword(Stream responseStream)
        {
            using (StreamReader reader = new StreamReader(responseStream))
            {
                string content = reader.ReadToEnd();

                Regex encryptedRegex = new Regex("EncryptedPassword=([A-F0-9]+)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                Match match = encryptedRegex.Match(content);
                if (match.Success)
                {
                    string encryptedPassword = match.Groups[1].Value;
                    log.InfoFormat("Parsed response for EncryptedPassword = {0}", encryptedPassword);

                    return encryptedPassword;
                }
                else
                {
                    throw new VolusionException("EncryptedPassword not found in help page.");
                }
            }
        }
    }
}
