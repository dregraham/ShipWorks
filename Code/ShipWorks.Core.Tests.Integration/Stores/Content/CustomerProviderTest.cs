using System;
using System.Threading.Tasks;
using Autofac;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Startup;
using ShipWorks.Stores;
using ShipWorks.Stores.Content;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Core.Tests.Integration.Stores.Content
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class CustomerProviderTest : IDisposable
    {
        private readonly DataContext context;

        public CustomerProviderTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));

            context.Container.Resolve<IConfigurationData>().UpdateConfiguration(x =>
            {
                x.CustomerUpdateBilling = true;
                x.CustomerUpdateShipping = true;
            });

            Modify.Entity(context.Customer)
                .Set(x => x.ShipCity = "CustomerShipCity")
                .Set(x => x.ShipPostalCode = "CustomerShipPostalCode")
                .Set(x => x.BillCity = "CustomerBillCity")
                .Set(x => x.BillPostalCode = "CustomerBillPostalCode")
                .Save();
        }

        [Fact]
        public async Task AcquireCustomer_UpdatesCustomerShipAddress_WhenOrderAddressIsNotEmpty()
        {
            Modify.Order(context.Order)
                .Set(x => x.ShipCity = "Foo")
                .Set(x => x.ShipPostalCode = "Bar")
                .Save();

            var storeTypeCode = context.Container.Resolve<IStoreTypeManager>().GetType(context.Store.StoreTypeCode);

            using (var sqlAdapter = context.Container.Resolve<ISqlAdapterFactory>().Create())
            {
                await CustomerProvider.AcquireCustomer(context.Order, storeTypeCode, sqlAdapter);
            }

            using (var sqlAdapter = context.Container.Resolve<ISqlAdapterFactory>().Create())
            {
                var customer = new CustomerEntity { CustomerID = context.Customer.CustomerID };
                sqlAdapter.FetchEntity(customer);

                Assert.Equal("Foo", customer.ShipCity);
                Assert.Equal("Bar", customer.ShipPostalCode);
            }
        }

        [Fact]
        public async Task AcquireCustomer_DoesNotUpdateCustomerShipAddress_WhenOrderAddressIsEmpty()
        {
            Modify.Order(context.Order)
                .Set(x => x.ShipCity = string.Empty)
                .Set(x => x.ShipPostalCode = string.Empty)
                .Save();

            var storeTypeCode = context.Container.Resolve<IStoreTypeManager>().GetType(context.Store.StoreTypeCode);

            using (var sqlAdapter = context.Container.Resolve<ISqlAdapterFactory>().Create())
            {
                await CustomerProvider.AcquireCustomer(context.Order, storeTypeCode, sqlAdapter);
            }

            using (var sqlAdapter = context.Container.Resolve<ISqlAdapterFactory>().Create())
            {
                var customer = new CustomerEntity { CustomerID = context.Customer.CustomerID };
                sqlAdapter.FetchEntity(customer);

                Assert.Equal(context.Customer.ShipCity, customer.ShipCity);
                Assert.Equal(context.Customer.ShipPostalCode, customer.ShipPostalCode);
            }
        }

        [Fact]
        public async Task AcquireCustomer_UpdatesCustomerBillAddress_WhenOrderAddressIsNotEmpty()
        {
            Modify.Order(context.Order)
                .Set(x => x.BillCity = "Foo")
                .Set(x => x.BillPostalCode = "Bar")
                .Save();

            var storeTypeCode = context.Container.Resolve<IStoreTypeManager>().GetType(context.Store.StoreTypeCode);

            using (var sqlAdapter = context.Container.Resolve<ISqlAdapterFactory>().Create())
            {
                await CustomerProvider.AcquireCustomer(context.Order, storeTypeCode, sqlAdapter);
            }

            using (var sqlAdapter = context.Container.Resolve<ISqlAdapterFactory>().Create())
            {
                var customer = new CustomerEntity { CustomerID = context.Customer.CustomerID };
                sqlAdapter.FetchEntity(customer);

                Assert.Equal("Foo", customer.BillCity);
                Assert.Equal("Bar", customer.BillPostalCode);
            }
        }

        [Fact]
        public async Task AcquireCustomer_DoesNotUpdateCustomerBillAddress_WhenOrderAddressIsEmpty()
        {
            Modify.Order(context.Order)
                .Set(x => x.BillCity = string.Empty)
                .Set(x => x.BillPostalCode = string.Empty)
                .Save();

            var storeTypeCode = context.Container.Resolve<IStoreTypeManager>().GetType(context.Store.StoreTypeCode);

            using (var sqlAdapter = context.Container.Resolve<ISqlAdapterFactory>().Create())
            {
                await CustomerProvider.AcquireCustomer(context.Order, storeTypeCode, sqlAdapter);
            }

            using (var sqlAdapter = context.Container.Resolve<ISqlAdapterFactory>().Create())
            {
                var customer = new CustomerEntity { CustomerID = context.Customer.CustomerID };
                sqlAdapter.FetchEntity(customer);

                Assert.Equal(context.Customer.BillCity, customer.BillCity);
                Assert.Equal(context.Customer.BillPostalCode, customer.BillPostalCode);
            }
        }

        public void Dispose() => context.Dispose();
    }
}