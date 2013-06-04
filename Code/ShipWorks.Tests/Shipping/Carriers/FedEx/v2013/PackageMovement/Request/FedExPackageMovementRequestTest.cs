using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.PackageMovement.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.PackageMovement.Response;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.PackageMovement;
using System.Collections.Generic;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.v2013.PackageMovement.Request
{
    [TestClass]
    public class FedExPackageMovementRequestTest
    {
        private FedExPackageMovementRequest testObject;


        private FedExAccountEntity account;
        Mock<ICarrierRequestManipulator> firstManipulator;
        Mock<ICarrierRequestManipulator> secondManipulator;

        [TestInitialize]
        public void Initialize()
        {
            account = new FedExAccountEntity
            {
                PostalCode = "60035",
                CountryCode = "USA"
            };

            Mock<IFedExServiceGateway> mockService = new Mock<IFedExServiceGateway>();
            mockService.Setup(service => service.PostalCodeInquiry(It.IsAny<PostalCodeInquiryRequest>()));

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

            testObject = new FedExPackageMovementRequest(manipulators, new ShipmentEntity(), account, mockService.Object);
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
        public void Submit_FedExPackageMovementResponseReturned_MakesValidRequest_Test()
        {
            FedExPackageMovementResponse response = (FedExPackageMovementResponse)testObject.Submit();

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void Submit_PostalAndCountryCodesArePopulatedInNativeRequest_CodesPopulatedInAccount_Test()
        {
            testObject.Submit();

            PostalCodeInquiryRequest postalCodeInquiryRequest = (PostalCodeInquiryRequest) testObject.NativeRequest;

            Assert.AreEqual(postalCodeInquiryRequest.PostalCode, account.PostalCode);

            Assert.AreEqual(postalCodeInquiryRequest.CountryCode, account.CountryCode);
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
