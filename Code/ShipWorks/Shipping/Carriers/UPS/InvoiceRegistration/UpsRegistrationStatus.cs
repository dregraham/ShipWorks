using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.Carriers.UPS.InvoiceRegistration
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum UpsRegistrationStatus
    {
        [Description("The registration was successful.")]
        Success = 0,

        [Description("The registration requires invoice information to authenticate.")]
        InvoiceAuthenticationRequired = 1,

        [Description("The registration failed.")]
        Failed = 2,
    }
}