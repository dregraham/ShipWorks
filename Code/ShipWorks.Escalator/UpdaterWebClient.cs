using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using log4net;
using System.Reflection;

namespace ShipWorks.Escalator
{
    /// <summary>
    /// Communicates with tango to download the requested version
    /// </summary>
    public class UpdaterWebClient
    {
        private readonly static Lazy<Version> version = new Lazy<Version>(() =>
        {
            // Tango requires a specific version in order to know when to return
            // legacy responses or new response for the customer license. This is
            // primarily for debug/internal versions of ShipWorks that have 0.0.0.x
            // version number.
            Version assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
            Version minimumVersion = new Version(5, 0, 0, 0);

            return assemblyVersion.Major == 0 ? minimumVersion : assemblyVersion;
        });

        private static readonly ILog log = LogManager.GetLogger(typeof(UpdaterWebClient));

        private static readonly Lazy<HttpClient> tangoClient = new Lazy<HttpClient>(GetHttpClient);
        private static readonly WebClient downloadClient = new WebClient();

        string tangoUrl = "http://www.interapptive.com/ShipWorksNet/ShipWorksV1.svc/account/shipworks";

        /// <summary>
        /// Download the requested version
        /// </summary>
        public async Task<InstallFile> Download(Version version)
        {
            log.Info("Download called");
            (Uri url, string sha) = await GetVersionToDownload(version).ConfigureAwait(false);

            string installationFileSavePath = GetInstallationFileSavePath(url);

            log.InfoFormat("Downloading file to {0}", installationFileSavePath);
            await downloadClient.DownloadFileTaskAsync(url, installationFileSavePath).ConfigureAwait(false);
            log.Info("File Downloaded");

            return new InstallFile(installationFileSavePath, sha);
        }

        /// <summary>
        /// Get the path to save the install file to
        /// </summary>
        private static string GetInstallationFileSavePath(Uri url)
        {
            string fileName = Path.GetFileName(url.LocalPath);
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            return Path.Combine(appData, "Interapptive\\ShipWorks\\Instances", ServiceName.GetInstanceID().ToString("B"), fileName);
        }

        /// <summary>
        /// Get the url and sha of requested version
        /// </summary>
        private async Task<(Uri url, string sha)> GetVersionToDownload(Version version)
        {
            log.InfoFormat("Attempting to get version {0}", version);

            var values = new Dictionary<string, string>
            {
                { "action", "getreleasebyversion" },
                { "version", version.ToString() }
            };

            FormUrlEncodedContent content = new FormUrlEncodedContent(values);

            string response;
            using (HttpResponseMessage responseMessage = await tangoClient.Value.PostAsync(tangoUrl, content).ConfigureAwait(false))
            {
                response = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            log.InfoFormat("Response received: {0}", response);

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(response);

            string url = xmlResponse.SelectSingleNode("//Update//Url")?.InnerText ?? string.Empty;
            Uri.TryCreate(url, UriKind.Absolute, out Uri uri);

            string sha = xmlResponse.SelectSingleNode("//Update//SHA256")?.InnerText ?? string.Empty;

            log.InfoFormat("Url: {0}", url);
            log.InfoFormat("sha: {0}", sha);
            return (uri, sha);
        }

        /// <summary>
        ///  Setup the HttpClient
        /// </summary>
        private static HttpClient GetHttpClient()
        {
            log.Info("Configuring Client");

            HttpClientHandler handler = new HttpClientHandler();

            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("X-SHIPWORKS-VERSION", version.ToString());
            client.DefaultRequestHeaders.Add("X-SHIPWORKS-USER", "$h1pw0rks");
            client.DefaultRequestHeaders.Add("X-SHIPWORKS-PASS", "q2*lrft");
            client.DefaultRequestHeaders.Add("SOAPAction", "http://stamps.com/xml/namespace/2015/06/shipworks/shipworksv1/IShipWorks/ShipworksPost");
            client.DefaultRequestHeaders.UserAgent.ParseAdd("shipworks");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

            return client;
        }
    }
}
