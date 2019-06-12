using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Security;
using Moq;
using Newtonsoft.Json;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.Stores.Warehouse;
using ShipWorks.Stores.Warehouse.Encryption;
using ShipWorks.Stores.Warehouse.StoreData;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Warehouse
{
    public class StoreDtoFactoryTest : IDisposable
    {
        private readonly AutoMock mock;

        public StoreDtoFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public async Task Create_CreatesStoreDataFromStoreEntity()
        {
            AmazonStoreEntity amazonStoreEntity = new AmazonStoreEntity();
            amazonStoreEntity.AuthToken = "authtoken";
            amazonStoreEntity.AmazonApiRegion = "US";
            amazonStoreEntity.MarketplaceID = "marketplaceid";
            amazonStoreEntity.MerchantID = "merchantid";
            amazonStoreEntity.ExcludeFBA = true;
            amazonStoreEntity.AmazonVATS = false;
            amazonStoreEntity.StoreTypeCode = StoreTypeCode.Amazon;
            amazonStoreEntity.MerchantToken = "blah";

            var downloadStartingPoint = mock.Mock<IDownloadStartingPoint>();
            downloadStartingPoint.Setup(x => x.OnlineLastModified(amazonStoreEntity)).ReturnsAsync(DateTime.Now);

            var encryptionProviderFactory = mock.Mock<IEncryptionProviderFactory>();

            var storeTypeManager = mock.Mock<IStoreTypeManager>();
            storeTypeManager.Setup(s => s.GetType(amazonStoreEntity))
                .Returns(mock.Create<AmazonStoreType>(new TypedParameter(typeof(StoreEntity), amazonStoreEntity)));

            var encryptionService = mock.Mock<IWarehouseEncryptionService>();

            StoreDtoFactory testObject = new StoreDtoFactory(downloadStartingPoint.Object, storeTypeManager.Object, encryptionService.Object,encryptionProviderFactory.Object);

            var storeDto = await testObject.Create(amazonStoreEntity);

            Assert.Equal((int) StoreTypeCode.Amazon, storeDto.StoreType);
            Assert.IsAssignableFrom<AmazonStore>(storeDto);
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}