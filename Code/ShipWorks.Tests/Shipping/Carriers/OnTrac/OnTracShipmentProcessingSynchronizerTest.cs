using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.OnTrac;

namespace ShipWorks.Tests.Shipping.Carriers.OnTrac
{
    public class OnTracShipmentProcessingSynchronizerTest
    {
        private OnTracShipmentProcessingSynchronizer testObject;

        private Mock<ICarrierAccountRepository<OnTracAccountEntity>> accountRepository;

        public OnTracShipmentProcessingSynchronizerTest()
        {
            accountRepository = new Mock<ICarrierAccountRepository<OnTracAccountEntity>>();

            testObject = new OnTracShipmentProcessingSynchronizer(accountRepository.Object);
        }

        [Fact]
        public void HasAccounts_DelegatesToRepository()
        {
            bool hasAccounts = testObject.HasAccounts;
            accountRepository.Verify(r => r.Accounts, Times.Once());
        }

        [Fact]
        public void SaveAccountToShipment_SetsAccountID_UsingFirstAccount()
        {
            List<OnTracAccountEntity> OnTracAccounts = new List<OnTracAccountEntity>()
            {
                new OnTracAccountEntity(123),
                new OnTracAccountEntity(456),
                new OnTracAccountEntity(789)
            };

            accountRepository.Setup(r => r.Accounts).Returns(OnTracAccounts);

            ShipmentEntity shipment = new ShipmentEntity
            {
                OnTrac = new OnTracShipmentEntity()
            };

            testObject.SaveAccountToShipment(shipment);

            Assert.Equal(123, shipment.OnTrac.OnTracAccountID);
        }

        [Fact]
        public void SaveAccountToShipment_ThrowsOnTracException_WhenNoAccounts()
        {
            accountRepository.Setup(r => r.Accounts).Returns(new List<OnTracAccountEntity>());

            Assert.Throws<OnTracException>(() => testObject.SaveAccountToShipment(new ShipmentEntity()));
        }
        
        [Fact]
        public void ReplaceInvalidAccount_SetsAccountID_WhenOneAccount()
        {
            List<OnTracAccountEntity> accounts = new List<OnTracAccountEntity>()
            {
                new OnTracAccountEntity(123)
            };

            accountRepository.Setup(r => r.Accounts).Returns(accounts);

            ShipmentEntity shipment = new ShipmentEntity
            {
                OnTrac = new OnTracShipmentEntity()
            };

            testObject.ReplaceInvalidAccount(shipment);

            Assert.Equal(123, shipment.OnTrac.OnTracAccountID);
        }

        [Fact]
        public void ReplaceInvalidAccount_SetsToFirstAccountID_WhenTwoAccounts()
        {
            List<OnTracAccountEntity> accounts = new List<OnTracAccountEntity>()
            {
                new OnTracAccountEntity(123),
                new OnTracAccountEntity(456)
            };

            accountRepository.Setup(r => r.Accounts).Returns(accounts);

            ShipmentEntity shipment = new ShipmentEntity
            {
                OnTrac = new OnTracShipmentEntity()
            };

            testObject.ReplaceInvalidAccount(shipment);

            Assert.Equal(123, shipment.OnTrac.OnTracAccountID);
        }
    }
}
