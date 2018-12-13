using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using BoDi;
using Interapptive.Shared.Threading;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Loading;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using TechTalk.SpecFlow;

namespace ShipWorks.Core.Specs.Shared
{
    [Binding]
    public class SharedOrderSteps
    {
        private readonly IObjectContainer objectContainer;

        public SharedOrderSteps(IObjectContainer objectContainer)
        {
            this.objectContainer = objectContainer;
        }

        public DataContext Context =>
            objectContainer.Resolve<DataContext>();

        public IDictionary<string, OrderEntity> Orders { get; private set; }

        [Given(@"the following orders")]
        public void GivenTheFollowingOrders(Table table)
        {
            Orders = table.Rows
                .ToDictionary(
                    x => x["Name"],
                    x =>
                    {
                        var orderBuilder = Create.Order(Context.Store, Context.Customer)
                            .WithShipAddress("123 Main", "", "St. Louis", "MO", "63102", "US");
                        var entityBuilder = orderBuilder.Set(o => o.OrderNumber = int.Parse(x["Number"]));
                        return entityBuilder.Save();
                    });

            objectContainer.RegisterInstanceAs(Orders);
            objectContainer.RegisterInstanceAs(Orders.Values);
        }
           
        [When(@"a shipment is created for each order")]
        public Task WhenAShipmentIsCreatedForEachOrder() =>
            IoC.UnsafeGlobalLifetimeScope
                .Resolve<IShipmentsLoader>()
                .StartTask(
                    Context.Mock.Build<IProgressProvider>(),
                    Orders.Values.Select(x => x.OrderID).ToList(),
                    new Dictionary<long, ShipmentEntity>(),
                    new BlockingCollection<ShipmentEntity>(),
                    true,
                    15000);
    }
}
