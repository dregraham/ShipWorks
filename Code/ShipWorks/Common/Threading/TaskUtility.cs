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

        /// <summary>
        /// Make an awaitable Task fire-and-forget
        /// </summary>
        /// <remarks>You should usually await an async Task, but there are some scenarios where you want to do
        /// fire and forget. Code analysis doesn't like this (rightly so). So the purpose of this method is to make
        /// sure we know that we're purposely not awaiting the Task</remarks>
        public static void Forget(this Task task)
        {
            // Do nothing, this is to make an awaitable task fire and forget
        }
    }
}
