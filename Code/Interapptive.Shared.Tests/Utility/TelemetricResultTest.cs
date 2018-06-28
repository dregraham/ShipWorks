using System;
using System.Diagnostics;
using System.Threading;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Shipping.Carriers.OnTrac.Schemas.EasyTrackResponse;
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
        public void Populate_PopulatesTrackedDurationWithEvent()
        {
            var testObject = new TelemetricResult<int>("base");
            testObject.StartTimedEvent("event1");
            testObject.StopTimedEvent("event1");

            var telemetricEvent = mock.CreateMock<ITrackedDurationEvent>();
            testObject.Populate(telemetricEvent.Object);
            
            telemetricEvent.Verify(t=>t.AddProperty("base.event1", AnyString), Times.Once);
        }

        [Fact]
        public void Populate_PopulatesTotalTime_AsBaseTelemetryName()
        {
            var testObject = new TelemetricResult<int>("base");

            var telemetricEvent = mock.CreateMock<ITrackedDurationEvent>();
            testObject.Populate(telemetricEvent.Object);
            
            telemetricEvent.Verify(t=>t.AddProperty("base", "0"), Times.Once);
        }

        [Theory]
        [InlineData(true, 2)]
        [InlineData(false, 1)]
        public void Combine_UsesCorrectValue_BasedOnUseNewResultsValue(bool useNewValue, int expectedValue)
        {
            var testObject = new TelemetricResult<int>("base1");
            testObject.SetValue(1);
            
            var toCombine = new TelemetricResult<int>("base2");
            toCombine.SetValue(2);
            
            testObject.Combine(toCombine, useNewValue);
            
            Assert.Equal(expectedValue, testObject.Value);
        }

        [Fact]
        public void Combine_CombinesEvents()
        {
            var testObject = new TelemetricResult<int>("base1");
            testObject.StartTimedEvent("event1");
            testObject.StopTimedEvent("event1");

            var toCombine = new TelemetricResult<int>("base2");
            toCombine.StartTimedEvent("event2");
            toCombine.StopTimedEvent("event2");

            testObject.Combine(toCombine, true);

            var telemetricEvent = mock.CreateMock<ITrackedDurationEvent>();
            testObject.Populate(telemetricEvent.Object);

            telemetricEvent.Verify(t => t.AddProperty("base1.event1", AnyString), Times.Once);
            telemetricEvent.Verify(t => t.AddProperty("base2.event2", AnyString), Times.Once);
        }
        
        [Fact]
        public void Combine_LastTimeIsTheSumOfBothEvents()
        {
            var testObject = new TelemetricResult<int>("base1");
            testObject.StartTimedEvent("event1");
            Thread.Sleep(10);
            testObject.StopTimedEvent("event1");
            int originalTestObjectTime = GetLastTime(testObject);

            var toCombine = new TelemetricResult<int>("base2");
            toCombine.StartTimedEvent("event2");
            Thread.Sleep(20);
            toCombine.StopTimedEvent("event2");
            int originalToCombineTestObjectTime = GetLastTime(toCombine);

            testObject.Combine(toCombine, true);
            int combinedTime = GetLastTime(testObject);

            Assert.Equal(originalTestObjectTime + originalToCombineTestObjectTime, combinedTime);
        }

        private int GetLastTime(TelemetricResult<int> result)
        {
            string lastTime = string.Empty;
            var telemetricEvent = mock.CreateMock<ITrackedDurationEvent>();
            telemetricEvent.Setup(t => t.AddProperty(AnyString, AnyString))
                .Callback<string, string>((name, time) => lastTime = time);

            result.Populate(telemetricEvent.Object);
            
            return int.Parse(lastTime);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}