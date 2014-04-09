using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.OnTrac;

namespace ShipWorks.Tests.Shipping.Carriers.OnTrac
{
    [TestClass]
    public class OnTracShipmentProcessingSynchronizerTest
    {
        private OnTracShipmentProcessingSynchronizer testObject;

        private Mock<ICarrierAccountRepository<OnTracAccountEntity>> accountRepository;

        [TestInitialize]
        public void Initialize()
        {
            accountRepository = new Mock<ICarrierAccountRepository<OnTracAccountEntity>>();

            testObject = new OnTracShipmentProcessingSynchronizer(accountRepository.Object);
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
            List<OnTracAccountEntity> OnTracAccounts = new List<OnTracAccountEntity>()
            {
                new OnTracAccountEntity(123),
                new OnTracAccountEntity(456),
                new OnTracAccountEntity(789)
            };

            accountRepository.Setup(r => r.Accounts).Returns(OnTracAccounts);

            ShipmentEntity shipment = new ShipmentEntity
            {
                OnTrac = new OnTracShipmentEntity()
            };

            testObject.SaveAccountToShipment(shipment);

            Assert.AreEqual(123, shipment.OnTrac.OnTracAccountID);
        }

        [TestMethod]
        [ExpectedException(typeof(OnTracException))]
        public void SaveAccountToShipment_ThrowsOnTracException_WhenNoAccounts_Test()
        {
            accountRepository.Setup(r => r.Accounts).Returns(new List<OnTracAccountEntity>());

            testObject.SaveAccountToShipment(new ShipmentEntity());
        }



        [TestMethod]
        public void ReplaceInvalidAccount_SetsAccountID_WhenOneAccount_Test()
        {
            List<OnTracAccountEntity> accounts = new List<OnTracAccountEntity>()
            {
                new OnTracAccountEntity(123)
            };

            accountRepository.Setup(r => r.Accounts).Returns(accounts);

            ShipmentEntity shipment = new ShipmentEntity
            {
                OnTrac = new OnTracShipmentEntity()
            };

            testObject.ReplaceInvalidAccount(shipment);

            Assert.AreEqual(123, shipment.OnTrac.OnTracAccountID);
        }

        [TestMethod]
        public void ReplaceInvalidAccount_DoesNotSetAccountID_WhenTwoAccounts_Test()
        {
            List<OnTracAccountEntity> accounts = new List<OnTracAccountEntity>()
            {
                new OnTracAccountEntity(123),
                new OnTracAccountEntity(456)
            };

            accountRepository.Setup(r => r.Accounts).Returns(accounts);

            ShipmentEntity shipment = new ShipmentEntity
            {
                OnTrac = new OnTracShipmentEntity()
            };

            testObject.ReplaceInvalidAccount(shipment);

            Assert.AreEqual(0, shipment.OnTrac.OnTracAccountID);
        }
    }
}
