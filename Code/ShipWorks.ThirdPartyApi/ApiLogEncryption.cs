using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.ApplicationCore.Logging
{
    /// <summary>
    /// Enum for controlling how log source encryption is handled
    /// </summary>
    public enum ApiLogEncryption
    {
        /// <summary>
        /// The log will be encrypted for sources marked with 'ApiPrivateLogSource'.  The log will not be encrypted if the Interapptive special registry key is present.
        /// </summary>
        Default = 0,

        /// <summary>
        /// The log will be encrypted.  The log will not be encrypted if the Interapptive special registry key is present.
        /// </summary>
        Encrypted = 1
    }
}
