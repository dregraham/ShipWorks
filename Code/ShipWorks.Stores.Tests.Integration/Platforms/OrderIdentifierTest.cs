using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared.Database;
using Xunit;
using Xunit.Abstractions;

namespace ShipWorks.Stores.Tests.Integration.Platforms
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class OrderIdentifierTest : IDisposable
    {
        private readonly DataContext context;
        private readonly ITestOutputHelper testOutputHelper;

        public OrderIdentifierTest(DatabaseFixture db, ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
        }

        [Fact]
        public void CreateCombinedSearchQuery_JoinsOrderSearch()
        {
            QueryFactory factory = new QueryFactory();

            using (ISqlAdapter sqlAdapter = context.Mock.Container.Resolve<ISqlAdapterFactory>().Create())
            {
                var storeTypes = Enum.GetValues(typeof(StoreTypeCode))
                    .OfType<StoreTypeCode>()
                    .Except(new[] { StoreTypeCode.Invalid })
                    .Select(x => context.Mock.Container.ResolveKeyed<StoreType>(x, TypedParameter.From<StoreEntity>(null)))
                    .Select(x => TestCreateCombinedSearchQuery(sqlAdapter, factory, x))
                    .Where(x => x.Failure)
                    .ToList();

                foreach (var result in storeTypes)
                {
                    testOutputHelper.WriteLine($"{result.Value} failed: {result.Message}");
                }

                Assert.Empty(storeTypes);

            }
        }

        private GenericResult<StoreTypeCode> TestCreateCombinedSearchQuery(ISqlAdapter sqlAdapter, QueryFactory factory, StoreType storeType)
        {
            try
            {
                var order = storeType.CreateOrder();

                if (orderConfig.ContainsKey(storeType.TypeCode))
                {
                    orderConfig[storeType.TypeCode](order);
                }
                else
                {
                    order.ChangeOrderNumber("12345");
                }

                var identifier = storeType.CreateOrderIdentifier(order);

                QuerySpec combinedSearchQuery = identifier.CreateCombinedSearchQuery(factory);
                combinedSearchQuery.AndWhere(OrderSearchFields.StoreID == 1005);
                DynamicQuery query = factory.Create().Select(combinedSearchQuery.Any());

                sqlAdapter.FetchScalar<bool?>(query);

                return GenericResult.FromSuccess(storeType.TypeCode);
            }
            catch (Exception ex)
            {
                return GenericResult.FromError(ex, storeType.TypeCode);
            }
        }

        private readonly IDictionary<StoreTypeCode, Action<OrderEntity>> orderConfig = new Dictionary<StoreTypeCode, Action<OrderEntity>>
        {
            { StoreTypeCode.MarketplaceAdvisor, x => x.ChangeOrderNumber("foo-2") }
        };

        public void Dispose() => context.Dispose();
    }
}