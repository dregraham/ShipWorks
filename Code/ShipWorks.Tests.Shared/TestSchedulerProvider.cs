using System.Reactive.Concurrency;
using Interapptive.Shared.Threading;
using Microsoft.Reactive.Testing;

namespace ShipWorks.Tests.Shared
{
    /// <summary>
    /// Scheduler provider that allows fine grained testing of schedulers
    /// </summary>
    /// <remarks>Use this provider when you want to actually test which scheduler is used or
    /// when you need to verify timing</remarks>
    public class TestSchedulerProvider : ISchedulerProvider
    {
        private readonly TestScheduler currentThread = new TestScheduler();
        private readonly TestScheduler dispatcher = new TestScheduler();
        private readonly TestScheduler windowsFormsEventLoop = new TestScheduler();
        private readonly TestScheduler immediate = new TestScheduler();
        private readonly TestScheduler newThread = new TestScheduler();
        private readonly TestScheduler threadPool = new TestScheduler();
        private readonly TestScheduler taskPool = new TestScheduler();
        private readonly TestScheduler @default = new TestScheduler();

        #region Explicit implementation of ISchedulerService
        IScheduler ISchedulerProvider.CurrentThread => currentThread;
        IScheduler ISchedulerProvider.Dispatcher => dispatcher;
        IScheduler ISchedulerProvider.WindowsFormsEventLoop => windowsFormsEventLoop;
        IScheduler ISchedulerProvider.Immediate => immediate;
        IScheduler ISchedulerProvider.NewThread => newThread;
        IScheduler ISchedulerProvider.ThreadPool => threadPool;
        IScheduler ISchedulerProvider.TaskPool => taskPool;
        IScheduler ISchedulerProvider.Default => @default;
        #endregion

        public TestScheduler CurrentThread => currentThread;
        public TestScheduler Dispatcher => dispatcher;
        public TestScheduler WindowsFormsEventLoop => windowsFormsEventLoop;
        public TestScheduler Immediate => immediate;
        public TestScheduler NewThread => newThread;
        public TestScheduler ThreadPool => threadPool;
        public TestScheduler TaskPool => taskPool;
        public TestScheduler Default => @default;
    }
}
