using System.Threading.Tasks;

namespace ShipWorks.Core.Common.Threading
{
    /// <summary>
    /// Extensions for tasks
    /// </summary>
    public static class TaskUtility
    {
        /// <summary>
        /// Completed task that can be used wherever asynchrony is not needed
        /// </summary>
        public static readonly Task CompletedTask = TaskEx.FromResult(false);
    }
}
