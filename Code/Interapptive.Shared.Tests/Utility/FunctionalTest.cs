﻿using System;
using System.Reactive;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using log4net;
using log4net.Core;
using Moq;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace Interapptive.Shared.Tests.Utility
{
    public class FunctionalTest
    {
        private readonly AutoMock mock;

        public FunctionalTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void Retry_ReturnsValueOfMethod_WhenMethodSucceeds()
        {
            var result = Functional.Retry(() => 99, 2, ex => true);

            Assert.Equal(99, result);
        }

        [Fact]
        public void Retry_CallsMethodOnce_WhenMethodSucceeds()
        {
            int count = 0;

            Functional.Retry(() => count += 1, 2, ex => true);

            Assert.Equal(1, count);
        }

        [Fact]
        public void Retry_CallsMethodTwice_WhenMethodFailsFirstButSucceedsSecondTime()
        {
            int count = 0;

            Functional.Retry(() =>
            {
                count += 1;

                if (count == 1)
                {
                    throw new InvalidOperationException("Failed");
                }

                return 0;
            }, 2, ex => true);

            Assert.Equal(2, count);
        }

        [Fact]
        public void Retry_DoesNotCallMethodTwice_WhenMethodFailsAndShouldRetryIsFalse()
        {
            var count = 0;

            Functional
                .Retry<Unit>(() =>
                {
                    count += 1;
                    throw new InvalidOperationException("Failed");
                }
                , 2, ex => false);

            Assert.Equal(1, count);
        }

        [Fact]
        public void Retry_ReturnsFailure_WhenMethodFailsAndShouldRetryIsFalse()
        {
            var result = Functional
                .Retry<Unit>(() => throw new InvalidOperationException("Failed"), 1, ex => false);

            Assert.True(result.Failure);
            Assert.Equal("Failed", result.Message);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(20)]
        public void Retry_CallsMethodMaxNumberOfTimes_WhenMethodFails(int times)
        {
            int count = 0;

            Functional.Retry<Unit>(() =>
            {
                count += 1;
                throw new InvalidOperationException("Failed");
            }, times, ex => true);

            Assert.Equal(times + 1, count);
        }

        [Fact]
        public void Retry_DoesNotCallLogger_WhenMethodSucceeds()
        {
            var logMock = new Mock<ILog>();

            Functional.Retry(() => 1, 1, ex => true, logMock.Object);

            logMock.Verify(x => x.Warn(AnyString, It.IsAny<Exception>()), Times.Never);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(20)]
        public void Retry_CallsLoggerWithWarning_ForEachFailure(int times)
        {
            var logMock = new Mock<ILog>();
            var exception = new InvalidOperationException("Failed");

            Functional.Retry<Unit>(() => throw exception, times, ex => true, logMock.Object);

            for (int i = times; i >= 1; i--)
            {
                logMock.Verify(x => x.Warn($"InvalidOperationException detected while trying to execute.  Retrying {i} more times.", exception));
            }

            logMock.Verify(x => x.Error("Could not execute due to maximum retry failures reached.", exception));
        }

        [Fact]
        public void Retry_CallsLoggerWithError_AfterLastFailure()
        {
            var logMock = new Mock<ILog>();
            var exception = new InvalidOperationException("Failed");

            Functional.Retry<Unit>(() => throw exception, 1, ex => true, logMock.Object);

            logMock.Verify(x => x.Error("Could not execute due to maximum retry failures reached.", exception));
        }

        [Fact]
        public async Task RetryAsync_ReturnsValueOfMethod_WhenMethodSucceeds()
        {
            var result = await Functional.RetryAsync(() => Task.FromResult(99), 2, ex => true);

            Assert.Equal(99, result);
        }

        [Fact]
        public async Task RetryAsync_CallsMethodOnce_WhenMethodSucceeds()
        {
            int count = 0;

            await Functional.RetryAsync(() => Task.FromResult(count += 1), 2, ex => true);

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task RetryAsync_CallsMethodTwice_WhenMethodFailsFirstButSucceedsSecondTime()
        {
            int count = 0;

            await Functional.RetryAsync(() =>
            {
                count += 1;

                if (count == 1)
                {
                    throw new InvalidOperationException("Failed");
                }

                return Task.FromResult(0);
            }, 2, ex => true);

            Assert.Equal(2, count);
        }

        [Fact]
        public async Task RetryAsync_DoesNotCallMethodTwice_WhenMethodFailsAndShouldRetryIsFalse()
        {
            var count = 0;

            await Functional
                .RetryAsync<Unit>(() =>
                {
                    count += 1;
                    throw new InvalidOperationException("Failed");
                }
                , 2, ex => false)
                .Recover(_ => Unit.Default);

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task RetryAsync_ReturnsFailure_WhenMethodFailsAndShouldRetryIsFalse()
        {
            await Functional
                .RetryAsync<Unit>(() => throw new InvalidOperationException("Failed"), 1, ex => false)
                .Do(_ => Assert.False(true), ex => Assert.Equal("Failed", ex.Message))
                .Recover(_ => Unit.Default);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(20)]
        public async Task RetryAsync_CallsMethodMaxNumberOfTimes_WhenMethodFails(int times)
        {
            int count = 0;

            await Functional
                .RetryAsync<Unit>(() =>
                {
                    count += 1;
                    throw new InvalidOperationException("Failed");
                }, times, ex => true)
                .Recover(_ => Unit.Default);

            Assert.Equal(times + 1, count);
        }

        [Fact]
        public async Task RetryAsync_DoesNotCallLogger_WhenMethodSucceeds()
        {
            var logMock = new Mock<ILog>();

            await Functional.RetryAsync(() => Task.FromResult(1), 1, ex => true, logMock.Object);

            logMock.Verify(x => x.Warn(AnyString, It.IsAny<Exception>()), Times.Never);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(20)]
        public async Task RetryAsync_CallsLoggerWithWarning_ForEachFailure(int times)
        {
            var logMock = new Mock<ILog>();
            var exception = new InvalidOperationException("Failed");

            await Functional
                .RetryAsync<Unit>(() => throw exception, times, ex => true, logMock.Object)
                .Recover(_ => Unit.Default);

            for (int i = times; i >= 1; i--)
            {
                logMock.Verify(x => x.Warn($"InvalidOperationException detected while trying to execute.  Retrying {i} more times.", exception));
            }

            logMock.Verify(x => x.Error("Could not execute due to maximum retry failures reached.", exception));
        }

        [Fact]
        public async Task RetryAsync_CallsLoggerWithError_AfterLastFailure()
        {
            var logMock = new Mock<ILog>();
            var exception = new InvalidOperationException("Failed");

            await Functional
                .RetryAsync<Unit>(() => throw exception, 1, ex => true, logMock.Object)
                .Recover(_ => Unit.Default);

            logMock.Verify(x => x.Error("Could not execute due to maximum retry failures reached.", exception));
        }

        public interface IInternalLogger
        {
            Unit Log(Level t1, string t2, object[] t3);
        }
    }
}
