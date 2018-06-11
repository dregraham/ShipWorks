using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using log4net.Core;
using Moq;
using ShipWorks.Tests.Shared;
using System;
using System.Reactive;
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

            var result = Functional
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

        [Fact]
        public void Retry_ReturnsFailureWithError_WhenMethodFails()
        {
            int count = 0;

            Functional.Retry<Unit>(() =>
            {
                count += 1;
                throw new InvalidOperationException("Failed");
            }, 2, ex => false);

            Assert.Equal(1, count);
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
            var logMock = new Mock<IInternalLogger>();

            Functional.Retry(() => 1, 1, ex => true, logMock.Object.Log);

            logMock.Verify(x => x.Log(It.IsAny<Level>(), AnyString, It.IsAny<object[]>()), Times.Never);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(20)]
        public void Retry_CallsLoggerWithWarning_ForEachFailure(int times)
        {
            var logMock = new Mock<IInternalLogger>();

            Functional.Retry<Unit>(() => throw new InvalidOperationException("Failed"), times, ex => true, logMock.Object.Log);

            for (int i = times; i >= 0; i--)
            {
                logMock.Verify(x => x.Log(Level.Warn, "{0} detected while trying to execute.  Retrying {1} more times.", new object[] { "InvalidOperationException", i }));
            }
        }

        [Fact]
        public void Retry_CallsLoggerWithError_AfterLastFailure()
        {
            var logMock = new Mock<IInternalLogger>();

            Functional.Retry<Unit>(() => throw new InvalidOperationException("Failed"), 1, ex => true, logMock.Object.Log);

            logMock.Verify(x => x.Log(Level.Error, "Could not execute due to maximum retry failures reached.", new object[] { }));
        }

        public interface IInternalLogger
        {
            Unit Log(Level t1, string t2, object[] t3);
        }
    }
}
