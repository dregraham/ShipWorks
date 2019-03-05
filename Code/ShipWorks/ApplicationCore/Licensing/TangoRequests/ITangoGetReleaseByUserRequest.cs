using System;
using Interapptive.Shared.Utility;

namespace ShipWorks.ApplicationCore.Licensing.TangoRequests
{
    /// <summary>
    /// Get release info for a given user
    /// </summary>
    public interface ITangoGetReleaseByUserRequest
    {
        /// <summary>
        /// Get release info for a specific user
        /// </summary>
        GenericResult<ShipWorksReleaseInfo> GetReleaseInfo(string tangoCustomerID, Version version);
    }
}
