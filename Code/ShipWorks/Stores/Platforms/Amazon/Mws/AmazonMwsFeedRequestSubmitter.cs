using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.Amazon.Mws
{
    /// <summary>
    /// Submits Amazon MWS Feed XML requests to Amazon.
    /// </summary>
    [SuppressMessage("CSharp.Analyzers",
        "CA5351: Do not use insecure cryptographic algorithm MD5",
        Justification = "This is what ShipWorks currently uses")]
    public class AmazonMwsFeedRequestSubmitter : HttpVariableRequestSubmitter
    {
        string feedContent = "";
        byte[] encodedContent = new byte[0];

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonMwsFeedRequestSubmitter()
        {
            ContentType = "text/xml; charset=iso-8859-1";

            // Subscribe to the request submitting event, so we can set the content type
            // and what format we expect the response to be in
            this.RequestSubmitting += new HttpRequestSubmittingEventHandler(OnRequestSubmitting);
        }

        /// <summary>
        /// The feed data to be uploaded to Amazon
        /// </summary>
        public string FeedContent
        {
            get
            {
                return feedContent;
            }
            set
            {
                feedContent = value;

                if (feedContent == null)
                {
                    encodedContent = new byte[0];
                }
                else
                {
                    encodedContent = StringUtility.Iso8859Encoding.GetBytes(feedContent);
                }
            }
        }

        /// <summary>
        /// Gets the querystring for the request.  We are POSTing content while still using querystring parameters
        /// </summary>
        protected override string GetQueryString()
        {
            return QueryStringUtility.GetQueryString(Variables, VariableEncodingCasing);
        }

        /// <summary>
        /// Content to send to Amazon
        /// </summary>
        public override byte[] GetPostContent()
        {
            // return the bytes
            return encodedContent;
        }

        /// <summary>
        /// Called when [request submitting]. We need to intercept the request, so the
        /// content type and response format can be set to XML.
        /// </summary>
        void OnRequestSubmitting(object sender, HttpRequestSubmittingEventArgs e)
        {
            // attach MD5 header
            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                byte[] hash = md5.ComputeHash(encodedContent);
                string md5Hash = Convert.ToBase64String(hash);

                e.HttpWebRequest.Headers.Add("Content-MD5", md5Hash);
            }
        }
    }
}
