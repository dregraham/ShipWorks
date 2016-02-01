using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Usps.Express1
{
    public class Express1UspsShipmentProcessingSynchronizerTest
    {
        private Express1UspsShipmentProcessingSynchronizer testObject;

        private Mock<ICarrierAccountRepository<UspsAccountEntity>> accountRepository;

        public Express1UspsShipmentProcessingSynchronizerTest()
        {
            accountRepository = new Mock<ICarrierAccountRepository<UspsAccountEntity>>();

            testObject = new Express1UspsShipmentProcessingSynchronizer(accountRepository.Object);
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

            Assert.Equal(123, shipment.Postal.Usps.UspsAccountID);
        }

        [Fact]
        public void SaveAccountToShipment_ThrowsUspsException_WhenNoAccounts()
        {
            accountRepository.Setup(r => r.Accounts).Returns(new List<UspsAccountEntity>());

            Assert.Throws<UspsException>(() => testObject.SaveAccountToShipment(new ShipmentEntity()));
        }

        [Fact]
        public void ReplaceInvalidAccount_SetsAccountID_WhenOneAccount()
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

            Assert.Equal(123, shipment.Postal.Usps.UspsAccountID);
        }

        [Fact]
        public void ReplaceInvalidAccount_SetsToFirstAccountID_WhenTwoAccounts()
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

            Assert.Equal(123, shipment.Postal.Usps.UspsAccountID);
        }
    }
}