using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal.Usps;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Usps
{
    public class UspsShipmentProcessingSynchronizerTest
    {
        private UspsShipmentProcessingSynchronizer testObject;

        private Mock<ICarrierAccountRepository<UspsAccountEntity>> accountRepository;

        [TestInitialize]
        public void Initialize()
        {
            accountRepository = new Mock<ICarrierAccountRepository<UspsAccountEntity>>();

            testObject = new UspsShipmentProcessingSynchronizer(accountRepository.Object);
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

        [Fact]
        [ExpectedException(typeof(UspsException))]
        public void SaveAccountToShipment_ThrowsUspsException_WhenNoAccounts_Test()
        {
            accountRepository.Setup(r => r.Accounts).Returns(new List<UspsAccountEntity>());

            testObject.SaveAccountToShipment(new ShipmentEntity());
        }

        [Fact]
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

        [Fact]
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