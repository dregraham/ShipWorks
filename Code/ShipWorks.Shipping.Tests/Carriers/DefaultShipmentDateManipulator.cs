using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers
{
    public class DefaultShipmentDateManipulatorTest : IDisposable
    {
        readonly AutoMock mock;

        public DefaultShipmentDateManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Theory]
        [InlineData(true, "2017-10-02 10:00:00", "2017-10-03 12:00:00", "2017-10-02 10:00:00")]
        [InlineData(false, "2017-10-02 10:00:00", "2017-10-03 10:00:00", "2017-10-03 5:00:00")]
        [InlineData(false, "2017-10-03 08:00:00", "2017-10-03 10:00:00", "2017-10-03 8:00:00")]
        [InlineData(false, "2017-10-04 08:00:00", "2017-10-03 10:00:00", "2017-10-04 8:00:00")]
        public void Manipulate_SetsDate_Properly(bool processed, string shipDateText, string nowText, string expectedText)
        {
            mock.Mock<IDateTimeProvider>().SetupGet(x => x.Now).Returns(DateTime.Parse(nowText));
            mock.Mock<IDateTimeProvider>().SetupGet(x => x.TimeZoneInfo).Returns(TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"));//StLouis time zone

            var shipment = new ShipmentEntity
            {
                Processed = processed,
                ShipDate = DateTime.Parse(shipDateText)
            };

            var testObject = mock.Create<DefaultShipmentDateManipulator>();
            testObject.Manipulate(shipment);

            Assert.Equal(DateTime.Parse(expectedText), shipment.ShipDate);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
