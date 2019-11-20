using System;
using System.Net;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using Moq;
using Newtonsoft.Json;
using RestSharp;
using ShipWorks.ApplicationCore.Logging;
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
    public class RakutenWebClientTest
    {
        private readonly AutoMock mock;
        private readonly Mock<IRestClient> restClient;
        private readonly Mock<IRestRequest> restRequest;
        private readonly Mock<IApiLogEntry> logger;
        private readonly Mock<IRakutenStoreEntity> store;

        private readonly Mock<IRestResponse> restResponse;

        public RakutenWebClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            restClient = mock.CreateMock<IRestClient>();
            mock.Mock<IRakutenRestClientFactory>()
                .Setup(f => f.Create(It.IsAny<string>()))
                .Returns(restClient.Object);

            restRequest = mock.CreateMock<IRestRequest>();
            mock.Mock<IRakutenRestRequestFactory>()
                .Setup(f => f.Create(It.IsAny<string>(), It.IsAny<Method>()))
                .Returns(restRequest.Object);


            logger = mock.CreateMock<IApiLogEntry>();
            var logFactory = mock.CreateMock<Func<ApiLogSource, string, IApiLogEntry>>();
            logFactory.Setup(f => f(It.IsAny<ApiLogSource>(), It.IsAny<string>()))
                .Returns(logger.Object);
            mock.Provide(logFactory.Object);


            restResponse = mock.CreateMock<IRestResponse>();
            restClient.Setup(s => s.ExecuteTaskAsync(It.IsAny<IRestRequest>())).Returns(new Task<IRestResponse>(() => restResponse.Object));

            store = mock.CreateMock<IRakutenStoreEntity>();
        }

        [Fact]
        public async Task GetOrders_SetsRequestVerbToPost()
        {
            var testObject = mock.Create<RakutenWebClient>();
            await testObject.GetOrders(store.Object, DateTime.Now);

            restRequest.VerifySet(r => r.Method = Method.POST);
        }

        [Fact]
        public async Task GetOrders_SetsRestClientBaseUrl()
        {
            var testObject = mock.Create<RakutenWebClient>();
            await testObject.GetOrders(store.Object, DateTime.Now);

            restClient.VerifySet(r => r.BaseUrl = new Uri("https://openapi-rms.global.rakuten.com/2.0"));
        }

        [Fact]
        public async Task ConfirmShipping_UsesOrderEndpoint()
        {
            ShipmentEntity shipment = new ShipmentEntity();

            var testObject = mock.Create<RakutenWebClient>();
            await testObject.ConfirmShipping(store.Object, shipment);

            restRequest.VerifySet(s => s.Resource = "ordersearch");
        }

        [Fact]
        public async Task ConfirmShipping_UsesShipmentEndpoint()
        {
            ShipmentEntity shipment = new ShipmentEntity();

            var testObject = mock.Create<RakutenWebClient>();
            await testObject.ConfirmShipping(store.Object, shipment);

            restRequest.VerifySet(s => s.Resource = "orders/");
        }


        [Fact]
        public async Task ConfirmShipping_SetsRequestVerbToPatch()
        {
            ShipmentEntity shipment = new ShipmentEntity();

            var testObject = mock.Create<RakutenWebClient>();
            await testObject.ConfirmShipping(store.Object, shipment);

            restRequest.VerifySet(r => r.Method = Method.PATCH);
        }

        [Fact]
        public void UploadShipmentDetails_GetsSubmitterWithCorrectBody_AndApplicationType()
        {
            RakutenShipmentEntity shipment = new RakutenShipmentEntity()
            {
                ShippedDateUtc = new DateTime(2017, 7, 19),
                ShippingCarrier = "UPS",
                ShippingClass = "Ground",
                TrackingNumber = "12345"
            };

            var testObject = mock.Create<RakutenWebClient>();
            testObject.UploadShipmentDetails(shipment, "token", "1");

            string serializedShipment =
                JsonConvert.SerializeObject(shipment,
                    new JsonSerializerSettings { DateFormatString = "yyyy-MM-ddThh:mm:ssZ" });

            string requestBody = $"{{\"Value\":{serializedShipment}}}";

            mock.Mock<IHttpRequestSubmitterFactory>().Verify(f => f.GetHttpTextPostRequestSubmitter(requestBody, "application/json"));
        }

        [Fact]
        public void UploadShipmentDetails_RequestIsLogged()
        {
            RakutenShipmentEntity shipment = new RakutenShipmentEntity();

            var testObject = mock.Create<RakutenWebClient>();

            testObject.UploadShipmentDetails(shipment, "refresh", "1");

            logger.Verify(l => l.LogRequest(postRequestSubmitter.Object), Times.Once);
        }

        [Fact]
        public void UploadShipmentDetail_ResponseIsLogged()
        {
            RakutenShipmentEntity shipment = new RakutenShipmentEntity();

            var testObject = mock.Create<RakutenWebClient>();

            var postResponseReader = mock.CreateMock<IHttpResponseReader>();

            postRequestSubmitter.Setup(s => s.GetResponse()).Returns(postResponseReader.Object);
            postResponseReader.Setup(r => r.ReadResult()).Returns("blah");

            testObject.UploadShipmentDetails(shipment, "refresh", "1");

            logger.Verify(l => l.LogResponse("blah", "json"), Times.Once);
        }

        [Fact]
        public void UploadShipmentDetails_GetsNewAccessToken()
        {
            RakutenShipmentEntity shipment = new RakutenShipmentEntity();

            var testObject = mock.Create<RakutenWebClient>();

            testObject.UploadShipmentDetails(shipment, "refresh", "1");

            variableRequestSubmitter.Verify(s => s.Variables.Add("grant_type", "refresh_token"), Times.Once);
        }

        [Fact]
        public void UploadShipmentDetails_DoesNotGetNewAccessToken_OnSubsequentCall()
        {
            RakutenShipmentEntity shipment = new RakutenShipmentEntity();

            var testObject = mock.Create<RakutenWebClient>();

            testObject.UploadShipmentDetails(shipment, "refresh", "1");
            testObject.UploadShipmentDetails(shipment, "refresh", "1");

            variableRequestSubmitter.Verify(s => s.Variables.Add("grant_type", "refresh_token"), Times.Once);
        }

        [Fact]
        public void UploadShipmentDetails_SetsSubmitterToAllowNoContentAndBadRequestCodes()
        {
            RakutenShipmentEntity shipment = new RakutenShipmentEntity();

            var testObject = mock.Create<RakutenWebClient>();

            testObject.UploadShipmentDetails(shipment, "refresh", "1");

            // No content is the expected response from Rakuten
            postRequestSubmitter.Verify(s => s.AllowHttpStatusCodes(HttpStatusCode.NoContent, HttpStatusCode.BadRequest));
        }

        [Fact]
        public void UploadShipmentDetails_GetsNewAccessToken_OnSubsequentCall_ThatReturns401()
        {
            RakutenShipmentEntity shipment = new RakutenShipmentEntity();

            var unauthorizedResponse = mock.CreateMock<HttpWebResponse>();
            unauthorizedResponse.Setup(r => r.StatusCode).Returns(HttpStatusCode.Unauthorized);

            var webException = new WebException("401", null, WebExceptionStatus.CacheEntryNotFound, unauthorizedResponse.Object);

            postRequestSubmitter.Setup(s => s.GetResponse()).Throws(webException);
            var testObject = mock.Create<RakutenWebClient>();

            // Since we are throwing a 401 for both attempts of uploading, the final result is throwing a CA exception
            Assert.Throws<RakutenException>(() => testObject.UploadShipmentDetails(shipment, "refresh", "1"));

            variableRequestSubmitter.Verify(s => s.Variables.Add("grant_type", "refresh_token"), Times.Exactly(2));
        }

        [Fact]
        public void UploadShipmentDetails_ReturnsErrorMessage()
        {
            RakutenShipmentEntity shipment = new RakutenShipmentEntity();

            var response = mock.CreateMock<HttpWebResponse>();
            response.Setup(r => r.StatusCode).Returns(HttpStatusCode.BadRequest);

            var postResponseReader = mock.CreateMock<IHttpResponseReader>();
            postResponseReader.Setup(r => r.ReadResult()).Returns("{\r\n  \"error\":{\r\n    \"code\":\"\",\"message\":\"Carrier and Class is invalid.\"\r\n  }\r\n}");
            postResponseReader.SetupGet(r => r.HttpWebResponse).Returns(response.Object);
            postRequestSubmitter.Setup(s => s.GetResponse()).Returns(postResponseReader.Object);

            var testObject = mock.Create<RakutenWebClient>();

            // Since we are throwing a 401 for both attempts of uploading, the final result is throwing a CA exception
            var ex = Assert.Throws<RakutenException>(() => testObject.UploadShipmentDetails(shipment, "refresh", "1"));
            Assert.Equal("Carrier and Class is invalid.", ex.Message);
        }

        [Fact]
        public void GetDistributionCenters_UsesCorrectEndpoint()
        {
            var testObject = mock.Create<RakutenWebClient>();
            testObject.GetDistributionCenters("token");
            variableRequestSubmitter.VerifySet(s => s.Uri =
                It.Is<Uri>(u => u.ToString() == "https://api.channeladvisor.com/v1/DistributionCenters"));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
