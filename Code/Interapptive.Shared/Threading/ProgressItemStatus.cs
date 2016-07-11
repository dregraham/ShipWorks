namespace Interapptive.Shared.Threading
{
    /// <summary>
    /// The states that an item can report its progress as
    /// </summary>
    public enum ProgressItemStatus
    {
        /// <summary>
        /// Scheduled to be run
        /// </summary>
        Pending,

        /// <summary>
        /// Currently running
        /// </summary>
        Running,

        /// <summary>
        /// Already ran and completed successfully
        /// </summary>
        Success,

        /// <summary>
        /// Ran, but finished in error
        /// </summary>
        Failure,

        /// <summary>
        /// Running or Pending item that was canceled
        /// </summary>
        Canceled,

        /// <summary>
        /// Pending item that did not run due to a previous item failure
        /// </summary>
        Terminated
    }
}
