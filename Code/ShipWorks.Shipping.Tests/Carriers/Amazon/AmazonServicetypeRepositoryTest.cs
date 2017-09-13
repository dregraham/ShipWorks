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
using ShipWorks.Shipping.Carriers.Amazon;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon
{
    public class AmazonServiceTypeRepositoryTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly Mock<ISqlAdapter> sqlAdapter;

        public AmazonServiceTypeRepositoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            sqlAdapter = mock.FromFactory<ISqlAdapterFactory>()
                .Mock(f => f.Create());
        }

        [Fact]
        public void Get_SelectsAmazonServiceTypesFromDatabase()
        {
            Mock<IEntityCollection2> typesCollection = SetupIEntityCollection(new List<AmazonServiceTypeEntity>());

            sqlAdapter.Setup(a => a.FetchQueryAsync(It.IsAny<EntityQuery<AmazonServiceTypeEntity>>()))
                .ReturnsAsync(typesCollection.Object);

            var testObject = mock.Create<AmazonServiceTypeRepository>();
            testObject.Get();

            sqlAdapter.Verify(a => a.FetchQueryAsync(It.IsAny<EntityQuery<AmazonServiceTypeEntity>>()), Times.Once);
        }

        [Fact]
        public void Get_ReturnsAmazonServiceTypesFromDatabase()
        {
            var serviceTypes = new List<AmazonServiceTypeEntity>();
            Mock<IEntityCollection2> typesCollection = SetupIEntityCollection(serviceTypes);

            sqlAdapter.Setup(a => a.FetchQueryAsync(It.IsAny<EntityQuery<AmazonServiceTypeEntity>>()))
                .ReturnsAsync(typesCollection.Object);

            var testObject = mock.Create<AmazonServiceTypeRepository>();
            var results = testObject.Get();

            Assert.Equal(serviceTypes, results);
        }

        [Fact]
        public void Get_CachesServiceTypesOnSubsequentCalls()
        {
            Mock<IEntityCollection2> typesCollection = SetupIEntityCollection(new List<AmazonServiceTypeEntity>());

            sqlAdapter.Setup(a => a.FetchQueryAsync(It.IsAny<EntityQuery<AmazonServiceTypeEntity>>()))
                .ReturnsAsync(typesCollection.Object);

            var testObject = mock.Create<AmazonServiceTypeRepository>();
            testObject.Get();

            sqlAdapter.Verify(a => a.FetchQueryAsync(It.IsAny<EntityQuery<AmazonServiceTypeEntity>>()), Times.Once);

            testObject.Get();
            sqlAdapter.Verify(a => a.FetchQueryAsync(It.IsAny<EntityQuery<AmazonServiceTypeEntity>>()), Times.Once);
        }

        [Fact]
        public void CreateNewService_SavesServiceType()
        {
            var testObject = mock.Create<AmazonServiceTypeRepository>();
            testObject.CreateNewService("val", "desc");

            sqlAdapter.Verify(a=>a.SaveAndRefetch(It.Is<AmazonServiceTypeEntity>(t=>t.ApiValue=="val" && t.Description=="desc")), Times.Once);
        }

        [Fact]
        public void CreateNewService_RefreshesServicesIfOrmQueryExecutionExceptionIsThrown()
        {
            Mock<IEntityCollection2> typesCollection = SetupIEntityCollection(new List<AmazonServiceTypeEntity>()
            {
                new AmazonServiceTypeEntity()
                {
                    ApiValue = "val"
                }
            });

            sqlAdapter.Setup(a => a.FetchQueryAsync(It.IsAny<EntityQuery<AmazonServiceTypeEntity>>()))
                .ReturnsAsync(typesCollection.Object);

            sqlAdapter.Setup(a => a.SaveAndRefetch(It.IsAny<AmazonServiceTypeEntity>()))
                .Throws(new ORMQueryExecutionException("", "", null, null, null));

            var testObject = mock.Create<AmazonServiceTypeRepository>();
            testObject.CreateNewService("val", "desc");

            sqlAdapter.Verify(a => a.FetchQueryAsync(It.IsAny<EntityQuery<AmazonServiceTypeEntity>>()), Times.Once);

        }

        private Mock<IEntityCollection2> SetupIEntityCollection(IEnumerable<AmazonServiceTypeEntity> serviceTypes)
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