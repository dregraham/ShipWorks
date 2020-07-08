using System;
using System.Linq;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.Shopify.DTOs
{
    /// <summary>
    /// ShopifyResponse
    /// </summary>
    public class ShopifyResponse
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyResponse(IHttpResponseReader responseReader)
        {
            Content = responseReader.ReadResult();

            var nextLinkHeader = responseReader.HttpWebResponse.Headers.GetValues("Link")?.FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(nextLinkHeader))
            {
                nextLinkHeader = nextLinkHeader
                    .Split(',')
                    .FirstOrDefault(u => u.Contains("next"));

                if (!nextLinkHeader.IsNullOrWhiteSpace())
                {
                    NextPageUrl = nextLinkHeader
                        .Replace("<", string.Empty)
                        .Substring(0, nextLinkHeader.IndexOf(">", StringComparison.Ordinal) - 1);
                }
            }
        }

        /// <summary>
        /// Content from Shopify
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// URL to next page of orders (empty string means we are on the last page)
        /// </summary>
        public string NextPageUrl { get; set; }
    }
}