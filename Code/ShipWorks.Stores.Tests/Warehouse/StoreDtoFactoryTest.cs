using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.Stores.Warehouse;
using ShipWorks.Stores.Warehouse.StoreData;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

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
            amazonStoreEntity.MerchantToken = "merchant-token";
            amazonStoreEntity.AmazonApiRegion = "US";
            amazonStoreEntity.MarketplaceID = "marketplaceid";
            amazonStoreEntity.MerchantID = "merchantid";
            amazonStoreEntity.ExcludeFBA = true;
            amazonStoreEntity.AmazonVATS = false;
            amazonStoreEntity.StoreTypeCode = StoreTypeCode.Amazon;

            var downloadStartingPoint = mock.Mock<IDownloadStartingPoint>();
            downloadStartingPoint.Setup(x => x.OnlineLastModified(amazonStoreEntity)).ReturnsAsync(DateTime.Now);

            var testObject = mock.Create<AmazonStoreDtoFactory>();
            mock.Mock<IStoreTypeManager>()
                .Setup(x => x.GetType(AnyStore))
                .Returns(mock.Create<AmazonStoreType>(TypedParameter.From<StoreEntity>(amazonStoreEntity)));

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