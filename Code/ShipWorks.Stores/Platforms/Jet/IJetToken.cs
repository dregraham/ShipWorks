using Interapptive.Shared.Net;

namespace ShipWorks.Stores.Platforms.Jet
{
    /// <summary>
    ///  Jet Token for authenticating jet requests
    /// </summary>
    public interface IJetToken
    {
        /// <summary>
        /// Check to see if the token is valid
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Attach the token to the request
        /// </summary>
        void AttachTo(IHttpRequestSubmitter requestSubmitter);
    }
}