using System;
using System.Windows;
using Autofac.Extras.Moq;
using ShipWorks.Core.UI.ValueConverters;
using ShipWorks.Shipping.Settings;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.ShippingPanel.ValueConverters
{
    public class ShipmentDateCutoffConverterTest : IDisposable
    {
        readonly AutoMock mock;

        public ShipmentDateCutoffConverterTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void Convert_ThrowsArgumentException_WhenTargetTypeIsNotStringOrVisibility()
        {
            var testObject = mock.Create<ShipmentDateCutoffConverter>();
            Assert.Throws<ArgumentException>(() => testObject.Convert(ShipmentTypeCode.Usps, typeof(int), null, null));
        }

        [Theory]
        [InlineData(false, "15:00", "")]
        [InlineData(true, "6:00", "6:00 AM")]
        [InlineData(true, "15:00", "3:00 PM")]
        public void Convert_ReturnsExpectedValue_ForUspsWhenTargetIsString(bool cutoffEnabled, string cutoffDate, string expected)
        {
            var settings = mock.FromFactory<IShippingSettings>()
                .Mock(x => x.FetchReadOnly());
            settings.SetupGet(x => x.UspsShippingDateCutoffEnabled).Returns(cutoffEnabled);
            settings.SetupGet(x => x.UspsShippingDateCutoffTime).Returns(TimeSpan.Parse(cutoffDate));

            var testObject = mock.Create<ShipmentDateCutoffConverter>();
            var result = testObject.Convert(ShipmentTypeCode.Usps, typeof(string), null, null) as string;

            Assert.Contains(expected, result);
        }

        [Theory]
        [InlineData(ShipmentTypeCode.Amazon)]
        [InlineData(ShipmentTypeCode.BestRate)]
        [InlineData(ShipmentTypeCode.Endicia)]
        [InlineData(ShipmentTypeCode.Express1Endicia)]
        [InlineData(ShipmentTypeCode.Express1Usps)]
        [InlineData(ShipmentTypeCode.FedEx)]
        [InlineData(ShipmentTypeCode.iParcel)]
        [InlineData(ShipmentTypeCode.None)]
        [InlineData(ShipmentTypeCode.OnTrac)]
        [InlineData(ShipmentTypeCode.Other)]
        [InlineData(ShipmentTypeCode.PostalWebTools)]
        [InlineData(ShipmentTypeCode.UpsOnLineTools)]
        [InlineData(ShipmentTypeCode.UpsWorldShip)]
        public void Convert_ReturnsEmptyString_ForShipmentType(ShipmentTypeCode shipmentType)
        {
            var testObject = mock.Create<ShipmentDateCutoffConverter>();
            var result = testObject.Convert(shipmentType, typeof(string), null, null);

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void Convert_ReturnsEmptyString_WhenTargetIsStringAndShipmentTypeIsNull()
        {
            var testObject = mock.Create<ShipmentDateCutoffConverter>();
            var result = testObject.Convert(null, typeof(string), null, null);

            Assert.Equal(string.Empty, result);
        }

        [Theory]
        [InlineData(false, Visibility.Hidden)]
        [InlineData(true, Visibility.Visible)]
        public void Convert_ReturnsExpectedValue_ForUspsWhenTargetIsVisibility(bool cutoffEnabled, Visibility expected)
        {
            var settings = mock.FromFactory<IShippingSettings>()
                .Mock(x => x.FetchReadOnly());
            settings.SetupGet(x => x.UspsShippingDateCutoffEnabled).Returns(cutoffEnabled);

            var testObject = mock.Create<ShipmentDateCutoffConverter>();
            var result = testObject.Convert(ShipmentTypeCode.Usps, typeof(Visibility), null, null);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(ShipmentTypeCode.Amazon)]
        [InlineData(ShipmentTypeCode.BestRate)]
        [InlineData(ShipmentTypeCode.Endicia)]
        [InlineData(ShipmentTypeCode.Express1Endicia)]
        [InlineData(ShipmentTypeCode.Express1Usps)]
        [InlineData(ShipmentTypeCode.FedEx)]
        [InlineData(ShipmentTypeCode.iParcel)]
        [InlineData(ShipmentTypeCode.None)]
        [InlineData(ShipmentTypeCode.OnTrac)]
        [InlineData(ShipmentTypeCode.Other)]
        [InlineData(ShipmentTypeCode.PostalWebTools)]
        [InlineData(ShipmentTypeCode.UpsOnLineTools)]
        [InlineData(ShipmentTypeCode.UpsWorldShip)]
        public void Convert_ReturnsHidden_ForShipmentTypeWhenTargetIsVisibility(ShipmentTypeCode shipmentType)
        {
            var testObject = mock.Create<ShipmentDateCutoffConverter>();
            var result = testObject.Convert(shipmentType, typeof(Visibility), null, null);

            Assert.Equal(Visibility.Hidden, result);
        }

        [Fact]
        public void Convert_ReturnsHidden_WhenTargetIsVisibilityAndShipmentTypeIsNull()
        {
            var testObject = mock.Create<ShipmentDateCutoffConverter>();
            var result = testObject.Convert(null, typeof(Visibility), null, null);

            Assert.Equal(Visibility.Hidden, result);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
