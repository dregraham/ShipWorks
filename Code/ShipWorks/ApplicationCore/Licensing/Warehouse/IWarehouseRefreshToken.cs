using System;
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
        GenericResult<TokenResponse> RefreshToken(string refreshToken);
    }
}
