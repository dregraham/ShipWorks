using System;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Settings;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Settings;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Postal
{
    public class PostalShipmentDateManipulatorTest
    {
        private Mock<DefaultShipmentDateManipulator> defaultShipmentDateManipulator;
        private Mock<IShippingSettingsEntity> shippingSettingsEntity;
        private ShipmentEntity shipment = new ShipmentEntity();
        private DateTime now = new DateTime(2017, 7, 1, 12, 0, 0);
        private readonly AutoMock mock;

        public PostalShipmentDateManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Manipulate_DoesNotModifyShipDate_WhenShipmentIsProcessed(bool cutoffEnabled)
        {
            SetupDefaultMocks(new ShipmentDateCutoff(cutoffEnabled, TimeSpan.FromHours(17)));

            shipment.Processed = true;
            shipment.IsDirty = false;

            PostalShipmentDateManipulator testObject = mock.Create<PostalShipmentDateManipulator>(TypedParameter.From(ShipmentTypeCode.Usps));
            testObject.Manipulate(shipment);

            Assert.False(shipment.IsDirty);
            Assert.Equal(now, shipment.ShipDate);
        }

        [Fact]
        public void Manipulate_DelegatesToDefaultShipmentDateManipulator_WhenUspsCutoffNotEnabled()
        {
            SetupDefaultMocks(new ShipmentDateCutoff(false, TimeSpan.MinValue));

            PostalShipmentDateManipulator testObject = mock.Create<PostalShipmentDateManipulator>(TypedParameter.From(ShipmentTypeCode.Usps));
            testObject.Manipulate(shipment);

            defaultShipmentDateManipulator.Verify(d => d.Manipulate(shipment), Times.Once);
            Assert.False(shipment.IsDirty);
            Assert.Equal(now, shipment.ShipDate);
        }

        [Fact]
        public void Manipulate_UsesShipment_ShipmentTypeCode()
        {
            SetupDefaultMocks(new ShipmentDateCutoff(false, TimeSpan.MinValue));

            PostalShipmentDateManipulator testObject = mock.Create<PostalShipmentDateManipulator>(TypedParameter.From(ShipmentTypeCode.Usps));
            shipment.ShipmentTypeCode = ShipmentTypeCode.Amazon;
            testObject.Manipulate(shipment);

            defaultShipmentDateManipulator.Verify(d => d.Manipulate(shipment), Times.Once);
            shippingSettingsEntity.Verify(x => x.GetShipmentDateCutoff(shipment.ShipmentTypeCode));
        }


        [Theory]
        // now TimeOfDay < cutoff   =>  ShipDate does not change
        // now Date < ShipDate.Date =>  ShipDate does not change
        // now TimeOfDay > cutoff && now Date > ShipDate.Date   =>  ShipDate set to now Date then go through below:
        // now TimeOfDay = cutoff && now Date = ShipDate.Date   =>  ShipDate.DayOfWeek == Thursday  => ShipDate should be Friday
        // now TimeOfDay = cutoff && now Date = ShipDate.Date   =>  ShipDate.DayOfWeek == Friday    => ShipDate should be Saturday
        // now TimeOfDay = cutoff && now Date = ShipDate.Date   =>  ShipDate.DayOfWeek == Saturday  => ShipDate should be Monday
        // now TimeOfDay > cutoff && now Date = ShipDate.Date   =>  ShipDate.DayOfWeek == Thursday  => ShipDate should be Friday
        // now TimeOfDay > cutoff && now Date = ShipDate.Date   =>  ShipDate.DayOfWeek == Friday    => ShipDate should be Saturday
        // now TimeOfDay > cutoff && now Date = ShipDate.Date   =>  ShipDate.DayOfWeek == Saturday  => ShipDate should be Monday

        // 9/25 Monday
        // 9/28 Thursday
        // 9/29 Friday
        // 9/30 Saturday
        // 10/1 Sunday

        //                  ShipDate                NOW             CutOff           Expected
        [InlineData("2017-09-25 10:00:00", "2017-09-25 08:00:00", "10:00:00", "2017-09-25 01:00:01")]
        [InlineData("2017-09-25 10:00:00", "2017-09-24 08:00:00", "10:00:00", "2017-09-25 02:00:02")]

        [InlineData("2017-09-28 10:00:00", "2017-09-28 11:00:00", "11:00:00", "2017-09-29 03:00:03")]
        [InlineData("2017-09-29 10:00:00", "2017-09-29 11:00:00", "11:00:00", "2017-09-30 04:00:04")]
        [InlineData("2017-09-30 10:00:00", "2017-09-30 11:00:00", "11:00:00", "2017-10-02 05:00:05")]

        [InlineData("2017-09-28 10:00:00", "2017-09-28 12:00:00", "11:00:00", "2017-09-29 06:00:06")]
        [InlineData("2017-09-29 10:00:00", "2017-09-29 12:00:00", "11:00:00", "2017-09-30 07:00:07")]
        [InlineData("2017-09-30 10:00:00", "2017-09-30 12:00:00", "11:00:00", "2017-10-02 08:00:08")]

        [InlineData("2017-09-25 10:00:00", "2017-09-28 11:00:00", "11:00:00", "2017-09-29 09:00:09")]
        [InlineData("2017-09-25 10:00:00", "2017-09-29 11:00:00", "11:00:00", "2017-09-30 10:00:10")]
        [InlineData("2017-09-25 10:00:00", "2017-09-30 11:00:00", "11:00:00", "2017-10-02 11:00:11")]

        [InlineData("2017-09-25 10:00:00", "2017-09-28 12:00:00", "11:00:00", "2017-09-29 12:00:12")]
        [InlineData("2017-09-25 10:00:00", "2017-09-29 12:00:00", "11:00:00", "2017-09-30 13:00:13")]
        [InlineData("2017-09-25 10:00:00", "2017-09-30 12:00:00", "11:00:00", "2017-10-02 14:00:14")]
        public void Manipulate_SetsDate_Properly(string shipDateText, string nowText, string cutoffTimespanText, string expectedText)
        {
            TimeSpan cutoffTime = TimeSpan.Parse(cutoffTimespanText);

            SetupDefaultMocks(new ShipmentDateCutoff(true, cutoffTime));

            mock.Mock<IDateTimeProvider>().SetupGet(x => x.Now).Returns(DateTime.Parse(nowText));

            var shipment = new ShipmentEntity
            {
                Processed = false,
                ShipDate = DateTime.Parse(shipDateText)
            };

            PostalShipmentDateManipulator testObject = mock.Create<PostalShipmentDateManipulator>(TypedParameter.From(ShipmentTypeCode.Usps));
            testObject.Manipulate(shipment);

            defaultShipmentDateManipulator.Verify(d => d.Manipulate(shipment), Times.Never);
            Assert.Equal(DateTime.Parse(expectedText).Date, shipment.ShipDate.Date);
        }

        private void SetupDefaultMocks(ShipmentDateCutoff cutoff)
        {
            shippingSettingsEntity = mock.FromFactory<IShippingSettings>()
                .Mock(x => x.FetchReadOnly());
            shippingSettingsEntity.Setup(x => x.GetShipmentDateCutoff(It.IsAny<ShipmentTypeCode>()))
                .Returns(cutoff);
            mock.Mock<IDateTimeProvider>().Setup(dtp => dtp.Now).Returns(now);

            defaultShipmentDateManipulator = mock.Override<DefaultShipmentDateManipulator>();

            shipment.Processed = false;
            shipment.ShipDate = now;
            shipment.IsDirty = false;
        }
    }
}
