using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Users.Audit
{
    /// <summary>
    /// Reasons why something may be audited.  This is related to the Super User, and what's going on
    /// when a the Super User does something.
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public enum AuditReasonType
    {
        // No reason specified.
        [Description("")]
        Default = 0,

        [Description("Action")]
        Action = 1,

        [Description("Automatic Download")]
        AutomaticDownload = 2,

        [Description("Manual Download")]
        ManualDownload = 3,

        [Description("Combine Order")]
        CombineOrder = 4
    }
}
