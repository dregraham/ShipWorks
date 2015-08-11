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

        [TestInitialize]
        public void Initialize()
        {
            accountRepository = new Mock<ICarrierAccountRepository<FedExAccountEntity>>();

            testObject = new FedExShipmentProcessingSynchronizer(accountRepository.Object);
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

            Assert.AreEqual(123, shipment.FedEx.FedExAccountID);
        }

        [Fact]
        [ExpectedException(typeof(FedExException))]
        public void SaveAccountToShipment_ThrowsFedExException_WhenNoAccounts_Test()
        {
            accountRepository.Setup(r => r.Accounts).Returns(new List<FedExAccountEntity>());

            testObject.SaveAccountToShipment(new ShipmentEntity());
        }


        [Fact]
        public void ReplaceInvalidAccount_SetsAccountID_WhenOneAccount_Test()
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

            Assert.AreEqual(123, shipment.FedEx.FedExAccountID);
        }

        [Fact]
        public void ReplaceInvalidAccount_SetsToFirstAccountID_WhenTwoAccounts_Test()
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

            Assert.AreEqual(123, shipment.FedEx.FedExAccountID);
        }
    }
}
