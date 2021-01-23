using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Tests.Shared;
using ShipWorks.Users;
using ShipWorks.Users.Logon;
using ShipWorks.Warehouse.Configuration.Stores;
using ShipWorks.Warehouse.Configuration.Stores.DTO;
using Xunit;

namespace ShipWorks.Tests.Warehouse.Configuration
{
    public class HubStoreSynchronizerTest
    {
        private AutoMock mock;
        public HubStoreSynchronizerTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void SynchronizeStoresIfNeeded_CallsSynchronizeStores_WhenNeeded()
        {
            string licenseId = "test";
            var client = mock.Mock<IWarehouseStoreClient>();
            var storeManager = mock.Mock<IStoreManager>();
            storeManager.Setup(x => x.GetAllStores()).Returns(new List<StoreEntity>
            {
                new StoreEntity()
            });
            var typeManager = mock.Mock<IStoreTypeManager>();
            typeManager.Setup(x => x.GetType(It.IsAny<StoreEntity>())).Returns(new FakeStoreType(licenseId));

            var sync = new HubStoreSynchronizer(storeManager.Object, typeManager.Object, client.Object);

            var configs = new List<StoreConfiguration>
            {
                new StoreConfiguration
                {
                    UniqueIdentifier = "NotTest"
                }
            };
            sync.SynchronizeStoresIfNeeded(configs);
            client.Verify(x => x.SynchronizeStores(It.IsAny<StoreSynchronizationRequest>()));
        }

        [Fact]
        public void SynchronizeStoresIfNeeded_DoesntCallSynchronizeStores_WhenNotNeeded()
        {
            string licenseId = "test";
            var client = mock.Mock<IWarehouseStoreClient>();
            var storeManager = mock.Mock<IStoreManager>();
            storeManager.Setup(x => x.GetAllStores()).Returns(new List<StoreEntity>
            {
                new StoreEntity()
            });
            var typeManager = mock.Mock<IStoreTypeManager>();
            typeManager.Setup(x => x.GetType(It.IsAny<StoreEntity>())).Returns(new FakeStoreType(licenseId));

            var sync = new HubStoreSynchronizer(storeManager.Object, typeManager.Object, client.Object);

            var configs = new List<StoreConfiguration>
            {
                new StoreConfiguration
                {
                    UniqueIdentifier = licenseId
                }
            };
            sync.SynchronizeStoresIfNeeded(configs);
            client.Verify(x => x.SynchronizeStores(It.IsAny<StoreSynchronizationRequest>()), Times.Never);
        }
    }

    public class FakeStoreType : StoreType
    {
        public FakeStoreType(string license)
        {
            InternalLicenseIdentifier = license;
        }

        protected override string InternalLicenseIdentifier { get; }

        public override StoreTypeCode TypeCode { get; }

        public override OrderIdentifier CreateOrderIdentifier(IOrderEntity order) => null;

        public override StoreEntity CreateStoreInstance() => null;
    }
}
