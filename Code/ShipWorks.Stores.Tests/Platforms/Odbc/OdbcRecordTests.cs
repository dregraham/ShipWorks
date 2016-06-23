using ShipWorks.Stores.Platforms.Odbc;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc
{
    public class OdbcRecordTests
    {
        [Fact]
        public void GetValue_ReturnsNull_WhenValueNotFound()
        {
            var testObject = new OdbcRecord(string.Empty);
            var result = testObject.GetValue("test");

            Assert.Null(result);
        }

        [Fact]
        public void GetValue_ReturnsValue_WhenValueFound()
        {
            var testObject = new OdbcRecord(string.Empty);

            var objectToAdd = new object();
            testObject.AddField("test", objectToAdd);

            var result = testObject.GetValue("test");

            Assert.Equal(objectToAdd, result);
        }

        [Fact]
        public void HasValues_ReturnsTrue_WhenHasValues()
        {
            var testObject = new OdbcRecord(string.Empty);
            testObject.AddField("blah",null);
            Assert.True(testObject.HasValues);
        }

        [Fact]
        public void HasValues_ReturnsFalse_WhenHasNoValues()
        {
            var testObject = new OdbcRecord(string.Empty);
            Assert.False(testObject.HasValues);
        }
    }
}