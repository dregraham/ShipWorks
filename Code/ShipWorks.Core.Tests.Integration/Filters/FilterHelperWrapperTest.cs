using System;
using System.Linq;
using Interapptive.Shared.Tests.Filters;
using Interapptive.Shared.Threading;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Connection;
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
                .Set(f => f.Definition = FilterTestingHelper.OrderNumberDefinitionXml(context.Order.OrderNumber))
                .Set(f => f.Name = $"Filter for {context.Order.OrderNumber}")
                .Set(f => f.IsFolder = false)
                .Set(f => f.State = (int) FilterState.Enabled)
                .Save();

            FilterNodeEntity rootOrderFilter = FilterLayoutContext.Current.FindNode(BuiltinFilter.GetTopLevelKey(FilterTarget.Orders));

            var node = FilterLayoutContext.Current.AddFilter(filter, rootOrderFilter, 0).First();

            FilterLayoutContext.Current.Reload();

            testObject.RegenerateFilters(SqlSession.Current.OpenConnection());

            IProgressReporter progressReporter = new ProgressItem("calc initial filter counts");
            testObject.CalculateInitialFilterCounts(SqlSession.Current.OpenConnection(), progressReporter, 0);

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
            var result = testObject.IsObjectInFilterContent(context.Order.OrderID, new ShippingProviderRuleEntity{ FilterNodeID = 1007});
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
