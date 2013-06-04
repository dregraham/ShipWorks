using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.InvoiceRegistration.Api.Response;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.WebServices.Registration;

namespace ShipWorks.Tests.Shipping.Carriers.UPS
{
    [TestClass]
    public class UpsResponseFactoryTest
    {
        private UpsResponseFactory testObject;
        private Mock<CarrierRequest> carrierRequest;

        [TestInitialize]
        public void Initialize()
        {
            carrierRequest = new Mock<CarrierRequest>(null, new ShipmentEntity());

            testObject = new UpsResponseFactory();
        }

        #region CreateRegisterUserResponse Tests
        [TestMethod]
        [ExpectedException(typeof(CarrierException))]
        public void CreateRegisterUserResponse_ThrowsCarrierException_WhenInvalidNativeResponseIsNull_Test()
        {
            testObject.CreateRegisterUserResponse(null, carrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(CarrierException))]
        public void CreateRegisterUserResponse_ThrowsCarrierException_WhenInvalidNativeResponseProvided_Test()
        {
            UpsResponseFactory invalidType = new UpsResponseFactory();
            testObject.CreateRegisterUserResponse(invalidType, carrierRequest.Object);
        }

        [TestMethod]
        public void CreateRegisterUserResponse_ReturnsUpsRegisterUserResponse_Test()
        {
            RegisterResponse validType = new RegisterResponse();

            ICarrierResponse response = testObject.CreateRegisterUserResponse(validType, carrierRequest.Object);

            Assert.IsInstanceOfType(response, typeof(UpsInvoiceRegistrationResponse));
        }
        #endregion
    }
}
