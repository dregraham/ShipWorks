using System;
using System.Data.Common;
using Autofac.Extras.Moq;
using log4net;
using Moq;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Filters;
using ShipWorks.Stores.Orders.Archive;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Orders.Archive
{
    public class OrderArchiveFilterRegeneratorTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly Mock<IConfigurationEntity> configuration;
        private readonly OrderArchiveFilterRegenerator testObject;

        public OrderArchiveFilterRegeneratorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            configuration = mock.Mock<IConfigurationEntity>();
            configuration.Setup(x => x.ArchivalSettingsXml)
                .Returns(needsRegenerationXml);

            mock.Mock<IConfigurationData>()
                .Setup(x => x.FetchReadOnly())
                .Returns(configuration);

            testObject = mock.Create<OrderArchiveFilterRegenerator>();
        }

        [Fact]
        public void Regenerate_DelegatesToConfigurationData_ToGetReadOnlyConfiguration()
        {
            testObject.Regenerate();

            mock.Mock<IConfigurationData>().Verify(x => x.FetchReadOnly());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("foo")]
        [InlineData("<stuff>")]
        public void Regenerate_LogsError_WhenConfigurationHasInvalidXml(string xml)
        {
            configuration
                .Setup(x => x.ArchivalSettingsXml)
                .Returns(xml);

            testObject.Regenerate();

            mock.Mock<ILog>()
                .Verify(x => x.Error("Could not regenerate filters", It.IsAny<Exception>()));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("foo")]
        [InlineData("<stuff>")]
        public void Regenerate_DoesNotOpenConnection_WhenNeedsFilterRegenerationIsInvalid(string xml)
        {
            configuration
                .Setup(x => x.ArchivalSettingsXml)
                .Returns(xml);

            testObject.Regenerate();

            mock.Mock<ISqlSession>()
                .Verify(x => x.OpenConnection(), Times.Never);
        }

        [Fact]
        public void Regenerate_DoesNotOpenConnection_WhenNeedsFilterRegenerationIsMissingRegenerationElement()
        {
            configuration
                .Setup(x => x.ArchivalSettingsXml)
                .Returns(doesNotNeedRegenerationXml);

            testObject.Regenerate();

            mock.Mock<ISqlSession>()
                .Verify(x => x.OpenConnection(), Times.Never);
        }

        [Fact]
        public void Regenerate_DoesNotOpenConnection_WhenNeedsFilterRegenerationIsMissingRegenerationFalse()
        {
            configuration
                .Setup(x => x.ArchivalSettingsXml)
                .Returns(missingRegenerationXml);

            testObject.Regenerate();

            mock.Mock<ISqlSession>()
                .Verify(x => x.OpenConnection(), Times.Never);
        }

        [Fact]
        public void Regenerate_OpensConnection_WhenFiltersNeedRegeneration()
        {
            testObject.Regenerate();

            mock.Mock<ISqlSession>().Verify(x => x.OpenConnection());
        }

        [Fact]
        public void Regenerate_DelegatesToFilterHelper_WhenFiltersNeedRegeneration()
        {
            var connection = mock.Build<DbConnection>();

            mock.Mock<ISqlSession>()
                .Setup(x => x.OpenConnection())
                .Returns(connection);

            testObject.Regenerate();

            mock.Mock<IFilterHelper>().Verify(x => x.RegenerateFilters(connection));
        }

        [Fact]
        public void Regenerate_UpdatesConfiguration_AfterFiltersAreRegenerated()
        {
            var config = new ConfigurationEntity();
            mock.Mock<IConfigurationData>()
                .Setup(x => x.UpdateConfiguration(It.IsAny<Action<ConfigurationEntity>>()))
                .Callback((Action<ConfigurationEntity> x) => x(config));

            testObject.Regenerate();

            Assert.Contains("<NeedsFilterRegeneration>false</NeedsFilterRegeneration>", config.ArchivalSettingsXml);
        }

        public void Dispose()
        {
            mock.Dispose();
        }

        private readonly string needsRegenerationXml =
@"<ArchivalSettings>
    <ArchivalSetting>
        <NeedsFilterRegeneration>true</NeedsFilterRegeneration>
    </ArchivalSetting>
</ArchivalSettings>";

        private readonly string doesNotNeedRegenerationXml =
@"<ArchivalSettings>
    <ArchivalSetting>
        <NeedsFilterRegeneration>false</NeedsFilterRegeneration>
    </ArchivalSetting>
</ArchivalSettings>";

        private readonly string missingRegenerationXml =
@"<ArchivalSettings>
    <ArchivalSetting>
    </ArchivalSetting>
</ArchivalSettings>";
    }
}
