namespace WindowsFormsApp1.StackTraceHelper
{
    /// <summary>
    /// Facade class for causality chain preservation engine
    /// </summary>
    public static class FlowReservoir
    {
        private const string storageUninitialized = "Execution flow storage is not initialized. Please call FlowReservoir.Enroll first.";

        private static readonly object initLocker = new object();
        private static StackStorage stackStorage;

        private static StackStorageListener listener;

        /// <summary>
        /// Start tracking causality in current application domain
        /// </summary>
        /// <param name="storeFileLineColumnInfo">Whether source code information has to be extracted (slower)</param>
        /// <param name="justMyCode">Omit boilerplate framework code</param>
        public static void Enroll(bool storeFileLineColumnInfo = true, bool justMyCode = true)
        {
            if (stackStorage == null)
            {
                lock (initLocker)
                {
                    if (stackStorage == null)
                    {
                        stackStorage = new StackStorage(storeFileLineColumnInfo, justMyCode);

                        listener = new StackStorageListener(stackStorage);

                        listener.EnableSubscriptions();
                        listener.Initialized.Wait();
                    }
                }
            }
        }

        /// <summary>
        /// Provides causality chain for current thread in live debugging
        /// (for post-mortem scenario or when native frame is on top of the stack refer to FlowViewer add-in)
        /// </summary>
        public static string ExtendedStack =>
            stackStorage == null ? storageUninitialized : stackStorage.GetAggregateStackString();
    }
}
