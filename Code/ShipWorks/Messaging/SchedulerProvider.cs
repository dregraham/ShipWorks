using System.Reactive.Concurrency;

namespace Interapptive.Shared.Threading
{
    /// <summary>
    /// Provide all the various schedulers
    /// </summary>
    public sealed class SchedulerProvider : ISchedulerProvider
    {
        /// <summary>
        /// Current thread scheduler
        /// </summary>
        public IScheduler CurrentThread => CurrentThreadScheduler.Instance;

        /// <summary>
        /// Dispatcher scheduler
        /// </summary>
        public IScheduler Dispatcher => DispatcherScheduler.Current;

        /// <summary>
        /// Immediate scheduler
        /// </summary>
        public IScheduler Immediate => ImmediateScheduler.Instance;

        /// <summary>
        /// New thread scheduler
        /// </summary>
        public IScheduler NewThread => NewThreadScheduler.Default;

        /// <summary>
        /// Thread pool scheduler
        /// </summary>
        public IScheduler ThreadPool => ThreadPoolScheduler.Instance;

        /// <summary>
        /// Task pool scheduler
        /// </summary>
        public IScheduler TaskPool => TaskPoolScheduler.Default;

        /// <summary>
        /// Default scheduler used by time based methods
        /// </summary>
        public IScheduler Default => DefaultScheduler.Instance;
    }
}
