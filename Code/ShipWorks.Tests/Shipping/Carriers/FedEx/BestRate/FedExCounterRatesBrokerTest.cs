using System.Collections.Generic;
using Interapptive.Shared.Net;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.BestRate;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.BestRate
{
    public class FedExCounterRatesBrokerTest
    {
        private FedExCounterRatesBroker testObject;

        private Mock<ICarrierAccountRepository<FedExAccountEntity>> accountRepository;
        private Mock<FedExShipmentType> fedExShipmentType;
        private Mock<ICarrierSettingsRepository> settingsRepository;
        private Mock<ICredentialStore> credentialStore;
        private Mock<ICertificateInspector> certificateInspector;

        [TestInitialize]
        public void Initialize()
        {
            accountRepository = new Mock<ICarrierAccountRepository<FedExAccountEntity>>();

            settingsRepository = new Mock<ICarrierSettingsRepository>();
            credentialStore = new Mock<ICredentialStore>();
            certificateInspector = new Mock<ICertificateInspector>();

            fedExShipmentType = new Mock<FedExShipmentType>();
            fedExShipmentType.Setup(s => s.GetRates(It.IsAny<ShipmentEntity>())).Returns(new RateGroup(new List<RateResult>()));
            fedExShipmentType.SetupAllProperties();

            testObject = new FedExCounterRatesBroker(fedExShipmentType.Object, accountRepository.Object, settingsRepository.Object, certificateInspector.Object);
        }

        [Fact]
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

        [Fact]
        public void GetBestRates_SetsCertificateInspector_Test()
        {
            List<BrokerException> brokerExceptions = new List<BrokerException>();
            ShipmentEntity shipment = new ShipmentEntity
            {
                FedEx = new FedExShipmentEntity()
            };

            testObject.GetBestRates(shipment, brokerExceptions);

            Assert.AreEqual(certificateInspector.Object, testObject.ShipmentType.CertificateInspector);
        }

    }
}
