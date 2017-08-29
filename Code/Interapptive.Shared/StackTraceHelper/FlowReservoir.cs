namespace Interapptive.Shared.StackTraceHelper
{
    /// <summary>
    /// Facade class for causality chain preservation engine
    /// </summary>
    /// <remarks>
    /// Translated from https://msdn.microsoft.com/en-us/magazine/jj891052.aspx
    /// </remarks>
    public static class FlowReservoir
    {
#if DEBUG
        private const string storageUninitialized = @"Execution flow storage is not initialized. Set TraceTasks to True in ShipWorks\Options registry.";
#else
        private const string storageUninitialized = "Execution flow storage is not initialized.";
#endif

        private static readonly object initLocker = new object();
        private static StackStorage stackStorage;

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

                        StackStorageListener.Initialize(stackStorage);
                    }
                }
            }
        }

        /// <summary>
        /// Provides causality chain for current thread in live debugging
        /// (for post-mortem scenario or when native frame is on top of the stack refer to FlowViewer add-in)
        /// </summary>
        public static string ExtendedStack => stackStorage?.GetAggregateStackString() ?? storageUninitialized;

        /// <summary>
        /// Tag the current stack so that we can show the causality chain of the crash and not the crash reporting
        /// </summary>
        public static void Tag() => stackStorage?.Tag();
    }
}
