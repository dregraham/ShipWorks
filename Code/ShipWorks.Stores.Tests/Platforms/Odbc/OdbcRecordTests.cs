using Autofac.Extras.Moq;
using ShipWorks.Stores.Platforms.Odbc;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc
{
    public class OdbcRecordTests
    {
        [Fact]
        public void GetValue_ReturnsNull_WhenValueNotFound()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = mock.Create<OdbcRecord>();
                var result = testObject.GetValue("test");

                Assert.Null(result);
            }
        }

        [Fact]
        public void GetValue_ReturnsValue_WhenValueFound()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = mock.Create<OdbcRecord>();

                var objectToAdd = new object();
                testObject.AddField("test", objectToAdd);

                var result = testObject.GetValue("test");

                Assert.Equal(objectToAdd, result);
            }
        }
    }
}