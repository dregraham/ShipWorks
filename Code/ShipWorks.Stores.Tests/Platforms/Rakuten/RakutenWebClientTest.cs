using System;
using System.Net;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using Moq;
using Newtonsoft.Json;
using RestSharp;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common.Net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.Rakuten;
using ShipWorks.Stores.Platforms.Rakuten.DTO;
using ShipWorks.Stores.Warehouse.StoreData;
using ShipWorks.Tests.Shared;
using Xunit;
using It = Moq.It;

namespace ShipWorks.Stores.Tests.Platforms.Rakuten
{
    public class RakutenWebClientTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly ILogEntryFactory logFactory;
        public RakutenWebClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            logFactory = mock.Build<ILogEntryFactory>();
        }

        [Theory]
        [InlineData( "url", "123456", "123456")]
        [InlineData( "url", "", "")]
        public void GetOrders_Succeeds(string shopUrl, string authKey, string marketplaceID)
        {
            RakutenStoreEntity store = new RakutenStoreEntity(1);
            store.AuthKey = authKey;
            store.ShopURL = shopUrl;
            store.MarketplaceID= marketplaceID;


            IRestResponse response = new RestResponse()
            {
                StatusCode = HttpStatusCode.OK,
                ErrorException = null
            };

            mock.FromFactory<IRakutenRestClientFactory>()
                .Mock<IRestClient>(x => x.Create(It.IsAny<string>()))
                .Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(response);

            IRakutenWebClient webClient = mock.Create<RakutenWebClient>(TypedParameter.From((IRakutenStoreEntity) store));

            webClient.GetOrders(store, DateTime.Now);
        }

        [Theory]
        [InlineData("url", "123456", "123456")]
        [InlineData("url", "", "")]
        public void ConfirmShipping_Succeeds(string shopUrl, string authKey, string marketplaceID)
        {
            RakutenStoreEntity store = new RakutenStoreEntity(1);
            store.AuthKey = authKey;
            store.ShopURL = shopUrl;
            store.MarketplaceID = marketplaceID;

            var order = new RakutenOrderEntity
            {
                RakutenPackageID = ""
            };

            var shipment = new ShipmentEntity
            {
                ShipmentTypeCode = Shipping.ShipmentTypeCode.Other,
                Order = order
            };

            IRestResponse response = new RestResponse()
            {
                StatusCode = HttpStatusCode.Created,
                ErrorException = null
            };

            mock.FromFactory<IRakutenRestClientFactory>()
                .Mock<IRestClient>(x => x.Create(It.IsAny<string>()))
                .Setup(x => x.Execute(It.IsAny<IRestRequest>()))
                .Returns(response);

            IRakutenWebClient webClient = mock.Create<RakutenWebClient>(TypedParameter.From((IRakutenStoreEntity) store));

            webClient.ConfirmShipping(store, shipment);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
