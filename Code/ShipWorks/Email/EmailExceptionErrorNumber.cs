using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ShipWorks.Email
{
    /// <summary>
    /// Enum for different types of email exceptions that can occur.
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum EmailExceptionErrorNumber
    {
        [Description("Unsent")]
        None = 0,

        [Description("Missing To field")]
        MissingToField = 1,

        [Description("Missing From field")]
        MissingFromField = 2,

        [Description("Failed to generate email body")]
        EmailBodyProcessingFailed = 3,

        [Description("Invalid email address")]
        InvalidEmailAddress = 4,

        [Description("Invalid template selected")]
        InvalidTemplateSelected = 5,

        [Description("Unable to determine template settings")]
        IndeterminateTemplateSettings = 6,

        [Description("Logon to email server failed")]
        LogonFailed = 7,

        [Description("Delay sending email")]
        DelaySending = 8,

        [Description("Invalid email account")]
        InvalidEmailAccount = 9,

        [Description("No email accounts configured")]
        NoEmailAccountsConfigured = 10,

        [Description("Maximum number of emails per hour reached")]
        MaxEmailsPerHourReached = 11,

        [Description("Email account has changed for selected email")]
        EmailAccountChanged = 12,
    }
}
