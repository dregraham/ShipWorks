namespace Interapptive.Shared.StackTraceHelper
{
    /// <summary>
    /// Stores and restores stack information on TPL and framework events
    /// </summary>
    /// <remarks>
    /// Translated from https://msdn.microsoft.com/en-us/magazine/jj891052.aspx
    /// </remarks>
    public class StackStorageListener : TplFrameworkListenerBase
    {
        private readonly StackStorage stackStorage;

        /// <summary>
        /// Constructor
        /// </summary>
        public StackStorageListener(StackStorage stackStorage)
        {
            this.stackStorage = stackStorage;
        }

        /// <summary>
        /// Task was scheduled
        /// </summary>
        protected override void TaskScheduled(int taskId) => stackStorage.StoreStack(taskId, false);

        /// <summary>
        /// Task wait is beginning
        /// </summary>
        protected override void TaskWaitBegin(int taskId, EventConstants.Tpl.TaskWaitBehavior behavior) =>
            stackStorage.StoreStack(taskId, behavior == EventConstants.Tpl.TaskWaitBehavior.Synchronous);

        /// <summary>
        /// Task wait end
        /// </summary>
        protected override void TaskWaitEnd(int taskId) => stackStorage.RestoreStack(taskId);

        /// <summary>
        /// Reset local
        /// </summary>
        protected override void ResetLocal() => stackStorage.ResetLocal();
    }
}
