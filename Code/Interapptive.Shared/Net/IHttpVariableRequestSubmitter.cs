namespace Interapptive.Shared.Net
{
    /// <summary>
    /// interface that represents an Http post request that posts name\value pairs.
    /// </summary>
    public interface IHttpVariableRequestSubmitter : IHttpRequestSubmitter
    {
        /// <summary>
        /// The casing to use when encoding query string values (like '%2c' vs '%2C').  According to the web rules
        /// this shouldn't matter but in practice sometimes it does.
        /// </summary>
        QueryStringEncodingCasing VariableEncodingCasing { get; set; }

        /// <summary>
        /// The variables to be posted
        /// </summary>
        IHttpVariableCollection Variables { get; }
    }
}