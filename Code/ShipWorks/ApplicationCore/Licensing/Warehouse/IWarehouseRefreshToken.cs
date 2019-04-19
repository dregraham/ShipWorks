using System;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse
{
    /// <summary>
    /// Refresh our token from the warehouse
    /// </summary>
    public interface IWarehouseRefreshToken
    {
        /// <summary>
        /// Refresh our token from the warehouse
        /// </summary>
        Task<GenericResult<TokenResponse>> RefreshToken(string refreshToken);
    }
}
