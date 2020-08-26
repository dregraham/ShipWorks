using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.Stores.Management;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Tests.ApplicationCore.Licensing.Warehouse
{
    public class HubMigratorTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly Mock<IStoreManager> storeManager;
        private readonly Mock<StoreType> storeType;
        private readonly Mock<IStoreTypeManager> storeTypeManager;
        private readonly Mock<IMessageHelper> messageHelper;
        private readonly Mock<IWarehouseStoreClient> warehouseStoreClient;
        private readonly StoreEntity store;

        public HubMigratorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            storeManager = mock.Mock<IStoreManager>();
            storeType = mock.Mock<StoreType>();
            storeTypeManager = mock.Mock<IStoreTypeManager>();
            messageHelper = mock.Mock<IMessageHelper>();
            warehouseStoreClient = mock.Mock<IWarehouseStoreClient>();
            store = new StoreEntity();
        }

        [Fact]
        public async Task MigrateStores_PromptsUserToMigrate_WhenThereAreStoresToMigrate()
        {
            SetupSuccessfulMigration();

            var testObject = mock.Create<HubMigrator>();
            await testObject.MigrateStores();

            messageHelper.Verify(x => x.ShowQuestion(AnyString), Times.Once);
        }

        [Fact]
        public async Task MigrateStores_DoesNotPromptUser_WhenThereAreNoStoresToMigrate()
        {
            storeManager.Setup(x => x.GetAllStores()).Returns(new List<StoreEntity> {store});
            storeType.Setup(x => x.ShouldUseHub(store)).Returns(false);
            storeTypeManager.Setup(x => x.GetType(store))
                .Returns(storeType);

            var testObject = mock.Create<HubMigrator>();
            await testObject.MigrateStores();

            messageHelper.Verify(x => x.ShowQuestion(AnyString), Times.Never);
        }

        [Fact]
        public async Task MigrateStores_PerformsMigration_WhenUserAnswersPromptWithOK()
        {
            SetupSuccessfulMigration();

            var testObject = mock.Create<HubMigrator>();
            await testObject.MigrateStores();

            warehouseStoreClient.Verify(x => x.UploadStoreToWarehouse(store), Times.Once);
        }

        [Fact]
        public async Task MigrateStores_DoesNotPerformMigration_WhenUserAnswersPromptWithCancel()
        {
            storeManager.Setup(x => x.GetAllStores()).Returns(new List<StoreEntity> {store});
            storeType.Setup(x => x.ShouldUseHub(store)).Returns(true);
            storeTypeManager.Setup(x => x.GetType(store))
                .Returns(storeType);
            messageHelper.Setup(x => x.ShowQuestion(AnyString)).Returns(DialogResult.Cancel);
            warehouseStoreClient.Setup(x => x.UploadStoreToWarehouse(store))
                .ReturnsAsync(Result.FromSuccess());

            var testObject = mock.Create<HubMigrator>();
            await testObject.MigrateStores();

            warehouseStoreClient.Verify(x => x.UploadStoreToWarehouse(store), Times.Never);
        }

        [Fact]
        public async Task MigrateStores_SavesStore_WhenStoreUploadsSuccessfully()
        {
            SetupSuccessfulMigration();

            var testObject = mock.Create<HubMigrator>();
            await testObject.MigrateStores();

            storeManager.Verify(x => x.SaveStore(store), Times.Once);
        }

        [Fact]
        public async Task MigrateStores_ShowsSuccessMessage_WhenStoreUploadsSuccessfully()
        {
            SetupSuccessfulMigration();

            var testObject = mock.Create<HubMigrator>();
            await testObject.MigrateStores();

            messageHelper.Verify(x => x.ShowInformation(AnyString));
        }


        [Fact]
        public async Task MigrateStores_DoesNotSaveStore_WhenStoreUploadFails()
        {
            storeManager.Setup(x => x.GetAllStores()).Returns(new List<StoreEntity> {store});
            storeType.Setup(x => x.ShouldUseHub(store)).Returns(true);
            storeTypeManager.Setup(x => x.GetType(store))
                .Returns(storeType);
            messageHelper.Setup(x => x.ShowQuestion(AnyString)).Returns(DialogResult.OK);
            warehouseStoreClient.Setup(x => x.UploadStoreToWarehouse(store))
                .ReturnsAsync(Result.FromError("error"));

            var testObject = mock.Create<HubMigrator>();
            await testObject.MigrateStores();

            storeManager.Verify(x => x.SaveStore(store), Times.Never);
        }

        [Fact]
        public async Task MigrateStores_ShowsErrorMessage_WhenStoreUploadFails()
        {
            storeManager.Setup(x => x.GetAllStores()).Returns(new List<StoreEntity> {store});
            storeType.Setup(x => x.ShouldUseHub(store)).Returns(true);
            storeTypeManager.Setup(x => x.GetType(store))
                .Returns(storeType);
            messageHelper.Setup(x => x.ShowQuestion(AnyString)).Returns(DialogResult.OK);
            warehouseStoreClient.Setup(x => x.UploadStoreToWarehouse(store))
                .ReturnsAsync(Result.FromError("error"));

            var testObject = mock.Create<HubMigrator>();
            await testObject.MigrateStores();

            messageHelper.Verify(x => x.ShowError(AnyString));
        }

        private void SetupSuccessfulMigration()
        {
            storeManager.Setup(x => x.GetAllStores()).Returns(new List<StoreEntity> {store});
            storeType.Setup(x => x.ShouldUseHub(store)).Returns(true);
            storeTypeManager.Setup(x => x.GetType(store))
                .Returns(storeType);
            messageHelper.Setup(x => x.ShowQuestion(AnyString)).Returns(DialogResult.OK);
            warehouseStoreClient.Setup(x => x.UploadStoreToWarehouse(store))
                .ReturnsAsync(Result.FromSuccess());
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
