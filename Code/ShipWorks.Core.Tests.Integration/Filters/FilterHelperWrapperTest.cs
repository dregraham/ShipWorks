using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Shipping;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Core.Tests.Integration.Filters
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class FilterHelperWrapperTest : IDisposable
    {
        private readonly DataContext context;
        private readonly FilterHelperWrapper testObject;
        private readonly ShippingProviderRuleEntity rule;

        public FilterHelperWrapperTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
            testObject = context.Mock.Create<FilterHelperWrapper>();

            var filter = Create.Entity<FilterEntity>()
                .Save();

            var sequence = Create.Entity<FilterSequenceEntity>()
                .Set(x => x.Filter, filter)
                .Save();

            var content = Create.Entity<FilterNodeContentEntity>()
                .Save();

            Create.Entity<FilterNodeContentDetailEntity>()
                .Set(x => x.FilterNodeContentID, content.FilterNodeContentID)
                .Set(x => x.ObjectID, context.Order.OrderID)
                .Save();

            var node = Create.Entity<FilterNodeEntity>()
                .Set(x => x.FilterSequence, sequence)
                .Set(x => x.FilterNodeContent, content)
                .Save();

            rule = Create.Entity<ShippingProviderRuleEntity>()
                .Set(x => x.ShipmentType, (int) ShipmentTypeCode.Usps)
                .Set(x => x.FilterNodeID, node.FilterNodeID)
                .Save();
        }

        [Fact]
        public void IsObjectInFilterContent_ReturnsFalse_WhenRuleIsNull()
        {
            var result = testObject.IsObjectInFilterContent(context.Order.OrderID, null);
            Assert.False(result);
        }

        [Fact]
        public void IsObjectInFilterContent_ReturnsFalse_WhenFilterContentIsNull()
        {
            var result = testObject.IsObjectInFilterContent(context.Order.OrderID, new ShippingProviderRuleEntity());
            Assert.False(result);
        }

        [Fact]
        public void IsObjectInFilterContent_ReturnsTrue_WhenObjectIsInFilterContent()
        {
            var result = testObject.IsObjectInFilterContent(context.Order.OrderID, rule);
            Assert.True(result);
        }

        public void Dispose() => context.Dispose();
    }
}
