using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using ShipWorks.Stores.Platforms.Odbc.Upload.FieldValueResolvers;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Upload.FieldValueResolvers
{
    public class OdbcDefaultFieldValueResolverTest
    {
        private readonly AutoMock mock;

        public OdbcDefaultFieldValueResolverTest()
        {
            mock = AutoMock.GetLoose();
        }

        [Fact]
        public void GetValue_ReturnsCurrentFieldValue()
        {
            string cityName = "the city name";
            ShipmentEntity shipment = new ShipmentEntity();
            shipment.ShipCity = cityName;

            Mock<IShipWorksOdbcMappableField> field = mock.MockRepository.Create<IShipWorksOdbcMappableField>();
            field.Setup(f => f.Name).Returns("ShipCity");
            field.Setup(f => f.ContainingObjectName).Returns("ShipmentEntity");

            OdbcDefaultFieldValueResolver testObject = mock.Create<OdbcDefaultFieldValueResolver>();

            Assert.Equal(cityName, testObject.GetValue(field.Object, shipment));
        }

        [Fact]
        public void GetValue_ReturnsNull_WhenTheGivenEntityDoesNotContainTheGivenField()
        {
            string cityName = "the city name";
            ShipmentEntity shipment = new ShipmentEntity();
            shipment.ShipCity = cityName;

            Mock<IShipWorksOdbcMappableField> field = mock.MockRepository.Create<IShipWorksOdbcMappableField>();
            field.Setup(f => f.Name).Returns("BadFieldName");
            field.Setup(f => f.ContainingObjectName).Returns("ShipmentEntity");

            OdbcDefaultFieldValueResolver testObject = mock.Create<OdbcDefaultFieldValueResolver>();

            Assert.Null(testObject.GetValue(field.Object, shipment));
        }

        [Fact]
        public void GetValue_ReturnsNull_WhenTheGivenEntityDoesNotCorespondToTheContainingObjectName()
        {
            string cityName = "the city name";
            ShipmentEntity shipment = new ShipmentEntity();
            shipment.ShipCity = cityName;

            Mock<IShipWorksOdbcMappableField> field = mock.MockRepository.Create<IShipWorksOdbcMappableField>();
            field.Setup(f => f.Name).Returns("ShipCity");
            field.Setup(f => f.ContainingObjectName).Returns("Order");

            OdbcDefaultFieldValueResolver testObject = mock.Create<OdbcDefaultFieldValueResolver>();

            Assert.Null(testObject.GetValue(field.Object, shipment));
        }
    }
}