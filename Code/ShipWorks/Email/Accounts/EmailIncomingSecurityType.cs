using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Email.Accounts
{
    /// <summary>
    /// Security type for incoming POP\IMAP connections
    /// </summary>
    public enum EmailIncomingSecurityType
    {
        /// <summary>
        /// Initially unsecured connection.
        /// </summary>
        Unsecure = 0,
        
        /// <summary>
        /// Implicitly secured connection.
        /// </summary>
        Implicit = 1,
                
        /// <summary>
        /// Explicitly secured connection. Same as Secure.
        /// </summary>
        Explicit = 2,
    }
}
