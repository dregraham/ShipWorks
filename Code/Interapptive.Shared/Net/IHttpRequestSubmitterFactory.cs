namespace Interapptive.Shared.Net
{
    /// <summary>
    /// HttpRequestSubmitterFactory
    /// </summary>
    public interface IHttpRequestSubmitterFactory
    {
        /// <summary>
        /// Get an HttpBinaryPostRequestSubmitter
        /// </summary>
        IHttpRequestSubmitter GetHttpBinaryPostRequestSubmitter(byte[] postData);

        /// <summary>
        /// Gets an HttpTextPostRequestSubmitter
        /// </summary>
        IHttpRequestSubmitter GetHttpTextPostRequestSubmitter(string text, string contentType);

        /// <summary>
        /// Gets the HTTP variable request submitter.
        /// </summary>
        IHttpVariableRequestSubmitter GetHttpVariableRequestSubmitter();
    }
}