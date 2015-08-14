using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Registration.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Registration.Request.Manipulators
{
    [TestClass]
    public class FedExSubscriberManipulatorTest
    {
        private FedExSubscriberManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private Mock<ICarrierSettingsRepository> settingsRepository;

        private SubscriptionRequest nativeRequest;
        private FedExAccountEntity account;

        [TestInitialize]
        public void Initialize()
        {
            account = new FedExAccountEntity
            {
                FirstName = "Michael",
                MiddleName = string.Empty,
                LastName = "Bolton",
                Street1 = "Not the",
                Street2 = "No Talent Ass-Clown",
                City = "Penetrode",
                StateProvCode = "MO",
                PostalCode = "63102"
            };

            nativeRequest = new SubscriptionRequest();

            // Nothing is used from the repository; the fedEx settings needs a repo to be created, though
            settingsRepository = new Mock<ICarrierSettingsRepository>();
            

            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), nativeRequest);
            carrierRequest.Setup(r => r.CarrierAccountEntity).Returns(account);

            testObject = new FedExSubscriberManipulator(settingsRepository.Object);
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
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotSubscriptionRequest_Test()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new RegisterWebUserRequest());

            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(CarrierException))]
        public void Manipulate_ThrowsCarrierException_WhenAccountIsNull_Test()
        {
            carrierRequest.Setup(r => r.CarrierAccountEntity).Returns<IEntity2>(null);
            testObject.Manipulate(carrierRequest.Object);
        }

        [TestMethod]
        public void Manipulate_GetsAccountFromRequest_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            carrierRequest.Verify(r => r.CarrierAccountEntity, Times.Once());
        }

        [TestMethod]
        public void Manipulate_SubscriberIsNotNull_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.IsNotNull(nativeRequest.Subscriber);
        }

        [TestMethod]
        public void Manipulate_SetsSubscriberAccountNumber_WithFedExAccount_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(account.AccountNumber, nativeRequest.Subscriber.AccountNumber);
        }

        [TestMethod]
        public void Manipulate_SetsSubscriberAddress_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // Just make sure this is assigned correctly since it's being populated by a separate class
            Assert.IsNotNull(nativeRequest.Subscriber.Address);
        }

        [TestMethod]
        public void Manipulate_SetsSubscriberContact_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // Just make sure this is assigned correctly since it's being populated by a separate class
            Assert.IsNotNull(nativeRequest.Subscriber.Contact);
        }

        [TestMethod]
        public void Manipulate_SetsSubscriberAccountShippingAddress_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // Just make sure this is assigned correctly since it's being populated by a separate class
            Assert.IsNotNull(nativeRequest.AccountShippingAddress);
        }

        [TestMethod]
        public void Manipulate_SetsCspSolutionId_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // Just make sure this is assigned correctly since it's being populated by a separate class
            Assert.IsFalse(string.IsNullOrEmpty(nativeRequest.CspSolutionId));
        }

        [TestMethod]
        public void Manipulate_SetsCspType_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.AreEqual(CspType.CERTIFIED_SOLUTION_PROVIDER, nativeRequest.CspType);
        }

        [TestMethod]
        public void Manipulate_CspTypeSpecifiedIsTrue_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.IsTrue(nativeRequest.CspTypeSpecified);
        }
    }
}
