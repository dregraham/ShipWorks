namespace Interapptive.Shared.Extensions
{
    /// <summary>
    /// Where should a task continue
    /// </summary>
    public enum ContinueOn
    {
        /// <summary>
        /// Continue on the current thread
        /// </summary>
        CurrentThread,

        /// <summary>
        /// Continue on any thread
        /// </summary>
        AnyThread,
    }
}
