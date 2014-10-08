using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// An enumeration for the type of postal account being registered/created.
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum PostalAccountRegistrationType
    {
        /// <summary>
        /// A standard, non-expedited postal account
        /// </summary>
        [Description("Standard")]
        Standard,

        /// <summary>
        /// An expedited postal account
        /// </summary>
        [Description("Expedited")]
        Expedited
    }
}
