using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Close.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Close;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Close.Request.Manipulators
{
    public class FedExCloseClientDetailManipulatorTest
    {
        private FedExCloseClientDetailManipulator testObject;

        private Mock<ICarrierSettingsRepository> settingsRepository;
        
        private Mock<CarrierRequest> groundCloseCarrierRequest;
        private GroundCloseRequest nativeGroundCloseRequest;

        private Mock<CarrierRequest> smartPostCloseCarrierRequest;
        private SmartPostCloseRequest nativeSmartPostRequest;


        private FedExAccountEntity account;

        [TestInitialize]
        public void Initialize()
        {
            account = new FedExAccountEntity {AccountNumber = "12345", MeterNumber = "67890"};

            settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>())).Returns(account);


            nativeGroundCloseRequest = new GroundCloseRequest { ClientDetail = new ClientDetail() };
            groundCloseCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeGroundCloseRequest);
            groundCloseCarrierRequest.Setup(r => r.CarrierAccountEntity).Returns(account);

            nativeSmartPostRequest = new SmartPostCloseRequest { ClientDetail = new ClientDetail() };
            smartPostCloseCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeSmartPostRequest);
            smartPostCloseCarrierRequest.Setup(r => r.CarrierAccountEntity).Returns(account);
            
            testObject = new FedExCloseClientDetailManipulator(settingsRepository.Object);
        }

        [Fact]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Manipulate_ThrowsArgumentNullException_WhenCarrierRequestIsNull_Test()
        {
            testObject.Manipulate(null);
        }

        [Fact]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNull_Test()
        {
            // Setup the native request to be null
            groundCloseCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), null);

            testObject.Manipulate(groundCloseCarrierRequest.Object);
        }

        [Fact]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotGroundCloseRequest_AndIsNotSmartPostCloseRequest_Test()
        {
            // Setup the native request to be an unexpected type
            groundCloseCarrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new SmartPostCloseReply());

            testObject.Manipulate(groundCloseCarrierRequest.Object);
        }

        [Fact]
        public void Manipulate_DelegatesToRequestForFedExAccount_ForGroundClose_Test()
        {
            testObject.Manipulate(groundCloseCarrierRequest.Object);

            groundCloseCarrierRequest.Verify(r => r.CarrierAccountEntity, Times.Once());
        }

        [Fact]
        public void Manipulate_SetsClientDetail_WhenWebAuthenticationDetailsIsNull_ForGroundClose_Test()
        {
            // Only setup is  to set the detail to null value
            nativeGroundCloseRequest.ClientDetail = null;

            testObject.Manipulate(groundCloseCarrierRequest.Object);

            ClientDetail detail = ((GroundCloseRequest)groundCloseCarrierRequest.Object.NativeRequest).ClientDetail;
            Assert.IsNotNull(detail);
        }

        [Fact]
        public void Manipulate_SetsClientDetail_WhenWebAuthenticationDetailsIsNotNull_ForGroundClose_Test()
        {
            // No additional setup since everything is in the Initialize method
            testObject.Manipulate(groundCloseCarrierRequest.Object);

            ClientDetail detail = ((GroundCloseRequest)groundCloseCarrierRequest.Object.NativeRequest).ClientDetail;
            Assert.IsNotNull(detail);
        }







        [Fact]
        public void Manipulate_DelegatesToRequestForFedExAccount_ForSmartPostClose_Test()
        {
            testObject.Manipulate(smartPostCloseCarrierRequest.Object);

            smartPostCloseCarrierRequest.Verify(r => r.CarrierAccountEntity, Times.Once());
        }

        [Fact]
        public void Manipulate_SetsClientDetail_WhenWebAuthenticationDetailsIsNull_ForSmartPostClose_Test()
        {
            // Only setup is  to set the detail to null value
            nativeSmartPostRequest.ClientDetail = null;

            testObject.Manipulate(smartPostCloseCarrierRequest.Object);

            ClientDetail detail = ((SmartPostCloseRequest)smartPostCloseCarrierRequest.Object.NativeRequest).ClientDetail;
            Assert.IsNotNull(detail);
        }

        [Fact]
        public void Manipulate_SetsClientDetail_WhenWebAuthenticationDetailsIsNotNull_ForSmartPostClose_Test()
        {
            // No additional setup since everything is in the Initialize method
            testObject.Manipulate(smartPostCloseCarrierRequest.Object);

            ClientDetail detail = ((SmartPostCloseRequest)smartPostCloseCarrierRequest.Object.NativeRequest).ClientDetail;
            Assert.IsNotNull(detail);
        }
    }
}
