using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Connection;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.Data.Administration.Retry
{
    public class SqlAdapterRetryTest : IDisposable
    {
        readonly AutoMock mock;

        public SqlAdapterRetryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void ExecuteWithRetry_CallsMethod_Once()
        {
            int calls = 0;
            var testObject = new SqlAdapterRetry<Exception>(1, 1, "Foo", TimeSpan.FromMilliseconds(10));

            testObject.ExecuteWithRetry(() => calls += 1);

            Assert.Equal(1, calls);
        }

        [Fact]
        public void ExecuteWithRetry_CallsMethodTwice_WhenExceptionIsThrownFirst()
        {
            int calls = 0;
            var testObject = new SqlAdapterRetry<Exception>(1, 1, "Foo", TimeSpan.FromMilliseconds(10));

            testObject.ExecuteWithRetry(() =>
            {
                calls += 1;

                if (calls == 1)
                {
                    throw new Exception();
                }
            });

            Assert.Equal(2, calls);
        }

        [Fact]
        public void ExecuteWithRetry_CallsMethodTwice_WhenInnerExceptionIsThrownFirst()
        {
            int calls = 0;
            var testObject = new SqlAdapterRetry<InvalidOperationException>(1, 1, "Foo", TimeSpan.FromMilliseconds(10));

            testObject.ExecuteWithRetry(() =>
            {
                calls += 1;

                if (calls == 1)
                {
                    throw new Exception("Foo", new InvalidOperationException());
                }
            });

            Assert.Equal(2, calls);
        }

        [Fact]
        public void ExecuteWithRetry_ThrowsException_WhenRetryLimitIsExceeded()
        {
            var testObject = new SqlAdapterRetry<Exception>(1, 1, "Foo", TimeSpan.FromMilliseconds(10));

            Assert.Throws<Exception>(() => testObject.ExecuteWithRetry(() => { throw new Exception(); }));
        }

        [Fact]
        public void ExecuteWithRetry_DefaultDelayIsOneSecond()
        {
            var testObject = new SqlAdapterRetry<Exception>(1, 1, "Foo");
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                testObject.ExecuteWithRetry(() => { throw new Exception(); });
            }
            catch (Exception)
            {
                // We don't care about the exception
            }

            stopwatch.Stop();
            Assert.True(stopwatch.ElapsedMilliseconds >= 1000);
        }

        [Fact]
        public void ExecuteWithRetry_ThrowsException_WhenRetryLimitIsExceededAndInnerExceptionIsExceptionType()
        {
            var testObject = new SqlAdapterRetry<InvalidOperationException>(1, 1, "Foo", TimeSpan.FromMilliseconds(10));

            Assert.Throws<ArgumentException>(() => testObject.ExecuteWithRetry(() =>
            {
                throw new ArgumentException("foo", new InvalidOperationException());
            }));
        }

        [Fact]
        public void ExecuteWithRetryAndAdapter_CallsMethod_Once()
        {
            int calls = 0;
            var testObject = new SqlAdapterRetry<Exception>(1, 1, "Foo", TimeSpan.FromMilliseconds(10));

            testObject.ExecuteWithRetry(x => calls += 1, () => mock.Create<ISqlAdapter>());

            Assert.Equal(1, calls);
        }

        [Fact]
        public void ExecuteWithRetryAndAdapter_CallsMethodTwice_WhenExceptionIsThrownFirst()
        {
            int calls = 0;
            var testObject = new SqlAdapterRetry<Exception>(1, 1, "Foo", TimeSpan.FromMilliseconds(10));

            testObject.ExecuteWithRetry(x =>
            {
                calls += 1;

                if (calls == 1)
                {
                    throw new Exception();
                }
            }, () => mock.Create<ISqlAdapter>());

            Assert.Equal(2, calls);
        }

        [Fact]
        public void ExecuteWithRetryAndAdapter_CreatesTwoAdapters_WhenExceptionIsThrownFirst()
        {
            var calls = 0;
            var testObject = new SqlAdapterRetry<Exception>(1, 1, "Foo", TimeSpan.FromMilliseconds(10));

            try
            {
                testObject.ExecuteWithRetry(x => { throw new Exception(); }, () =>
                {
                    calls += 1;
                    return mock.Create<ISqlAdapter>();
                });
            }
            catch (Exception)
            {
                // We don't care about the exception
            }

            Assert.Equal(2, calls);
        }

        [Fact]
        public void ExecuteWithRetryAndAdapter_CallsMethodTwice_WhenInnerExceptionIsThrownFirst()
        {
            int calls = 0;
            var testObject = new SqlAdapterRetry<InvalidOperationException>(1, 1, "Foo", TimeSpan.FromMilliseconds(10));

            testObject.ExecuteWithRetry(x =>
            {
                calls += 1;

                if (calls == 1)
                {
                    throw new Exception("Foo", new InvalidOperationException());
                }
            }, () => mock.Create<ISqlAdapter>());

            Assert.Equal(2, calls);
        }

        [Fact]
        public void ExecuteWithRetryAndAdapter_ThrowsException_WhenRetryLimitIsExceeded()
        {
            var testObject = new SqlAdapterRetry<Exception>(1, 1, "Foo", TimeSpan.FromMilliseconds(10));

            Assert.Throws<Exception>(() => testObject.ExecuteWithRetry(x => { throw new Exception(); }, () => mock.Create<ISqlAdapter>()));
        }

        [Fact]
        public void ExecuteWithRetryAndAdapter_DefaultDelayIsOneSecond()
        {
            var testObject = new SqlAdapterRetry<Exception>(1, 1, "Foo");
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                testObject.ExecuteWithRetry(x =>
                {
                    throw new Exception();
                }, () => mock.Create<ISqlAdapter>());
            }
            catch (Exception)
            {
                // We don't care about the exception
            }

            stopwatch.Stop();
            Assert.True(stopwatch.ElapsedMilliseconds >= 1000);
        }

        [Fact]
        public void ExecuteWithRetryAndAdapter_ThrowsException_WhenRetryLimitIsExceededAndInnerExceptionIsExceptionType()
        {
            var testObject = new SqlAdapterRetry<InvalidOperationException>(1, 1, "Foo", TimeSpan.FromMilliseconds(10));

            Assert.Throws<ArgumentException>(() => testObject.ExecuteWithRetry(x =>
            {
                throw new ArgumentException("foo", new InvalidOperationException());
            }, () => mock.Create<ISqlAdapter>()));
        }

        [Fact]
        public async Task ExecuteWithRetryAsyncTask_CallsMethod_Once()
        {
            int calls = 0;
            var testObject = new SqlAdapterRetry<Exception>(1, 1, "Foo", TimeSpan.FromMilliseconds(10));

            await testObject.ExecuteWithRetryAsync(() => { calls += 1; return Task.CompletedTask; });

            Assert.Equal(1, calls);
        }

        [Fact]
        public async Task ExecuteWithRetryAsyncTask_CallsMethodTwice_WhenExceptionIsThrownFirst()
        {
            int calls = 0;
            var testObject = new SqlAdapterRetry<Exception>(1, 1, "Foo", TimeSpan.FromMilliseconds(10));

            await testObject.ExecuteWithRetryAsync(() =>
            {
                calls += 1;

                if (calls == 1)
                {
                    throw new Exception();
                }

                return Task.CompletedTask;
            });

            Assert.Equal(2, calls);
        }

        [Fact]
        public async Task ExecuteWithRetryAsyncTask_CallsMethodTwice_WhenInnerExceptionIsThrownFirst()
        {
            int calls = 0;
            var testObject = new SqlAdapterRetry<InvalidOperationException>(1, 1, "Foo", TimeSpan.FromMilliseconds(10));

            await testObject.ExecuteWithRetryAsync(() =>
            {
                calls += 1;

                if (calls == 1)
                {
                    throw new Exception("Foo", new InvalidOperationException());
                }

                return Task.CompletedTask;
            });

            Assert.Equal(2, calls);
        }

        [Fact]
        public async Task ExecuteWithRetryAsyncTask_CallsMethodTwice_WhenCustomExceptionCheckerMatchesFirst()
        {
            int calls = 0;
            var testObject = new SqlAdapterRetry<InvalidOperationException>(1, 1, "Foo", TimeSpan.FromMilliseconds(10));

            await testObject.ExecuteWithRetryAsync(() =>
            {
                calls += 1;

                if (calls == 1)
                {
                    throw new Exception("Foo", new Exception());
                }

                return Task.CompletedTask;
            }, ex => true);

            Assert.Equal(2, calls);
        }

        [Fact]
        public async Task ExecuteWithRetryAsyncTask_ThrowsException_WhenRetryLimitIsExceeded()
        {
            var testObject = new SqlAdapterRetry<Exception>(1, 1, "Foo", TimeSpan.FromMilliseconds(10));

            await Assert.ThrowsAsync<Exception>(() => testObject.ExecuteWithRetryAsync(() =>
            {
                throw new Exception();
            }));
        }

        [Fact]
        public async Task ExecuteWithRetryAsyncTask_DefaultDelayIsOneSecond()
        {
            var testObject = new SqlAdapterRetry<Exception>(1, 1, "Foo");
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                await testObject.ExecuteWithRetryAsync(() =>
                {
                    throw new Exception();
                });
            }
            catch (Exception)
            {
                // We don't care about the exception
            }

            stopwatch.Stop();
            Assert.True(stopwatch.ElapsedMilliseconds >= 1000);
        }

        [Fact]
        public async Task ExecuteWithRetryAsyncTask_ThrowsException_WhenRetryLimitIsExceededAndInnerExceptionIsExceptionType()
        {
            var testObject = new SqlAdapterRetry<InvalidOperationException>(1, 1, "Foo", TimeSpan.FromMilliseconds(10));

            await Assert.ThrowsAsync<ArgumentException>(() => testObject.ExecuteWithRetryAsync(() =>
            {
                throw new ArgumentException("foo", new InvalidOperationException());
            }));
        }

        [Fact]
        public async Task ExecuteWithRetryAsyncTaskT_CallsMethod_Once()
        {
            int calls = 0;
            var testObject = new SqlAdapterRetry<Exception>(1, 1, "Foo", TimeSpan.FromMilliseconds(10));

            await testObject.ExecuteWithRetryAsync(() => Task.FromResult(calls += 1));

            Assert.Equal(1, calls);
        }

        [Fact]
        public async Task ExecuteWithRetryAsyncTaskT_ReturnsValue_ThatWasReturnedFromInnerFunc()
        {
            var testObject = new SqlAdapterRetry<Exception>(1, 1, "Foo", TimeSpan.FromMilliseconds(10));

            var result = await testObject.ExecuteWithRetryAsync(() => Task.FromResult(36));

            Assert.Equal(36, result);
        }

        [Fact]
        public async Task ExecuteWithRetryAsyncTaskT_CallsMethodTwice_WhenExceptionIsThrownFirst()
        {
            int calls = 0;
            var testObject = new SqlAdapterRetry<Exception>(1, 1, "Foo", TimeSpan.FromMilliseconds(10));

            await testObject.ExecuteWithRetryAsync(() =>
            {
                calls += 1;

                if (calls == 1)
                {
                    throw new Exception();
                }

                return Task.FromResult(0);
            });

            Assert.Equal(2, calls);
        }

        [Fact]
        public async Task ExecuteWithRetryAsyncTaskT_CallsMethodTwice_WhenCustomExceptionCheckerMatchesFirst()
        {
            int calls = 0;
            var testObject = new SqlAdapterRetry<ArgumentException>(1, 1, "Foo", TimeSpan.FromMilliseconds(10));

            await testObject.ExecuteWithRetryAsync(() =>
            {
                calls += 1;

                if (calls == 1)
                {
                    throw new Exception();
                }

                return Task.FromResult(0);
            }, ex => true);

            Assert.Equal(2, calls);
        }

        [Fact]
        public async Task ExecuteWithRetryAsyncTaskT_CallsMethodTwice_WhenInnerExceptionIsThrownFirst()
        {
            int calls = 0;
            var testObject = new SqlAdapterRetry<InvalidOperationException>(1, 1, "Foo", TimeSpan.FromMilliseconds(10));

            await testObject.ExecuteWithRetryAsync(() =>
            {
                calls += 1;

                if (calls == 1)
                {
                    throw new Exception("Foo", new InvalidOperationException());
                }

                return Task.FromResult(0);
            });

            Assert.Equal(2, calls);
        }

        [Fact]
        public async Task ExecuteWithRetryAsyncTaskT_ThrowsException_WhenRetryLimitIsExceeded()
        {
            var testObject = new SqlAdapterRetry<Exception>(1, 1, "Foo", TimeSpan.FromMilliseconds(10));

            await Assert.ThrowsAsync<Exception>(() => testObject.ExecuteWithRetryAsync<int>(() =>
            {
                throw new Exception();
            }));
        }

        [Fact]
        public async Task ExecuteWithRetryAsyncTaskT_DefaultDelayIsOneSecond()
        {
            var testObject = new SqlAdapterRetry<Exception>(1, 1, "Foo");
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                await testObject.ExecuteWithRetryAsync<int>(() => { throw new Exception(); });
            }
            catch (Exception)
            {
                // We don't care about the exception
            }

            stopwatch.Stop();
            Assert.True(stopwatch.ElapsedMilliseconds >= 1000);
        }

        [Fact]
        public async Task ExecuteWithRetryAsyncTaskT_ThrowsException_WhenRetryLimitIsExceededAndInnerExceptionIsExceptionType()
        {
            var testObject = new SqlAdapterRetry<InvalidOperationException>(1, 1, "Foo");

            await Assert.ThrowsAsync<ArgumentException>(() => testObject.ExecuteWithRetryAsync<int>(() =>
            {
                throw new ArgumentException("foo", new InvalidOperationException());
            }));
        }

        [Fact]
        public async Task ExecuteWithRetryAsyncAndAdapter_CallsMethod_Once()
        {
            int calls = 0;
            var testObject = new SqlAdapterRetry<Exception>(1, 1, "Foo", TimeSpan.FromMilliseconds(10));

            await testObject.ExecuteWithRetryAsync(x => { calls += 1; return Task.CompletedTask; }, () => mock.Create<ISqlAdapter>());

            Assert.Equal(1, calls);
        }

        [Fact]
        public async Task ExecuteWithRetryAsyncAndAdapter_CallsMethodTwice_WhenExceptionIsThrownFirst()
        {
            int calls = 0;
            var testObject = new SqlAdapterRetry<Exception>(1, 1, "Foo", TimeSpan.FromMilliseconds(10));

            await testObject.ExecuteWithRetryAsync(x =>
            {
                calls += 1;

                if (calls == 1)
                {
                    throw new Exception();
                }

                return Task.CompletedTask;
            }, () => mock.Create<ISqlAdapter>());

            Assert.Equal(2, calls);
        }

        [Fact]
        public async Task ExecuteWithRetryAsyncAndAdapter_CreatesTwoAdapters_WhenExceptionIsThrownFirst()
        {
            var calls = 0;
            var testObject = new SqlAdapterRetry<Exception>(1, 1, "Foo", TimeSpan.FromMilliseconds(10));

            try
            {
                await testObject.ExecuteWithRetryAsync(x => { throw new Exception(); }, () =>
                {
                    calls += 1;
                    return mock.Create<ISqlAdapter>();
                });
            }
            catch (Exception)
            {
                // We don't care about the exception
            }

            Assert.Equal(2, calls);
        }

        [Fact]
        public async Task ExecuteWithRetryAsyncAndAdapter_CallsMethodTwice_WhenInnerExceptionIsThrownFirst()
        {
            int calls = 0;
            var testObject = new SqlAdapterRetry<InvalidOperationException>(1, 1, "Foo", TimeSpan.FromMilliseconds(10));

            await testObject.ExecuteWithRetryAsync(x =>
            {
                calls += 1;

                if (calls == 1)
                {
                    throw new Exception("Foo", new InvalidOperationException());
                }

                return Task.CompletedTask;
            }, () => mock.Create<ISqlAdapter>());

            Assert.Equal(2, calls);
        }

        [Fact]
        public async Task ExecuteWithRetryAsyncAndAdapter_ThrowsException_WhenRetryLimitIsExceeded()
        {
            var testObject = new SqlAdapterRetry<Exception>(1, 1, "Foo", TimeSpan.FromMilliseconds(10));

            await Assert.ThrowsAsync<Exception>(() => testObject.ExecuteWithRetryAsync(x =>
            {
                throw new Exception();
            }, () => mock.Create<ISqlAdapter>()));
        }

        [Fact]
        public async Task ExecuteWithRetryAsyncAndAdapter_DefaultDelayIsOneSecond()
        {
            var testObject = new SqlAdapterRetry<Exception>(1, 1, "Foo");
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                await testObject.ExecuteWithRetryAsync(x =>
                {
                    throw new Exception();
                }, () => mock.Create<ISqlAdapter>());
            }
            catch (Exception)
            {
                // We don't care about the exception
            }

            stopwatch.Stop();
            Assert.True(stopwatch.ElapsedMilliseconds >= 1000);
        }

        [Fact]
        public async Task ExecuteWithRetryAsyncAndAdapter_ThrowsException_WhenRetryLimitIsExceededAndInnerExceptionIsExceptionType()
        {
            var testObject = new SqlAdapterRetry<InvalidOperationException>(1, 1, "Foo", TimeSpan.FromMilliseconds(10));

            await Assert.ThrowsAsync<ArgumentException>(() => testObject.ExecuteWithRetryAsync(x =>
            {
                throw new ArgumentException("foo", new InvalidOperationException());
            }, () => mock.Create<ISqlAdapter>()));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
