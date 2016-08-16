using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Startup;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Tests.Integration.Utilities
{
    [Collection("Database collection")]
    public class OrderMatrix : IDisposable
    {
        private PersonAdapter[] personAdapters = new[]
        {
            new PersonAdapter
            {
                ParsedName = new PersonName("John", string.Empty, "Doe"),
                Street1 = "1 Memorial Dr.",
                Street2 = "Suite 2000",
                City = "St. Louis",
                StateProvCode = "MO",
                PostalCode = "63102",
                CountryCode = "US",
            },
            new PersonAdapter
            {
                ParsedName = new PersonName("Jane", string.Empty, "Doe"),
                Street1 = "14 St James's Square",
                City = "London",
                PostalCode = "SW1Y 4LG",
                CountryCode = "UK",
            },
        };
        private readonly DataContext context;
        private readonly Random rand = new Random();

        public OrderMatrix(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
        }

        private Action<OrderEntityBuilder<TOrder>> CreateCharges<TOrder>(int count) where TOrder : OrderEntity, new()
        {
            return orderBuilder =>
            {
                for (int i = 0; i < count; i++)
                {
                    orderBuilder.WithCharge(x =>
                    {
                        x.Set(c => c.Amount = Convert.ToDecimal(GetRandom(0.25, 10.00)));
                        x.Set(c => c.Description = Faker.Lorem.Sentence(5));
                        x.Set(c => c.Type = Faker.Lorem.Words(1).FirstOrDefault().ToUpperInvariant());
                    });
                }
            };
        }

        private IEnumerable<Action<OrderEntityBuilder<TOrder>>> CreateItems<TOrder, TOrderItem>(int count)
            where TOrder : OrderEntity, new() where TOrderItem : OrderItemEntity, new()
        {
            return Enumerable.Range(0, 3)
                .Select(x => CreateItem<TOrder, TOrderItem>(count, x));
        }

        private Action<OrderEntityBuilder<TOrder>> CreateItem<TOrder, TOrderItem>(int count, int attributeCount)
            where TOrder : OrderEntity, new() where TOrderItem : OrderItemEntity, new()
        {
            return orderBuilder =>
            {
                for (int i = 0; i < count; i++)
                {
                    orderBuilder.WithItem<TOrderItem>(x =>
                   {
                       itemValue = (itemValue + 1) % 100;
                       x.Set(c => c.SKU = GetSKU(itemValue));
                       x.Set(c => c.Code = GetCode(itemValue));
                       x.Set(c => c.Quantity = rand.Next(1, 5));
                       x.Set(c => c.Description = Faker.Lorem.Sentence(15));
                       x.Set(c => c.UnitPrice = Convert.ToDecimal(GetRandom(0.25, 10.00)));
                       x.Set(c => c.Weight = GetRandom(0.25, 10.00));
                       x.Set(c => c.Name = Faker.Lorem.Words(1).FirstOrDefault());

                       for (int attributeIndex = 0; attributeIndex < attributeCount; attributeIndex++)
                       {
                           x.WithItemAttribute(a =>
                           {
                               a.Set(v => v.Name = Faker.Lorem.Words(1).FirstOrDefault());
                               a.Set(v => v.Description = Faker.Lorem.Sentence(10));
                               a.Set(v => v.UnitPrice = Convert.ToDecimal(GetRandom(0.1, 3.00)));
                           });
                       }
                   });
                }
            };
        }

        private double GetRandom(double min, double max)
        {
            return rand.NextDouble() * (max - min) + min;
        }

        int itemValue = 0;
        private string GetSKU(int value)
        {
            return new string((char) ('A' + (value / 10)), 3);
        }

        private string GetCode(int value)
        {
            return new string((char) ('K' + (value % 10)), 3);
        }

        [Fact]
        public void CreateOrderMatrix()
        {
            using (SqlAdapter adapter = SqlAdapter.Create(false))
            {
                adapter.DeleteEntity(context.Order);
            }

            long orderNumber = 0;
            var amazonStore = Create.Store<AmazonStoreEntity>()
                .Set(x => x.AmazonApi, (int) AmazonApi.MarketplaceWebService)
                .Set(x => x.MarketplaceID, "abcd")
                .Set(x => x.MerchantID, "xyz")
                .Set(x => x.StoreName = "Amazon Store")
                .Set(x => x.TypeCode = (int) StoreTypeCode.Amazon)
                .Set(x => x.Enabled = true)
                .Save();
            var channelAdvisorStore = Create.Store<ChannelAdvisorStoreEntity>()
                .Set(x => x.ProfileID, 123)
                .Set(x => x.StoreName = "Channel Advisor Store")
                .Set(x => x.TypeCode = (int) StoreTypeCode.ChannelAdvisor)
                .Set(x => x.Enabled = true)
                .Save();
            var ebayStore = Create.Store<EbayStoreEntity>()
                .Set(x => x.EBayUserID, "abc123")
                .Set(x => x.StoreName = "ebay Store")
                .Set(x => x.TypeCode = (int) StoreTypeCode.Ebay)
                .Set(x => x.Enabled = true)
                .Save();

            int isPrimeValue = 0;
            int isEbayGsp = 0;

            foreach (var shipmentTypeCode in EnumHelper.GetEnumList<ShipmentTypeCode>())
            {
                foreach (var address in personAdapters)
                {
                    foreach (var addCharges in Enumerable.Range(0, 3).Select(CreateCharges<AmazonOrderEntity>))
                    {
                        foreach (var addItems in Enumerable.Range(0, 3).SelectMany(CreateItems<AmazonOrderEntity, AmazonOrderItemEntity>))
                        {
                            isPrimeValue = (isPrimeValue + 1) % 3;

                            var amazonOrderBuilder = CreateOrderBuilder<AmazonOrderEntity>(amazonStore, orderNumber++, shipmentTypeCode.Key);
                            amazonOrderBuilder.WithAddress(x => x.ShipPerson, address);
                            addCharges(amazonOrderBuilder);
                            addItems(amazonOrderBuilder);
                            amazonOrderBuilder.Set(x => x.IsPrime = isPrimeValue == 2 ? 1 : 0);
                            amazonOrderBuilder.Save();
                        }
                    }

                    foreach (var addCharges in Enumerable.Range(0, 3).Select(CreateCharges<ChannelAdvisorOrderEntity>))
                    {
                        foreach (var addItems in Enumerable.Range(0, 3).SelectMany(CreateItems<ChannelAdvisorOrderEntity, ChannelAdvisorOrderItemEntity>))
                        {
                            var channelAdvisorOrderBuilder = CreateOrderBuilder<ChannelAdvisorOrderEntity>(channelAdvisorStore, orderNumber++, shipmentTypeCode.Key);
                            channelAdvisorOrderBuilder.WithAddress(x => x.ShipPerson, address);
                            addCharges(channelAdvisorOrderBuilder);
                            addItems(channelAdvisorOrderBuilder);
                            channelAdvisorOrderBuilder.Save();
                        }
                    }

                    foreach (var addCharges in Enumerable.Range(0, 3).Select(CreateCharges<EbayOrderEntity>))
                    {
                        foreach (var addItems in Enumerable.Range(0, 3).SelectMany(CreateItems<EbayOrderEntity, EbayOrderItemEntity>))
                        {
                            isEbayGsp = (isEbayGsp + 1) % 3;

                            var ebayOrderBuilder = CreateOrderBuilder<EbayOrderEntity>(ebayStore, orderNumber++, shipmentTypeCode.Key);
                            ebayOrderBuilder.WithAddress(x => x.ShipPerson, address);
                            addCharges(ebayOrderBuilder);
                            addItems(ebayOrderBuilder);

                            if (isEbayGsp == 2)
                            {
                                ebayOrderBuilder.Set(x => x.GspEligible = true);
                                ebayOrderBuilder.Set(x => x.GspFirstName = "Jeff");
                                ebayOrderBuilder.Set(x => x.GspLastName = "Thompson");
                                ebayOrderBuilder.Set(x => x.GspStreet1 = "123 Main");
                                ebayOrderBuilder.Set(x => x.GspCity = "St. Louis");
                                ebayOrderBuilder.Set(x => x.GspStateProvince = "MO");
                                ebayOrderBuilder.Set(x => x.GspPostalCode = "63102");
                                ebayOrderBuilder.Set(x => x.GspCountryCode = "US");
                            }

                            ebayOrderBuilder.Save();
                        }
                    }
                }
            }
        }

        private OrderEntityBuilder<TOrder> CreateOrderBuilder<TOrder>(StoreEntity store,
            long orderNumber, string shipmentType) where TOrder : OrderEntity, new()
        {
            return Create.Order<TOrder>(store, context.Customer)
                .WithOrderNumber(orderNumber++, $"{shipmentType}-", string.Empty);
        }

        public void Dispose() => context.Dispose();
    }
}