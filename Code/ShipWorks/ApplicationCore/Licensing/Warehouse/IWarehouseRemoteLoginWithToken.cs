using System;
using Interapptive.Shared.Utility;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse
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
