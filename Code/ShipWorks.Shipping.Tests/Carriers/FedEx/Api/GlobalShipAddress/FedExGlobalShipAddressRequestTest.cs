using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.GlobalShipAddress;
using ShipWorks.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Request;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.GlobalShipAddress
{
    public class FedExGlobalShipAddressRequestTest
    {
        private Mock<IFedExGlobalShipAddressRequestManipulator> firstManipulator;
        private Mock<IFedExGlobalShipAddressRequestManipulator> secondManipulator;
        private Mock<Func<IFedExSettingsRepository, IFedExGlobalShipAddressRequestManipulator>> firstManipulatorFactory;
        private Mock<Func<IFedExSettingsRepository, IFedExGlobalShipAddressRequestManipulator>> secondManipulatorFactory;

        private readonly AutoMock mock;
        private IShipmentEntity shipmentEntity;

        public FedExGlobalShipAddressRequestTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            shipmentEntity = Create.Shipment().AsFedEx(f => f.Set(x => x.ReferencePO, "testPO")).Build();

            firstManipulator = mock.CreateMock<IFedExGlobalShipAddressRequestManipulator>();
            firstManipulator.Setup(x => x.Manipulate(AnyIShipment, It.IsAny<SearchLocationsRequest>()))
                .Returns(new SearchLocationsRequest());
            secondManipulator = mock.CreateMock<IFedExGlobalShipAddressRequestManipulator>();
            secondManipulator.Setup(x => x.Manipulate(AnyIShipment, It.IsAny<SearchLocationsRequest>()))
                .Returns(new SearchLocationsRequest());

            firstManipulatorFactory = mock.MockFunc<IFedExSettingsRepository, IFedExGlobalShipAddressRequestManipulator>(firstManipulator);
            secondManipulatorFactory = mock.MockFunc<IFedExSettingsRepository, IFedExGlobalShipAddressRequestManipulator>(secondManipulator);

            mock.Provide<IEnumerable<Func<IFedExSettingsRepository, IFedExGlobalShipAddressRequestManipulator>>>(
                new List<Func<IFedExSettingsRepository, IFedExGlobalShipAddressRequestManipulator>>
                {
                    firstManipulatorFactory.Object,
                    secondManipulatorFactory.Object,
                });
        }

        [Fact]
        public void Submit_DelegatesToManipulators()
        {
            var testObject = mock.Create<FedExGlobalShipAddressRequest>();
            testObject.Submit(shipmentEntity);

            // Verify all of the manipulators were called and our test object was passed to them
            firstManipulator.Verify(m => m.Manipulate(shipmentEntity, It.IsAny<SearchLocationsRequest>()), Times.Once());
            secondManipulator.Verify(m => m.Manipulate(shipmentEntity, It.IsAny<SearchLocationsRequest>()), Times.Once());
        }

        [Fact]
        public void Submit_DelegatesToFedExService()
        {
            var service = mock.FromFactory<IFedExServiceGatewayFactory>()
                .Mock(x => x.Create(It.IsAny<IFedExSettingsRepository>()));

            var testObject = mock.Create<FedExGlobalShipAddressRequest>();
            testObject.Submit(shipmentEntity);

            service.Verify(s => s.GlobalShipAddressInquiry(It.IsAny<SearchLocationsRequest>()), Times.Once());
        }

        [Fact]
        public void Submit_ReturnsResponseFromGlobalShipAddressResult()
        {
            var addressReply = new SearchLocationsReply();
            var response = mock.CreateMock<IFedExGlobalShipAddressResponse>();

            mock.FromFactory<IFedExServiceGatewayFactory>()
                .Mock(x => x.Create(It.IsAny<IFedExSettingsRepository>()))
                .Setup(x => x.GlobalShipAddressInquiry(It.IsAny<SearchLocationsRequest>()))
                .Returns(addressReply);

            mock.MockFunc<SearchLocationsReply, IFedExGlobalShipAddressResponse>()
                .Setup(f => f(addressReply)).Returns(() => response.Object);

            var testObject = mock.Create<FedExGlobalShipAddressRequest>();
            var result = testObject.Submit(shipmentEntity);

            Assert.Equal(response.Object, result.Value);
        }
    }
}
