using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.PackageMovement.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.PackageMovement.Response;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.PackageMovement;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.PackageMovement.Request
{
    public class FedExPackageMovementRequestTest
    {
        private FedExPackageMovementRequest testObject;


        private FedExAccountEntity account;
        Mock<ICarrierRequestManipulator> firstManipulator;
        Mock<ICarrierRequestManipulator> secondManipulator;

        public FedExPackageMovementRequestTest()
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

        [Fact]
        public void CarrierAccountEntity_IsNotNull_Test()
        {
            Assert.NotNull(testObject.CarrierAccountEntity as FedExAccountEntity);
        }

        [Fact]
        public void CarrierAccountEntity_ReturnsAccountProvidedInConstructor_Test()
        {
            Assert.Equal(account, testObject.CarrierAccountEntity as FedExAccountEntity);
        }

        [Fact]
        public void Submit_FedExPackageMovementResponseReturned_MakesValidRequest_Test()
        {
            FedExPackageMovementResponse response = (FedExPackageMovementResponse)testObject.Submit();

            Assert.NotNull(response);
        }

        [Fact]
        public void Submit_PostalAndCountryCodesArePopulatedInNativeRequest_CodesPopulatedInAccount_Test()
        {
            testObject.Submit();

            PostalCodeInquiryRequest postalCodeInquiryRequest = (PostalCodeInquiryRequest) testObject.NativeRequest;

            Assert.Equal(postalCodeInquiryRequest.PostalCode, account.PostalCode);

            Assert.Equal(postalCodeInquiryRequest.CountryCode, account.CountryCode);
        }

        [Fact]
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
