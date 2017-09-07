using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating.Validation
{
    public class UpsLocalRateRecentShipmentRepositoryTest : IDisposable
    {
        private readonly AutoMock mock;

        public UpsLocalRateRecentShipmentRepositoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void GetRecentShipments_DelegatesToSqlAdapterFactoryForAdapter()
        {
            UpsAccountEntity account = new UpsAccountEntity();
            mock.Create<UpsLocalRateRecentShipmentRepository>().GetRecentShipments(account);
            
            mock.Mock<ISqlAdapterFactory>().Verify(i => i.Create());
        }

        [Fact]
        public void GetRecentShipments_ThrowsUpsLocalRatingException_WhenAdapterThrowsOrmException()
        {
            UpsAccountEntity account = new UpsAccountEntity() {UpsAccountID = 123123};

            Mock<ISqlAdapter> sqlAdapter = mock.Mock<ISqlAdapter>();
            sqlAdapter.Setup(a => a.FetchEntityCollection(It.IsAny<ShipmentCollection>(),
                It.IsAny<RelationPredicateBucket>(), It.IsAny<int>(), It.IsAny<ISortExpression>())).Throws(new ORMException("some error"));

            mock.Mock<ISqlAdapterFactory>().Setup(f => f.Create()).Returns(sqlAdapter);
            
            
            var testObject = mock.Create<UpsLocalRateRecentShipmentRepository>();

            var ex = Assert.Throws<UpsLocalRatingException>(() => testObject.GetRecentShipments(account));

            Assert.Equal($"Failed to validate local rates:{Environment.NewLine}{Environment.NewLine}Error retrieving list of shipments:{Environment.NewLine}{Environment.NewLine}some error", ex.Message);
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}