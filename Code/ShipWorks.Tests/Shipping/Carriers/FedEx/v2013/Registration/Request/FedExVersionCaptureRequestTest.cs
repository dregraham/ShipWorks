using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Registration.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Registration.Response;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Registration;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.v2013.Registration.Request
{
    [TestClass]
    public class FedExVersionCaptureRequestTest
    {
        private FedExVersionCaptureRequest testObject;

        Mock<ICarrierRequestManipulator> firstManipulator;
        Mock<ICarrierRequestManipulator> secondManipulator;
        private FedExAccountEntity account;

        [TestInitialize]
        public void Initialize()
        {
            account = new FedExAccountEntity { AccountNumber = "1234", MeterNumber = "45453" };

            Mock<IFedExServiceGateway> mockService = new Mock<IFedExServiceGateway>();
            mockService.Setup(service => service.VersionCapture(It.IsAny<VersionCaptureRequest>()));

            firstManipulator = new Mock<ICarrierRequestManipulator>();
            firstManipulator.Setup(m => m.Manipulate(It.IsAny<CarrierRequest>()));

            secondManipulator = new Mock<ICarrierRequestManipulator>();
            secondManipulator.Setup(m => m.Manipulate(It.IsAny<CarrierRequest>()));

            // Create some mocked manipulators for testing purposes
            List<ICarrierRequestManipulator> manipulators = new List<ICarrierRequestManipulator>()
            {
                firstManipulator.Object,
                secondManipulator.Object
            };

            testObject = new FedExVersionCaptureRequest(manipulators, new ShipmentEntity(), "123", mockService.Object, account);
        }

        [TestMethod]
        public void CarrierAccountEntity_IsNotNull_Test()
        {
            Assert.IsNotNull(testObject.CarrierAccountEntity as FedExAccountEntity);
        }

        [TestMethod]
        public void CarrierAccountEntity_ReturnsAccountProvidedInConstructor_Test()
        {
            Assert.AreEqual(account, testObject.CarrierAccountEntity as FedExAccountEntity);
        }

        [TestMethod]
        public void Submit_FedExVersionCaptureResponseReturned_MakesValidRequest_Test()
        {
            FedExVersionCaptureResponse response = (FedExVersionCaptureResponse) testObject.Submit();

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void Submit_LocationIDIsPopulatedInNativeRequest_LocationPassedInConstructor_Test()
        {
            testObject.Submit();

            VersionCaptureRequest request = (VersionCaptureRequest) testObject.NativeRequest;

            Assert.AreEqual("123",request.OriginLocationId);
        }

        [TestMethod]
        public void Submit_DelegatesToManipulators_Test()
        {
            // No additional setup needed since it was performed in Initialize()
            testObject.Submit();

            // Verify all of the manipulators were called and our test object was passed to them
            firstManipulator.Verify(m => m.Manipulate(testObject), Times.Once());
            secondManipulator.Verify(m => m.Manipulate(testObject), Times.Once());
        }
    }
}
