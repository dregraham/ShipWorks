using System.Reflection;

namespace ShipWorks.UI.Dialogs.SetupGuide
{
    /// <summary>
    /// Selected section of the New User Experience screen
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public enum SetupGuideSection
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