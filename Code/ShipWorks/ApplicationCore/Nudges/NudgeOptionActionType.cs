namespace ShipWorks.ApplicationCore.Nudges
{
    /// <summary>
    /// An enumeration for the different actions that should be taken when a nudge option is selected.
    /// </summary>
    public enum NudgeOptionActionType
    {
        /// <summary>
        /// Nothing should occur. This is only for dismissing the nudge.
        /// </summary>
        None = 0,

        /// <summary>
        /// ShipWorks should shut down.
        /// </summary>
        Shutdown = 1,

        /// <summary>
        /// Open the register Usps account wizard
        /// </summary>
        RegisterUspsAccount = 2
    }
}
