using System;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services.ShipmentProcessorSteps.LabelRetrieval;
using ShipWorks.Shipping.Settings;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Services.ShipmentProcessorSteps.LabelRetrieval
{
    public class ShipmentPhoneManipulatorTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly ShipmentPhoneManipulator testObject;

        public ShipmentPhoneManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<ShipmentPhoneManipulator>();
        }

        [Theory]
        [InlineData("a")]
        [InlineData("314-555-1234")]
        public void Manipulate_DoNotChangeShipPhone_WhenShipPhoneIsNotEmpty(string shipPhone)
        {
            var result = testObject.Manipulate(new ShipmentEntity { ShipPhone = shipPhone });

            Assert.Equal(shipPhone, result.ShipPhone);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Manipulate_SetsShipPhoneToGlobalValue_WhenInputIsEmpty(string shipPhone)
        {
            mock.Mock<IShippingSettings>()
                .Setup(x => x.FetchReadOnly())
                .Returns(new ShippingSettingsEntity
                {
                    BlankPhoneOption = (int) ShipmentBlankPhoneOption.SpecifiedPhone,
                    BlankPhoneNumber = "314-555-1234",
                });

            var result = testObject.Manipulate(new ShipmentEntity { ShipPhone = shipPhone });

            Assert.Equal("314-555-1234", result.ShipPhone);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Manipulate_SetsShipPhoneToOriginPhone_WhenInputIsEmpty(string shipPhone)
        {
            mock.Mock<IShippingSettings>()
                .Setup(x => x.FetchReadOnly())
                .Returns(new ShippingSettingsEntity
                {
                    BlankPhoneOption = (int) ShipmentBlankPhoneOption.ShipperPhone,
                    BlankPhoneNumber = "314-555-1234",
                });

            var result = testObject.Manipulate(new ShipmentEntity { ShipPhone = shipPhone, OriginPhone = "314-555-5678" });

            Assert.Equal("314-555-5678", result.ShipPhone);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
