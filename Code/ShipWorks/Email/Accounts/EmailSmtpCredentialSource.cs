using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Email.Accounts
{
    /// <summary>
    /// Type of credentials to use when logging in to SMTP
    /// </summary>
    public enum EmailSmtpCredentialSource
    {
        None = 0,
        SameAsIncoming = 1,
        Specify = 2,
        PopBeforeSmtp = 3
    }
}
