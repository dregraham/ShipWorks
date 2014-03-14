using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Endicia.Express1
{
    [TestClass]
    public class Express1EndiciaShipmentProcessingSynchronizerTest
    {
        private Express1EndiciaShipmentProcessingSynchronizer testObject;

        private Mock<ICarrierAccountRepository<EndiciaAccountEntity>> accountRepository;

        [TestInitialize]
        public void Initialize()
        {
            accountRepository = new Mock<ICarrierAccountRepository<EndiciaAccountEntity>>();

            testObject = new Express1EndiciaShipmentProcessingSynchronizer(accountRepository.Object);
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
            List<EndiciaAccountEntity> EndiciaAccounts = new List<EndiciaAccountEntity>()
            {
                new EndiciaAccountEntity(123),
                new EndiciaAccountEntity(456),
                new EndiciaAccountEntity(789)
            };

            accountRepository.Setup(r => r.Accounts).Returns(EndiciaAccounts);

            ShipmentEntity shipment = new ShipmentEntity
            {
                Postal = new PostalShipmentEntity()
                {
                    Endicia = new EndiciaShipmentEntity()
                }
            };

            testObject.SaveAccountToShipment(shipment);

            Assert.AreEqual(123, shipment.Postal.Endicia.EndiciaAccountID);
        }

        [TestMethod]
        [ExpectedException(typeof(Express1EndiciaException))]
        public void SaveAccountToShipment_ThrowsEndiciaException_WhenNoAccounts_Test()
        {
            accountRepository.Setup(r => r.Accounts).Returns(new List<EndiciaAccountEntity>());

            testObject.SaveAccountToShipment(new ShipmentEntity());
        }

    }
}