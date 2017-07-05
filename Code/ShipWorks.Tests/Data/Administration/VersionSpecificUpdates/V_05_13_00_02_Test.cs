using System;
using Autofac.Extras.Moq;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using SD.LLBLGen.Pro.QuerySpec.Adapter;
using ShipWorks.Data.Administration.VersionSpecificUpdates;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.ShopSite;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.Data.Administration.VersionSpecificUpdates
{
    public class V_05_13_00_02_Test : IDisposable
    {
        readonly AutoMock mock;

        public V_05_13_00_02_Test()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void AppliesTo_ReturnsCorrectVersion()
        {
            var testObject = mock.Create<V_05_13_00_02>();
            var result = testObject.AppliesTo;
            Assert.Equal(new Version(5, 13, 0, 2), result);
        }

        [Fact]
        public void AlwaysRuns_ReturnsFalse()
        {
            var testObject = mock.Create<V_05_13_00_02>();
            var result = testObject.AlwaysRun;
            Assert.False(result);
        }

        [Fact]
        public void Update_DelegatesToSqlAdapter_ToFetchStores()
        {
            var sqlAdapter = mock.FromFactory<ISqlAdapterFactory>()
                .Mock(x => x.Create());
            sqlAdapter.Setup(x => x.FetchQueryAsync(It.IsAny<EntityQuery<ShopSiteStoreEntity>>()))
                .ReturnsAsync(mock.CreateMock<IEntityCollection2>().Object);
            var testObject = mock.Create<V_05_13_00_02>();

            testObject.Update();

            sqlAdapter.Verify(x => x.FetchQueryAsync(It.IsAny<EntityQuery<ShopSiteStoreEntity>>()));
        }

        [Fact]
        public void Update_DelegatesToIdentifier_ForEachStore()
        {
            var store1 = new ShopSiteStoreEntity { ApiUrl = "Foo", Password = "pwd", Username = "pwd" };
            var store2 = new ShopSiteStoreEntity { ApiUrl = "Bar", Password = "pwd", Username = "pwd" };
            var collection = new ShopSiteStoreCollection { store1, store2 };

            mock.FromFactory<ISqlAdapterFactory>()
                .Mock(x => x.Create())
                .Setup(x => x.FetchQueryAsync(It.IsAny<EntityQuery<ShopSiteStoreEntity>>()))
                .ReturnsAsync(collection);
            var testObject = mock.Create<V_05_13_00_02>();

            testObject.Update();

            mock.Mock<IShopSiteIdentifier>().Verify(x => x.Set(store1, "Foo"));
            mock.Mock<IShopSiteIdentifier>().Verify(x => x.Set(store2, "Bar"));
        }

        [Fact]
        public void Update_DelegatesToSqlAdapter_ToSaveCollection()
        {
            var collection = mock.CreateMock<IEntityCollection2>().Object;
            var sqlAdapter = mock.FromFactory<ISqlAdapterFactory>()
                .Mock(x => x.Create());
            sqlAdapter.Setup(x => x.FetchQueryAsync(It.IsAny<EntityQuery<ShopSiteStoreEntity>>()))
                .ReturnsAsync(collection);
            var testObject = mock.Create<V_05_13_00_02>();

            testObject.Update();

            sqlAdapter.Verify(x => x.SaveEntityCollection(collection, true, false));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
