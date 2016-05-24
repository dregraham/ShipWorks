using System;
using System.Collections.Generic;
using System.IO;
using Autofac;
using Autofac.Core.Activators.Reflection;
using Autofac.Extras.Moq;
using Interapptive.Shared.Security;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using ShipWorks.Stores.UI.Platforms.Odbc;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc
{
    public class OdbcImportFieldMappingControlViewModelTest
    {
        [Fact]
        public void Load_ThrowsArgumentNullException_WhenStoreIsNull()
        {
            using (var mock = AutoMock.GetLoose())
            {
                OdbcImportFieldMappingControlViewModel testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();
                Assert.Throws<ArgumentNullException>(() => testObject.Load(null));
            }
        }

        [Fact]
        public void Load_LoadsSchemaTables()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<OdbcStoreEntity> store = new Mock<OdbcStoreEntity>();
                OdbcImportFieldMappingControlViewModel testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();
                testObject.Load(store.Object);
                Assert.NotNull(testObject.Tables);
            }
        }

        [Fact]
        public void Load_DisplaysErrorMessage_WhenShipWorksOdbcExceptionIsThrown()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<OdbcStoreEntity> store = new Mock<OdbcStoreEntity>();
                Mock<IOdbcSchema> shema = mock.Mock<IOdbcSchema>();
                Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
                Mock<IMessageHelper> messageHelper = mock.Mock<IMessageHelper>();
                shema.Setup(s => s.Load(dataSource.Object)).Throws<ShipWorksOdbcException>();
                OdbcImportFieldMappingControlViewModel testObject =
                    mock.Create<OdbcImportFieldMappingControlViewModel>();

                testObject.Load(store.Object);

                messageHelper.Verify(m => m.ShowError(It.IsAny<string>()), Times.Once);
            }
        }

        [Fact]
        public void Save_ThrowsArgumentNullException_WhenStoreIsNull()
        {
            using (var mock = AutoMock.GetLoose())
            {
                OdbcImportFieldMappingControlViewModel testObject = mock.Create<OdbcImportFieldMappingControlViewModel>();
                Assert.Throws<ArgumentNullException>(() => testObject.Save(null));
            }
        }
    }
}