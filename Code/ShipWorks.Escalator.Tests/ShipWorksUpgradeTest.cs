using System;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Escalator.Tests
{
    public class ShipWorksUpgradeTest
    {
        private readonly AutoMock mock;
        private readonly ShipWorksUpgrade testObject;

        public ShipWorksUpgradeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<ShipWorksUpgrade>();
        }

        [Fact]
        public async Task Upgrade_DelegatesToUpdaterWebClientWithVersion()
        {
            Version version = new Version(1, 1, 1);

            await testObject.Upgrade(version);

            mock.Mock<IUpdaterWebClient>().Verify(w => w.GetVersionToDownload(version));
        }
    }
}
