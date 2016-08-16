using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interapptive.Shared.Security
{
    /// <summary>
    /// An interface intended to provide encryption provider the appropriate initialization vectory 
    /// and key to perform encryption.
    /// </summary>
    public interface ICipherKey
    {
        /// <summary>
        /// Gets the initialization vector.
        /// </summary>
        byte[] InitializationVector { get; }

        /// <summary>
        /// Gets the key.
        /// </summary>
        byte[] Key { get; }
    }
}
