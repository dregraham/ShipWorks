using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.BestRate;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.BestRate
{
    [TestClass]
    public class FedExCounterRatesBrokerTest
    {
        private FedExCounterRatesBroker testObject;

        private Mock<ICarrierAccountRepository<FedExAccountEntity>> accountRepository;
        private Mock<FedExShipmentType> fedExShipmentType;
        private Mock<ICarrierSettingsRepository> settingsRepository;

        [TestInitialize]
        public void Initialize()
        {
            accountRepository = new Mock<ICarrierAccountRepository<FedExAccountEntity>>();

            settingsRepository = new Mock<ICarrierSettingsRepository>();

            fedExShipmentType = new Mock<FedExShipmentType>();
            fedExShipmentType.Setup(s => s.GetRates(It.IsAny<ShipmentEntity>())).Returns(new RateGroup(new List<RateResult>()));

            testObject = new FedExCounterRatesBroker(fedExShipmentType.Object, accountRepository.Object, settingsRepository.Object);
        }

        [TestMethod]
        public void GetBestRates_SetsSettingsRepository_Test()
        {
            ShipmentEntity shipment = new ShipmentEntity
            {
                FedEx = new FedExShipmentEntity()
            };

            List<BrokerException> brokerExceptions = new List<BrokerException>();
           
            testObject.GetBestRates(shipment, brokerExceptions);

            Assert.AreEqual(settingsRepository.Object, fedExShipmentType.Object.SettingsRepository);
        }
    }
}
