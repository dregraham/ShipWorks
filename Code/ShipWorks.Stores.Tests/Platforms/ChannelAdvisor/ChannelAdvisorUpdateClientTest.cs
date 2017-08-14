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
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Content.CombinedOrderSearchProviders;
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
        private readonly OrderEntity combinedOrder;

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

            order = new OrderEntity { OrderNumber = 1234, CombineSplitStatus = CombineSplitStatusType.None };
            combinedOrder = new OrderEntity { OrderNumber = 1234, CombineSplitStatus = CombineSplitStatusType.Combined };

            Mock<ICombineOrderSearchProvider<long>> combinedSearchProvider = mock.Mock<ICombineOrderSearchProvider<long>>();
            combinedSearchProvider.Setup(st => st.GetOrderIdentifiers(order)).Returns(Task.FromResult(new[] { (long) 1234 }.AsEnumerable()));
            combinedSearchProvider.Setup(st => st.GetOrderIdentifiers(combinedOrder)).Returns(Task.FromResult(new[] { (long) 1234, 5678 }.AsEnumerable()));

            mock.Provide(combinedSearchProvider);
        }

        [Fact]
        public async Task UploadShipmentDetails_DelegatesToSoapClient_WhenNonCombinedOrderAndStoreHasEmptyRefreshToken()
        {
            var testObject = mock.Create<ChannelAdvisorUpdateClient>();
            await testObject.UploadShipmentDetails(store, shipment, order);

            soapClient.Verify(c=>c.UploadShipmentDetails(1234,shipment.ShippedDateUtc, "carrier", "class", "track this!"), Times.Once);
        }

        [Fact]
        public async Task UploadShipmentDetails_DoesNotDelegateToSoapClient_WhenNonCombinedOrderAndStoreHasRefreshToken()
        {
            store.RefreshToken = "ha";
            var testObject = mock.Create<ChannelAdvisorUpdateClient>();
            await testObject.UploadShipmentDetails(store, shipment, order);

            soapClient.Verify(c => c.UploadShipmentDetails(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task UploadShipmentDetails_DecryptsRefreshToken_WhenNonCombinedOrderAndStoreHasRefreshToken()
        {
            store.RefreshToken = "ha";

            var testObject = mock.Create<ChannelAdvisorUpdateClient>();
            await testObject.UploadShipmentDetails(store, shipment, order);

            encryptionProvider.Verify(p=>p.Decrypt("ha"), Times.Once);
        }

        [Fact]
        public async Task UploadShipmentDetails_GetsProperEncryptionProvider_WhenNonCombinedOrderAndStoreHasRefreshToken()
        {
            store.RefreshToken = "ha";

            var testObject = mock.Create<ChannelAdvisorUpdateClient>();
            await testObject.UploadShipmentDetails(store, shipment, order);

            encryptionProviderFactory.Verify(p => p.CreateSecureTextEncryptionProvider("ChannelAdvisor"), Times.Once);
        }

        [Fact]
        public async Task UploadShipmentDetails_DelegatesToRestClient_WhenNonCombinedOrderAndStoreHasRefreshToken()
        {
            store.RefreshToken = "ha";

            var testObject = mock.Create<ChannelAdvisorUpdateClient>();
            await testObject.UploadShipmentDetails(store, shipment, order).ConfigureAwait(false);

            mock.Mock<IChannelAdvisorRestClient>().Verify(c => c.UploadShipmentDetails(shipment, "EncryptedText", "1234"),
                Times.Once);
        }

        [Fact]
        public async Task UploadShipmentDetails_DoesNotDelegateToRestClient_WhenNonCombinedOrderAndStoreHasEmptyRefreshToken()
        {
            var testObject = mock.Create<ChannelAdvisorUpdateClient>();
            await testObject.UploadShipmentDetails(store, shipment, order);

            mock.Mock<IChannelAdvisorRestClient>().Verify(c => c.UploadShipmentDetails(
                It.IsAny<ChannelAdvisorShipment>(),
                It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task UploadShipmentDetails_DoesDelegateToRestClientOnce_WhenNonCombinedOrderAndNoCombinedOrderSearchEntities()
        {
            store.RefreshToken = "ha";
            
            var testObject = mock.Create<ChannelAdvisorUpdateClient>();
            await testObject.UploadShipmentDetails(store, shipment, order).ConfigureAwait(false);

            mock.Mock<IChannelAdvisorRestClient>().Verify(c => c.UploadShipmentDetails(
                It.IsAny<ChannelAdvisorShipment>(),
                It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task UploadShipmentDetails_DoesDelegateToSoapClientTwice_WhenNonCombinedOrderAndTwoCombinedOrderSearchEntities()
        {
            order.CombineSplitStatus = CombineSplitStatusType.Combined;
            var testObject = mock.Create<ChannelAdvisorUpdateClient>();
            await testObject.UploadShipmentDetails(store, shipment, order);

            soapClient.Verify(c => c.UploadShipmentDetails(1234, shipment.ShippedDateUtc, "carrier", "class", "track this!"), Times.Once);
        }

        [Fact]
        public async Task UploadShipmentDetails_DoesDelegateToSoapClientOnce_WhenNonCombinedOrderAndNoCombinedOrderSearchEntities()
        {
            var testObject = mock.Create<ChannelAdvisorUpdateClient>();
            await testObject.UploadShipmentDetails(store, shipment, order);

            soapClient.Verify(c => c.UploadShipmentDetails(1234, shipment.ShippedDateUtc, "carrier", "class", "track this!"), Times.Once);
        }
        
        [Fact]
        public async Task UploadShipmentDetails_DelegatesToSoapClient_WhenCombinedOrderAndStoreHasEmptyRefreshToken()
        {
            var testObject = mock.Create<ChannelAdvisorUpdateClient>();
            await testObject.UploadShipmentDetails(store, shipment, combinedOrder);

            soapClient.Verify(c => c.UploadShipmentDetails(1234, shipment.ShippedDateUtc, "carrier", "class", "track this!"), Times.Once);
        }

        [Fact]
        public async Task UploadShipmentDetails_DoesNotDelegateToSoapClient_WhenCombinedOrderAndStoreHasRefreshToken()
        {
            store.RefreshToken = "ha";
            var testObject = mock.Create<ChannelAdvisorUpdateClient>();
            await testObject.UploadShipmentDetails(store, shipment, combinedOrder);

            soapClient.Verify(c => c.UploadShipmentDetails(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task UploadShipmentDetails_DecryptsRefreshToken_WhenCombinedOrderAndStoreHasRefreshToken()
        {
            store.RefreshToken = "ha";

            var testObject = mock.Create<ChannelAdvisorUpdateClient>();
            await testObject.UploadShipmentDetails(store, shipment, combinedOrder);

            encryptionProvider.Verify(p => p.Decrypt("ha"), Times.Exactly(2));
        }

        [Fact]
        public async Task UploadShipmentDetails_GetsProperEncryptionProvider_WhenCombinedOrderAndStoreHasRefreshToken()
        {
            store.RefreshToken = "ha";

            var testObject = mock.Create<ChannelAdvisorUpdateClient>();
            await testObject.UploadShipmentDetails(store, shipment, combinedOrder);

            encryptionProviderFactory.Verify(p => p.CreateSecureTextEncryptionProvider("ChannelAdvisor"), Times.Once);
        }

        [Fact]
        public async Task UploadShipmentDetails_DelegatesToRestClient_WhenCombinedOrderAndStoreHasRefreshToken()
        {
            store.RefreshToken = "ha";

            var testObject = mock.Create<ChannelAdvisorUpdateClient>();
            await testObject.UploadShipmentDetails(store, shipment, order).ConfigureAwait(false);

            mock.Mock<IChannelAdvisorRestClient>().Verify(c => c.UploadShipmentDetails(shipment, "EncryptedText", "1234"),
                Times.Once);
        }

        [Fact]
        public async Task UploadShipmentDetails_DoesNotDelegateToRestClient_WhenCombinedOrderAndStoreHasEmptyRefreshToken()
        {
            var testObject = mock.Create<ChannelAdvisorUpdateClient>();
            await testObject.UploadShipmentDetails(store, shipment, combinedOrder);

            mock.Mock<IChannelAdvisorRestClient>().Verify(c => c.UploadShipmentDetails(
                It.IsAny<ChannelAdvisorShipment>(),
                It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task UploadShipmentDetails_DoesDelegateToRestClientOnce_WhenCombinedOrderAndNoCombinedOrderSearchEntities()
        {
            store.RefreshToken = "ha";

            var testObject = mock.Create<ChannelAdvisorUpdateClient>();
            await testObject.UploadShipmentDetails(store, shipment, order).ConfigureAwait(false);

            mock.Mock<IChannelAdvisorRestClient>().Verify(c => c.UploadShipmentDetails(
                It.IsAny<ChannelAdvisorShipment>(),
                It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task UploadShipmentDetails_DoesDelegateToSoapClientTwice_WhenCombinedOrderAndTwoCombinedOrderSearchEntities()
        {
            order.CombineSplitStatus = CombineSplitStatusType.Combined;
            var testObject = mock.Create<ChannelAdvisorUpdateClient>();
            await testObject.UploadShipmentDetails(store, shipment, combinedOrder);

            soapClient.Verify(c => c.UploadShipmentDetails(1234, shipment.ShippedDateUtc, "carrier", "class", "track this!"), Times.Once);
        }

        [Fact]
        public async Task UploadShipmentDetails_DoesDelegateToSoapClientOnce_WhenCombinedOrderAndNoCombinedOrderSearchEntities()
        {
            var testObject = mock.Create<ChannelAdvisorUpdateClient>();
            await testObject.UploadShipmentDetails(store, shipment, combinedOrder);

            soapClient.Verify(c => c.UploadShipmentDetails(1234, shipment.ShippedDateUtc, "carrier", "class", "track this!"), Times.Once);
        }

        [Fact]
        public async Task UploadShipmentDetails_DoesDelegateToRestClientTwice_WhenCombinedOrderAndTwoCombinedOrderSearchEntities()
        {
            store.RefreshToken = "ha";
            order.CombineSplitStatus = CombineSplitStatusType.Combined;

            var testObject = mock.Create<ChannelAdvisorUpdateClient>();
            await testObject.UploadShipmentDetails(store, shipment, combinedOrder).ConfigureAwait(false);

            mock.Mock<IChannelAdvisorRestClient>().Verify(c => c.UploadShipmentDetails(
                It.IsAny<ChannelAdvisorShipment>(),
                It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}