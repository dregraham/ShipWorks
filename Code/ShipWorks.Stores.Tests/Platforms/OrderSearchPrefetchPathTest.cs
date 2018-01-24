using System;
using System.Linq;
using ShipWorks.Stores.Orders.Split;
using ShipWorks.Tests.Shared;
using Xunit;
using Xunit.Abstractions;

namespace ShipWorks.Stores.Tests.Platforms
{
    public class OrderSearchPrefetchPathTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        public OrderSearchPrefetchPathTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void EnsureAllOrderSearchTablesArePrefetched()
        {
            var loadedPrefetchPaths = OrderSplitGateway.OrderSplitPrefetchPath.Value.Select(x => x.Relation)
                .Where(x => x != null)
                .Select(x => x.HintTargetNameRightOperand);

            var missingPrefetchPaths = AssemblyProvider.GetShipWorksTypesInAppDomain()
                .Select(x => x.Name)
                .Where(x => x.EndsWith("OrderSearchEntity", StringComparison.Ordinal))
                .Where(x => !x.StartsWith("ReadOnly", StringComparison.Ordinal))
                .Except(loadedPrefetchPaths)
                .ToArray();

            foreach (var entityName in missingPrefetchPaths)
            {
                testOutputHelper.WriteLine($"Prefetch missing for {entityName} in OrderSplitGateway.OrderSplitPrefetchPath");
            }

            Assert.Empty(missingPrefetchPaths);
        }
    }
}
