using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.Ebay.Tokens.Writers
{
    /// <summary>
    /// An interface for writing the data of a token.
    /// </summary>
    public interface ITokenWriter
    {
        /// <summary>
        /// Writes the specified token.
        /// </summary>
        void Write(EbayToken token);
    }
}
