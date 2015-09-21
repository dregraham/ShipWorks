using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Endicia.Express1
{
    public class Express1EndiciaShipmentProcessingSynchronizerTest
    {
        private Express1EndiciaShipmentProcessingSynchronizer testObject;

        private Mock<ICarrierAccountRepository<EndiciaAccountEntity>> accountRepository;

        public Express1EndiciaShipmentProcessingSynchronizerTest()
        {
            accountRepository = new Mock<ICarrierAccountRepository<EndiciaAccountEntity>>();

            testObject = new Express1EndiciaShipmentProcessingSynchronizer(accountRepository.Object);
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

            Assert.Equal(123, shipment.Postal.Endicia.EndiciaAccountID);
        }

        [Fact]
        public void SaveAccountToShipment_ThrowsEndiciaException_WhenNoAccounts_Test()
        {
            accountRepository.Setup(r => r.Accounts).Returns(new List<EndiciaAccountEntity>());

            Assert.Throws<Express1EndiciaException>(() => testObject.SaveAccountToShipment(new ShipmentEntity()));
        }

        [Fact]
        public void ReplaceInvalidAccount_SetsAccountID_WhenOneAccount_Test()
        {
            List<EndiciaAccountEntity> accounts = new List<EndiciaAccountEntity>()
            {
                new EndiciaAccountEntity(123)
            };

            accountRepository.Setup(r => r.Accounts).Returns(accounts);

            ShipmentEntity shipment = new ShipmentEntity
            {
                Postal = new PostalShipmentEntity()
                {
                    Endicia = new EndiciaShipmentEntity()
                }
            };

            testObject.ReplaceInvalidAccount(shipment);

            Assert.Equal(123, shipment.Postal.Endicia.EndiciaAccountID);
        }

        [Fact]
        public void ReplaceInvalidAccount_SetsToFirstAccountID_WhenTwoAccounts_Test()
        {
            List<EndiciaAccountEntity> accounts = new List<EndiciaAccountEntity>()
            {
                new EndiciaAccountEntity(123),
                new EndiciaAccountEntity(456)
            };

            accountRepository.Setup(r => r.Accounts).Returns(accounts);

            ShipmentEntity shipment = new ShipmentEntity
            {
                Postal = new PostalShipmentEntity()
                {
                    Endicia = new EndiciaShipmentEntity()
                }
            };

            testObject.ReplaceInvalidAccount(shipment);

            Assert.Equal(123, shipment.Postal.Endicia.EndiciaAccountID);
        }
    }
}