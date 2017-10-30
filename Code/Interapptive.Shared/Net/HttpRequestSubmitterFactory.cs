using Interapptive.Shared.ComponentRegistration;

namespace Interapptive.Shared.Net
{
    /// <summary>
    /// Creates requested HttpRequestSubmitterFactory
    /// </summary>
    [Component]
    public class HttpRequestSubmitterFactory : IHttpRequestSubmitterFactory
    {
        /// <summary>
        /// Get an HttpBinaryPostRequestSubmitter
        /// </summary>
        public IHttpRequestSubmitter GetHttpBinaryPostRequestSubmitter(byte[] postData) =>
            new HttpBinaryPostRequestSubmitter(postData);

        /// <summary>
        /// Gets an HttpTextPostRequestSubmitter
        /// </summary>
        public IHttpRequestSubmitter GetHttpTextPostRequestSubmitter(string text, string contentType) =>
            new HttpTextPostRequestSubmitter(text, contentType);

        /// <summary>
        /// Gets the HTTP variable request submitter.
        /// </summary>
        public IHttpVariableRequestSubmitter GetHttpVariableRequestSubmitter() =>
            new HttpVariableRequestSubmitter();
    }
}
