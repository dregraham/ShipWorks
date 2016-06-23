using ShipWorks.Stores.Platforms.Odbc;
using System;
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

        [Fact]
        public void Cleanse_RemovesNulls()
        {
            var testObject = new OdbcRecord(string.Empty);
            testObject.AddField("blah", null);
            testObject.Cleanse();
            Assert.False(testObject.HasValues);
        }

        [Fact]
        public void Cleanse_RemovesDBNull()
        {
            var testObject = new OdbcRecord(string.Empty);
            testObject.AddField("blah", DBNull.Value);
            testObject.Cleanse();
            Assert.False(testObject.HasValues);
        }

        [Fact]
        public void Cleanse_RemovesEmptyString()
        {
            var testObject = new OdbcRecord(string.Empty);
            testObject.AddField("blah", string.Empty);
            testObject.Cleanse();
            Assert.False(testObject.HasValues);
        }

        [Fact]
        public void Cleanse_KeepsNonEmptyValue_WhenNumberAdded()
        {
            var testObject = new OdbcRecord(string.Empty);
            testObject.AddField("blah", 5);
            testObject.Cleanse();
            Assert.Equal(5, testObject.GetValue("blah"));
        }

        [Fact]
        public void Cleanse_KeepsNonEmptyValue_WhenStringAdded()
        {
            var testObject = new OdbcRecord(string.Empty);
            testObject.AddField("blah", "foo");
            testObject.Cleanse();
            Assert.Equal("foo", testObject.GetValue("blah"));
        }
    }
}