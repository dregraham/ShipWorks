using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ShipWorks.Stores.Platforms.Ebay.Authorization.Readers
{
    /// <summary>
    /// An interface for reading token data.
    /// </summary>
    public interface ITokenReader
    {
        /// <summary>
        /// Reads token data.
        /// </summary>
        /// <returns>A TokenData object that is the result of the data that was read.</returns>
        TokenData Read();
    }
}
