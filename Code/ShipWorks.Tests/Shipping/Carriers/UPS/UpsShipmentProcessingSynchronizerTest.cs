using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.UPS;

namespace ShipWorks.Tests.Shipping.Carriers.UPS
{
    public class UpsShipmentProcessingSynchronizerTest
    {
        private UpsShipmentProcessingSynchronizer testObject;

        private Mock<ICarrierAccountRepository<UpsAccountEntity>> accountRepository;

        public UpsShipmentProcessingSynchronizerTest()
        {
            accountRepository = new Mock<ICarrierAccountRepository<UpsAccountEntity>>();

            testObject = new UpsShipmentProcessingSynchronizer(accountRepository.Object);
        }

        [Fact]
        public void HasAccounts_DelegatesToRepository_Test()
        {
            bool hasAccounts = testObject.HasAccounts;
            accountRepository.Verify(r => r.Accounts, Times.Once());
        }

        [Fact]
        public void SaveAccountToShipment_SetsAccountID_UsingFirstAccount_Test()
        {
            List<UpsAccountEntity> UpsAccounts = new List<UpsAccountEntity>()
            {
                new UpsAccountEntity(123),
                new UpsAccountEntity(456),
                new UpsAccountEntity(789)
            };

            accountRepository.Setup(r => r.Accounts).Returns(UpsAccounts);

            ShipmentEntity shipment = new ShipmentEntity
            {
                Ups = new UpsShipmentEntity()
            };

            testObject.SaveAccountToShipment(shipment);

            Assert.Equal(123, shipment.Ups.UpsAccountID);
        }

        [Fact]
        public void SaveAccountToShipment_ThrowsUpsException_WhenNoAccounts_Test()
        {
            accountRepository.Setup(r => r.Accounts).Returns(new List<UpsAccountEntity>());

            Assert.Throws<UpsException>(() => testObject.SaveAccountToShipment(new ShipmentEntity()));
        }

        [Fact]
        public void ReplaceInvalidAccount_SetsAccountID_WhenOneAccount_Test()
        {
            List<UpsAccountEntity> accounts = new List<UpsAccountEntity>()
            {
                new UpsAccountEntity(123)
            };

            accountRepository.Setup(r => r.Accounts).Returns(accounts);

            ShipmentEntity shipment = new ShipmentEntity
            {
                Ups = new UpsShipmentEntity()
            };

            testObject.ReplaceInvalidAccount(shipment);

            Assert.Equal(123, shipment.Ups.UpsAccountID);
        }

        [Fact]
        public void ReplaceInvalidAccount_SetsToFirstAccountID_WhenTwoAccounts_Test()
        {
            List<UpsAccountEntity> accounts = new List<UpsAccountEntity>()
            {
                new UpsAccountEntity(123),
                new UpsAccountEntity(456)
            };

            accountRepository.Setup(r => r.Accounts).Returns(accounts);

            ShipmentEntity shipment = new ShipmentEntity
            {
                Ups = new UpsShipmentEntity()
            };

            testObject.ReplaceInvalidAccount(shipment);

            Assert.Equal(123, shipment.Ups.UpsAccountID);
        }
    }
}