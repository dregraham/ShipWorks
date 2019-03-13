using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using Xunit;
using ShipWorks.Shipping.Carriers.Amazon.SFP;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon
{
    public class AmazonSFPServiceTypeRepositoryTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly Mock<ISqlAdapter> sqlAdapter;

        public AmazonSFPServiceTypeRepositoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            sqlAdapter = mock.FromFactory<ISqlAdapterFactory>()
                .Mock(f => f.Create());
        }

        [Fact]
        public void Get_SelectsAmazonServiceTypesFromDatabase()
        {
            Mock<IEntityCollection2> typesCollection = SetupIEntityCollection(new List<AmazonSFPServiceTypeEntity>());

            sqlAdapter.Setup(a => a.FetchQueryAsync(It.IsAny<EntityQuery<AmazonSFPServiceTypeEntity>>()))
                .ReturnsAsync(typesCollection.Object);

            var testObject = mock.Create<AmazonSFPServiceTypeRepository>();
            testObject.Get();

            sqlAdapter.Verify(a => a.FetchQueryAsync(It.IsAny<EntityQuery<AmazonSFPServiceTypeEntity>>()), Times.Once);
        }

        [Fact]
        public void Get_ReturnsAmazonServiceTypesFromDatabase()
        {
            var serviceTypes = new List<AmazonSFPServiceTypeEntity>()
            {
                new AmazonSFPServiceTypeEntity()
                {
                    AmazonSFPServiceTypeID = 42,
                    ApiValue = "API",
                    Description = "From Data Base"
                }
            };

            Mock<IEntityCollection2> typesCollection = SetupIEntityCollection(serviceTypes);

            sqlAdapter.Setup(a => a.FetchQueryAsync(It.IsAny<EntityQuery<AmazonSFPServiceTypeEntity>>()))
                .ReturnsAsync(typesCollection.Object);

            var testObject = mock.Create<AmazonSFPServiceTypeRepository>();
            var results = testObject.Get();

            Assert.Contains(results, entity => entity.AmazonSFPServiceTypeID == 42 && entity.ApiValue == "API" && entity.Description == "From Data Base");
        }

        [Fact]
        public void Get_CachesServiceTypesOnSubsequentCalls()
        {
            Mock<IEntityCollection2> typesCollection = SetupIEntityCollection(new List<AmazonSFPServiceTypeEntity>());

            sqlAdapter.Setup(a => a.FetchQueryAsync(It.IsAny<EntityQuery<AmazonSFPServiceTypeEntity>>()))
                .ReturnsAsync(typesCollection.Object);

            var testObject = mock.Create<AmazonSFPServiceTypeRepository>();
            testObject.Get();

            sqlAdapter.Verify(a => a.FetchQueryAsync(It.IsAny<EntityQuery<AmazonSFPServiceTypeEntity>>()), Times.Once);

            testObject.Get();
            sqlAdapter.Verify(a => a.FetchQueryAsync(It.IsAny<EntityQuery<AmazonSFPServiceTypeEntity>>()), Times.Once);
        }

        [Fact]
        public void SaveNewService_SavesServiceType()
        {
            Mock<IEntityCollection2> typesCollection = SetupIEntityCollection(new List<AmazonSFPServiceTypeEntity>()
            {
                new AmazonSFPServiceTypeEntity()
                {
                    ApiValue = "val1",
                    Description = "description"
                }
            });

            sqlAdapter.Setup(a => a.FetchQueryAsync(It.IsAny<EntityQuery<AmazonSFPServiceTypeEntity>>()))
                .ReturnsAsync(typesCollection.Object);

            var testObject = mock.Create<AmazonSFPServiceTypeRepository>();
            testObject.SaveNewService("val2", "desc");

            sqlAdapter.Verify(
                a => a.SaveAndRefetch(
                    It.Is<AmazonSFPServiceTypeEntity>(t => t.ApiValue == "val2" && t.Description == "desc")), Times.Once);
        }

        [Fact]
        public void SaveNewService_RefreshesServicesIfOrmQueryExecutionExceptionIsThrown()
        {
            Mock<IEntityCollection2> typesCollection = SetupIEntityCollection(new List<AmazonSFPServiceTypeEntity>()
            {
                new AmazonSFPServiceTypeEntity()
                {
                    ApiValue = "val",
                    Description = "description"
                }
            });

            sqlAdapter.Setup(a => a.FetchQueryAsync(It.IsAny<EntityQuery<AmazonSFPServiceTypeEntity>>()))
                .ReturnsAsync(typesCollection.Object);

            sqlAdapter.Setup(a => a.SaveAndRefetch(It.IsAny<AmazonSFPServiceTypeEntity>()))
                .Throws(new ORMQueryExecutionException("", "", null, null, null));

            var testObject = mock.Create<AmazonSFPServiceTypeRepository>();
            testObject.SaveNewService("val", "desc");

            sqlAdapter.Verify(a => a.FetchQueryAsync(It.IsAny<EntityQuery<AmazonSFPServiceTypeEntity>>()), Times.Exactly(2));
        }

        private Mock<IEntityCollection2> SetupIEntityCollection(IEnumerable<AmazonSFPServiceTypeEntity> serviceTypes)
        {
            var typesCollection = mock.Mock<IEntityCollection2>();
            typesCollection.Setup(m => m.Count).Returns(serviceTypes.Count);
            typesCollection.Setup(m => m[It.IsAny<int>()]).Returns<int>(i => serviceTypes.ElementAt(i));
            typesCollection.Setup(m => m.GetEnumerator()).Returns(() => serviceTypes.GetEnumerator());
            return typesCollection;
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}