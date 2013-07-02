using System.Reflection;


namespace ShipWorks.Actions
{
    /// <summary>
    /// Specifies the type of computer limitation for an action.
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ComputerLimitationType
    {
        /// <summary>
        /// The action may run on any computer.
        /// </summary>
        None = 0,

        /// <summary>
        /// The action may only run on the computer that triggered it.
        /// </summary>
        TriggeringComputer = 1,

        /// <summary>
        /// The action may only run on a named subset of computers.
        /// </summary>
        NamedList = 2
    }
}
