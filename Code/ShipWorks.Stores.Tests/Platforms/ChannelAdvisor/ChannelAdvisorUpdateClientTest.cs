using System;
using System.Linq;
using System.Threading.Tasks;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Security;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
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
        private readonly OrderEntity order;

        public ChannelAdvisorUpdateClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            store = new ChannelAdvisorStoreEntity()
            {
                StoreTypeCode = StoreTypeCode.ChannelAdvisor
            };

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

            order = new OrderEntity {OrderNumber = 1234};

            Mock<IChannelAdvisorStoreType> storeType = mock.Mock<IChannelAdvisorStoreType>();
            storeType.Setup(st => st.GetCombinedOnlineOrderIdentifiers(It.IsAny<IOrderEntity>())).Returns(Task.FromResult(new[] { 1234, 5678 }.AsEnumerable()));
            storeType.Setup(st => st.GetOnlineOrderIdentifier(It.IsAny<IOrderEntity>())).Returns(1234);
            
            mock.Provide(storeType);

            Mock<Func<StoreEntity, IChannelAdvisorStoreType>> getStoreType = new Mock<Func<StoreEntity, IChannelAdvisorStoreType>>();
            getStoreType.Setup(x => x(It.IsAny<ChannelAdvisorStoreEntity>())).Returns(storeType.Object);
            mock.Provide(getStoreType);
        }

        [Fact]
        public async Task UploadShipmentDetails_DelegatesToSoapClient_WhenStoreHasEmptyRefreshToken()
        {
            var testObject = mock.Create<ChannelAdvisorUpdateClient>();
            await testObject.UploadShipmentDetails(store, shipment, order);

            soapClient.Verify(c=>c.UploadShipmentDetails(1234,shipment.ShippedDateUtc, "carrier", "class", "track this!"), Times.Once);
        }

        [Fact]
        public async Task UploadShipmentDetails_DoesNotDelegateToSoapClient_WhenStoreHasRefreshToken()
        {
            store.RefreshToken = "ha";
            var testObject = mock.Create<ChannelAdvisorUpdateClient>();
            await testObject.UploadShipmentDetails(store, shipment, order);

            soapClient.Verify(c => c.UploadShipmentDetails(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task UploadShipmentDetails_DecryptsRefreshToken_WhenStoreHasRefreshToken()
        {
            store.RefreshToken = "ha";

            var testObject = mock.Create<ChannelAdvisorUpdateClient>();
            await testObject.UploadShipmentDetails(store, shipment, order);

            encryptionProvider.Verify(p=>p.Decrypt("ha"), Times.Once);
        }

        [Fact]
        public async Task UploadShipmentDetails_GetsProperEncryptionProvider_WhenStoreHasRefreshToken()
        {
            store.RefreshToken = "ha";

            var testObject = mock.Create<ChannelAdvisorUpdateClient>();
            await testObject.UploadShipmentDetails(store, shipment, order);

            encryptionProviderFactory.Verify(p => p.CreateSecureTextEncryptionProvider("ChannelAdvisor"), Times.Once);
        }

        [Fact]
        public async Task UploadShipmentDetails_DelegatesToRestClient_WhenStoreHasRefreshToken()
        {
            store.RefreshToken = "ha";

            var testObject = mock.Create<ChannelAdvisorUpdateClient>();
            await testObject.UploadShipmentDetails(store, shipment, order).ConfigureAwait(false);

            mock.Mock<IChannelAdvisorRestClient>().Verify(c => c.UploadShipmentDetails(shipment, "EncryptedText", "1234"),
                Times.Once);
        }

        [Fact]
        public async Task UploadShipmentDetails_DoesNotDelegateToRestClient_WhenStoreHasEmptyRefreshToken()
        {
            var testObject = mock.Create<ChannelAdvisorUpdateClient>();
            await testObject.UploadShipmentDetails(store, shipment, order);

            mock.Mock<IChannelAdvisorRestClient>().Verify(c => c.UploadShipmentDetails(
                It.IsAny<ChannelAdvisorShipment>(),
                It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task UploadShipmentDetails_DoesDelegateToRestClientOnce_WhenNoCombinedOrderSearchEntities()
        {
            store.RefreshToken = "ha";

            Mock<IChannelAdvisorStoreType> storeType = mock.Mock<IChannelAdvisorStoreType>();
            storeType.Setup(st => st.GetCombinedOnlineOrderIdentifiers(It.IsAny<IOrderEntity>())).Returns(Task.FromResult(Enumerable.Empty<int>()));
            storeType.Setup(st => st.GetOnlineOrderIdentifier(It.IsAny<IOrderEntity>())).Returns(1234);
            mock.Provide(storeType);

            var testObject = mock.Create<ChannelAdvisorUpdateClient>();
            await testObject.UploadShipmentDetails(store, shipment, order).ConfigureAwait(false);

            mock.Mock<IChannelAdvisorRestClient>().Verify(c => c.UploadShipmentDetails(
                It.IsAny<ChannelAdvisorShipment>(),
                It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task UploadShipmentDetails_DoesDelegateToRestClientTwice_WhenTwoCombinedOrderSearchEntities()
        {
            store.RefreshToken = "ha";
            order.CombineSplitStatus = CombineSplitStatusType.Combined;

            var testObject = mock.Create<ChannelAdvisorUpdateClient>();
            await testObject.UploadShipmentDetails(store, shipment, order).ConfigureAwait(false);

            mock.Mock<IChannelAdvisorRestClient>().Verify(c => c.UploadShipmentDetails(
                It.IsAny<ChannelAdvisorShipment>(),
                It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public async Task UploadShipmentDetails_DoesDelegateToSoapClientTwice_WhenTwoCombinedOrderSearchEntities()
        {
            order.CombineSplitStatus = CombineSplitStatusType.Combined;
            var testObject = mock.Create<ChannelAdvisorUpdateClient>();
            await testObject.UploadShipmentDetails(store, shipment, order);

            soapClient.Verify(c => c.UploadShipmentDetails(1234, shipment.ShippedDateUtc, "carrier", "class", "track this!"), Times.Once);
        }

        [Fact]
        public async Task UploadShipmentDetails_DoesDelegateToSoapClientOnce_WhenNoCombinedOrderSearchEntities()
        {
            Mock<IChannelAdvisorStoreType> storeType = mock.Mock<IChannelAdvisorStoreType>();
            storeType.Setup(st => st.GetCombinedOnlineOrderIdentifiers(It.IsAny<IOrderEntity>())).Returns(Task.FromResult(Enumerable.Empty<int>()));
            storeType.Setup(st => st.GetOnlineOrderIdentifier(It.IsAny<IOrderEntity>())).Returns(1234);
            mock.Provide(storeType);

            var testObject = mock.Create<ChannelAdvisorUpdateClient>();
            await testObject.UploadShipmentDetails(store, shipment, order);

            soapClient.Verify(c => c.UploadShipmentDetails(1234, shipment.ShippedDateUtc, "carrier", "class", "track this!"), Times.Once);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}