using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.IO;
using log4net;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using Interapptive.Shared.ComponentRegistration;
using System.Reflection;

namespace ShipWorks.Escalator
{
    /// <summary>
    /// Communicates with tango to download the requested version
    /// </summary>
    [Component(SingleInstance = true)]
    public class UpdaterWebClient : IUpdaterWebClient
    {
        private readonly ILog log;

        private static readonly Lazy<HttpClient> tangoClient = new Lazy<HttpClient>(GetHttpClient);
        private static readonly WebClient downloadClient = new WebClient();
        private readonly string tangoUrl = "https://www.interapptive.com/ShipWorksNet/ShipWorksV1.svc/account/shipworks";
        private readonly IServiceName serviceName;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpdaterWebClient(IServiceName serviceName, Func<Type, ILog> logFactory)
        {
            this.serviceName = serviceName;
            log = logFactory(GetType());
        }

        /// <summary>
        /// Download the requested version
        /// </summary>
        public async Task<InstallFile> Download(Uri url, string sha)
        {
            log.Info("Download called");

            string installationFileSavePath = GetInstallationFileSavePath(url);

            log.InfoFormat("Downloading file to {0}", installationFileSavePath);
            await downloadClient.DownloadFileTaskAsync(url, installationFileSavePath).ConfigureAwait(false);
            log.Info("File Downloaded");

            return new InstallFile(installationFileSavePath, sha);
        }

        /// <summary>
        /// Get the path to save the install file to
        /// </summary>
        private string GetInstallationFileSavePath(Uri url)
        {
            string fileName = Path.GetFileName(url.LocalPath);
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            return Path.Combine(appData, "Interapptive\\ShipWorks\\Instances", serviceName.GetInstanceID().ToString("B"), fileName);
        }

        /// <summary>
        /// Get the url and sha of requesting customer id
        /// </summary>
        public async Task<ShipWorksRelease> GetVersionToDownload(string tangoCustomerId, Version currentVersion)
        {
            log.InfoFormat("Attempting to get new version for tango customer {0} running version {1}", tangoCustomerId, currentVersion);

            var values = new Dictionary<string, string>
            {
                { "action", "getreleasebyuser" },
                { "customerid", tangoCustomerId },
                { "version", currentVersion.ToString() }
            };

            return await GetVersionToDownload(values).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the url and sha of requested version
        /// </summary>
        public async Task<ShipWorksRelease> GetVersionToDownload(Version version)
        {
            log.InfoFormat("Attempting to get version {0}", version);

            var values = new Dictionary<string, string>
            {
                { "action", "getreleasebyversion" },
                { "version", version.ToString() }
            };

            return await GetVersionToDownload(values).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the url and sha for the requested form values
        /// </summary>
        private async Task<ShipWorksRelease> GetVersionToDownload(Dictionary<string, string> formValues)
        {
            FormUrlEncodedContent content = new FormUrlEncodedContent(formValues);

            string response;
            using (HttpResponseMessage responseMessage = await tangoClient.Value.PostAsync(tangoUrl, content).ConfigureAwait(false))
            {
                response = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            log.InfoFormat("Response received: {0}", response);

            if (response.TryParseXml(out ShipWorksRelease result))
            {
                log.Info("Desearialized result to ShipWorksRelease.");
                return result;
            }
            else
            {
                log.Info("Could not deserialize result to ShipWorksRelease.");
                return null;
            }
        }

        /// <summary>
        ///  Setup the HttpClient
        /// </summary>
        private static HttpClient GetHttpClient()
        {
            HttpClientHandler handler = new HttpClientHandler();

            HttpClient client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("X-SHIPWORKS-VERSION", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            client.DefaultRequestHeaders.Add("X-SHIPWORKS-USER", "$h1pw0rks");
            client.DefaultRequestHeaders.Add("X-SHIPWORKS-PASS", "q2*lrft");
            client.DefaultRequestHeaders.Add("SOAPAction", "http://stamps.com/xml/namespace/2015/06/shipworks/shipworksv1/IShipWorks/ShipworksPost");
            client.DefaultRequestHeaders.UserAgent.ParseAdd("shipworks");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

            return client;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            downloadClient?.Dispose();
        }
    }
}
