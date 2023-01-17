using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.Tests.Shared;
using ShipWorks.Users;
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
        private readonly Mock<IConfigurationData> configurationData;
        private readonly Mock<IUserSession> userSession;
        private readonly StoreEntity store;
        private readonly IWin32Window owner;

        public HubMigratorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            storeManager = mock.Mock<IStoreManager>();
            storeType = mock.Mock<StoreType>();
            storeTypeManager = mock.Mock<IStoreTypeManager>();
            messageHelper = mock.Mock<IMessageHelper>();
            configurationData = mock.Mock<IConfigurationData>();
            userSession = mock.Mock<IUserSession>();
            store = new StoreEntity { ShouldMigrate = true };
            owner = mock.Mock<IWin32Window>().Object;
        }

        [Fact]
        public void MigrateStores_PromptsUserToMigrate_WhenTheyAreAdminAndHaveALinkedWarehouseAndThereAreStoresToMigrate()
        {
            configurationData.Setup(x => x.FetchReadOnly()).Returns(new ConfigurationEntity {WarehouseID = "foo"});
            userSession.Setup(x => x.User).Returns(new UserEntity() {IsAdmin = true});
            storeManager.Setup(x => x.GetAllStores()).Returns(new List<StoreEntity> {store});
            storeType.Setup(x => x.ShouldUseHub(store)).Returns(true);
            storeTypeManager.Setup(x => x.GetType(store))
                .Returns(storeType);
            messageHelper.Setup(x => x.ShowQuestion(It.IsAny<IWin32Window>(), AnyString)).Returns(DialogResult.Cancel);

            var testObject = mock.Create<HubMigrator>();
            testObject.MigrateStores(owner);

            messageHelper.Verify(x => x.ShowQuestion(owner, AnyString), Times.Once);
        }

        [Fact]
        public void MigrateStores_DoesNotPromptUser_WhenThereIsNoWarehouseLinked()
        {
            configurationData.Setup(x => x.FetchReadOnly()).Returns(new ConfigurationEntity {WarehouseID = null});
            userSession.Setup(x => x.User).Returns(new UserEntity() {IsAdmin = true});
            storeManager.Setup(x => x.GetAllStores()).Returns(new List<StoreEntity> {store});
            storeType.Setup(x => x.ShouldUseHub(store)).Returns(false);
            storeTypeManager.Setup(x => x.GetType(store))
                .Returns(storeType);

            var testObject = mock.Create<HubMigrator>();
            testObject.MigrateStores(owner);

            messageHelper.Verify(x => x.ShowQuestion(owner, AnyString), Times.Never);
        }

        [Fact]
        public void MigrateStores_DoesNotPromptUser_WhenTheUserIsNotAnAdmin()
        {
            configurationData.Setup(x => x.FetchReadOnly()).Returns(new ConfigurationEntity {WarehouseID = "foo"});
            userSession.Setup(x => x.User).Returns(new UserEntity() {IsAdmin = false});
            storeManager.Setup(x => x.GetAllStores()).Returns(new List<StoreEntity> {store});
            storeType.Setup(x => x.ShouldUseHub(store)).Returns(false);
            storeTypeManager.Setup(x => x.GetType(store))
                .Returns(storeType);

            var testObject = mock.Create<HubMigrator>();
            testObject.MigrateStores(owner);

            messageHelper.Verify(x => x.ShowQuestion(owner, AnyString), Times.Never);
        }

        [Fact]
        public void MigrateStores_DoesNotPromptUser_WhenThereAreNoStoresToMigrate()
        {
            configurationData.Setup(x => x.FetchReadOnly()).Returns(new ConfigurationEntity {WarehouseID = "foo"});
            userSession.Setup(x => x.User).Returns(new UserEntity() {IsAdmin = true});
            storeManager.Setup(x => x.GetAllStores()).Returns(new List<StoreEntity> {store});
            storeType.Setup(x => x.ShouldUseHub(store)).Returns(false);
            storeTypeManager.Setup(x => x.GetType(store))
                .Returns(storeType);

            var testObject = mock.Create<HubMigrator>();
            testObject.MigrateStores(owner);

            messageHelper.Verify(x => x.ShowQuestion(owner, AnyString), Times.Never);
        }

        [Fact]
        public void MigrateStores_DoesNotPromptUser_WhenStoreIsSetToNotMigrate()
        {
            configurationData.Setup(x => x.FetchReadOnly()).Returns(new ConfigurationEntity { WarehouseID = "foo" });
            userSession.Setup(x => x.User).Returns(new UserEntity() { IsAdmin = true });
            storeManager.Setup(x => x.GetAllStores()).Returns(new List<StoreEntity> { store });
            storeType.Setup(x => x.ShouldUseHub(store)).Returns(true);
            storeTypeManager.Setup(x => x.GetType(store))
                .Returns(storeType);

            store.ShouldMigrate = false;

            var testObject = mock.Create<HubMigrator>();
            testObject.MigrateStores(owner);

            messageHelper.Verify(x => x.ShowQuestion(owner, AnyString), Times.Never);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
