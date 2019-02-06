using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml;
using System.Net;
using System.IO;
using System.Security.Cryptography;

namespace ShipWorks.Escalator
{
    /// <summary>
    /// Communicates with tango to download the requested version
    /// </summary>
    public class UpdaterWebClient
    {
        private static readonly Lazy<HttpClient> tangoClient = new Lazy<HttpClient>(GetHttpClient);
        private static readonly WebClient downloadClient = new WebClient();

        string tangoUrl = "https://www.interapptive.com/ShipWorksNet/ShipWorksV1.svc/account/shipworks";
        SHA256 SHA256 = SHA256.Create();

        /// <summary>
        /// Download the requested version
        /// </summary>
        public async Task<InstallFile> Download(Version version)
        {
            (Uri url, string sha) = await GetVersionToDownload(version).ConfigureAwait(false);

            downloadClient.DownloadFile(url, Path.GetFileName(url.LocalPath));

            return new InstallFile(url.LocalPath, sha);
        }

        /// <summary>
        /// Get the url and sha of requested version
        /// </summary>
        private async Task<(Uri url, string sha)> GetVersionToDownload(Version version)
        {
            var values = new Dictionary<string, string>
            {
                { "action", "getreleasebyversion" },
                { "version", version.ToString() }
            };

            FormUrlEncodedContent content = new FormUrlEncodedContent(values);

            string response;
            using (HttpResponseMessage responseMessage = await tangoClient.Value.PostAsync(tangoUrl, content).ConfigureAwait(false))
            {
                response = await responseMessage.Content.ReadAsStringAsync();
            }

            XmlDocument xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(response);

            string url = xmlResponse.SelectSingleNode("//Update//Url")?.InnerText ?? string.Empty;
            Uri.TryCreate(url, UriKind.Absolute, out Uri uri);

            string sha = xmlResponse.SelectSingleNode("//Update//SHA256")?.InnerText ?? string.Empty;

            return (uri, sha);
        }

        /// <summary>
        ///  Setup the HttpClient
        /// </summary>
        private static HttpClient GetHttpClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-SHIPWORKS-VERSION", "5.0.0.0");
            client.DefaultRequestHeaders.Add("X-SHIPWORKS-USER", "$h1pw0rks");
            client.DefaultRequestHeaders.Add("X-SHIPWORKS-PASS", "q2*lrft");
            client.DefaultRequestHeaders.Add("SOAPAction", "http://stamps.com/xml/namespace/2015/06/shipworks/shipworksv1/IShipWorks/ShipworksPost");
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("shipworks"));
            client.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");

            return client;
        }
    }
}
