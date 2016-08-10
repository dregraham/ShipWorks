using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Mapping
{
    public class JsonOdbcFieldMapWriterTest : IDisposable
    {
        private readonly AutoMock mock;

        public JsonOdbcFieldMapWriterTest()
        {
            mock = AutoMock.GetLoose();
        }

        [Fact]
        public void JsonOdbcFieldMapWriter_ThrowsArgumentNullException_WhenMapIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new JsonOdbcFieldMapSerializer(null));
        }

        [Fact]
        public void Write_WritesSerializedMapToStream()
        {
            string expectedResult =
                "{\"Entries\":[{\"Index\":0,\"ShipWorksField\":{\"ContainingObjectName\":\"OrderEntity\",\"Name\":\"OrderNumber\",\"DisplayName\":\"Order Number\",\"ResolutionStrategy\":0},\"ExternalField\":{\"Column\":{\"Name\":\"OrderNumberColumn\"}}}]}";

            Mock <IOdbcFieldMapIOFactory> ioFactory = mock.Mock<IOdbcFieldMapIOFactory>();
            OdbcFieldMap map = mock.Create<OdbcFieldMap>();

            JsonOdbcFieldMapSerializer testObject = new JsonOdbcFieldMapSerializer(map);
            ioFactory.Setup(f => f.CreateWriter(map)).Returns(testObject);

            OdbcColumn column = new OdbcColumn("OrderNumberColumn");

            ExternalOdbcMappableField externalOdbcMappableField = new ExternalOdbcMappableField(column);
            ShipWorksOdbcMappableField shipworksOdbcMappableField =
                new ShipWorksOdbcMappableField(OrderFields.OrderNumber, "Order Number", OdbcFieldValueResolutionStrategy.Default);

            OdbcFieldMapEntry entry = new OdbcFieldMapEntry(shipworksOdbcMappableField, externalOdbcMappableField);

            map.AddEntry(entry);

            Assert.Equal(expectedResult, testObject.Serialize());
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}