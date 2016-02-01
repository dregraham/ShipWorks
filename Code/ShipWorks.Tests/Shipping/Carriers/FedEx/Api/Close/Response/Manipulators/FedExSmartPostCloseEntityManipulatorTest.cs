using System;
using Xunit;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Api.Close.Response;
using ShipWorks.Shipping.Carriers.FedEx.Api.Close.Response.Manipulators;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Close.Response.Manipulators
{
    public class FedExSmartPostCloseEntityManipulatorTest
    {
        private FedExSmartPostCloseEntityManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private Mock<ICarrierResponse> carrierResponse;
        private Mock<IFedExEndOfDayCloseRepository> closeRepository;

        private FedExAccountEntity account;
        private FedExEndOfDayCloseEntity closeEntity;

        public FedExSmartPostCloseEntityManipulatorTest()
        {
            account = new FedExAccountEntity { AccountNumber = "12345", FedExAccountID = 1001 };
            closeEntity = new FedExEndOfDayCloseEntity();

            // Setup the carrier request to return the preconfigured account since this will be 
            // used in the manipulator to obtain the account number and account ID
            carrierRequest = new Mock<CarrierRequest>(null, null);
            carrierRequest.Setup(r => r.CarrierAccountEntity).Returns(account);

            carrierResponse = new Mock<ICarrierResponse>();
            carrierResponse.Setup(r => r.Request).Returns(carrierRequest.Object);

            closeRepository = new Mock<IFedExEndOfDayCloseRepository>();
            closeRepository.Setup(r => r.Save(It.IsAny<FedExEndOfDayCloseEntity>()));

            testObject = new FedExSmartPostCloseEntityManipulator(closeRepository.Object);
        }

        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenCloseEntityIsNull()
        {
            closeEntity = null;

            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(carrierResponse.Object, closeEntity));
        }

        [Fact]
        public void Manipulate_ThrowsFedExException_WhenCarrierAccountIsNotFedEx()
        {
            carrierRequest.Setup(r => r.CarrierAccountEntity).Returns(new UpsAccountEntity());

            Assert.Throws<FedExException>(() => testObject.Manipulate(carrierResponse.Object, closeEntity));
        }

        [Fact]
        public void Manipulate_ThrowsFedExException_WhenCarrierAccountIsNull()
        {
            carrierRequest.Setup(r => r.CarrierAccountEntity).Returns<IEntity2>(null);

            Assert.Throws<FedExException>(() => testObject.Manipulate(carrierResponse.Object, closeEntity));
        }

        [Fact]
        public void Manipulate_SetsFedExAccountId()
        {
            testObject.Manipulate(carrierResponse.Object, closeEntity);

            Assert.Equal(account.FedExAccountID, closeEntity.FedExAccountID);
        }

        [Fact]
        public void Manipulate_SetsAccountNumber()
        {
            testObject.Manipulate(carrierResponse.Object, closeEntity);

            Assert.Equal(account.AccountNumber, closeEntity.AccountNumber);
        }

        [Fact]
        public void Manipulate_SetsCloseDate()
        {
            DateTime utcStart = DateTime.UtcNow;

            testObject.Manipulate(carrierResponse.Object, closeEntity);

            // Verify that the date assigned to the close date falls between the time the test started
            // and the after manipulate was completed
            Assert.True(utcStart.Ticks <= closeEntity.CloseDate.Ticks);
            Assert.True(closeEntity.CloseDate.Ticks <= DateTime.UtcNow.Ticks);
        }

        [Fact]
        public void Manipulate_IsSmartPostIsTrue()
        {
            testObject.Manipulate(carrierResponse.Object, closeEntity);

            Assert.True(closeEntity.IsSmartPost);
        }

        [Fact]
        public void Manipulate_DelegatesToRepositoryWhenToSave()
        {
            testObject.Manipulate(carrierResponse.Object, closeEntity);

            closeRepository.Verify(r => r.Save(closeEntity), Times.Once());
        }
    }
}
