using System.ComponentModel;
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
        [Description("Add a store")]
        AddStore,

        /// <summary>
        /// Add shipping account section
        /// </summary>
        [Description("Add a shipping account")]
        AddShippingAccount,

        /// <summary>
        /// Use ShipWorks section
        /// </summary>
        [Description("Use ShipWorks")]
        UseShipWorks
    }
}