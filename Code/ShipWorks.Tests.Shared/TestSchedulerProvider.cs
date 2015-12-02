using Interapptive.Shared.Threading;
using System.Reactive.Concurrency;
using Microsoft.Reactive.Testing;

namespace ShipWorks.Tests.Shared
{
    public class TestSchedulerProvider : ISchedulerProvider
    {
        private readonly TestScheduler currentThread = new TestScheduler();
        private readonly TestScheduler dispatcher = new TestScheduler();
        private readonly TestScheduler immediate = new TestScheduler();
        private readonly TestScheduler newThread = new TestScheduler();
        private readonly TestScheduler threadPool = new TestScheduler();
        private readonly TestScheduler taskPool = new TestScheduler();
        private readonly TestScheduler @default = new TestScheduler();

        #region Explicit implementation of ISchedulerService
        IScheduler ISchedulerProvider.CurrentThread => currentThread;
        IScheduler ISchedulerProvider.Dispatcher => dispatcher;
        IScheduler ISchedulerProvider.Immediate => immediate;
        IScheduler ISchedulerProvider.NewThread => newThread;
        IScheduler ISchedulerProvider.ThreadPool => threadPool;
        IScheduler ISchedulerProvider.TaskPool => taskPool;
        IScheduler ISchedulerProvider.Default => @default;
        #endregion

        public TestScheduler CurrentThread => currentThread;
        public TestScheduler Dispatcher => dispatcher;
        public TestScheduler Immediate => immediate;
        public TestScheduler NewThread => newThread;
        public TestScheduler ThreadPool => threadPool;
        public TestScheduler TaskPool => threadPool;
        public TestScheduler Default => @default;
    }
}
