using System;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.AutoUpdate;
using Moq;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Escalator.Tests
{
    public class ShipWorksUpgradeTest
    {
        private readonly AutoMock mock;
        private readonly ShipWorksUpgrade testObject;
        private readonly Mock<IUpdaterWebClient> updaterWebClient;

        public ShipWorksUpgradeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            updaterWebClient = mock.Mock<IUpdaterWebClient>();
            testObject = mock.Create<ShipWorksUpgrade>();
        }

        [Fact]
        public async Task Upgrade_DelegatesToUpdaterWebClientWithVersion()
        {
            Version version = new Version(1, 1, 1);

            await testObject.Upgrade(version);

            updaterWebClient.Verify(w => w.GetVersionToDownload(version));
        }

        [Fact]
        public async Task Upgrade_DelegatesToUpdaterWebClientCustomerID()
        {
            await testObject.Upgrade("1234");

            updaterWebClient.Verify(w => w.GetVersionToDownload("1234", It.IsAny<Version>()));
        }

        [Fact]
        public async Task Upgrade_DelegatesToUpdaterWebClientaCustomerID()
        {
            ShipWorksRelease shipworksRelease = new ShipWorksRelease()
            {
                ReleaseVersion = "9.9.9",
                DownloadUrl = "http://www.google.com",
                Hash = "hashbrowns"
            };

            updaterWebClient.Setup(w => w.GetVersionToDownload(It.IsAny<Version>()))
                .ReturnsAsync(shipworksRelease);

            await testObject.Upgrade(new Version(1, 1, 1));

            updaterWebClient.Verify(u => u.Download(shipworksRelease.DownloadUri, shipworksRelease.Hash));
        }
    }
}
