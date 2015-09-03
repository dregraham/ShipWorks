using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Void.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Void.Request.Manipulators
{
    public class FedExVoidClientDetailManipulatorTest
    {
        private FedExVoidClientDetailManipulator testObject;

        private Mock<ICarrierSettingsRepository> settingsRepository;

        private Mock<CarrierRequest> voidCarrierRequest;
        private DeleteShipmentRequest nativeRequest;

        private FedExAccountEntity account;

        public FedExVoidClientDetailManipulatorTest()
        {
            account = new FedExAccountEntity { AccountNumber = "12345", MeterNumber = "67890" };

            settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>())).Returns(account);

            nativeRequest = new DeleteShipmentRequest { ClientDetail = new ClientDetail() };
            voidCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeRequest);
            voidCarrierRequest.Setup(r => r.CarrierAccountEntity).Returns(account);

            testObject = new FedExVoidClientDetailManipulator(settingsRepository.Object);
        }

        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenCarrierRequestIsNull_Test()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(null));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNull_Test()
        {
            // Setup the native request to be null
            voidCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), null);

            Assert.Throws<CarrierException>(() => testObject.Manipulate(voidCarrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotVoidRequest_AndIsNotVoidRequest_Test()
        {
            // Setup the native request to be an unexpected type
            voidCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new ShipmentReply());

            Assert.Throws<CarrierException>(() => testObject.Manipulate(voidCarrierRequest.Object));
        }

        [Fact]
        public void Manipulate_DelegatesToRequestForFedExAccount_ForVoid_Test()
        {
            testObject.Manipulate(voidCarrierRequest.Object);

            voidCarrierRequest.Verify(r => r.CarrierAccountEntity, Times.Once());
        }

        [Fact]
        public void Manipulate_SetsClientDetail_WhenWebAuthenticationDetailsIsNull_ForVoid_Test()
        {
            // Only setup is  to set the detail to null value
            nativeRequest.ClientDetail = null;

            testObject.Manipulate(voidCarrierRequest.Object);

            ClientDetail detail = ((DeleteShipmentRequest)voidCarrierRequest.Object.NativeRequest).ClientDetail;
            Assert.NotNull(detail);
        }

        [Fact]
        public void Manipulate_SetsClientDetail_WhenWebAuthenticationDetailsIsNotNull_ForVoid_Test()
        {
            // No additional setup since everything is in the Initialize method
            testObject.Manipulate(voidCarrierRequest.Object);

            ClientDetail detail = ((DeleteShipmentRequest)voidCarrierRequest.Object.NativeRequest).ClientDetail;
            Assert.NotNull(detail);
        }
    }
}
