using System;
using Interapptive.Shared.Utility;

namespace ShipWorks.ApplicationCore.Licensing.TangoRequests
{
    /// <summary>
    /// Get Tango redirect token
    /// </summary>
    public interface ITangoGetRedirectToken
    {
        /// <summary>
        /// Get Tango redirect token
        /// </summary>
        GenericResult<TokenResponse> GetRedirectToken();
    }
}
