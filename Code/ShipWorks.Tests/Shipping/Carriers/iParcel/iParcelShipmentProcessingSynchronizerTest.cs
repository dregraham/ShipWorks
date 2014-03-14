using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.iParcel;

namespace ShipWorks.Tests.Shipping.Carriers.iParcel
{
    [TestClass]
    public class iParcelShipmentProcessingSynchronizerTest
    {
        private iParcelShipmentProcessingSynchronizer testObject;

        private Mock<ICarrierAccountRepository<IParcelAccountEntity>> accountRepository;

        [TestInitialize]
        public void Initialize()
        {
            accountRepository = new Mock<ICarrierAccountRepository<IParcelAccountEntity>>();

            testObject = new iParcelShipmentProcessingSynchronizer(accountRepository.Object);
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

            Assert.AreEqual(123, shipment.IParcel.IParcelAccountID);
        }

        [TestMethod]
        [ExpectedException(typeof(iParcelException))]
        public void SaveAccountToShipment_ThrowsiParcelException_WhenNoAccounts_Test()
        {
            accountRepository.Setup(r => r.Accounts).Returns(new List<IParcelAccountEntity>());

            testObject.SaveAccountToShipment(new ShipmentEntity());
        }


        [TestMethod]
        public void ReplaceInvalidAccount_SetsAccountID_WhenOneAccount_Test()
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

            Assert.AreEqual(123, shipment.IParcel.IParcelAccountID);
        }

        [TestMethod]
        public void ReplaceInvalidAccount_DoesNotSetAccountID_WhenTwoAccounts_Test()
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

            Assert.AreEqual(0, shipment.IParcel.IParcelAccountID);
        }

    }
}
