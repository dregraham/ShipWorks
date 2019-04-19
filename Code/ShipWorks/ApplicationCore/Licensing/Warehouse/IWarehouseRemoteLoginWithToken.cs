using System;
using System.Threading.Tasks;
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
        Task<GenericResult<TokenResponse>> RemoteLoginWithToken();
    }
}
