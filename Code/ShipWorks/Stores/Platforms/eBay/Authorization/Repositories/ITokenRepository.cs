using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.Ebay.Authorization.Repositories
{
    /// <summary>
    /// An interface intended to abstract the underlying data sources and dependencies that
    /// are needed to retreive raw token data.
    /// </summary>
    public interface ITokenRepository
    {
        /// <summary>
        /// Gets the raw data about the token for the given license in a string format.
        /// </summary>
        /// <returns>A string containing the token data.</returns>
        string GetTokenData();
    }
}
