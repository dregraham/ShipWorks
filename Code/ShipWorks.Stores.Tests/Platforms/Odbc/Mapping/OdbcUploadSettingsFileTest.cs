using Autofac.Extras.Moq;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Mapping
{
    public class OdbcUploadSettingsFileTest
    {
        [Fact]
        public void Action_IsUpload()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = mock.Create<OdbcUploadSettingsFile>();
                Assert.Equal("Upload", testObject.Action);
            }
        }

        [Fact]
        public void Extension_IsSwoum()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = mock.Create<OdbcUploadSettingsFile>();
                Assert.Equal(".swoum", testObject.Extension);
            }
        }
    }
}
