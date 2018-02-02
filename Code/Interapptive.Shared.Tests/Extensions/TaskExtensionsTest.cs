using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Extensions;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.XUnitExtensions.STAThreadAttributes;
using Xunit;

namespace Interapptive.Shared.Tests.Extensions
{
    public class TaskExtensionsTest : IDisposable
    {
        readonly AutoMock mock;

        public TaskExtensionsTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        private Task<int> SuccessfulTask => Task.FromResult(6);
        private Task<int> FaultedTask => Task.FromException<int>(new Exception("Foo"));
        private Func<int, Task<int>> MultiplyByTwo => x => Task.FromResult(x * 2);

        [Fact]
        public async Task Bind_ReturnsNewTask_WhenInitialTaskIsSuccessful()
        {
            var result = await SuccessfulTask.Bind(MultiplyByTwo);
            Assert.Equal(12, result);
        }

        [Fact]
        public async Task Bind_ReturnsFailedTask_WhenInitialTaskIsFaulted()
        {
            var exception = await Assert.ThrowsAsync<Exception>(() => FaultedTask.Bind(MultiplyByTwo));
            Assert.Equal("Foo", exception.Message);
        }

        [Fact]
        public async Task Bind_CallsOnSuccess_WhenInitialTaskIsSuccessful()
        {
            var result = await SuccessfulTask.Bind(MultiplyByTwo, ex => Task.FromResult(1));
            Assert.Equal(12, result);
        }

        [Fact]
        public async Task Bind_CallsOnFailure_WhenInitialTaskIsFaulted()
        {
            var result = await FaultedTask.Bind(MultiplyByTwo, ex => Task.FromResult(1));
            Assert.Equal(1, result);
        }

        [STAFact]
        public async Task Bind_UsesCurrentThread_WhenRequested()
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            int taskId = 0;

            await Task.Run(() => 6).Bind(x =>
            {
                taskId = Thread.CurrentThread.ManagedThreadId;
                return Task.FromResult(x * 2);
            }, ex => Task.FromResult(1), ContinueOn.CurrentThread);

            Assert.Equal(id, taskId);
        }

        [STAFact]
        public async Task Bind_UsesDifferentThread_WhenRequested()
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            int taskId = 0;

            await Task.Run(() => 6).Bind(x =>
            {
                taskId = Thread.CurrentThread.ManagedThreadId;
                return Task.FromResult(x * 2);
            }, ex => Task.FromResult(1), ContinueOn.AnyThread);

            Assert.NotEqual(id, taskId);
        }

        [Fact]
        public async Task Map_ReturnsNewTask_WhenInitialTaskIsSuccessful()
        {
            var result = await SuccessfulTask.Map(x => x * 2);
            Assert.Equal(12, result);
        }

        [Fact]
        public async Task Map_ReturnsFailedTask_WhenInitialTaskIsFaulted()
        {
            var exception = await Assert.ThrowsAsync<Exception>(() => FaultedTask.Map(x => x * 2));
            Assert.Equal("Foo", exception.Message);
        }

        [Fact]
        public async Task Map_CallsOnSuccess_WhenInitialTaskIsSuccessful()
        {
            var result = await SuccessfulTask.Map(x => x * 2, ex => 1);
            Assert.Equal(12, result);
        }

        [Fact]
        public async Task Map_CallsOnFailure_WhenInitialTaskIsFaulted()
        {
            var result = await FaultedTask.Map(x => x * 2, ex => 1);
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task Do_ReturnsNewTask_WhenInitialTaskIsSuccessful()
        {
            int calledValue = int.MinValue;
            var result = await SuccessfulTask.Do(x => calledValue = x);
            Assert.Equal(6, calledValue);
            Assert.Equal(6, result);
        }

        [Fact]
        public async Task Do_ReturnsFailedTask_WhenInitialTaskIsFaulted()
        {
            bool wasCalled = false;
            var exception = await Assert.ThrowsAsync<Exception>(() => FaultedTask.Do(x => wasCalled = true));
            Assert.False(wasCalled);
            Assert.Equal("Foo", exception.Message);
        }

        [Fact]
        public async Task Do_CallsOnSuccess_WhenInitialTaskIsSuccessful()
        {
            int calledValue = int.MinValue;
            var result = await SuccessfulTask.Do(x => calledValue = x, ex => calledValue = int.MaxValue);
            Assert.Equal(6, calledValue);
            Assert.Equal(6, result);
        }

        [Fact]
        public async Task Do_CallsOnFailure_WhenInitialTaskIsFaulted()
        {
            bool wasCalled = false;
            var exception = await Assert.ThrowsAsync<Exception>(() => FaultedTask.Do(x => wasCalled = false, ex => wasCalled = true));
            Assert.True(wasCalled);
            Assert.Equal("Foo", exception.Message);
        }

        [Fact]
        public async Task Recover_ReturnsTaskValue_WhenInitialTaskIsSuccessful()
        {
            var result = await SuccessfulTask.Recover(ex => 1);
            Assert.Equal(6, result);
        }

        [Fact]
        public async Task Recover_ReturnsFallBackValue_WhenInitialTaskIsFaulted()
        {
            var result = await FaultedTask.Recover(ex => 1);
            Assert.Equal(1, result);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
