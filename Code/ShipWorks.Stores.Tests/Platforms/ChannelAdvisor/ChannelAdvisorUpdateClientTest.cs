using System;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Interapptive.Shared.Security;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.ChannelAdvisor;
using ShipWorks.Stores.Platforms.ChannelAdvisor.DTO;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.ChannelAdvisor
{
    public class ChannelAdvisorUpdateClientTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly ChannelAdvisorStoreEntity store;
        private readonly ChannelAdvisorShipment shipment;
        private readonly Mock<IChannelAdvisorSoapClient> soapClient;
        private readonly Mock<IEncryptionProvider> encryptionProvider;
        private readonly Mock<IEncryptionProviderFactory> encryptionProviderFactory;

        public ChannelAdvisorUpdateClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            store = new ChannelAdvisorStoreEntity();
            shipment = new ChannelAdvisorShipment()
            {
                ShippedDateUtc = new DateTime(2017, 1, 1),
                ShippingCarrier = "carrier",
                ShippingClass = "class",
                TrackingNumber = "track this!"
            };
            soapClient = mock.CreateMock<IChannelAdvisorSoapClient>();

            mock.MockFunc<ChannelAdvisorStoreEntity, IChannelAdvisorSoapClient>(soapClient);

            encryptionProvider = mock.CreateMock<IEncryptionProvider>();
            encryptionProvider.Setup(p => p.Decrypt(It.IsAny<string>())).Returns("EncryptedText");

            encryptionProviderFactory = mock.Mock<IEncryptionProviderFactory>();
            encryptionProviderFactory.Setup(e => e.CreateSecureTextEncryptionProvider("ChannelAdvisor"))
                .Returns(encryptionProvider.Object);
        }

        [Fact]
        public void UploadShipmentDetails_DelegatesToSoapClient_WhenStoreHasEmptyRefreshToken()
        {
            var testObject = mock.Create<ChannelAdvisorUpdateClient>();
            testObject.UploadShipmentDetails(store, shipment, 1234);

            soapClient.Verify(c=>c.UploadShipmentDetails(1234,shipment.ShippedDateUtc, "carrier", "class", "track this!"), Times.Once);
        }

        [Fact]
        public void UploadShipmentDetails_DoesNotDelegateToSoapClient_WhenStoreHasRefreshToken()
        {
            store.RefreshToken = "ha";
            var testObject = mock.Create<ChannelAdvisorUpdateClient>();
            testObject.UploadShipmentDetails(store, shipment, 1234);

            soapClient.Verify(c => c.UploadShipmentDetails(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void UploadShipmentDetails_DecryptsRefreshToken_WhenStoreHasRefreshToken()
        {
            store.RefreshToken = "ha";

            var testObject = mock.Create<ChannelAdvisorUpdateClient>();
            testObject.UploadShipmentDetails(store, shipment, 1234);

            encryptionProvider.Verify(p=>p.Decrypt("ha"), Times.Once);
        }

        [Fact]
        public void UploadShipmentDetails_GetsProperEncryptionProvider_WhenStoreHasRefreshToken()
        {
            store.RefreshToken = "ha";

            var testObject = mock.Create<ChannelAdvisorUpdateClient>();
            testObject.UploadShipmentDetails(store, shipment, 1234);

            encryptionProviderFactory.Verify(p => p.CreateSecureTextEncryptionProvider("ChannelAdvisor"), Times.Once);
        }

        [Fact]
        public void UploadShipmentDetails_DelegatesToRestClient_WhenStoreHasRefreshToken()
        {
            store.RefreshToken = "ha";

            var testObject = mock.Create<ChannelAdvisorUpdateClient>();
            testObject.UploadShipmentDetails(store, shipment, 1234);

            mock.Mock<IChannelAdvisorRestClient>().Verify(c => c.UploadShipmentDetails(shipment, "EncryptedText", "1234"),
                Times.Once);
        }

        [Fact]
        public void UploadShipmentDetails_DoesNotDelegateToRestClient_WhenStoreHasEmptyRefreshToken()
        {
            var testObject = mock.Create<ChannelAdvisorUpdateClient>();
            testObject.UploadShipmentDetails(store, shipment, 1234);

            mock.Mock<IChannelAdvisorRestClient>().Verify(c => c.UploadShipmentDetails(
                It.IsAny<ChannelAdvisorShipment>(),
                It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}