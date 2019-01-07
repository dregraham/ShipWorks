using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace Interapptive.Shared.Tests.Utility
{
    public class TelemetricResultTest : IDisposable
    {
        private readonly AutoMock mock;

        public TelemetricResultTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void Value_ReturnsValueFromSetValue()
        {
            var testObject = new TelemetricResult<int>("");

            testObject.SetValue(42);

            Assert.Equal(42, testObject.Value);
        }

        [Fact]
        public void RunTimedEvent_AddsMetricForEvent()
        {
            var eventToWriteTo = mock.CreateMock<ITrackedDurationEvent>();

            var testObject = new TelemetricResult<int>("bar");
            testObject.RunTimedEvent(TelemetricEventType.GetLabel, () => { });
            testObject.WriteTo(eventToWriteTo.Object);

            eventToWriteTo.Verify(e => e.AddMetric("bar.GetLabel", AnyDouble), Times.Once);
        }

        [Fact]
        public async Task RunTimedEventAsync_AddsMetricForEvent()
        {
            var eventToWriteTo = mock.CreateMock<ITrackedDurationEvent>();

            var testObject = new TelemetricResult<int>("bar");
            await testObject.RunTimedEventAsync(TelemetricEventType.GetLabel, () => Task.CompletedTask);
            testObject.WriteTo(eventToWriteTo.Object);

            eventToWriteTo.Verify(e => e.AddMetric("bar.GetLabel", AnyDouble), Times.Once);
        }

        [Fact]
        public void WriteTo_PopulatesTrackedDurationWithEvent()
        {
            var testObject = new TelemetricResult<int>("base");
            testObject.RunTimedEvent(TelemetricEventType.CleanseAddress, () => { });

            var telemetricEvent = mock.CreateMock<ITrackedDurationEvent>();
            testObject.WriteTo(telemetricEvent.Object);

            telemetricEvent.Verify(t => t.AddMetric("base.CleanseAddress", AnyDouble), Times.Once);
        }

        [Fact]
        public void WriteTo_PopulatesTotalTime_AsBaseTelemetryName()
        {
            var testObject = new TelemetricResult<int>("base");

            var telemetricEvent = mock.CreateMock<ITrackedDurationEvent>();
            testObject.WriteTo(telemetricEvent.Object);

            telemetricEvent.Verify(t => t.AddMetric("base", 0), Times.Once);
        }

        [Theory]
        [InlineData(true, 2)]
        [InlineData(false, 1)]
        public void CopyFrom_UsesCorrectValue_BasedOnUseNewResultsValue(bool useNewValue, int expectedValue)
        {
            var testObject = new TelemetricResult<int>("base1");
            testObject.SetValue(1);

            var toCombine = new TelemetricResult<int>("base2");
            toCombine.SetValue(2);

            testObject.CopyFrom<int>(toCombine, useNewValue);

            Assert.Equal(expectedValue, testObject.Value);
        }

        [Fact]
        public void CopyFrom_CopiesEvents()
        {
            var testObject = new TelemetricResult<int>("base1");
            testObject.RunTimedEvent(TelemetricEventType.GetLabel, () => { });

            var toCombine = new TelemetricResult<int>("base2");
            toCombine.RunTimedEvent(TelemetricEventType.GetRates, () => { });

            testObject.CopyFrom<int>(toCombine, true);

            var telemetricEvent = mock.CreateMock<ITrackedDurationEvent>();
            testObject.WriteTo(telemetricEvent.Object);

            telemetricEvent.Verify(t => t.AddMetric("base1.GetLabel", AnyDouble), Times.Once);
            telemetricEvent.Verify(t => t.AddMetric("base2.GetRates", AnyDouble), Times.Once);
        }

        [Fact]
        public void CopyFrom_LastTimeIsTheSumOfBothEvents()
        {
            var testObject = new TelemetricResult<int>("base1");
            testObject.RunTimedEvent(TelemetricEventType.GetRates, () => Thread.Sleep(10));
            double originalTestObjectTime = GetLastTime(testObject);

            var toCombine = new TelemetricResult<int>("base2");
            toCombine.RunTimedEvent(TelemetricEventType.GetLabel, () => Thread.Sleep(20));
            double originalToCombineTestObjectTime = GetLastTime(toCombine);

            testObject.CopyFrom<int>(toCombine, true);
            double combinedTime = GetLastTime(testObject);

            Assert.Equal(originalTestObjectTime + originalToCombineTestObjectTime, combinedTime);
        }

        private double GetLastTime(TelemetricResult<int> result)
        {
            double lastTime = 0;
            var telemetricEvent = mock.CreateMock<ITrackedDurationEvent>();
            telemetricEvent.Setup(t => t.AddMetric(AnyString, AnyDouble))
                .Callback<string, double>((name, time) => lastTime = time);

            result.WriteTo(telemetricEvent.Object);

            return lastTime;
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}