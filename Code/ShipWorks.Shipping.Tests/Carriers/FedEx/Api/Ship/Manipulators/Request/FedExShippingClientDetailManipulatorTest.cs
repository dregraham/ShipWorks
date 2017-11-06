using System;
using Autofac.Extras.Moq;
using Xunit;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    public class FedExShippingClientDetailManipulatorTest
    {
        private readonly ShipmentEntity shipment;
        private readonly FedExShippingClientDetailManipulator testObject;
        private readonly AutoMock mock;

        public FedExShippingClientDetailManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            shipment = Create.Shipment().AsFedEx().Build();

            testObject = mock.Create<FedExShippingClientDetailManipulator>();
        }

        [Fact]
        public void ShouldApply_ReturnsTrue()
        {
            Assert.True(testObject.ShouldApply(shipment));
        }

        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(null, new ProcessShipmentRequest(), 0));
        }

        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenProcessShipmentRequestIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(new ShipmentEntity(), null, 0));
        }

        [Fact]
        public void Manipulate_SetsClientDetail_WhenWebAuthenticationDetailsIsNull()
        {
            mock.Mock<IFedExSettingsRepository>()
                .Setup(x => x.GetAccountReadOnly(AnyIShipment))
                .Returns(new FedExAccountEntity { AccountNumber = "12345", MeterNumber = "67890" });

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest(), 0);

            Assert.Equal("12345", result.Value.ClientDetail.AccountNumber);
            Assert.Equal("67890", result.Value.ClientDetail.MeterNumber);
        }

        [Fact]
        public void Manipulate_OverwritesClientDetail_WhenWebAuthenticationDetailsIsNotNull()
        {
            mock.Mock<IFedExSettingsRepository>()
                .Setup(x => x.GetAccountReadOnly(AnyIShipment))
                .Returns(new FedExAccountEntity { AccountNumber = "12345", MeterNumber = "67890" });

            var result = testObject.Manipulate(shipment, new ProcessShipmentRequest { ClientDetail = new ClientDetail { AccountNumber = "111" } }, 0);

            Assert.Equal("12345", result.Value.ClientDetail.AccountNumber);
        }
    }
}
