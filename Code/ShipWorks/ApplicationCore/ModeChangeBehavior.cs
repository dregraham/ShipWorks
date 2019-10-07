using System.Reflection;

namespace ShipWorks
{
    /// <summary>
    /// Behavior when changing modes
    /// </summary>
    internal enum ModeChangeBehavior
    {
        /// <summary>
        /// Always change tabs when changing modes
        /// </summary>
        ChangeTabAlways,

        /// <summary>
        /// Only change tabs if the current tab is not appropriate for the given mode
        /// </summary>
        ChangeTabIfNotAppropriate
    }
}