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
        /// <remarks>This is the default for Generate, Range, Repeat, TakeLast,
        /// ToObservable, and the ReplaySubject</remarks>
        IScheduler CurrentThread { get; }

        /// <summary>
        /// Dispatcher scheduler
        /// </summary>
        IScheduler Dispatcher { get; }

        /// <summary>
        /// Schedule work on the Windows Forms event loop
        /// </summary>
        /// <remarks>This is equivalent to calling Control.Invoke</remarks>
        IScheduler WindowsFormsEventLoop { get; }

        /// <summary>
        /// Immediate scheduler
        /// </summary>
        /// <remarks>This is the default for Empty, GetSchedulerForCurrentContext, Return,
        /// StartWith, Throw, Run</remarks>
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

        /// <summary>
        /// Default scheduler used by Reactive
        /// </summary>
        /// <remarks>This is the default for Start, ToAsync, FromAsyncPattern, Buffer, Delay, DelaySubscription,
        /// Generate, Interval, Sample, Skip, SkipLast, SkipUntil, Take, TakeLast, TakeLastBuffer, TakeUntil,
        /// Throttle, TimeInterval, Timeout, Timer, Timestamp, Window</remarks>
        IScheduler Default { get; }
    }
}
