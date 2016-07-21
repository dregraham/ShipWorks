using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Options for when a dialog should be displayed
    /// </summary>
    [Obfuscation]
    public enum ProgressDisplayOptions
    {
        /// <summary>
        /// Show the dialog if the operation takes longer than a predetermined amount of time
        /// </summary>
        [Description("Delay")]
        Delay,

        /// <summary>
        /// Never show the progress dialog
        /// </summary>
        [Description("Never Show")]
        NeverShow
    }
}