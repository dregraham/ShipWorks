using System.Reactive.Concurrency;

namespace Interapptive.Shared.Threading
{
    /// <summary>
    /// Provide all the various schedulers
    /// </summary>
    public interface ISchedulerProvider
    {
        /// <summary>
        /// Current thread scheduler
        /// </summary>
        IScheduler CurrentThread { get; }

        /// <summary>
        /// Dispatcher scheduler
        /// </summary>
        IScheduler Dispatcher { get; }

        /// <summary>
        /// Immediate scheduler
        /// </summary>
        IScheduler Immediate { get; }

        /// <summary>
        /// New thread scheduler
        /// </summary>
        IScheduler NewThread { get; }

        /// <summary>
        /// Thread pool scheduler
        /// </summary>
        IScheduler ThreadPool { get; }

        /// <summary>
        /// Task pool scheduler
        /// </summary>
        IScheduler TaskPool { get; }
    }
}
