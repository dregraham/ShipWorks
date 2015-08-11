using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Moq;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.InvoiceRegistration.Api.Response;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.WebServices.Registration;

namespace ShipWorks.Tests.Shipping.Carriers.UPS
{
    public class UpsResponseFactoryTest
    {
        private UpsResponseFactory testObject;
        private Mock<CarrierRequest> carrierRequest;

        public UpsResponseFactoryTest()
        {
            carrierRequest = new Mock<CarrierRequest>(null, new ShipmentEntity());

            testObject = new UpsResponseFactory();
        }

        #region CreateRegisterUserResponse Tests
        [Fact]
        public void CreateRegisterUserResponse_ThrowsCarrierException_WhenInvalidNativeResponseIsNull_Test()
        {
            Assert.Throws<CarrierException>(() => testObject.CreateRegisterUserResponse(null, carrierRequest.Object));
        }

        [Fact]
        public void CreateRegisterUserResponse_ThrowsCarrierException_WhenInvalidNativeResponseProvided_Test()
        {
            UpsResponseFactory invalidType = new UpsResponseFactory();
            Assert.Throws<CarrierException>(() => testObject.CreateRegisterUserResponse(invalidType, carrierRequest.Object));
        }

        [Fact]
        public void CreateRegisterUserResponse_ReturnsUpsRegisterUserResponse_Test()
        {
            RegisterResponse validType = new RegisterResponse();

            ICarrierResponse response = testObject.CreateRegisterUserResponse(validType, carrierRequest.Object);

            Assert.IsAssignableFrom<UpsInvoiceRegistrationResponse>(response);
        }
        #endregion
    }
}
