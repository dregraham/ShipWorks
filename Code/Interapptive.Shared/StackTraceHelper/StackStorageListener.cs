namespace WindowsFormsApp1.StackTraceHelper
{
    /// <summary>
    /// Stores and restores stack information on TPL and framework events
    /// </summary>
    public class StackStorageListener : TplFrameworkListenerBase
    {
        private readonly StackStorage stackStorage;

        public StackStorageListener(StackStorage stackStorage)
        {
            this.stackStorage = stackStorage;
        }

        protected override void TaskScheduled(int taskId) => stackStorage.StoreStack(taskId, false);

        protected override void TaskWaitBegin(int taskId, EventConstants.Tpl.TaskWaitBehavior behavior) =>
            stackStorage.StoreStack(taskId, behavior == EventConstants.Tpl.TaskWaitBehavior.Synchronous);

        protected override void TaskWaitEnd(int taskId) => stackStorage.RestoreStack(taskId);

        protected override void ResetLocal() => stackStorage.ResetLocal();
    }
}
