using System;
using System.Windows;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Core.UI.ValueConverters;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Settings;
using ShipWorks.Shipping.Settings;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.ShippingPanel.ValueConverters
{
    public class ShipmentDateCutoffConverterTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly Mock<IShippingSettingsEntity> settings;

        public ShipmentDateCutoffConverterTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            settings = mock.FromFactory<IShippingSettings>()
                .Mock(x => x.FetchReadOnly());

            settings.Setup(x => x.GetShipmentDateCutoff(It.IsAny<ShipmentTypeCode>()))
                .Returns(new ShipmentDateCutoff(false, TimeSpan.MinValue));
        }

        [Fact]
        public void Convert_ThrowsInvalidOperationException_WhenTargetTypeIsNotStringOrVisibility()
        {
            var testObject = mock.Create<ShipmentDateCutoffConverter>();
            Assert.Throws<InvalidOperationException>(() => testObject.Convert(ShipmentTypeCode.Usps, typeof(int), null, null));
        }

        [Theory]
        [InlineData(ShipmentTypeCode.Usps)]
        [InlineData(ShipmentTypeCode.FedEx)]
        [InlineData(ShipmentTypeCode.UpsOnLineTools)]
        public void ConvertString_DelegatesToShippingSettings_ToGetShipmentDateCutoff(ShipmentTypeCode shipmentType)
        {
            var testObject = mock.Create<ShipmentDateCutoffConverter>();
            testObject.Convert(shipmentType, typeof(string), null, null);

            settings.Verify(x => x.GetShipmentDateCutoff(shipmentType));
        }

        [Fact]
        public void ConvertString_ReturnsEmpty_WhenShipmentTypeIsNull()
        {
            var testObject = mock.Create<ShipmentDateCutoffConverter>();
            var result = testObject.Convert(null, typeof(string), null, null) as string;

            Assert.Contains(string.Empty, result);
        }

        [Theory]
        [InlineData(false, "15:00", "", "")]
        [InlineData(true, "6:00", "6:00 AM", "USPS")]
        [InlineData(true, "15:00", "3:00 PM", "USPS")]
        public void ConvertString_ReturnsExpectedValue_ForUspsWhenTargetIsString(bool cutoffEnabled, string cutoffDate,
            string expectedTime, string expectedCarrierName)
        {
            settings.Setup(x => x.GetShipmentDateCutoff(ShipmentTypeCode.Usps))
                .Returns(new ShipmentDateCutoff(cutoffEnabled, TimeSpan.Parse(cutoffDate)));

            var testObject = mock.Create<ShipmentDateCutoffConverter>();
            var result = testObject.Convert(ShipmentTypeCode.Usps, typeof(string), null, null) as string;

            Assert.Contains(expectedTime, result);
            Assert.Contains(expectedCarrierName, result);
        }

        [Theory]
        [InlineData(ShipmentTypeCode.Usps)]
        [InlineData(ShipmentTypeCode.FedEx)]
        [InlineData(ShipmentTypeCode.UpsOnLineTools)]
        public void ConvertVisibility_DelegatesToShippingSettings_ToGetShipmentDateCutoff(ShipmentTypeCode shipmentType)
        {
            var testObject = mock.Create<ShipmentDateCutoffConverter>();
            testObject.Convert(shipmentType, typeof(Visibility), null, null);

            settings.Verify(x => x.GetShipmentDateCutoff(shipmentType));
        }

        [Theory]
        [InlineData(false, "15:00", Visibility.Hidden)]
        [InlineData(true, "6:00", Visibility.Visible)]
        public void ConvertVisibility_ReturnsExpectedValue_ForUspsWhenTargetIsString(bool cutoffEnabled, string cutoffDate,
            Visibility expected)
        {
            settings.Setup(x => x.GetShipmentDateCutoff(ShipmentTypeCode.Usps))
                .Returns(new ShipmentDateCutoff(cutoffEnabled, TimeSpan.Parse(cutoffDate)));

            var testObject = mock.Create<ShipmentDateCutoffConverter>();
            var result = (Visibility) testObject.Convert(ShipmentTypeCode.Usps, typeof(Visibility), null, null);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void ConvertVisibility_ReturnsHidden_WhenShipmentTypeIsNull()
        {
            var testObject = mock.Create<ShipmentDateCutoffConverter>();
            var result = (Visibility) testObject.Convert(null, typeof(Visibility), null, null);

            Assert.Equal(Visibility.Hidden, result);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
