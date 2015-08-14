using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Registration.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Registration.Request.Manipulators
{
    [TestClass]
    public class FedExCspContactManipulatorTest
    {
        private FedExCspContactManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;

        private RegisterWebUserRequest nativeRequest;
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

            nativeRequest = new RegisterWebUserRequest();
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeRequest);
            carrierRequest.Setup(r => r.CarrierAccountEntity).Returns(account);

            testObject = new FedExCspContactManipulator();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Manipulate_ThrowsArgumentNullException_WhenCarrierRequestIsNull_Test()
        {
            testObject.Manipulate(null);
        }

        [TestMethod]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNull_Test()
        {
            // Setup the native request to be null
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), null);

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotRegisterWebCspUserRequest_Test()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new SubscriptionRequest());

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        public void Manipulate_SetsBillingAddress_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // The act of actually setting the individual fields of the billing address is deferred to
            // another object, so we just want to make sure it is not empty
            Assert.IsNotNull(nativeRequest.ShippingAddress);
        }

        [TestMethod]
        public void Manipulate_ContactIsNotNull_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNotNull(nativeRequest.UserContactAndAddress);
        }

        [TestMethod]
        public void Manipulate_SetsContactAddress_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // The act of actually setting the individual fields of the contact address is deferred to
            // another object, so we just want to make sure it is not empty
            Assert.IsNotNull(nativeRequest.UserContactAndAddress.Address);
        }

        [TestMethod]
        public void Manipulate_SetsContactPerson_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // The act of actually setting the individual fields of the contact info is deferred to
            // another object, so we just want to make sure it is not empty
            Assert.IsNotNull(nativeRequest.UserContactAndAddress.Contact);
        }
    }
}
