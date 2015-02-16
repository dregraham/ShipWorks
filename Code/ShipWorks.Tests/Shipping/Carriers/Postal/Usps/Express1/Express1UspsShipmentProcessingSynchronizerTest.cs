using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Usps.Express1
{
    [TestClass]
    public class Express1UspsShipmentProcessingSynchronizerTest
    {
        private Express1UspsShipmentProcessingSynchronizer testObject;

        private Mock<ICarrierAccountRepository<UspsAccountEntity>> accountRepository;

        [TestInitialize]
        public void Initialize()
        {
            accountRepository = new Mock<ICarrierAccountRepository<UspsAccountEntity>>();

            testObject = new Express1UspsShipmentProcessingSynchronizer(accountRepository.Object);
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
            List<UspsAccountEntity> accounts = new List<UspsAccountEntity>()
            {
                new UspsAccountEntity(123),
                new UspsAccountEntity(456),
                new UspsAccountEntity(789)
            };

            accountRepository.Setup(r => r.Accounts).Returns(accounts);

            ShipmentEntity shipment = new ShipmentEntity
            {
                Postal = new PostalShipmentEntity()
                {
                    Usps = new UspsShipmentEntity()
                }
            };

            testObject.SaveAccountToShipment(shipment);

            Assert.AreEqual(123, shipment.Postal.Usps.UspsAccountID);
        }

        [TestMethod]
        [ExpectedException(typeof(UspsException))]
        public void SaveAccountToShipment_ThrowsStampsException_WhenNoAccounts_Test()
        {
            accountRepository.Setup(r => r.Accounts).Returns(new List<UspsAccountEntity>());

            testObject.SaveAccountToShipment(new ShipmentEntity());
        }

        [TestMethod]
        public void ReplaceInvalidAccount_SetsAccountID_WhenOneAccount_Test()
        {
            List<UspsAccountEntity> accounts = new List<UspsAccountEntity>()
            {
                new UspsAccountEntity(123)
            };

            accountRepository.Setup(r => r.Accounts).Returns(accounts);

            ShipmentEntity shipment = new ShipmentEntity
            {
                Postal = new PostalShipmentEntity()
                {
                    Usps = new UspsShipmentEntity()
                }
            };

            testObject.ReplaceInvalidAccount(shipment);

            Assert.AreEqual(123, shipment.Postal.Usps.UspsAccountID);
        }

        [TestMethod]
        public void ReplaceInvalidAccount_SetsToFirstAccountID_WhenTwoAccounts_Test()
        {
            List<UspsAccountEntity> accounts = new List<UspsAccountEntity>()
            {
                new UspsAccountEntity(123),
                new UspsAccountEntity(456)
            };

            accountRepository.Setup(r => r.Accounts).Returns(accounts);

            ShipmentEntity shipment = new ShipmentEntity
            {
                Postal = new PostalShipmentEntity()
                {
                    Usps = new UspsShipmentEntity()
                }
            };

            testObject.ReplaceInvalidAccount(shipment);

            Assert.AreEqual(123, shipment.Postal.Usps.UspsAccountID);
        }
    }
}