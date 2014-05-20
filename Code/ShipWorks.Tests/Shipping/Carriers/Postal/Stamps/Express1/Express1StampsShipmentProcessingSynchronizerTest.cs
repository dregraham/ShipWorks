using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Express1;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Stamps.Express1
{
    [TestClass]
    public class Express1StampsShipmentProcessingSynchronizerTest
    {
        private Express1StampsShipmentProcessingSynchronizer testObject;

        private Mock<ICarrierAccountRepository<StampsAccountEntity>> accountRepository;

        [TestInitialize]
        public void Initialize()
        {
            accountRepository = new Mock<ICarrierAccountRepository<StampsAccountEntity>>();

            testObject = new Express1StampsShipmentProcessingSynchronizer(accountRepository.Object);
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
            List<StampsAccountEntity> StampsAccounts = new List<StampsAccountEntity>()
            {
                new StampsAccountEntity(123),
                new StampsAccountEntity(456),
                new StampsAccountEntity(789)
            };

            accountRepository.Setup(r => r.Accounts).Returns(StampsAccounts);

            ShipmentEntity shipment = new ShipmentEntity
            {
                Postal = new PostalShipmentEntity()
                {
                    Stamps = new StampsShipmentEntity()
                }
            };

            testObject.SaveAccountToShipment(shipment);

            Assert.AreEqual(123, shipment.Postal.Stamps.StampsAccountID);
        }

        [TestMethod]
        [ExpectedException(typeof(StampsException))]
        public void SaveAccountToShipment_ThrowsStampsException_WhenNoAccounts_Test()
        {
            accountRepository.Setup(r => r.Accounts).Returns(new List<StampsAccountEntity>());

            testObject.SaveAccountToShipment(new ShipmentEntity());
        }

        [TestMethod]
        public void ReplaceInvalidAccount_SetsAccountID_WhenOneAccount_Test()
        {
            List<StampsAccountEntity> accounts = new List<StampsAccountEntity>()
            {
                new StampsAccountEntity(123)
            };

            accountRepository.Setup(r => r.Accounts).Returns(accounts);

            ShipmentEntity shipment = new ShipmentEntity
            {
                Postal = new PostalShipmentEntity()
                {
                    Stamps = new StampsShipmentEntity()
                }
            };

            testObject.ReplaceInvalidAccount(shipment);

            Assert.AreEqual(123, shipment.Postal.Stamps.StampsAccountID);
        }

        [TestMethod]
        public void ReplaceInvalidAccount_SetsToFirstAccountID_WhenTwoAccounts_Test()
        {
            List<StampsAccountEntity> accounts = new List<StampsAccountEntity>()
            {
                new StampsAccountEntity(123),
                new StampsAccountEntity(456)
            };

            accountRepository.Setup(r => r.Accounts).Returns(accounts);

            ShipmentEntity shipment = new ShipmentEntity
            {
                Postal = new PostalShipmentEntity()
                {
                    Stamps = new StampsShipmentEntity()
                }
            };

            testObject.ReplaceInvalidAccount(shipment);

            Assert.AreEqual(123, shipment.Postal.Stamps.StampsAccountID);
        }
    }
}