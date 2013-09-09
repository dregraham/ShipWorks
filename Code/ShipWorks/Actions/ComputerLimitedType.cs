namespace ShipWorks.Actions
{
    /// <summary>
    /// Specifies the type of computer limitation for an action.
    /// </summary>
    public enum ComputerLimitedType
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
        /// The action may only run on a specific list of computers.
        /// </summary>
        List = 2
    }
}
