using System;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using Newtonsoft.Json;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Warehouse;
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

            var downloadStartingPoint = mock.Mock<IDownloadStartingPoint>();
            downloadStartingPoint.Setup(x => x.OnlineLastModified(amazonStoreEntity)).ReturnsAsync(DateTime.Now);

            var storeTypeManager = mock.Mock<IStoreTypeManager>();

            StoreDtoFactory testObject = new StoreDtoFactory(downloadStartingPoint.Object, storeTypeManager.Object);

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