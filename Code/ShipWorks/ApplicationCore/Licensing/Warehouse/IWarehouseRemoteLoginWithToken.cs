using System;
using Interapptive.Shared.Utility;

namespace ShipWorks.ApplicationCore.Licensing.TangoRequests
{
    /// <summary>
    /// Login to the warehouse
    /// </summary>
    public interface IWarehouseRemoteLoginWithToken
    {
        /// <summary>
        /// Login to the warehouse
        /// </summary>
        GenericResult<TokenResponse> RemoteLoginWithToken();
    }
}
