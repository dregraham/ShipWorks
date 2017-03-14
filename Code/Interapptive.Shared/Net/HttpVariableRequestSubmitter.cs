using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Net;
using System.Web;
using Interapptive.Shared.Utility;

namespace Interapptive.Shared.Net
{
    /// <summary>
    /// Http post request that posts name\value pairs.
    /// </summary>
    public class HttpVariableRequestSubmitter : HttpRequestSubmitter, IHttpVariableRequestSubmitter
    {
        // The variables to be posted
        HttpVariableCollection variables = new HttpVariableCollection();

        // Casing to be used for query string encoding
        QueryStringEncodingCasing encodingCasing = QueryStringEncodingCasing.Default;

        /// <summary>
        /// Constructor
        /// </summary>
        public HttpVariableRequestSubmitter()
        {
            // Specify the contenttype
            ContentType = "application/x-www-form-urlencoded";
        }

        /// <summary>
        /// The variables to be posted
        /// </summary>
        public HttpVariableCollection Variables
        {
            get { return variables; }
        }

        /// <summary>
        /// The casing to use when encoding query string values (like '%2c' vs '%2C').  According to the web rules
        /// this shouldn't matter but in practice sometimes it does.
        /// </summary>
        public QueryStringEncodingCasing VariableEncodingCasing
        {
            get { return encodingCasing; }
            set { encodingCasing = value; }
        }

        /// <summary>
        /// Gets the Uri for the request
        /// </summary>
        protected override string GetQueryString()
        {
            if (Verb == HttpVerb.Get)
            {
               return QueryStringUtility.GetQueryString(Variables, VariableEncodingCasing);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Prepare the content of the request
        /// </summary>
        public override byte[] GetPostContent()
        {
            // Encode the query string to bytes
            return StringUtility.Iso8859Encoding.GetBytes(QueryStringUtility.GetQueryString(Variables, VariableEncodingCasing));
        }
    }
}
