using System;
using Autofac.Extras.Moq;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Administration.VersionSpecificUpdates;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.Data.Administration.VersionSpecificUpdates
{
    public class V_05_16_01_00_Test : IDisposable
    {
        readonly AutoMock mock;

        public V_05_16_01_00_Test()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void AppliesTo_ReturnsCorrectVersion()
        {
            var testObject = mock.Create<V_05_16_01_00>();
            var result = testObject.AppliesTo;
            Assert.Equal(new Version(5, 16, 1, 0), result);
        }

        [Fact]
        public void AlwaysRuns_ReturnsTrue()
        {
            var testObject = mock.Create<V_05_16_01_00>();
            var result = testObject.AlwaysRun;
            Assert.True(result);
        }

        [Fact]
        public void Update_DelegatesToSqlAdapterToFetchStores()
        {
            var sqlAdapter = mock.FromFactory<ISqlAdapterFactory>()
                .Mock(x => x.Create());
            sqlAdapter.Setup(x => x.FetchQueryAsync(It.IsAny<EntityQuery<OdbcStoreEntity>>()))
                .ReturnsAsync(mock.CreateMock<IEntityCollection2>().Object);
            var testObject = mock.Create<V_05_16_01_00>();

            testObject.Update();

            sqlAdapter.Verify(x => x.FetchQueryAsync(It.IsAny<EntityQuery<OdbcStoreEntity>>()), Times.Once());
        }

        [Fact]
        public void Update_DelegatesToStoreUpgrade_ForEachStore()
        {
            var store1 = new OdbcStoreEntity();
            var store2 = new OdbcStoreEntity();
            var collection = new OdbcStoreCollection { store1, store2 };

            mock.FromFactory<ISqlAdapterFactory>()
                .Mock(x => x.Create())
                .Setup(x => x.FetchQueryAsync<OdbcStoreEntity>(It.IsAny<EntityQuery<OdbcStoreEntity>>()))
                .ReturnsAsync(collection);
            var testObject = mock.Create<V_05_16_01_00>();

            testObject.Update();

            mock.Mock<IStoreUpgrade>().Verify(x => x.Upgrade(store1), Times.Once());
            mock.Mock<IStoreUpgrade>().Verify(x => x.Upgrade(store2), Times.Once());
        }

        [Fact]
        public void Update_DelegatesToSqlAdapter_ToSaveCollection()
        {
            var collection = mock.CreateMock<IEntityCollection2>().Object;
            var sqlAdapter = mock.FromFactory<ISqlAdapterFactory>()
                .Mock(x => x.Create());
            sqlAdapter.Setup(x => x.FetchQueryAsync(It.IsAny<EntityQuery<OdbcStoreEntity>>()))
                .ReturnsAsync(collection);
            var testObject = mock.Create<V_05_16_01_00>();

            testObject.Update();

            sqlAdapter.Verify(x => x.SaveEntityCollection(collection, true, false));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
