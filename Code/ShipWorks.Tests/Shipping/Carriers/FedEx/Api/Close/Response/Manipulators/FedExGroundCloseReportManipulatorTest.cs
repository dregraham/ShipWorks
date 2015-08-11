using System;
using Xunit;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Api.Close.Response;
using ShipWorks.Shipping.Carriers.FedEx.Api.Close.Response.Manipulators;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Close;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Close.Response.Manipulators
{
    public class FedExGroundCloseReportManipulatorTest
    {
        private FedExGroundCloseReportManipulator testObject;

        private Mock<CarrierRequest> carrierRequest;
        private Mock<ICarrierResponse> carrierResponse;
        private Mock<IFedExEndOfDayCloseRepository> closeRepository;

        private FedExAccountEntity account;
        private FedExEndOfDayCloseEntity closeEntity;

        public FedExGroundCloseReportManipulatorTest()
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
            closeRepository.Setup(r => r.Save(It.IsAny<FedExEndOfDayCloseEntity>(), It.IsAny<GroundCloseReply>()));

            testObject = new FedExGroundCloseReportManipulator(closeRepository.Object);
        }

        [Fact]
        public void Manipulate_ThrowsArgumentNullException_WhenCloseEntityIsNull_Test()
        {
            closeEntity = null;

            Assert.Throws<ArgumentNullException>(() => testObject.Manipulate(carrierResponse.Object, closeEntity));

            Assert.NotNull(closeEntity);
        }

        [Fact]
        public void Manipulate_ThrowsFedExException_WhenCarrierAccountIsNotFedEx_Test()
        {
            carrierRequest.Setup(r => r.CarrierAccountEntity).Returns(new UpsAccountEntity());

            Assert.Throws<FedExException>(() => testObject.Manipulate(carrierResponse.Object, closeEntity));
        }

        [Fact]
        public void Manipulate_ThrowsFedExException_WhenCarrierAccountIsNull_Test()
        {
            carrierRequest.Setup(r => r.CarrierAccountEntity).Returns<IEntity2>(null);

            Assert.Throws<FedExException>(() => testObject.Manipulate(carrierResponse.Object, closeEntity));
        }

        [Fact]
        public void Manipulate_SetsFedExAccountId_Test()
        {
            testObject.Manipulate(carrierResponse.Object, closeEntity);

            Assert.Equal(account.FedExAccountID, closeEntity.FedExAccountID);
        }

        [Fact]
        public void Manipulate_SetsAccountNumber_Test()
        {
            testObject.Manipulate(carrierResponse.Object, closeEntity);

            Assert.Equal(account.AccountNumber, closeEntity.AccountNumber);
        }

        [Fact]
        public void Manipulate_SetsCloseDate_Test()
        {
            DateTime utcStart = DateTime.UtcNow;

            testObject.Manipulate(carrierResponse.Object, closeEntity);

            // Verify that the date assigned to the close date falls between the time the test started
            // and the after manipulate was completed
            Assert.True(utcStart.Ticks <= closeEntity.CloseDate.Ticks);
            Assert.True(closeEntity.CloseDate.Ticks <= DateTime.UtcNow.Ticks);
        }

        [Fact]
        public void Manipulate_IsSmartPostIsFalse_Test()
        {
            testObject.Manipulate(carrierResponse.Object, closeEntity);

            Assert.False(closeEntity.IsSmartPost);
        }

        [Fact]
        public void Manipulate_DelegatesToRepositoryWhenToSave_Test()
        {
            testObject.Manipulate(carrierResponse.Object, closeEntity);

            closeRepository.Verify(r => r.Save(closeEntity, It.IsAny<GroundCloseReply>()), Times.Once());
        }
    }
}
