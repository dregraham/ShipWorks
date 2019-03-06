using System;
using Interapptive.Shared.Utility;

namespace ShipWorks.ApplicationCore.Licensing.TangoRequests
{
    /// <summary>
    /// Get release info for a given version
    /// </summary>
    public interface ITangoGetReleaseByCustomerRequest
    {
        /// <summary>
        /// Get release info for a specific customer
        /// </summary>
        GenericResult<ShipWorksReleaseInfo> GetReleaseInfo();
    }
}
