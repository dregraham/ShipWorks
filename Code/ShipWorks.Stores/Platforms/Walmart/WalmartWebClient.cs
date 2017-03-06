using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using Interapptive.Shared.Utility;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Walmart.DTO;

namespace ShipWorks.Stores.Platforms.Walmart
{
    /// <summary>
    /// Web client for interacting with Walmart
    /// </summary>
    /// <seealso cref="ShipWorks.Stores.Platforms.Walmart.IWalmartWebClient" />
    [Component]
    public class WalmartWebClient : IWalmartWebClient
    {
        private readonly IWalmartRequestSigner requestSigner;
        private readonly Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory;
        private const string TestConnectionUrl = "https://marketplace.walmartapis.com/v3/feeds";

        /// <summary>
        /// Initializes a new instance of the <see cref="WalmartWebClient"/> class.
        /// </summary>
        public WalmartWebClient(IWalmartRequestSigner requestSigner, Func<ApiLogSource, string, IApiLogEntry> apiLogEntryFactory)
        {
            this.requestSigner = requestSigner;
            this.apiLogEntryFactory = apiLogEntryFactory;
        }

        /// <summary>
        /// Tests the connection to Walmart, throws if invalid credentials
        /// </summary>
        public void TestConnection(WalmartStoreEntity store)
        {
            HttpXmlVariableRequestSubmitter requestSubmitter = new HttpXmlVariableRequestSubmitter();
            requestSubmitter.Uri = new Uri(TestConnectionUrl);
            requestSubmitter.Verb = HttpVerb.Get;

            ProcessRequest(store, requestSubmitter, "TestConnection");
        }

        /// <summary>
        /// Executes a request
        /// </summary>
        private void ProcessRequest(WalmartStoreEntity store, HttpRequestSubmitter submitter, string action)
        {
            submitter.Headers.Add("WM_SVC.NAME", "Walmart Marketplace");
            submitter.Headers.Add("WM_CONSUMER.ID", store.ConsumerID);
            submitter.Headers.Add("WM_CONSUMER.CHANNEL.TYPE", store.ChannelType);
            submitter.Headers.Add("WM_QOS.CORRELATION_ID", Guid.NewGuid().ToString());

            string epoch = (DateTimeUtility.ToUnixTimestamp(DateTime.UtcNow) * 1000).ToString(CultureInfo.InvariantCulture);

            requestSigner.Sign(submitter, store, epoch);

            try
            {
                IApiLogEntry logEntry = apiLogEntryFactory(ApiLogSource.Walmart, action);
                logEntry.LogRequest(submitter);

                using (IHttpResponseReader reader = submitter.GetResponse())
                {
                    string responseData = reader.ReadResult();
                    logEntry.LogResponse(responseData, "xml");
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(WalmartException));
            }
        }

        public IEnumerable<Order> GetOrders(DateTime start)
        {
            throw new NotImplementedException();
        }

        private void Acknowledge(string purchaseOrderId)
        {
            throw new NotImplementedException();
        }
    }
}