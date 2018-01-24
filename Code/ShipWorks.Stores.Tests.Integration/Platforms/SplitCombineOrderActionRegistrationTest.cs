using System;
using System.Linq;
using Autofac;
using Interapptive.Shared.Collections;
using ShipWorks.Startup;
using ShipWorks.Stores.Orders.Combine.Actions;
using ShipWorks.Stores.Orders.Split.Actions;
using ShipWorks.Tests.Shared;
using Xunit;
using Xunit.Abstractions;

namespace ShipWorks.Stores.Tests.Integration.Platforms
{
    [Trait("Category", "ContinuousIntegration")]
    public class SplitCombineOrderActionRegistrationTest : IDisposable
    {
        private readonly IContainer container;
        private readonly ITestOutputHelper testOutputHelper;
        private readonly static StoreTypeCode[] splitOrderExceptions = { StoreTypeCode.Groupon, StoreTypeCode.MarketplaceAdvisor, StoreTypeCode.Walmart };

        public SplitCombineOrderActionRegistrationTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
            container = ContainerInitializer.Build();
        }

        [Fact]
        public void EnsureStoresWithOrderSearchTableHaveCombineOrderActionRegistered() =>
            PerformTest<IStoreSpecificCombineOrderAction>();

        [Fact]
        public void EnsureStoresWithOrderSearchTableHaveSplitOrderActionRegistered() =>
            PerformTest<IStoreSpecificSplitOrderAction>(splitOrderExceptions);

        [Fact]
        public void EnsureExceptionsDoNotHaveOrderSplitActionsRegistered()
        {
            var incorrectRegistrations = splitOrderExceptions
                .Where(x => container.IsRegisteredWithKey<IStoreSpecificSplitOrderAction>(x))
                .ToList();

            foreach (var type in incorrectRegistrations)
            {
                testOutputHelper.WriteLine($"There is a IStoreSpecificSplitOrderAction registered for {type}, but one is not expected.");
            }

            Assert.Empty(incorrectRegistrations);
        }

        private void PerformTest<T>(params StoreTypeCode[] exceptions)
        {
            var storesWithOrderSearchTables = AssemblyProvider.GetShipWorksTypesInAppDomain()
                .Select(x => x.Name)
                .Where(x => x.EndsWith("OrderSearchEntity", StringComparison.Ordinal))
                .Where(x => !x.StartsWith("ReadOnly", StringComparison.Ordinal))
                .Select(x => x.Replace("OrderSearchEntity", ""))
                .Where(x => !string.IsNullOrEmpty(x))
                .ToLookup(x => x);

            // Make sure we match a store type to all stores that have an OrderSearchEntity.  If we don't do
            // this, we could get a false success if a StoreTypeCode is skipped because it doesn't match the name
            // of the table exactly.
            var typesToCheck = storesWithOrderSearchTables
                .LeftJoin(
                    Enum.GetValues(typeof(StoreTypeCode)).OfType<StoreTypeCode?>(),
                    x => x.Key,
                    x => x.ToString());

            Assert.Empty(typesToCheck.Where(x => x.Item2 == null));

            var missingActions = typesToCheck
                .Select(x => x.Item2.Value)
                .Except(exceptions)
                .Where(x => !container.IsRegisteredWithKey<T>(x))
                .ToList();

            foreach (var type in missingActions)
            {
                testOutputHelper.WriteLine($"No {typeof(T).Name} registered for {type}");
            }

            Assert.Empty(missingActions);
        }

        public void Dispose() => container?.Dispose();
    }
}
