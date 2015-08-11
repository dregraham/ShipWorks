using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Registration.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Registration.Request.Manipulators
{
    public class FedExCspContactManipulatorTest
    {
        private FedExCspContactManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;

        private RegisterWebCspUserRequest nativeRequest;
        private FedExAccountEntity account;

        [TestInitialize]
        public void Initialize()
        {
            account = new FedExAccountEntity
            {
                FirstName = "Michael",
                LastName = "Bolton",
                Street1 = "Not the",
                Street2 = "No Talent Ass-Clown",
                City = "Penetrode",
                StateProvCode = "MO",
                PostalCode = "63102"
            };

            nativeRequest = new RegisterWebCspUserRequest();
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeRequest);
            carrierRequest.Setup(r => r.CarrierAccountEntity).Returns(account);

            testObject = new FedExCspContactManipulator();
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
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), null);

            testObject.Manipulate(carrierRequest.Object);
        }

        [Fact]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotRegisterWebCspUserRequest_Test()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new SubscriptionRequest());

            testObject.Manipulate(carrierRequest.Object);
        }

        [Fact]
        public void Manipulate_SetsBillingAddress_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // The act of actually setting the individual fields of the billing address is deferred to
            // another object, so we just want to make sure it is not empty
            Assert.IsNotNull(nativeRequest.ShippingAddress);
        }

        [Fact]
        public void Manipulate_ContactIsNotNull_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNotNull(nativeRequest.UserContactAndAddress);
        }

        [Fact]
        public void Manipulate_SetsContactAddress_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // The act of actually setting the individual fields of the contact address is deferred to
            // another object, so we just want to make sure it is not empty
            Assert.IsNotNull(nativeRequest.UserContactAndAddress.Address);
        }

        [Fact]
        public void Manipulate_SetsContactPerson_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // The act of actually setting the individual fields of the contact info is deferred to
            // another object, so we just want to make sure it is not empty
            Assert.IsNotNull(nativeRequest.UserContactAndAddress.Contact);
        }
    }
}
