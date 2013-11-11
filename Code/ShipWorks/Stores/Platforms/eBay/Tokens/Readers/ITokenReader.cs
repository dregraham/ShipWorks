using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ShipWorks.Stores.Platforms.Ebay.Tokens.Readers
{
    /// <summary>
    /// An interface for reading token data.
    /// </summary>
    public interface ITokenReader
    {
        /// <summary>
        /// Reads token data.
        /// </summary>
        EbayToken Read();
    }
}
