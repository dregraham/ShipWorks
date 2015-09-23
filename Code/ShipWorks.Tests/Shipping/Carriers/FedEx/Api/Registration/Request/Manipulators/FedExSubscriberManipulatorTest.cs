using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Registration.Request.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Registration;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Registration.Request.Manipulators
{
    public class FedExSubscriberManipulatorTest
    {
        private FedExSubscriberManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private Mock<ICarrierSettingsRepository> settingsRepository;

        private SubscriptionRequest nativeRequest;
        private FedExAccountEntity account;

        public FedExSubscriberManipulatorTest()
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

        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenCarrierRequestIsNull_Test()
        {
            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(null));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNull_Test()
        {
            // Setup the native request to be null
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), null);

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenNativeRequestIsNotSubscriptionRequest_Test()
        {
            // Setup the native request to be an unexpected type
            carrierRequest = new Mock<CarrierRequest>(new List<ICarrierRequestManipulator>(), new ShipmentEntity(), new RegisterWebUserRequest());

            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_ThrowsCarrierException_WhenAccountIsNull_Test()
        {
            carrierRequest.Setup(r => r.CarrierAccountEntity).Returns<IEntity2>(null);
            Assert.Throws<CarrierException>(() => testObject.Manipulate(carrierRequest.Object));
        }

        [Fact]
        public void Manipulate_GetsAccountFromRequest_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            carrierRequest.Verify(r => r.CarrierAccountEntity, Times.Once());
        }

        [Fact]
        public void Manipulate_SubscriberIsNotNull_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.NotNull(nativeRequest.Subscriber);
        }

        [Fact]
        public void Manipulate_SetsSubscriberAccountNumber_WithFedExAccount_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(account.AccountNumber, nativeRequest.Subscriber.AccountNumber);
        }

        [Fact]
        public void Manipulate_SetsSubscriberAddress_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // Just make sure this is assigned correctly since it's being populated by a separate class
            Assert.NotNull(nativeRequest.Subscriber.Address);
        }

        [Fact]
        public void Manipulate_SetsSubscriberContact_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // Just make sure this is assigned correctly since it's being populated by a separate class
            Assert.NotNull(nativeRequest.Subscriber.Contact);
        }

        [Fact]
        public void Manipulate_SetsSubscriberAccountShippingAddress_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // Just make sure this is assigned correctly since it's being populated by a separate class
            Assert.NotNull(nativeRequest.AccountShippingAddress);
        }

        [Fact]
        public void Manipulate_SetsCspSolutionId_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            // Just make sure this is assigned correctly since it's being populated by a separate class
            Assert.False(string.IsNullOrEmpty(nativeRequest.CspSolutionId));
        }

        [Fact]
        public void Manipulate_SetsCspType_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.Equal(CspType.CERTIFIED_SOLUTION_PROVIDER, nativeRequest.CspType);
        }

        [Fact]
        public void Manipulate_CspTypeSpecifiedIsTrue_Test()
        {
            testObject.Manipulate(carrierRequest.Object);

            Assert.True(nativeRequest.CspTypeSpecified);
        }
    }
}
