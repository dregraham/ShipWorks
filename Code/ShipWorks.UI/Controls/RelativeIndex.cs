using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.UI.Controls
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    /// Named locations in a list
    /// </summary>
    public enum RelativeIndex
    {
        [Description("None")]
        /// Don't select a location
        /// </summary>
        None,

        [Description("First")]
        /// Select the first item in a list
        /// </summary>
        First,

        [Description("Last")]
        /// Select the last item in a list
        /// </summary>
        Last
    }
}
