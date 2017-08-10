using Autofac;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms.Choxi;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.Stores.NoMoreRack
{
    public class IdentifierTests
    {
        private readonly AutoMock mock;

        public IdentifierTests()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        /// <summary>
        /// Check that the given url is the desired identifier
        /// </summary>
        private void DoTest(string url)
        {
            DoTest(url, "staging.vendor-api.nomorerack.com/api/v1/shipworks/60", "1.0.0.0");
        }

        /// <summary>
        /// Check that the given url is the desired identifier
        /// </summary>
        private void DoTest(string url, string identifier, string schemaVersion)
        {
            GenericModuleStoreEntity nmrStore = new GenericModuleStoreEntity();
            nmrStore.ModuleUrl = url;
            nmrStore.TypeCode = (int) StoreTypeCode.Choxi;
            nmrStore.SchemaVersion = schemaVersion;

            StoreType storeType = mock.Create<ChoxiStoreType>(TypedParameter.From<StoreEntity>(nmrStore));

            Assert.Equal(identifier, storeType.LicenseIdentifier);
        }

        [Fact]
        public void InternalLicenseIdentifier_ReturnsNoMoreRack_WhenModuleUrlIsChoxi()
        {
            DoTest("http://staging.vendor-api.choxi.com/api/v1/shipworks/60");
        }

        [Fact]
        public void InternalLicenseIdentifier_ReturnsNoMoreRack_WhenModuleUrlIsNoMoreRack()
        {
            DoTest("http://staging.vendor-api.nomorerack.com/api/v1/shipworks/60");
        }
    }
}
