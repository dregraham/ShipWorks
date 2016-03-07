using System;
using System.Reactive.Concurrency;
using System.Windows.Forms;

namespace Interapptive.Shared.Threading
{
    /// <summary>
    /// Provide all the various schedulers
    /// </summary>
    public sealed class SchedulerProvider : ISchedulerProvider
    {
        Lazy<IScheduler> windowsFormsEventLoopScheduler;

        public SchedulerProvider(Func<Control> getSchedulerControl)
        {
            windowsFormsEventLoopScheduler = new Lazy<IScheduler>(() => new ControlScheduler(getSchedulerControl()));
        }

        /// <summary>
        /// Current thread scheduler
        /// </summary>
        public IScheduler CurrentThread => CurrentThreadScheduler.Instance;

        /// <summary>
        /// Dispatcher scheduler
        /// </summary>
        public IScheduler Dispatcher => DispatcherScheduler.Current;

        /// <summary>
        /// Schedule work on the Windows Forms event loop
        /// </summary>
        /// <remarks>This is equivalent to calling Control.Invoke</remarks>
        public IScheduler WindowsFormsEventLoop => windowsFormsEventLoopScheduler.Value;

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
