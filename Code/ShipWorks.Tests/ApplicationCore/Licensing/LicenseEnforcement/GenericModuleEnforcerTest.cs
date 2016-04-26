using Autofac.Extras.Moq;
using ShipWorks.ApplicationCore.Licensing.LicenseEnforcement;
using ShipWorks.Editions;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing.LicenseEnforcement
{
    public class GenericModuleEnforcerTest
    {
        [Fact]
        public void EditionFeature_ReturnsGenericFile()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = mock.Create<GenericModuleEnforcer>();

                Assert.Equal(EditionFeature.GenericModule, testObject.EditionFeature);
            }
        }
    }
}