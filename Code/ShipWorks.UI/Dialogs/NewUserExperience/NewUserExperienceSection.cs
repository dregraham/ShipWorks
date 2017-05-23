using System.Reflection;

namespace ShipWorks.UI.Dialogs.NewUserExperience
{
    /// <summary>
    /// Selected section of the New User Experience screen
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum NewUserExperienceSection
    {
        /// <summary>
        /// Add store section
        /// </summary>
        AddStore,

        /// <summary>
        /// Add shipping account section
        /// </summary>
        AddShippingAccount,

        /// <summary>
        /// Use ShipWorks section
        /// </summary>
        UseShipWorks
    }
}