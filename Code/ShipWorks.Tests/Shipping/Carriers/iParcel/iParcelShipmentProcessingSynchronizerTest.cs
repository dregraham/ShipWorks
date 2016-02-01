using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.iParcel;

namespace ShipWorks.Tests.Shipping.Carriers.iParcel
{
    public class iParcelShipmentProcessingSynchronizerTest
    {
        private iParcelShipmentProcessingSynchronizer testObject;

        private Mock<ICarrierAccountRepository<IParcelAccountEntity>> accountRepository;

        public iParcelShipmentProcessingSynchronizerTest()
        {
            accountRepository = new Mock<ICarrierAccountRepository<IParcelAccountEntity>>();

            testObject = new iParcelShipmentProcessingSynchronizer(accountRepository.Object);
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
            List<IParcelAccountEntity> iParcelAccounts = new List<IParcelAccountEntity>()
            {
                new IParcelAccountEntity(123),
                new IParcelAccountEntity(456),
                new IParcelAccountEntity(789)
            };

            accountRepository.Setup(r => r.Accounts).Returns(iParcelAccounts);

            ShipmentEntity shipment = new ShipmentEntity
            {
                IParcel = new IParcelShipmentEntity()
            };

            testObject.SaveAccountToShipment(shipment);

            Assert.Equal(123, shipment.IParcel.IParcelAccountID);
        }

        [Fact]
        public void SaveAccountToShipment_ThrowsiParcelException_WhenNoAccounts()
        {
            accountRepository.Setup(r => r.Accounts).Returns(new List<IParcelAccountEntity>());

            Assert.Throws<iParcelException>(() => testObject.SaveAccountToShipment(new ShipmentEntity()));
        }


        [Fact]
        public void ReplaceInvalidAccount_SetsAccountID_WhenOneAccount()
        {
            List<IParcelAccountEntity> accounts = new List<IParcelAccountEntity>()
            {
                new IParcelAccountEntity(123)
            };

            accountRepository.Setup(r => r.Accounts).Returns(accounts);

            ShipmentEntity shipment = new ShipmentEntity
            {
                IParcel = new IParcelShipmentEntity()
            };

            testObject.ReplaceInvalidAccount(shipment);

            Assert.Equal(123, shipment.IParcel.IParcelAccountID);
        }

        [Fact]
        public void ReplaceInvalidAccount_SetsToFirstAccountID_WhenTwoAccounts()
        {
            List<IParcelAccountEntity> accounts = new List<IParcelAccountEntity>()
            {
                new IParcelAccountEntity(123),
                new IParcelAccountEntity(456)
            };

            accountRepository.Setup(r => r.Accounts).Returns(accounts);

            ShipmentEntity shipment = new ShipmentEntity
            {
                IParcel = new IParcelShipmentEntity()
            };

            testObject.ReplaceInvalidAccount(shipment);

            Assert.Equal(123, shipment.IParcel.IParcelAccountID);
        }

    }
}
