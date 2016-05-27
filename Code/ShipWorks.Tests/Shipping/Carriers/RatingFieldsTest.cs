using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers
{
    public class RatingFieldsTest
    {
        const string expectedHash = "A8a9EN4NAHrzp3Lv4vvq42vAXPNIcuYl0QLAiB1DpmQ=";

        [Fact]
        public void AddShipmentField_AddFieldMultipleTimesWithDifferentNames_Succeeds()
        {
            var testObject = new RatingFields();
            testObject.AddShipmentField(ShipmentFields.ShipCity, "Foo");
            testObject.AddShipmentField(ShipmentFields.ShipCity, "Bar");
            Assert.True(true);
        }

        [Fact]
        public void FieldContainsName_ReturnsTrue_WhenFieldWasAddedWithNoName()
        {
            var testObject = new RatingFields();
            testObject.AddShipmentField(ShipmentFields.ShipCity);
            Assert.True(testObject.FieldsContainName("ShipCity"));
        }

        [Fact]
        public void FieldContainsName_ReturnsTrue_WhenFieldWasAddedWithCustomName()
        {
            var testObject = new RatingFields();
            testObject.AddShipmentField(ShipmentFields.ShipCity, "Foo");
            Assert.True(testObject.FieldsContainName("Foo"));
        }

        [Fact]
        public void FieldContainsName_ReturnsTrue_WhenFieldWasAddedWithCustomNameAndOriginalNameIsRequested()
        {
            var testObject = new RatingFields();
            testObject.AddShipmentField(ShipmentFields.ShipCity, "Foo");
            Assert.True(testObject.FieldsContainName("ShipCity"));
        }

        [Fact]
        public void GetRatingHash_ReturnsHash_WhenFieldIsAddedWithNoName()
        {
            var testObject = new RatingFields();
            testObject.AddShipmentField(ShipmentFields.ShipCity);

            var hash = testObject.GetRatingHash(new ShipmentEntity { ShipCity = "St. Louis" });
            Assert.Equal(expectedHash, hash);
        }

        [Fact]
        public void GetRatingHash_ReturnsHash_WhenFieldIsAddedWithCustomName()
        {
            var testObject = new RatingFields();
            testObject.AddShipmentField(ShipmentFields.ShipCity, "Foo");

            var hash = testObject.GetRatingHash(new ShipmentEntity { ShipCity = "St. Louis" });
            Assert.Equal(expectedHash, hash);
        }

        [Fact]
        public void GetRatingHash_ReturnsHash_WhenFieldIsAddedMultipleTimes()
        {
            var testObject = new RatingFields();
            testObject.AddShipmentField(ShipmentFields.ShipCity, "Foo");
            testObject.AddShipmentField(ShipmentFields.ShipCity);

            var hash = testObject.GetRatingHash(new ShipmentEntity { ShipCity = "St. Louis" });
            Assert.Equal(expectedHash, hash);
        }
    }
}
