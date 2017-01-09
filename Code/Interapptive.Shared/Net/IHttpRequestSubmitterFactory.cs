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
        HttpRequestSubmitter GetHttpBinaryPostRequestSubmitter(byte[] postData);

        /// <summary>
        /// Gets an HttpTextPostRequestSubmitter
        /// </summary>
        HttpRequestSubmitter GetHttpTextPostRequestSubmitter(string text, string contentType);
    }
}