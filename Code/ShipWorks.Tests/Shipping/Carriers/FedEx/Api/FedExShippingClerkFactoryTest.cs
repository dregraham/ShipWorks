using System.Collections.Generic;
using Interapptive.Shared.Net;
using log4net;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Fims;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api
{
    public class FedExShippingClerkFactoryTest
    {
        private Mock<ICarrierSettingsRepository> settingsRepository;
        private Mock<ICertificateInspector> certificateInspector;
        private Mock<ICertificateRequest> certificateRequest;
        private Mock<IFedExRequestFactory> requestFactory;
        private Mock<ILog> log;
        private Mock<ILabelRepository> labelRepository;
        private ShipmentEntity shipmentEntity;

        public FedExShippingClerkFactoryTest()
        {
            log = new Mock<ILog>();
            log.Setup(l => l.Info(It.IsAny<string>()));
            log.Setup(l => l.Error(It.IsAny<string>()));

            settingsRepository = new Mock<ICarrierSettingsRepository>();
            settingsRepository.Setup(r => r.GetAccounts()).Returns
                (
                    new List<FedExAccountEntity>()
                    {
                        new FedExAccountEntity() {MeterNumber = "123"},
                        new FedExAccountEntity() {MeterNumber = "456"},
                        new FedExAccountEntity() {MeterNumber = "789"}
                    }
                );
            settingsRepository.Setup(r => r.UseTestServer).Returns(false);

            // Return a FedEx account that has been migrated
            settingsRepository.Setup(r => r.GetAccount(It.IsAny<ShipmentEntity>())).Returns(new FedExAccountEntity() {MeterNumber =  "123"});

            certificateInspector = new Mock<ICertificateInspector>();
            certificateInspector.Setup(i => i.Inspect(It.IsAny<ICertificateRequest>())).Returns(CertificateSecurityLevel.Trusted);

            certificateRequest = new Mock<ICertificateRequest>();
            certificateRequest.Setup(r => r.Submit()).Returns(CertificateSecurityLevel.Trusted);


            requestFactory = new Mock<IFedExRequestFactory>();
            requestFactory.Setup(f => f.CreateCertificateRequest(It.IsAny<ICertificateInspector>())).Returns(certificateRequest.Object);

            labelRepository = new Mock<ILabelRepository>();
            labelRepository.Setup(f => f.ClearReferences(It.IsAny<ShipmentEntity>()));

            shipmentEntity = BuildFedExShipmentEntity.SetupBaseShipmentEntity();
            shipmentEntity.FedEx.SmartPostHubID = "5571";
        }

        [Fact]
        public void FedExShippingClerkReturned_WhenNullShipmentRequested_Test()
        {
            IFedExShippingClerk shippingClerk = new FedExShippingClerkFactory().CreateShippingClerk(null, settingsRepository.Object);

            Assert.True(shippingClerk is FedExShippingClerk);
        }

        [Fact]
        public void FakeFimsShippingClerkReturned_WhenUseTestServerAndFimsShipmentRequested_Test()
        {
            settingsRepository.Setup(s => s.UseTestServer).Returns(true);
            shipmentEntity.FedEx.Service = (int) FedExServiceType.FedExFims;
            IFedExShippingClerk shippingClerk = new FedExShippingClerkFactory().CreateShippingClerk(shipmentEntity, settingsRepository.Object);

            Assert.True(shippingClerk is FimsShippingClerk);

            shipmentEntity.FedEx.Service = (int)FedExServiceType.FedExFims;
            shippingClerk = new FedExShippingClerkFactory().CreateShippingClerk(shipmentEntity, settingsRepository.Object);

            Assert.True(shippingClerk is FimsShippingClerk);
        }
    }
}
