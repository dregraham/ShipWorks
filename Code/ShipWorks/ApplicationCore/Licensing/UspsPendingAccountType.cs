using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.ApplicationCore.Licensing
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UspsPendingAccountType
    {
        [Description("None")]
        None = 0,

        /// <summary>
        /// This represents a shell account. The user chose not to create an account during 
        /// registration so stamps created a shell account. When setting up, we will prompt
        /// the user for payment information, but skip a lot of the other info as we have it
        /// from registration.
        /// </summary>
        [Description("Create")]
        Create = 1,

        /// <summary>
        /// This represents an active account. The user created a stamps account when
        /// registering. We will still need credit card and address info.
        /// </summary>
        [Description("Existing")]
        Existing = 2
    }
}