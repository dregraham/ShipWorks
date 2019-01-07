using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;

namespace Interapptive.Shared.Net
{
    /// <summary>
    /// Http post request that posts name\value pairs.
    /// </summary>
    [Component]
    public class HttpVariableRequestSubmitter : HttpRequestSubmitter, IHttpVariableRequestSubmitter
    {
        // The variables to be posted
        IHttpVariableCollection variables = new HttpVariableCollection();

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
        public IHttpVariableCollection Variables
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
