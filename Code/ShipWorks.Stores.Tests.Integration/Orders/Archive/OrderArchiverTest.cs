using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Moq;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Email;
using ShipWorks.Shipping;
using ShipWorks.Startup;
using ShipWorks.Stores.Orders.Archive;
using ShipWorks.Tests.Shared.Database;
using Interapptive.Shared.Threading;
using ShipWorks.Common.Threading;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;
using ShipWorks.Tests.Shared;

namespace ShipWorks.Stores.Tests.Integration.Orders.Archive
{
    [Collection("Database collection")]
    [Trait("Category", "SmokeTest")]
    public class OrderArchiverTest
    {
        private readonly DataContext context;
        private Mock<IAsyncMessageHelper> asyncMessageHelper;
        private IProgressProvider progressProvider;

        public OrderArchiverTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x), mock =>
            {
                mock.Override<IMessageHelper>();
                asyncMessageHelper = mock.Override<IAsyncMessageHelper>();
            });

            progressProvider = new ProgressProvider();
            progressProvider.ProgressItems.CollectionChanged += OnProgressItemsCollectionChanged;


            asyncMessageHelper.Setup(x => x.CreateProgressProvider()).Returns(progressProvider);

            asyncMessageHelper.Setup(x => x.ShowProgressDialog(AnyString, AnyString, It.IsAny<IProgressProvider>(), TimeSpan.Zero))
                .ReturnsAsync(context.Mock.Build<ISingleItemProgressDialog>());

            CreateOrderForAllStores();
        }

        private void OnProgressItemsCollectionChanged(object sender, Interapptive.Shared.Collections.CollectionChangedEventArgs<IProgressReporter> e)
        {
            e.NewItem.Changed += ProgressItemChanged;
        }

        private void ProgressItemChanged(object sender, EventArgs e)
        {
            if (progressProvider.IsComplete)
            {
                progressProvider.Terminate();
            }
        }

        [Theory]
        [InlineDataAttribute("1950-07-01 00:01:00.000")]
        [InlineDataAttribute("2017-06-30 23:59:59.000")]
        [InlineDataAttribute("2017-07-01 00:00:00.000")]
        [InlineDataAttribute("2017-07-01 00:01:00.000")]
        [InlineDataAttribute("2027-07-01 00:01:00.000")]
        public async Task OrderArchiver_ArchivesCorrectly(string orderDate)
        {
            DateTime maxOrderDate = DateTime.Parse(orderDate);
            var queryFactory = new QueryFactory();
            var toDeleteQuery = queryFactory.Order.Where(OrderFields.OrderDate < maxOrderDate.Date).Select(OrderFields.OrderID.CountBig());
            var toKeepQuery = queryFactory.Order.Where(OrderFields.OrderDate >= maxOrderDate.Date).Select(OrderFields.OrderID.CountBig());

            long countToKeep = 0;

            using (ISqlAdapter sqlAdapter = context.Mock.Container.Resolve<ISqlAdapterFactory>().Create())
            {
                countToKeep = await sqlAdapter.FetchScalarAsync<long>(toKeepQuery).ConfigureAwait(false);
            }

            IOrderArchiver orderArchiver = context.Mock.Create<IOrderArchiver>();

            await orderArchiver.Archive(maxOrderDate).ConfigureAwait(false);

            using (ISqlAdapter sqlAdapter = context.Mock.Container.Resolve<ISqlAdapterFactory>().Create())
            {
                long numberOfOrders = await sqlAdapter.FetchScalarAsync<long>(toDeleteQuery).ConfigureAwait(false);
                Assert.Equal(0, numberOfOrders);

                numberOfOrders = await sqlAdapter.FetchScalarAsync<long>(toKeepQuery).ConfigureAwait(false);
                Assert.Equal(countToKeep, numberOfOrders);
            }
        }

        private IEnumerable<DateTime> OrderDates => new[]
        {
            DateTime.Parse("5/1/2017"),
            DateTime.Parse("6/1/2017"),
            DateTime.Parse("7/1/2017").AddMinutes(-1),
            DateTime.Parse("7/1/2017"),
            DateTime.Parse("7/1/2017").AddMinutes(1),
            DateTime.Parse("8/1/2017"),
        };

        private void CreateOrderForAllStores()
        {
            using (ISqlAdapter sqlAdapter = new SqlAdapter())
            {
                var customer = new CustomerEntity();
                customer.InitializeNullsToDefault();
                customer.BillCountryCode = "US";
                customer.ShipCountryCode = "US";
                customer.ShipFirstName = "Joe";
                customer.ShipLastName = "Doe";
                customer.BillFirstName = "Joe";
                customer.BillLastName = "Doe";
                sqlAdapter.SaveAndRefetch(customer);

                ShippingOriginEntity shippingOrigin = new ShippingOriginEntity();
                shippingOrigin.InitializeNullsToDefault();
                shippingOrigin.Description = "Ship Origin";
                shippingOrigin.Street1 = "123 main";
                shippingOrigin.City = "St. Louis";
                shippingOrigin.Company = "SW";
                shippingOrigin.CountryCode = "US";
                shippingOrigin.Email = "me@me.com";
                shippingOrigin.Phone = "18885551212";
                shippingOrigin.PostalCode = "63102";
                shippingOrigin.StateProvCode = "MO";
                sqlAdapter.SaveAndRefetch(shippingOrigin);

                ComputerEntity computer = new ComputerEntity()
                {
                    Identifier = Guid.NewGuid(),
                    Name = "Computer1"
                };
                sqlAdapter.SaveAndRefetch(computer);

                foreach (StoreTypeCode storeTypeCode in EnumHelper.GetEnumList<StoreTypeCode>().Select(e => e.Value).Where(stc => stc != StoreTypeCode.Invalid))
                {
                    StoreType storeType = StoreTypeManager.GetType(storeTypeCode);
                    var store = storeType.CreateStoreInstance();
                    store.InitializeNullsToDefault();
                    store.StoreName = EnumHelper.GetDescription(storeTypeCode);
                    store.Address.AddressType = (int) AddressType.Residential;
                    store.Address.Street1 = "123 main";
                    store.Address.City = "St. Louis";
                    store.Address.Company = "SW";
                    store.Address.CountryCode = "US";
                    store.Address.Email = "me@me.com";
                    store.Address.FirstName = "First";
                    store.Address.LastName = "Name";
                    store.Address.MiddleName = "None";
                    store.Address.Phone = "18885551212";
                    store.Address.PostalCode = "63102";
                    store.Address.StateProvCode = "MO";
                    store.City = "St. Louis";
                    store.Company = "SW";
                    store.CountryCode = "US";
                    store.Email = "me@me.com";
                    store.Phone = "18885551212";
                    store.PostalCode = "63102";
                    store.StateProvCode = "MO";

                    sqlAdapter.SaveAndRefetch(store);

                    DownloadEntity download = new DownloadEntity()
                    {
                        Store = store,
                        StoreID = store.StoreID,
                        Computer = computer,
                        ComputerID = computer.ComputerID,
                        UserID = 1002
                    };
                    download.InitializeNullsToDefault();
                    sqlAdapter.SaveAndRefetch(download);

                    StoreManager.CheckForChanges();

                    foreach (var orderDate in OrderDates)
                    {
                        var order = storeType.CreateOrder();
                        order.InitializeNullsToDefault();
                        order.CustomerID = customer.CustomerID;
                        order.StoreID = store.StoreID;
                        order.Store = store;
                        order.OrderDate = orderDate;
                        sqlAdapter.SaveAndRefetch(order);

                        DownloadDetailEntity downloadDetail = new DownloadDetailEntity()
                        {
                            DownloadID = download.DownloadID,
                            OrderID = order.OrderID
                        };
                        downloadDetail.InitializeNullsToDefault();
                        sqlAdapter.SaveEntity(downloadDetail);

                        AddEmailOutbound(order.OrderID, sqlAdapter);

                        AddOrderSearch(order, storeTypeCode, sqlAdapter);

                        AddNote(order.OrderID, sqlAdapter);

                        AddValidatedAddress(order.OrderID, sqlAdapter);

                        AddPrintResult(order.OrderID, computer.ComputerID, sqlAdapter);

                        OrderChargeEntity orderCharge = new OrderChargeEntity();
                        orderCharge.InitializeNullsToDefault();
                        orderCharge.OrderID = order.OrderID;
                        sqlAdapter.SaveAndRefetch(orderCharge);

                        OrderPaymentDetailEntity orderPayment = new OrderPaymentDetailEntity();
                        orderPayment.InitializeNullsToDefault();
                        orderPayment.OrderID = order.OrderID;
                        sqlAdapter.SaveAndRefetch(orderPayment);

                        var orderItem = storeType.CreateOrderItemInstance();
                        orderItem.InitializeNullsToDefault();
                        orderItem.OrderID = order.OrderID;
                        sqlAdapter.SaveAndRefetch(orderItem);

                        var orderItemAttribute = storeType.CreateOrderItemAttributeInstance();
                        orderItemAttribute.InitializeNullsToDefault();
                        orderItemAttribute.OrderItemID = orderItem.OrderItemID;
                        sqlAdapter.SaveEntity(orderItemAttribute);

                        foreach (ShipmentTypeCode shipmentTypeCode in EnumHelper.GetEnumList<ShipmentTypeCode>().Select(e => e.Value).Where(stc => stc != ShipmentTypeCode.None))
                        {
                            var shipment = new ShipmentEntity()
                            {
                                ShipmentTypeCode = shipmentTypeCode,
                                ContentWeight = 1,
                                TotalWeight = 1
                            };
                            shipment.InitializeNullsToDefault();
                            shipment.OrderID = order.OrderID;
                            shipment.Order = order;
                            shipment.BestRateEvents = 0;
                            shipment.ShipSenseEntry = new byte[]{0};
                            shipment.OriginOriginID = shippingOrigin.ShippingOriginID;

                            sqlAdapter.SaveAndRefetch(shipment);

                            var shipmentType = ShipmentTypeManager.GetType(shipmentTypeCode);
                            shipmentType.ConfigureNewShipment(shipment);
                            shipmentType.UpdateDynamicShipmentData(shipment);
                            shipment.InitializeNullsToDefault();
                            sqlAdapter.SaveAndRefetch(shipment);

                            AddEmailOutbound(shipment.ShipmentID, sqlAdapter);

                            InsurancePolicyEntity insurancePolicy = new InsurancePolicyEntity();
                            insurancePolicy.InitializeNullsToDefault();
                            insurancePolicy.ShipmentID = shipment.ShipmentID;
                            sqlAdapter.SaveAndRefetch(insurancePolicy);

                            ShipmentReturnItemEntity shipmentReturnItem = new ShipmentReturnItemEntity();
                            shipmentReturnItem.InitializeNullsToDefault();
                            shipmentReturnItem.ShipmentID = shipment.ShipmentID;
                            sqlAdapter.SaveAndRefetch(shipmentReturnItem);

                            AddValidatedAddress(shipment.ShipmentID, sqlAdapter);

                            AddNote(shipment.ShipmentID, sqlAdapter);

                            AddPrintResult(shipment.ShipmentID, computer.ComputerID, sqlAdapter);

                            if (shipmentTypeCode == ShipmentTypeCode.UpsWorldShip)
                            {
                                WorldShipShipmentEntity worldShipShipment = new WorldShipShipmentEntity();
                                worldShipShipment.InitializeNullsToDefault();
                                worldShipShipment.ShipmentID = shipment.ShipmentID;
                                sqlAdapter.SaveEntity(worldShipShipment);

                                WorldShipPackageEntity worldShipPackage = new WorldShipPackageEntity();
                                worldShipPackage.InitializeNullsToDefault();
                                worldShipPackage.ShipmentID = shipment.ShipmentID;
                                worldShipPackage.UpsPackageID = shipment.Ups.Packages.First().UpsPackageID;
                                sqlAdapter.SaveEntity(worldShipPackage);

                                WorldShipGoodsEntity worldShipGoods = new WorldShipGoodsEntity();
                                worldShipGoods.InitializeNullsToDefault();
                                worldShipGoods.ShipmentID = shipment.ShipmentID;
                                sqlAdapter.SaveEntity(worldShipGoods);
                            }
                        }
                    }
                }
            }
        }

        private static void AddValidatedAddress(long consumerID, ISqlAdapter sqlAdapter)
        {
            ValidatedAddressEntity validatedAddress = new ValidatedAddressEntity();
            validatedAddress.InitializeNullsToDefault();
            validatedAddress.ConsumerID = consumerID;
            sqlAdapter.SaveEntity(validatedAddress);
        }

        private static void AddNote(long entityID, ISqlAdapter sqlAdapter)
        {
            NoteEntity note = new NoteEntity();
            note.InitializeNullsToDefault();
            note.EntityID = entityID;
            note.UserID = 1002;
            sqlAdapter.SaveEntity(note);
        }

        private static void AddEmailOutbound(long entityID, ISqlAdapter sqlAdapter)
        {
            EmailOutboundEntity emailOutbound = new EmailOutboundEntity();
            emailOutbound.InitializeNullsToDefault();
            emailOutbound.ContextID = entityID;
            sqlAdapter.SaveAndRefetch(emailOutbound);

            EmailOutboundRelationEntity emailOutboundRelation = new EmailOutboundRelationEntity();
            emailOutboundRelation.InitializeNullsToDefault();
            emailOutboundRelation.EmailOutboundID = emailOutbound.EmailOutboundID;
            emailOutboundRelation.EntityID = entityID;
            emailOutboundRelation.RelationType = (int) EmailOutboundRelationType.ContextObject;
            sqlAdapter.SaveEntity(emailOutboundRelation);
        }

        private void AddPrintResult(long entityID, long computerID, ISqlAdapter sqlAdapter)
        {
            ResourceEntity resource = new ResourceEntity();
            resource.InitializeNullsToDefault();
            resource.Data = new byte[]{0};
            resource.Filename = Guid.NewGuid().ToString("N");
            resource.Checksum = CalculateChecksum(resource.Filename);
            sqlAdapter.SaveAndRefetch(resource);

            ObjectReferenceEntity objectReference = new ObjectReferenceEntity();
            objectReference.InitializeNullsToDefault();
            objectReference.ConsumerID = entityID;
            objectReference.EntityID = resource.ResourceID;
            objectReference.ReferenceKey = resource.Filename;
            sqlAdapter.SaveEntity(objectReference);

            PrintResultEntity printResult = new PrintResultEntity();
            printResult.InitializeNullsToDefault();
            printResult.ComputerID = computerID;
            printResult.RelatedObjectID = entityID;
            printResult.ContentResourceID = resource.ResourceID;
            printResult.JobIdentifier = Guid.NewGuid();
            sqlAdapter.SaveEntity(printResult);

            resource = new ResourceEntity();
            resource.InitializeNullsToDefault();
            resource.Data = new byte[] { 0 };
            resource.Filename = Guid.NewGuid().ToString("N");
            resource.Checksum = CalculateChecksum(resource.Filename);
            sqlAdapter.SaveAndRefetch(resource);

            objectReference = new ObjectReferenceEntity();
            objectReference.InitializeNullsToDefault();
            objectReference.ConsumerID = entityID;
            objectReference.EntityID = resource.ResourceID;
            objectReference.ReferenceKey = resource.Filename;
            sqlAdapter.SaveEntity(objectReference);

            printResult = new PrintResultEntity();
            printResult.InitializeNullsToDefault();
            printResult.ComputerID = computerID;
            printResult.ContextObjectID = entityID;
            printResult.ContentResourceID = resource.ResourceID;
            printResult.JobIdentifier = Guid.NewGuid();
            sqlAdapter.SaveEntity(printResult);
        }

        private byte[] CalculateChecksum(string dataToCalculate)
        {
            using (SHA256 sha = SHA256.Create())
            {
                return sha.ComputeHash(Encoding.UTF8.GetBytes(dataToCalculate));
            }
        }

        private void AddOrderSearch(OrderEntity order, StoreTypeCode storeTypeCode, ISqlAdapter sqlAdapter)
        {
            OrderSearchEntity orderSearch = new OrderSearchEntity();
            orderSearch.InitializeNullsToDefault();
            orderSearch.OrderID = order.OrderID;
            orderSearch.StoreID = order.StoreID;
            sqlAdapter.SaveEntity(orderSearch);

            switch (storeTypeCode)
            {
                case StoreTypeCode.Miva:
                    break;
                case StoreTypeCode.Ebay:
                    EbayOrderSearchEntity storeEbayOrderSearch = new EbayOrderSearchEntity();
                    storeEbayOrderSearch.InitializeNullsToDefault();
                    storeEbayOrderSearch.OrderID = order.OrderID;
                    sqlAdapter.SaveEntity(storeEbayOrderSearch);

                    EbayCombinedOrderRelationEntity ebayCombinedOrderRelation = new EbayCombinedOrderRelationEntity();
                    ebayCombinedOrderRelation.OrderID = order.OrderID;
                    ebayCombinedOrderRelation.StoreID = order.StoreID;
                    ebayCombinedOrderRelation.EbayOrderID = order.OrderID;
                    sqlAdapter.SaveEntity(ebayCombinedOrderRelation);
                    break;
                case StoreTypeCode.Yahoo:
                    YahooOrderSearchEntity storeYahooOrderSearch = new YahooOrderSearchEntity();
                    storeYahooOrderSearch.InitializeNullsToDefault();
                    storeYahooOrderSearch.OrderID = order.OrderID;
                    sqlAdapter.SaveEntity(storeYahooOrderSearch);
                    break;
                case StoreTypeCode.ShopSite:
                    break;
                case StoreTypeCode.MarketplaceAdvisor:
                    MarketplaceAdvisorOrderSearchEntity storeMarketplaceAdvisorOrderSearch = new MarketplaceAdvisorOrderSearchEntity();
                    storeMarketplaceAdvisorOrderSearch.InitializeNullsToDefault();
                    storeMarketplaceAdvisorOrderSearch.OrderID = order.OrderID;
                    sqlAdapter.SaveEntity(storeMarketplaceAdvisorOrderSearch);
                    break;
                case StoreTypeCode.osCommerce:
                    break;
                case StoreTypeCode.ProStores:
                    ProStoresOrderSearchEntity storeProStoresOrderSearch = new ProStoresOrderSearchEntity();
                    storeProStoresOrderSearch.InitializeNullsToDefault();
                    storeProStoresOrderSearch.OrderID = order.OrderID;
                    sqlAdapter.SaveEntity(storeProStoresOrderSearch);
                    break;
                case StoreTypeCode.ChannelAdvisor:
                    ChannelAdvisorOrderSearchEntity storeChannelAdvisorOrderSearch = new ChannelAdvisorOrderSearchEntity();
                    storeChannelAdvisorOrderSearch.InitializeNullsToDefault();
                    storeChannelAdvisorOrderSearch.OrderID = order.OrderID;
                    sqlAdapter.SaveEntity(storeChannelAdvisorOrderSearch);
                    break;
                case StoreTypeCode.Infopia:
                    break;
                case StoreTypeCode.CreLoaded:
                    break;
                case StoreTypeCode.Amazon:
                    AmazonOrderSearchEntity storeOrderSearch = new AmazonOrderSearchEntity();
                    storeOrderSearch.InitializeNullsToDefault();
                    storeOrderSearch.OrderID = order.OrderID;
                    sqlAdapter.SaveEntity(storeOrderSearch);
                    break;
                case StoreTypeCode.XCart:
                    break;
                case StoreTypeCode.OrderMotion:
                    OrderMotionOrderSearchEntity storeOrderMotionOrderSearch = new OrderMotionOrderSearchEntity();
                    storeOrderMotionOrderSearch.InitializeNullsToDefault();
                    storeOrderMotionOrderSearch.OrderID = order.OrderID;
                    sqlAdapter.SaveEntity(storeOrderMotionOrderSearch);
                    break;
                case StoreTypeCode.ZenCart:
                    break;
                case StoreTypeCode.VirtueMart:
                    break;
                case StoreTypeCode.ClickCartPro:
                    ClickCartProOrderSearchEntity storeClickCartProOrderSearch = new ClickCartProOrderSearchEntity();
                    storeClickCartProOrderSearch.InitializeNullsToDefault();
                    storeClickCartProOrderSearch.OrderID = order.OrderID;
                    sqlAdapter.SaveEntity(storeClickCartProOrderSearch);
                    break;
                case StoreTypeCode.PayPal:
                    PayPalOrderSearchEntity storePayPalOrderSearch = new PayPalOrderSearchEntity();
                    storePayPalOrderSearch.InitializeNullsToDefault();
                    storePayPalOrderSearch.OrderID = order.OrderID;
                    sqlAdapter.SaveEntity(storePayPalOrderSearch);
                    break;
                case StoreTypeCode.Volusion:
                    break;
                case StoreTypeCode.NetworkSolutions:
                    NetworkSolutionsOrderSearchEntity storeNetworkSolutionsOrderSearch = new NetworkSolutionsOrderSearchEntity();
                    storeNetworkSolutionsOrderSearch.InitializeNullsToDefault();
                    storeNetworkSolutionsOrderSearch.OrderID = order.OrderID;
                    sqlAdapter.SaveEntity(storeNetworkSolutionsOrderSearch);
                    break;
                case StoreTypeCode.Magento:
                    MagentoOrderSearchEntity storeMagentoOrderSearch = new MagentoOrderSearchEntity();
                    storeMagentoOrderSearch.InitializeNullsToDefault();
                    storeMagentoOrderSearch.OrderID = order.OrderID;
                    sqlAdapter.SaveEntity(storeMagentoOrderSearch);
                    break;
                case StoreTypeCode.OrderDynamics:
                    break;
                case StoreTypeCode.SellerVantage:
                    break;
                case StoreTypeCode.WebShopManager:
                    break;
                case StoreTypeCode.AmeriCommerce:
                    break;
                case StoreTypeCode.CommerceInterface:
                    CommerceInterfaceOrderSearchEntity storeCommerceInterfaceOrderSearch = new CommerceInterfaceOrderSearchEntity();
                    storeCommerceInterfaceOrderSearch.InitializeNullsToDefault();
                    storeCommerceInterfaceOrderSearch.OrderID = order.OrderID;
                    sqlAdapter.SaveEntity(storeCommerceInterfaceOrderSearch);
                    break;
                case StoreTypeCode.SearchFit:
                    break;
                case StoreTypeCode.GenericModule:
                    break;
                case StoreTypeCode.ThreeDCart:
                    ThreeDCartOrderSearchEntity storeThreeDCartOrderSearch = new ThreeDCartOrderSearchEntity();
                    storeThreeDCartOrderSearch.InitializeNullsToDefault();
                    storeThreeDCartOrderSearch.OrderID = order.OrderID;
                    sqlAdapter.SaveEntity(storeThreeDCartOrderSearch);
                    break;
                case StoreTypeCode.BigCommerce:
                    break;
                case StoreTypeCode.GenericFile:
                    break;
                case StoreTypeCode.Shopify:
                    ShopifyOrderSearchEntity storeShopifyOrderSearch = new ShopifyOrderSearchEntity();
                    storeShopifyOrderSearch.InitializeNullsToDefault();
                    storeShopifyOrderSearch.OrderID = order.OrderID;
                    sqlAdapter.SaveEntity(storeShopifyOrderSearch);
                    break;
                case StoreTypeCode.Etsy:
                    break;
                case StoreTypeCode.NeweggMarketplace:
                    break;
                case StoreTypeCode.BuyDotCom:
                    break;
                case StoreTypeCode.Sears:
                    SearsOrderSearchEntity storeSearsOrderSearch = new SearsOrderSearchEntity();
                    storeSearsOrderSearch.InitializeNullsToDefault();
                    storeSearsOrderSearch.OrderID = order.OrderID;
                    sqlAdapter.SaveEntity(storeSearsOrderSearch);
                    break;
                case StoreTypeCode.SolidCommerce:
                    break;
                case StoreTypeCode.Brightpearl:
                    break;
                case StoreTypeCode.OrderDesk:
                    break;
                case StoreTypeCode.Cart66Lite:
                    break;
                case StoreTypeCode.Cart66Pro:
                    break;
                case StoreTypeCode.Shopp:
                    break;
                case StoreTypeCode.Shopperpress:
                    break;
                case StoreTypeCode.WPeCommerce:
                    break;
                case StoreTypeCode.Jigoshop:
                    break;
                case StoreTypeCode.WooCommerce:
                    break;
                case StoreTypeCode.ChannelSale:
                    break;
                case StoreTypeCode.Fortune3:
                    break;
                case StoreTypeCode.LiveSite:
                    break;
                case StoreTypeCode.SureDone:
                    break;
                case StoreTypeCode.Zenventory:
                    break;
                case StoreTypeCode.nopCommerce:
                    break;
                case StoreTypeCode.LimeLightCRM:
                    break;
                case StoreTypeCode.OpenCart:
                    break;
                case StoreTypeCode.SellerExpress:
                    break;
                case StoreTypeCode.PowersportsSupport:
                    break;
                case StoreTypeCode.CloudConversion:
                    break;
                case StoreTypeCode.CsCart:
                    break;
                case StoreTypeCode.PrestaShop:
                    break;
                case StoreTypeCode.LoadedCommerce:
                    break;
                case StoreTypeCode.Choxi:
                    break;
                case StoreTypeCode.Groupon:
                    GrouponOrderSearchEntity storeGrouponOrderSearch = new GrouponOrderSearchEntity();
                    storeGrouponOrderSearch.InitializeNullsToDefault();
                    storeGrouponOrderSearch.OrderID = order.OrderID;
                    sqlAdapter.SaveEntity(storeGrouponOrderSearch);
                    break;
                case StoreTypeCode.StageBloc:
                    break;
                case StoreTypeCode.RevolutionParts:
                    break;
                case StoreTypeCode.InstaStore:
                    break;
                case StoreTypeCode.OrderBot:
                    break;
                case StoreTypeCode.OpenSky:
                    break;
                case StoreTypeCode.LemonStand:
                    LemonStandOrderSearchEntity storeLemonStandOrderSearch = new LemonStandOrderSearchEntity();
                    storeLemonStandOrderSearch.InitializeNullsToDefault();
                    storeLemonStandOrderSearch.OrderID = order.OrderID;
                    sqlAdapter.SaveEntity(storeLemonStandOrderSearch);
                    break;
                case StoreTypeCode.SparkPay:
                    break;
                case StoreTypeCode.Odbc:
                    break;
                case StoreTypeCode.Amosoft:
                    break;
                case StoreTypeCode.SellerCloud:
                    break;
                case StoreTypeCode.InfiPlex:
                    break;
                case StoreTypeCode.Walmart:
                    WalmartOrderSearchEntity storeWalmartOrderSearch = new WalmartOrderSearchEntity();
                    storeWalmartOrderSearch.InitializeNullsToDefault();
                    storeWalmartOrderSearch.OrderID = order.OrderID;
                    sqlAdapter.SaveEntity(storeWalmartOrderSearch);
                    break;
                case StoreTypeCode.SellerActive:
                    break;
                case StoreTypeCode.GeekSeller:
                    break;
                case StoreTypeCode.Jet:
                    JetOrderSearchEntity storeJetOrderSearch = new JetOrderSearchEntity();
                    storeJetOrderSearch.InitializeNullsToDefault();
                    storeJetOrderSearch.OrderID = order.OrderID;
                    sqlAdapter.SaveEntity(storeJetOrderSearch);
                    break;
            }
        }
    }
}
