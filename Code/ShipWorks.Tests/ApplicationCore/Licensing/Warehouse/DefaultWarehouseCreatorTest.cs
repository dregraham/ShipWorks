using System;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.ApplicationCore.Settings.Warehouse;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing.Warehouse
{
    public class DefaultWarehouseCreatorTest : IDisposable
    {
        private readonly AutoMock mock;

        public DefaultWarehouseCreatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public async Task NeedsDefaultWarehouse_ReturnsFalse_WhenUserDoesNotHaveHubAccess()
        {
            mock.Mock<ILicenseService>().Setup(x => x.IsHub).Returns(false);

            var testObject = mock.Create<DefaultWarehouseCreator>();

            var result = await testObject.NeedsDefaultWarehouse();

            Assert.False(result.Value);
            Assert.Equal(result.Message, "User does not have access to hub");
        }

        [Fact]
        public async Task NeedsDefaultWarehouse_ReturnsFalse_WhenWarehouseIsAlreadyLinkedToDatabase()
        {
            mock.Mock<ILicenseService>().Setup(x => x.IsHub).Returns(true);
            var configurationEntity = mock.Mock<IConfigurationEntity>();
            configurationEntity.Setup(x => x.WarehouseID).Returns("foo");
            mock.Mock<IConfigurationData>().Setup(x => x.FetchReadOnly()).Returns(configurationEntity.Object);

            var testObject = mock.Create<DefaultWarehouseCreator>();

            var result = await testObject.NeedsDefaultWarehouse();

            Assert.False(result.Value);
            Assert.Equal(result.Message, "Customer already has warehouse linked to this database");
        }

        [Fact]
        public async Task NeedsDefaultWarehouse_ReturnsFalse_WhenGetWarehousesFails()
        {
            mock.Mock<ILicenseService>().Setup(x => x.IsHub).Returns(true);
            var configurationEntity = mock.Mock<IConfigurationEntity>();
            configurationEntity.SetupGet(x => x.WarehouseID).Returns(string.Empty);
            mock.Mock<IConfigurationData>().Setup(x => x.FetchReadOnly()).Returns(configurationEntity.Object);

            mock.Mock<IWarehouseSettingsApi>()
                .Setup(x => x.GetAllWarehouses())
                .Returns(Task.FromResult(GenericResult.FromError("foo", new WarehouseListDto())));

            var testObject = mock.Create<DefaultWarehouseCreator>();

            var result = await testObject.NeedsDefaultWarehouse();

            Assert.False(result.Value);
            Assert.Equal(result.Message, "Failed to retrieve warehouses from hub");
        }

        [Fact]
        public async Task NeedsDefaultWarehouse_ReturnsFalse_WhenUserAlreadyHasWarehousesInTheHub()
        {
            mock.Mock<ILicenseService>().Setup(x => x.IsHub).Returns(true);
            var configurationEntity = mock.Mock<IConfigurationEntity>();
            configurationEntity.SetupGet(x => x.WarehouseID).Returns(string.Empty);
            mock.Mock<IConfigurationData>().Setup(x => x.FetchReadOnly()).Returns(configurationEntity.Object);

            mock.Mock<IWarehouseSettingsApi>()
                .Setup(x => x.GetAllWarehouses())
                .Returns(Task.FromResult(GenericResult.FromSuccess(new WarehouseListDto {count = 1})));

            var testObject = mock.Create<DefaultWarehouseCreator>();

            var result = await testObject.NeedsDefaultWarehouse();

            Assert.False(result.Value);
            Assert.Equal(result.Message, "Customer already has warehouses in the hub");
        }

        [Fact]
        public async Task NeedsDefaultWarehouse_ReturnsTrue_WhenAllCriteriaMet()
        {
            SetupNeedsDefaultWarehouseToReturnTrue();

            var testObject = mock.Create<DefaultWarehouseCreator>();

            var result = await testObject.NeedsDefaultWarehouse();

            Assert.True(result.Value);
        }

        [Fact]
        public async Task Create_ReturnsFailure_WhenNeedsDefaultWarehouseReturnsFalse()
        {
            mock.Mock<ILicenseService>().Setup(x => x.IsHub).Returns(false);

            IStoreEntity store = new StoreEntity();

            var testObject = mock.Create<DefaultWarehouseCreator>();

            var result = await testObject.Create(store);

            Assert.True(result.Failure);
        }

        [Fact]
        public async Task Create_ReturnsFailure_WhenCreatingWarehouseFails()
        {
            SetupNeedsDefaultWarehouseToReturnTrue();

            mock.Mock<IWarehouseSettingsApi>().Setup(x => x.Create(It.IsAny<Details>()))
                .ReturnsAsync(GenericResult.FromError<string>("foo"));

            IStoreEntity store = new StoreEntity();

            var testObject = mock.Create<DefaultWarehouseCreator>();

            var result = await testObject.Create(store);

            Assert.True(result.Failure);
            Assert.Equal("Failed to create default warehouse in the hub", result.Message);
        }

        [Fact]
        public async Task Create_DoesNotTryToLink_WhenCreatingWarehouseFails()
        {
            SetupNeedsDefaultWarehouseToReturnTrue();

            var settingsApi = mock.Mock<IWarehouseSettingsApi>();
            settingsApi.Setup(x => x.Create(It.IsAny<Details>()))
                .ReturnsAsync(GenericResult.FromError<string>("foo"));

            IStoreEntity store = new StoreEntity();

            var testObject = mock.Create<DefaultWarehouseCreator>();

            await testObject.Create(store);

            settingsApi.Verify(x => x.Link(It.IsAny<string>()), Times.Never());
        }

        [Fact]
        public async Task Create_ReturnsFailure_WhenLinkingWarehouseFails()
        {
            SetupNeedsDefaultWarehouseToReturnTrue();

            var settingsApi = mock.Mock<IWarehouseSettingsApi>();
            settingsApi.Setup(x => x.Create(It.IsAny<Details>()))
                .ReturnsAsync(GenericResult.FromSuccess("foo"));
            settingsApi.Setup(x => x.Link("foo"))
                .ReturnsAsync(Result.FromError("foo"));

            IStoreEntity store = new StoreEntity();

            var testObject = mock.Create<DefaultWarehouseCreator>();

            var result = await testObject.Create(store);

            Assert.True(result.Failure);
            Assert.Equal("Failed to link default warehouse to this database", result.Message);
        }

        [Fact]
        public async Task Create_ReturnsSuccess_WhenWarehouseCreatesAndLinksSuccessfully()
        {
            SetupNeedsDefaultWarehouseToReturnTrue();

            var settingsApi = mock.Mock<IWarehouseSettingsApi>();
            settingsApi.Setup(x => x.Create(It.IsAny<Details>()))
                .ReturnsAsync(GenericResult.FromSuccess("foo"));
            settingsApi.Setup(x => x.Link("foo"))
                .ReturnsAsync(Result.FromSuccess);

            IStoreEntity store = new StoreEntity();

            var testObject = mock.Create<DefaultWarehouseCreator>();

            var result = await testObject.Create(store);

            Assert.True(result.Success);
        }

        private void SetupNeedsDefaultWarehouseToReturnTrue()
        {
            mock.Mock<ILicenseService>().Setup(x => x.IsHub).Returns(true);
            var configurationEntity = mock.Mock<IConfigurationEntity>();
            configurationEntity.SetupGet(x => x.WarehouseID).Returns(string.Empty);
            mock.Mock<IConfigurationData>().Setup(x => x.FetchReadOnly()).Returns(configurationEntity.Object);

            mock.Mock<IWarehouseSettingsApi>()
                .Setup(x => x.GetAllWarehouses())
                .Returns(Task.FromResult(GenericResult.FromSuccess(new WarehouseListDto {count = 0})));
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}
