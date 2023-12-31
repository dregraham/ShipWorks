﻿using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;

namespace ShipWorks.Stores.Platforms.Jet
{
    /// <summary>
    /// Jet Token for authenticating jet requests
    /// </summary>
    [Component]
    public class JetToken : IJetToken
    {
        private readonly string token;
        private static JetToken invalidToken;

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
        public static IJetToken InvalidToken => invalidToken ?? (invalidToken = new JetToken(string.Empty));

        /// <summary>
        /// Check to see if the token is valid
        /// </summary>
        public bool IsValid => !string.IsNullOrWhiteSpace(token);

        /// <summary>
        /// Attach the token to the request
        /// </summary>
        public void AttachTo(IHttpRequestSubmitter requestSubmitter) => 
            requestSubmitter.Headers.Set("Authorization", $"bearer {token}");
    }
}