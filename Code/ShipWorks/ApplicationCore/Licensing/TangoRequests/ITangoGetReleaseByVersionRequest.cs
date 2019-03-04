using System;
using Interapptive.Shared.Utility;

namespace ShipWorks.ApplicationCore.Licensing.TangoRequests
{
    /// <summary>
    /// Get release info for a given version
    /// </summary>
    public interface ITangoGetReleaseByVersionRequest
    {
        /// <summary>
        /// Get release info for a specific version
        /// </summary>
        GenericResult<ShipWorksReleaseInfo> GetReleaseInfo(Version version);
    }
}
