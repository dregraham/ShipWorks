using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.LicenseEnforcement;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing
{
    public class StoreLicenseTest
    {
        [Fact]
        public void EnforceCapabilities_ReturnsOneEnumResultComplianceLevel()
        {
            using (var mock = AutoMock.GetLoose())
            {
                StoreLicense storeLicense = mock.Create<StoreLicense>(new TypedParameter(typeof(StoreEntity), new StoreEntity()));

                IEnumerable<EnumResult<ComplianceLevel>> result = storeLicense.EnforceCapabilities(EditionFeature.ChannelCount, EnforcementContext.NotSpecified);

                Assert.Equal(1, result.Count());
            }
        }

        [Fact]
        public void EnforceCapabilities_ReturnsCompliant()
        {
            using (var mock = AutoMock.GetLoose())
            {
                StoreLicense storeLicense = mock.Create<StoreLicense>(new TypedParameter(typeof(StoreEntity), new StoreEntity()));

                IEnumerable<EnumResult<ComplianceLevel>> result = storeLicense.EnforceCapabilities(EditionFeature.ChannelCount, EnforcementContext.NotSpecified);

                Assert.Equal(ComplianceLevel.Compliant, result.FirstOrDefault()?.Value);
            }
        }
    }
}