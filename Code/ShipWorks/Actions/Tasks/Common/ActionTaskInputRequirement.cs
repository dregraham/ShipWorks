namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// Describes if input is required to run an action task
    /// </summary>
    public enum ActionTaskInputRequirement
    {
        /// <summary>
        /// No input should be used for the task
        /// </summary>
        None,

        /// <summary>
        /// Input may optionally be used for the task
        /// </summary>
        Optional,

        /// <summary>
        /// Input is required for the task
        /// </summary>
        Required
    }
}
