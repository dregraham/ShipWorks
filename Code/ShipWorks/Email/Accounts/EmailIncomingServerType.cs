using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Email.Accounts
{
    /// <summary>
    /// Types of supported incoming mail servers
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum EmailIncomingServerType
    {
        [Description("POP3")]
        Pop3 = 0,

        [Description("IMAP")]
        Imap = 1
    }
}
