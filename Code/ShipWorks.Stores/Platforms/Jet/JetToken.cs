using System.Linq;
using Interapptive.Shared.Net;

namespace ShipWorks.Stores.Platforms.Jet
{
    /// <summary>
    /// Jet Token for authenticating jet requests
    /// </summary>
    public class JetToken
    {
        private readonly string token;

        /// <summary>
        /// Constructor
        /// </summary>
        public JetToken(string token)
        {
            this.token = token;
        }
        
        /// <summary>
        /// Returns an invalid token
        /// </summary>
        public static JetToken InvalidToken => new JetToken(string.Empty);

        /// <summary>
        /// Check to see if the token is valid
        /// </summary>
        public bool IsValid => token != string.Empty;

        /// <summary>
        /// Attach the token to the request
        /// </summary>
        /// <param name="requestSubmitter"></param>
        public void AttachTo(IHttpRequestSubmitter requestSubmitter) => 
            requestSubmitter.Headers.Set("Authorization", $"bearer {token}");
    }
}