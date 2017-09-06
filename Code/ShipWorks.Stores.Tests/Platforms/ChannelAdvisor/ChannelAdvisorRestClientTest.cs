using System;
using System.Net;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using Moq;
using Newtonsoft.Json;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Stores.Platforms.ChannelAdvisor;
using ShipWorks.Stores.Platforms.ChannelAdvisor.DTO;
using Xunit;
using It = Moq.It;

namespace ShipWorks.Stores.Tests.Platforms.ChannelAdvisor
{
    public class ChannelAdvisorRestClientTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly Mock<IHttpVariableRequestSubmitter> variableRequestSubmitter;
        private readonly Mock<IHttpRequestSubmitter> postRequestSubmitter;
        private readonly Mock<IApiLogEntry> logger;

        private readonly string getTokenResult = @"{
                    ""access_token"": ""atoken"",
                    ""token_type"": ""bearer"",
                    ""expires_in"": 3599,
                    ""refresh_token"": ""rtoken""
                }";

        private readonly Mock<IHttpResponseReader> responseReader;

        public ChannelAdvisorRestClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            variableRequestSubmitter = mock.CreateMock<IHttpVariableRequestSubmitter>();
            mock.Mock<IHttpRequestSubmitterFactory>()
                .Setup(f => f.GetHttpVariableRequestSubmitter())
                .Returns(variableRequestSubmitter.Object);

            postRequestSubmitter = mock.CreateMock<IHttpRequestSubmitter>();
            mock.Mock<IHttpRequestSubmitterFactory>()
                .Setup(f => f.GetHttpTextPostRequestSubmitter(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(postRequestSubmitter.Object);

            logger = mock.CreateMock<IApiLogEntry>();
            var logFactory = mock.CreateMock<Func<ApiLogSource, string, IApiLogEntry>>();
            logFactory.Setup(f => f(It.IsAny<ApiLogSource>(), It.IsAny<string>()))
                .Returns(logger.Object);
            mock.Provide(logFactory.Object);

            responseReader = mock.CreateMock<IHttpResponseReader>();
            variableRequestSubmitter.Setup(s => s.GetResponse()).Returns(responseReader);

            responseReader.Setup(r => r.ReadResult()).Returns(getTokenResult);
        }

        [Fact]
        public void GetRefreshToken_UsesExpectedEndpoint()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetRefreshToken("blah", "blah");
            variableRequestSubmitter.VerifySet(s => s.Uri =
                It.Is<Uri>(u => u.ToString() == "https://api.channeladvisor.com/oauth2/token"));
        }

        [Fact]
        public void GetRefreshToken_SetsVerbToPost()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetRefreshToken("blah", "blah");
            variableRequestSubmitter.VerifySet(s => s.Verb = HttpVerb.Post);
        }

        [Fact]
        public void GetRefreshToken_SetsContentTypeToWwwForm()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetRefreshToken("blah", "blah");
            variableRequestSubmitter.VerifySet(s => s.ContentType = "application/x-www-form-urlencoded");
        }

        [Fact]
        public void GetRefreshToken_SetsCorrectAuthorization()
        {
            mock.FromFactory<IEncryptionProviderFactory>()
                .Mock(f => f.CreateChannelAdvisorEncryptionProvider())
                .Setup(p => p.Decrypt("hij91GRVDQQP9SvJq7tKvrTVAyaqNeyG8AwzcuRHXg4="))
                .Returns("Preb8E42ckWZZpFHh6OV2w");

            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetRefreshToken("blah", "https%3A%2F%2Fwww.interapptive.com%2Fchanneladvisor%2Fsubscribe.php");

            variableRequestSubmitter.Verify(s => s.Headers.Add("Authorization",
                "Basic d3g3NmRnempjd2xmeTFjazNuYjhva2U3cWwydWt2MDU6UHJlYjhFNDJja1daWnBGSGg2T1Yydw=="));
        }

        [Theory]
        [InlineData("grant_type", "authorization_code")]
        [InlineData("code", "blah")]
        public void GetRefreshToken_VariablesSetCorrectly(string variableName, string value)
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetRefreshToken("blah", "blah");

            variableRequestSubmitter.Verify(s => s.Variables.Add(variableName, value));
        }

        [Fact]
        public void GetRefreshToken_RedirectUriSetCorrectly()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetRefreshToken("blah", "https%3A%2F%2Fwww.interapptive.com%2Fchanneladvisor%2Fsubscribe.php");

            variableRequestSubmitter.Verify(s => s.Variables.Add(
                It.Is<HttpVariable>(v => v.Name == "redirect_uri" &&
                                         v.Value == "https%3A%2F%2Fwww.interapptive.com%2Fchanneladvisor%2Fsubscribe.php" &&
                                         !v.UrlEncode)));
        }

        [Fact]
        public void GetRefreshToken_RequestIsLogged()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetRefreshToken("blah", "blah");

            logger.Verify(l=>l.LogRequest(variableRequestSubmitter.Object));
        }

        [Fact]
        public void GetRefreshToken_ResponseIsLogged()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetRefreshToken("blah", "blah");

            logger.Verify(l => l.LogResponse(getTokenResult, "json"));
        }

        [Fact]
        public void GetRefreshToken_ReturnsRefreshToken()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            var refreshToken = testObject.GetRefreshToken("blah", "blah");

            Assert.Equal("rtoken", refreshToken.Value);
        }

        [Fact]
        public void GetOrders_SetsRequestVerbToGet()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetOrders(DateTime.UtcNow, "token");

            variableRequestSubmitter.VerifySet(r => r.Verb = HttpVerb.Get);
        }

        [Fact]
        public void GetOrders_SetsUriToOrdersEndPoint()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetOrders(DateTime.UtcNow, "token");

            variableRequestSubmitter.VerifySet(r => r.Uri = new Uri("https://api.channeladvisor.com/v1/Orders"));
        }

        [Fact]
        public void GetOrders_SetsFilterVariable()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            var start = DateTime.UtcNow;

            testObject.GetOrders(start, "token");

            variableRequestSubmitter.Verify(s => s.Variables.Add("$filter", $"PaymentDateUtc gt {start:yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffff'Z'} and PaymentStatus eq 'Cleared'"));
        }

        [Fact]
        public void GetOrders_SetsExpandVariable()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetOrders(DateTime.UtcNow, "token");

            variableRequestSubmitter.Verify(s => s.Variables.Add("$expand", "Fulfillments,Items($expand=FulfillmentItems)"));
        }

        [Fact]
        public void GetOrders_SetsCountVariable()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetOrders(DateTime.UtcNow, "token");

            variableRequestSubmitter.Verify(s => s.Variables.Add("$count", "true"));
        }

        [Fact]
        public void GetOrders_SetsAccesstokenVariable()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetOrders(DateTime.UtcNow, "token");

            variableRequestSubmitter.Verify(s => s.Variables.Add("access_token", "atoken"));
        }

        [Fact]
        public void GetOrders_UsesCachedAccessToken()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetOrders(DateTime.UtcNow, "token");
            testObject.GetOrders(DateTime.UtcNow, "token");

            variableRequestSubmitter.Verify(s => s.Variables.Add("grant_type", "refresh_token"), Times.Once);
        }

        [Fact]
        public void GetProfiles_UsesProfilesEndpoint()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetProfiles("blah");
            variableRequestSubmitter.VerifySet(s => s.Uri =
                It.Is<Uri>(u => u.ToString() == "https://api.channeladvisor.com/v1/Profiles"));
        }

        [Fact]
        public void UploadShipmentDetails_UsesShipmentEndpoint()
        {
            ChannelAdvisorShipment shipment = new ChannelAdvisorShipment();

            var testObject = mock.Create<ChannelAdvisorRestClient>();

            testObject.UploadShipmentDetails(shipment, "refresh", "1");

            postRequestSubmitter.VerifySet(s => s.Uri =
                It.Is<Uri>(u => u.ToString() == "https://api.channeladvisor.com/v1/Orders(1)/Ship?access_token=atoken"));
        }

        [Fact]
        public void UploadShipmentDetails_GetsSubmitterWithCorrectBody_AndApplicationType()
        {
            ChannelAdvisorShipment shipment = new ChannelAdvisorShipment()
            {
                ShippedDateUtc = new DateTime(2017, 7, 19),
                ShippingCarrier = "UPS",
                ShippingClass = "Ground",
                TrackingNumber = "12345"
            };

            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.UploadShipmentDetails(shipment, "token", "1");

            string serializedShipment =
                JsonConvert.SerializeObject(shipment,
                    new JsonSerializerSettings { DateFormatString = "yyyy-MM-ddThh:mm:ssZ" });

            string requestBody = $"{{\"Value\":{serializedShipment}}}";

            mock.Mock<IHttpRequestSubmitterFactory>().Verify(f=>f.GetHttpTextPostRequestSubmitter(requestBody, "application/json"));
        }

        [Fact]
        public void UploadShipmentDetails_RequestIsLogged()
        {
            ChannelAdvisorShipment shipment = new ChannelAdvisorShipment();

            var testObject = mock.Create<ChannelAdvisorRestClient>();

            testObject.UploadShipmentDetails(shipment, "refresh", "1");

            logger.Verify(l=>l.LogRequest(postRequestSubmitter.Object), Times.Once);
        }

        [Fact]
        public void UploadShipmentDetail_ResponseIsLogged()
        {
            ChannelAdvisorShipment shipment = new ChannelAdvisorShipment();

            var testObject = mock.Create<ChannelAdvisorRestClient>();

            var postResponseReader = mock.CreateMock<IHttpResponseReader>();

            postRequestSubmitter.Setup(s => s.GetResponse()).Returns(postResponseReader.Object);
            postResponseReader.Setup(r => r.ReadResult()).Returns("blah");

            testObject.UploadShipmentDetails(shipment, "refresh", "1");

            logger.Verify(l => l.LogResponse("blah", "json"), Times.Once);
        }

        [Fact]
        public void UploadShipmentDetails_GetsNewAccessToken()
        {
            ChannelAdvisorShipment shipment = new ChannelAdvisorShipment();

            var testObject = mock.Create<ChannelAdvisorRestClient>();

            testObject.UploadShipmentDetails(shipment, "refresh", "1");

            variableRequestSubmitter.Verify(s => s.Variables.Add("grant_type", "refresh_token"), Times.Once);
        }

        [Fact]
        public void UploadShipmentDetails_DoesNotGetNewAccessToken_OnSubsequentCall()
        {
            ChannelAdvisorShipment shipment = new ChannelAdvisorShipment();

            var testObject = mock.Create<ChannelAdvisorRestClient>();

            testObject.UploadShipmentDetails(shipment, "refresh", "1");
            testObject.UploadShipmentDetails(shipment, "refresh", "1");

            variableRequestSubmitter.Verify(s => s.Variables.Add("grant_type", "refresh_token"), Times.Once);
        }

        [Fact]
        public void UploadShipmentDetails_SetsSubmitterToAllowNoContentCode()
        {
            ChannelAdvisorShipment shipment = new ChannelAdvisorShipment();

            var testObject = mock.Create<ChannelAdvisorRestClient>();

            testObject.UploadShipmentDetails(shipment, "refresh", "1");

            // No content is the expected response from ChannelAdvisor
            postRequestSubmitter.Verify(s=>s.AllowHttpStatusCodes(HttpStatusCode.NoContent));
        }

        [Fact]
        public void UploadShipmentDetails_GetsNewAccessToken_OnSubsequentCall_ThatReturns401()
        {
            ChannelAdvisorShipment shipment = new ChannelAdvisorShipment();

            var unauthorizedResponse = mock.CreateMock<HttpWebResponse>();
            unauthorizedResponse.Setup(r => r.StatusCode).Returns(HttpStatusCode.Unauthorized);

            var webException = new WebException("401", null, WebExceptionStatus.CacheEntryNotFound, unauthorizedResponse.Object);

            postRequestSubmitter.Setup(s => s.GetResponse()).Throws(webException);
            var testObject = mock.Create<ChannelAdvisorRestClient>();

            // Since we are throwing a 401 for both attempts of uploading, the final result is throwing a CA exception
            Assert.Throws<ChannelAdvisorException>(() => testObject.UploadShipmentDetails(shipment, "refresh", "1"));

            variableRequestSubmitter.Verify(s => s.Variables.Add("grant_type", "refresh_token"), Times.Exactly(2));
        }

        [Fact]
        public void GetDistributionCenters_UsesCorrectEndpoint()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
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