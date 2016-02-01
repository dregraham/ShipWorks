using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.FedEx;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx
{
    public class FedExShipmentProcessingSynchronizerTest
    {
        private FedExShipmentProcessingSynchronizer testObject;

        private Mock<ICarrierAccountRepository<FedExAccountEntity>> accountRepository;

        public FedExShipmentProcessingSynchronizerTest()
        {
            accountRepository = new Mock<ICarrierAccountRepository<FedExAccountEntity>>();

            testObject = new FedExShipmentProcessingSynchronizer(accountRepository.Object);
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
            List<FedExAccountEntity> fedExAccounts = new List<FedExAccountEntity>()
            {
                new FedExAccountEntity(123),
                new FedExAccountEntity(456),
                new FedExAccountEntity(789)
            };

            accountRepository.Setup(r => r.Accounts).Returns(fedExAccounts);

            ShipmentEntity shipment = new ShipmentEntity
            {
                FedEx = new FedExShipmentEntity()
            };

            testObject.SaveAccountToShipment(shipment);

            Assert.Equal(123, shipment.FedEx.FedExAccountID);
        }

        [Fact]
        public void SaveAccountToShipment_ThrowsFedExException_WhenNoAccounts()
        {
            accountRepository.Setup(r => r.Accounts).Returns(new List<FedExAccountEntity>());

            Assert.Throws<FedExException>(() => testObject.SaveAccountToShipment(new ShipmentEntity()));
        }


        [Fact]
        public void ReplaceInvalidAccount_SetsAccountID_WhenOneAccount()
        {
            List<FedExAccountEntity> fedExAccounts = new List<FedExAccountEntity>()
            {
                new FedExAccountEntity(123)
            };

            accountRepository.Setup(r => r.Accounts).Returns(fedExAccounts);

            ShipmentEntity shipment = new ShipmentEntity
            {
                FedEx = new FedExShipmentEntity()
            };

            testObject.ReplaceInvalidAccount(shipment);

            Assert.Equal(123, shipment.FedEx.FedExAccountID);
        }

        [Fact]
        public void ReplaceInvalidAccount_SetsToFirstAccountID_WhenTwoAccounts()
        {
            List<FedExAccountEntity> fedExAccounts = new List<FedExAccountEntity>()
            {
                new FedExAccountEntity(123),
                new FedExAccountEntity(456)
            };

            accountRepository.Setup(r => r.Accounts).Returns(fedExAccounts);

            ShipmentEntity shipment = new ShipmentEntity
            {
                FedEx = new FedExShipmentEntity()
            };

            testObject.ReplaceInvalidAccount(shipment);

            Assert.Equal(123, shipment.FedEx.FedExAccountID);
        }
    }
}
