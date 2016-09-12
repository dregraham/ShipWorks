using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.UPS;
using System.Collections.Generic;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.UPS
{
    public class UpsShipmentProcessingSynchronizerTest
    {
        private UpsShipmentProcessingSynchronizer testObject;

        private Mock<ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity>> accountRepository;

        public UpsShipmentProcessingSynchronizerTest()
        {
            accountRepository = new Mock<ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity>>();
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, false)]
        public void HasAccounts_ReturnsAppropriateValue(bool shipmentTypeConfigured,
            bool accountRepoHasAccounts,
            bool expectedResult)
        {
            testObject = new UpsShipmentProcessingSynchronizer(accountRepository.Object, shipmentTypeConfigured);
            if (accountRepoHasAccounts)
            {
                accountRepository.Setup(a => a.Accounts).Returns(new[] { new UpsAccountEntity() });
            }

            Assert.Equal(expectedResult, testObject.HasAccounts);
        }

        [Fact]
        public void HasAccounts_DelegatesToRepository()
        {
            testObject = new UpsShipmentProcessingSynchronizer(accountRepository.Object, true);
            bool hasAccounts = testObject.HasAccounts;
            accountRepository.Verify(r => r.Accounts, Times.Once());
        }

        [Fact]
        public void SaveAccountToShipment_SetsAccountID_UsingFirstAccount()
        {
            testObject = new UpsShipmentProcessingSynchronizer(accountRepository.Object, true);

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
        public void SaveAccountToShipment_ThrowsUpsException_WhenNoAccounts()
        {
            testObject = new UpsShipmentProcessingSynchronizer(accountRepository.Object, true);

            accountRepository.Setup(r => r.Accounts).Returns(new List<UpsAccountEntity>());

            Assert.Throws<UpsException>(() => testObject.SaveAccountToShipment(new ShipmentEntity()));
        }

        [Fact]
        public void ReplaceInvalidAccount_SetsAccountID_WhenOneAccount()
        {
            testObject = new UpsShipmentProcessingSynchronizer(accountRepository.Object, true);

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
        public void ReplaceInvalidAccount_SetsToFirstAccountID_WhenTwoAccounts()
        {
            testObject = new UpsShipmentProcessingSynchronizer(accountRepository.Object, true);

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