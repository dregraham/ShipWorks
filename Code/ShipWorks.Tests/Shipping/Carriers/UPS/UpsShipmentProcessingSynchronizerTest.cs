using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.UPS;

namespace ShipWorks.Tests.Shipping.Carriers.UPS
{
    [TestClass]
    public class UpsShipmentProcessingSynchronizerTest
    {
        private UpsShipmentProcessingSynchronizer testObject;

        private Mock<ICarrierAccountRepository<UpsAccountEntity>> accountRepository;

        [TestInitialize]
        public void Initialize()
        {
            accountRepository = new Mock<ICarrierAccountRepository<UpsAccountEntity>>();

            testObject = new UpsShipmentProcessingSynchronizer(accountRepository.Object);
        }

        [TestMethod]
        public void HasAccounts_DelegatesToRepository_Test()
        {
            bool hasAccounts = testObject.HasAccounts;
            accountRepository.Verify(r => r.Accounts, Times.Once());
        }

        [TestMethod]
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

            Assert.AreEqual(123, shipment.Ups.UpsAccountID);
        }

        [TestMethod]
        [ExpectedException(typeof(UpsException))]
        public void SaveAccountToShipment_ThrowsUpsException_WhenNoAccounts_Test()
        {
            accountRepository.Setup(r => r.Accounts).Returns(new List<UpsAccountEntity>());

            testObject.SaveAccountToShipment(new ShipmentEntity());
        }

        [TestMethod]
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

            Assert.AreEqual(123, shipment.Ups.UpsAccountID);
        }

        [TestMethod]
        public void ReplaceInvalidAccount_DoesNotSetAccountID_WhenTwoAccounts_Test()
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

            Assert.AreEqual(0, shipment.Ups.UpsAccountID);
        }
    }
}