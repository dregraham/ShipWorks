using System.Reactive.Concurrency;
using Interapptive.Shared.Threading;

namespace ShipWorks.Tests.Shared
{
    /// <summary>
    /// Test scheduler that will run all actions immediately
    /// </summary>
    /// <remarks>This should be used when you don't want to deal with which scheduler is used or don't care
    /// about timing.</remarks>
    public class ImmediateSchedulerProvider : ISchedulerProvider
    {
        public IScheduler CurrentThread => ImmediateScheduler.Instance;
        public IScheduler Dispatcher => ImmediateScheduler.Instance;
        public IScheduler WindowsFormsEventLoop => ImmediateScheduler.Instance;
        public IScheduler Immediate => ImmediateScheduler.Instance;
        public IScheduler NewThread => ImmediateScheduler.Instance;
        public IScheduler ThreadPool => ImmediateScheduler.Instance;
        public IScheduler TaskPool => ImmediateScheduler.Instance;
        public IScheduler Default => ImmediateScheduler.Instance;
    }
}
