using Autofac.Extras.Moq;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
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
            Mock<IEntity2> entity = mock.Mock<IEntity2>();
            Mock<IEntityField2> entityField = mock.Mock<IEntityField2>();
            entityField.Setup(ef => ef.CurrentValue).Returns("A Value");
            entity.Setup(e => e.Fields["MatchingFieldName"]).Returns(entityField.Object);

            Mock<IShipWorksOdbcMappableField> field = mock.Mock<IShipWorksOdbcMappableField>();
            field.Setup(f => f.Name).Returns("MatchingFieldName");

            OdbcDefaultFieldValueResolver testObject = mock.Create<OdbcDefaultFieldValueResolver>();

            Assert.Equal("A Value", testObject.GetValue(field.Object, entity.Object));
        }

        [Fact]
        public void GetValue_ReturnsNull_WhenTheGivenEntityDoesNotContainTheGivenField()
        {
            Mock<IEntity2> entity = mock.Mock<IEntity2>();
            Mock<IEntityField2> entityField = mock.Mock<IEntityField2>();
            entityField.Setup(ef => ef.CurrentValue).Returns("A Value");
            entity.Setup(e => e.Fields["MatchingFieldName"]).Returns(entityField.Object);

            Mock<IShipWorksOdbcMappableField> field = mock.Mock<IShipWorksOdbcMappableField>();
            field.Setup(f => f.Name).Returns("Not a MatchingFieldName");

            OdbcDefaultFieldValueResolver testObject = mock.Create<OdbcDefaultFieldValueResolver>();

            Assert.Null(testObject.GetValue(field.Object, entity.Object));
        }
    }
}