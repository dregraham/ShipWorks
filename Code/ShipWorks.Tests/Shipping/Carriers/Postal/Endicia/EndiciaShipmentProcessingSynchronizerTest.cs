using System.Collections.Generic;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal.Endicia;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Endicia
{
    public class EndiciaShipmentProcessingSynchronizerTest
    {
        private EndiciaShipmentProcessingSynchronizer testObject;

        private Mock<ICarrierAccountRepository<EndiciaAccountEntity>> accountRepository;

        public EndiciaShipmentProcessingSynchronizerTest()
        {
            accountRepository = new Mock<ICarrierAccountRepository<EndiciaAccountEntity>>();

            testObject = new EndiciaShipmentProcessingSynchronizer(accountRepository.Object);
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
        public void SaveAccountToShipment_ThrowsEndiciaException_WhenNoAccounts()
        {
            accountRepository.Setup(r => r.Accounts).Returns(new List<EndiciaAccountEntity>());

            Assert.Throws<EndiciaException>(() => testObject.SaveAccountToShipment(new ShipmentEntity()));
        }

        [Fact]
        public void ReplaceInvalidAccount_SetsAccountID_WhenOneAccount()
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
        public void ReplaceInvalidAccount_SetsToFirstAccountID_WhenTwoAccounts()
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