using System;
using System.Diagnostics;
using System.Reactive;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Connection;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Data.Administration.Retry.SqlAdapterRetry;

namespace ShipWorks.Tests.Data.Administration.Retry
{
    public class SqlAdapterRetryTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly SqlAdapterRetryOptions testOptions;

        public SqlAdapterRetryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testOptions = new SqlAdapterRetryOptions(retryDelay: TimeSpan.FromMilliseconds(1), retries: 1);
        }

        [Fact]
        public void ExecuteWithRetry_CallsMethod_Once()
        {
            int calls = 0;
            var testObject = new SqlAdapterRetry<Exception>(1, 1, "Foo", TimeSpan.FromMilliseconds(10));

            ExecuteWithRetry("Foo", testOptions, () => calls += 1, ex => true);

            Assert.Equal(1, calls);
        }

        [Fact]
        public void ExecuteWithRetry_CallsMethodTwice_WhenExceptionIsThrownFirst()
        {
            int calls = 0;

            ExecuteWithRetry("Foo",
                testOptions,
                () =>
                {
                    calls += 1;

                    if (calls == 1)
                    {
                        throw new Exception();
                    }
                },
                ex => true);

            Assert.Equal(2, calls);
        }

        [Fact]
        public void ExecuteWithRetry_ThrowsException_WhenRetryLimitIsExceeded()
        {
            var testObject = new SqlAdapterRetry<Exception>(1, 1, "Foo", TimeSpan.FromMilliseconds(10));

            ExecuteWithRetry("Foo", testOptions, () => throw new Exception(), ex => true)
                .Do(_ => Assert.True(false))
                .OnFailure(ex => Assert.IsAssignableFrom<Exception>(ex));
        }

        [Fact]
        public void ExecuteWithRetry_DefaultDelayIsOneSecond()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                ExecuteWithRetry("Foo", new SqlAdapterRetryOptions(retries: 1), () => throw new Exception(), ex => true);
            }
            catch (Exception)
            {
                // We don't care about the exception
            }

            stopwatch.Stop();
            Assert.True(stopwatch.ElapsedMilliseconds >= 1000);
        }

        [Fact]
        public void ExecuteWithRetryAndAdapter_CallsMethod_Once()
        {
            int calls = 0;
            var testObject = new SqlAdapterRetry<Exception>(1, 1, "Foo", TimeSpan.FromMilliseconds(10));

            ExecuteWithRetry("Foo", x => calls += 1, () => mock.Build<ISqlAdapter>(), ex => true);

            Assert.Equal(1, calls);
        }

        [Fact]
        public void ExecuteWithRetryAndAdapter_CallsMethodTwice_WhenExceptionIsThrownFirst()
        {
            int calls = 0;
            var testObject = new SqlAdapterRetry<Exception>(1, 1, "Foo", TimeSpan.FromMilliseconds(10));

            ExecuteWithRetry("Foo",
                testOptions,
                x =>
                {
                    calls += 1;

                    if (calls == 1)
                    {
                        throw new Exception();
                    }
                },
                () => mock.Build<ISqlAdapter>(),
                ex => true);

            Assert.Equal(2, calls);
        }

        [Fact]
        public void ExecuteWithRetryAndAdapter_CreatesTwoAdapters_WhenExceptionIsThrownFirst()
        {
            var calls = 0;

            try
            {
                ExecuteWithRetry("Foo",
                    testOptions,
                    x => throw new Exception(),
                    () =>
                    {
                        calls += 1;
                        return mock.Build<ISqlAdapter>();
                    },
                    ex => true);
            }
            catch (Exception)
            {
                // We don't care about the exception
            }

            Assert.Equal(2, calls);
        }

        [Fact]
        public void ExecuteWithRetryAndAdapter_ThrowsException_WhenRetryLimitIsExceeded()
        {
            var testObject = new SqlAdapterRetry<Exception>(1, 1, "Foo", TimeSpan.FromMilliseconds(10));

            ExecuteWithRetry("Foo", testOptions, x => throw new Exception(), () => mock.Build<ISqlAdapter>(), ex => true)
                .Do(_ => Assert.True(false))
                .OnFailure(ex => Assert.IsAssignableFrom<Exception>(ex));
        }

        [Fact]
        public void ExecuteWithRetryAndAdapter_DefaultDelayIsOneSecond()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                ExecuteWithRetry("Foo",
                    x => throw new Exception(),
                    () => mock.Build<ISqlAdapter>(),
                    ex => true);
            }
            catch (Exception)
            {
                // We don't care about the exception
            }

            stopwatch.Stop();
            Assert.True(stopwatch.ElapsedMilliseconds >= 1000);
        }

        [Fact]
        public async Task ExecuteWithRetryAsyncTask_CallsMethod_Once()
        {
            int calls = 0;
            var testObject = new SqlAdapterRetry<Exception>(1, 1, "Foo", TimeSpan.FromMilliseconds(10));

            await ExecuteWithRetryAsync("Foo", testOptions, () => { calls += 1; return Task.FromResult(Unit.Default); }, ex => true);

            Assert.Equal(1, calls);
        }

        [Fact]
        public async Task ExecuteWithRetryAsyncTask_CallsMethodTwice_WhenExceptionIsThrownFirst()
        {
            int calls = 0;
            var testObject = new SqlAdapterRetry<Exception>(1, 1, "Foo", TimeSpan.FromMilliseconds(10));

            await ExecuteWithRetryAsync(
                "Foo",
                testOptions,
                () =>
                {
                    calls += 1;

                    if (calls == 1)
                    {
                        throw new Exception();
                    }

                    return Task.FromResult(Unit.Default);
                },
                ex => true);

            Assert.Equal(2, calls);
        }

        [Fact]
        public async Task ExecuteWithRetryAsyncTask_ThrowsException_WhenRetryLimitIsExceeded()
        {
            var testObject = new SqlAdapterRetry<Exception>(1, 1, "Foo", TimeSpan.FromMilliseconds(10));

            await Assert.ThrowsAsync<Exception>(() => ExecuteWithRetryAsync<Unit>(
                "Foo",
                testOptions,
                () => throw new Exception(),
                ex => true));
        }

        [Fact]
        public async Task ExecuteWithRetryAsyncTask_DefaultDelayIsOneSecond()
        {
            var testObject = new SqlAdapterRetry<Exception>(1, 1, "Foo");
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                await ExecuteWithRetryAsync<Unit>("Foo", () => throw new Exception(), ex => true);
            }
            catch (Exception)
            {
                // We don't care about the exception
            }

            stopwatch.Stop();
            Assert.True(stopwatch.ElapsedMilliseconds >= 1000);
        }

        [Fact]
        public async Task ExecuteWithRetryAsyncTaskT_CallsMethod_Once()
        {
            int calls = 0;
            var testObject = new SqlAdapterRetry<Exception>(1, 1, "Foo", TimeSpan.FromMilliseconds(10));

            await ExecuteWithRetryAsync("Foo", testOptions, () => Task.FromResult(calls += 1), ex => true);

            Assert.Equal(1, calls);
        }

        [Fact]
        public async Task ExecuteWithRetryAsyncTaskT_ReturnsValue_ThatWasReturnedFromInnerFunc()
        {
            var result = await ExecuteWithRetryAsync("Foo", testOptions, () => Task.FromResult(36), ex => true);

            Assert.Equal(36, result);
        }

        [Fact]
        public async Task ExecuteWithRetryAsyncTaskT_CallsMethodTwice_WhenExceptionIsThrownFirst()
        {
            int calls = 0;

            await ExecuteWithRetryAsync(
                "Foo",
                testOptions,
                () =>
                {
                    calls += 1;

                    if (calls == 1)
                    {
                        throw new Exception();
                    }

                    return Task.FromResult(0);
                },
                ex => true);

            Assert.Equal(2, calls);
        }

        [Fact]
        public async Task ExecuteWithRetryAsyncTaskT_ThrowsException_WhenRetryLimitIsExceeded()
        {
            await Assert.ThrowsAsync<Exception>(() =>
                ExecuteWithRetryAsync<int>("Foo", testOptions, () => throw new Exception(), ex => true));
        }

        [Fact]
        public async Task ExecuteWithRetryAsyncTaskT_DefaultDelayIsOneSecond()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                await ExecuteWithRetryAsync<int>("Foo", () => throw new Exception(), ex => true);
            }
            catch (Exception)
            {
                // We don't care about the exception
            }

            stopwatch.Stop();
            Assert.True(stopwatch.ElapsedMilliseconds >= 1000);
        }

        [Fact]
        public async Task ExecuteWithRetryAsyncAndAdapter_CallsMethod_Once()
        {
            int calls = 0;

            await ExecuteWithRetryAsync(
                "Foo",
                testOptions,
                x => { calls += 1; return Task.FromResult(Unit.Default); },
                () => mock.Build<ISqlAdapter>(),
                ex => true);

            Assert.Equal(1, calls);
        }

        [Fact]
        public async Task ExecuteWithRetryAsyncAndAdapter_CallsMethodTwice_WhenExceptionIsThrownFirst()
        {
            int calls = 0;

            await ExecuteWithRetryAsync(
                "Foo",
                testOptions,
                x =>
                {
                    calls += 1;

                    if (calls == 1)
                    {
                        throw new Exception();
                    }

                    return Task.FromResult(Unit.Default);
                },
                () => mock.Build<ISqlAdapter>(),
                ex => true);

            Assert.Equal(2, calls);
        }

        [Fact]
        public async Task ExecuteWithRetryAsyncAndAdapter_CreatesTwoAdapters_WhenExceptionIsThrownFirst()
        {
            var calls = 0;

            try
            {
                await ExecuteWithRetryAsync<Unit>(
                    "Foo",
                    testOptions,
                    x => throw new Exception(),
                    () =>
                    {
                        calls += 1;
                        return mock.Build<ISqlAdapter>();
                    },
                    ex => true);
            }
            catch (Exception)
            {
                // We don't care about the exception
            }

            Assert.Equal(2, calls);
        }

        [Fact]
        public async Task ExecuteWithRetryAsyncAndAdapter_ThrowsException_WhenRetryLimitIsExceeded()
        {
            await Assert.ThrowsAsync<Exception>(() => ExecuteWithRetryAsync<Unit>(
                "Foo",
                testOptions,
                x => throw new Exception(),
                () => mock.Build<ISqlAdapter>(),
                ex => true));
        }

        [Fact]
        public async Task ExecuteWithRetryAsyncAndAdapter_DefaultDelayIsOneSecond()
        {
            var testObject = new SqlAdapterRetry<Exception>(1, 1, "Foo");
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                await ExecuteWithRetryAsync<Unit>(
                    "Foo",
                    x => throw new Exception(),
                    () => mock.Build<ISqlAdapter>(),
                    ex => true);
            }
            catch (Exception)
            {
                // We don't care about the exception
            }

            stopwatch.Stop();
            Assert.True(stopwatch.ElapsedMilliseconds >= 1000);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
