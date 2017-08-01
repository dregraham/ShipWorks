using System;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Mapping
{
    public class OdbcStoreUpgradeTest : IDisposable
    {
        private readonly AutoMock mock;

        public OdbcStoreUpgradeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void Upgrade_DelegatesMapCreation_ToFieldMapFactory()
        {
            var store = new OdbcStoreEntity()
            {
                ImportMap = "Import Map",
                UploadMap = "Upload Map"
            };

            var testObject = mock.Create<OdbcStoreUpgrade>();
            testObject.Upgrade(store);

            mock.Mock<IOdbcFieldMapFactory>().Verify(f => f.CreateFieldMapFrom("Import Map"), Times.Once());
            mock.Mock<IOdbcFieldMapFactory>().Verify(f => f.CreateFieldMapFrom("Upload Map"), Times.Once());
        }

        [Fact]
        public void Upgrade_CallsUpgradetoAlphanumericOrderNumbers_ForImportMap()
        {
            var store = new OdbcStoreEntity()
            {
                ImportMap = "Import Map",
                UploadMap = "Upload Map"
            };

            var importFieldMapMock = mock.CreateMock<IOdbcFieldMap>();
            mock.Mock<IOdbcFieldMapFactory>()
                .Setup(f => f.CreateFieldMapFrom("Import Map"))
                .Returns(importFieldMapMock.Object);

            var testObject = mock.Create<OdbcStoreUpgrade>();
            testObject.Upgrade(store);

            importFieldMapMock.Verify(m=>m.UpgradeToAlphanumericOrderNumbers(), Times.Once());
        }

        [Fact]
        public void Upgrade_CallsUpgradetoAlphanumericOrderNumbers_ForUploadMap()
        {
            var store = new OdbcStoreEntity()
            {
                ImportMap = "Import Map",
                UploadMap = "Upload Map"
            };

            var uploadFieldMap = mock.CreateMock<IOdbcFieldMap>();
            mock.Mock<IOdbcFieldMapFactory>()
                .Setup(f => f.CreateFieldMapFrom("Upload Map"))
                .Returns(uploadFieldMap.Object);

            var testObject = mock.Create<OdbcStoreUpgrade>();
            testObject.Upgrade(store);

            uploadFieldMap.Verify(m => m.UpgradeToAlphanumericOrderNumbers(), Times.Once());
        }

        [Fact]
        public void Upgrade_DoesNothing_WhenStoreIsNotOdbc()
        {
            var testObject = mock.Create<OdbcStoreUpgrade>();
            testObject.Upgrade(new StoreEntity());

            mock.Mock<IOdbcFieldMapFactory>().Verify(f=>f.CreateFieldMapFrom(It.IsAny<string>()), Times.Never());
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}