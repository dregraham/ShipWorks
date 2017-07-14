using System;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using Moq;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Stores.Platforms.ChannelAdvisor;
using Xunit;
using It = Moq.It;

namespace ShipWorks.Stores.Tests.Platforms.ChannelAdvisor
{
    public class ChannelAdvisorRestClientTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly Mock<IHttpVariableRequestSubmitter> submitter;
        private readonly Mock<IApiLogEntry> logger;

        private readonly string getTokenResult = @"{
                    ""access_token"": ""atoken"",
                    ""token_type"": ""bearer"",
                    ""expires_in"": 3599,
                    ""refresh_token"": ""rtoken""
                }";

        public ChannelAdvisorRestClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            submitter = mock.CreateMock<IHttpVariableRequestSubmitter>();
            mock.MockFunc(submitter);

            logger = mock.CreateMock<IApiLogEntry>();
            var logFactory = mock.CreateMock<Func<ApiLogSource, string, IApiLogEntry>>();
            logFactory.Setup(f => f(It.IsAny<ApiLogSource>(), It.IsAny<string>()))
                .Returns(logger.Object);
            mock.Provide(logFactory.Object);

            var responseReader = mock.CreateMock<IHttpResponseReader>();
            submitter.Setup(s => s.GetResponse()).Returns(responseReader);

            responseReader.Setup(r => r.ReadResult()).Returns(getTokenResult);
        }

        [Fact]
        public void GetRefreshToken_UsesExpectedEndpoint()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetRefreshToken("blah", "blah");
            submitter.VerifySet(s => s.Uri =
                It.Is<Uri>(u => u.ToString() == "https://api.channeladvisor.com/oauth2/token"));
        }

        [Fact]
        public void GetRefreshToken_SetsVerbToPost()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetRefreshToken("blah", "blah");
            submitter.VerifySet(s => s.Verb = HttpVerb.Post);
        }

        [Fact]
        public void GetRefreshToken_SetsContentTypeToWwwForm()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetRefreshToken("blah", "blah");
            submitter.VerifySet(s => s.ContentType = "application/x-www-form-urlencoded");
        }

        [Fact]
        public void GetRefreshToken_SetsCorrectAuthorization()
        {
            var encryptionProvider = mock.Mock<IEncryptionProvider>();
            encryptionProvider.Setup(p => p.Decrypt("hij91GRVDQQP9SvJq7tKvrTVAyaqNeyG8AwzcuRHXg4=")).Returns("Preb8E42ckWZZpFHh6OV2w");
            mock.Mock<IEncryptionProviderFactory>()
                .Setup(f => f.CreateChannelAdvisorEncryptionProvider())
                .Returns(encryptionProvider);

            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetRefreshToken("blah", "https%3A%2F%2Fwww.interapptive.com%2Fchanneladvisor%2Fsubscribe.php");

            submitter.Verify(s => s.Headers.Add("Authorization",
                "Basic d3g3NmRnempjd2xmeTFjazNuYjhva2U3cWwydWt2MDU6UHJlYjhFNDJja1daWnBGSGg2T1Yydw=="));
        }

        [Theory]
        [InlineData("grant_type", "authorization_code")]
        [InlineData("code", "blah")]
        public void GetRefreshToken_VariablesSetCorrectly(string variableName, string value)
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetRefreshToken("blah", "blah");

            submitter.Verify(s => s.Variables.Add(variableName, value));
        }

        [Fact]
        public void GetRefreshToken_RedirectUriSetCorrectly()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetRefreshToken("blah", "https%3A%2F%2Fwww.interapptive.com%2Fchanneladvisor%2Fsubscribe.php");

            submitter.Verify(s => s.Variables.Add(
                It.Is<HttpVariable>(v => v.Name == "redirect_uri" &&
                                         v.Value == "https%3A%2F%2Fwww.interapptive.com%2Fchanneladvisor%2Fsubscribe.php" &&
                                         !v.UrlEncode)));
        }

        [Fact]
        public void GetRefreshToken_RequestIsLogged()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetRefreshToken("blah", "blah");

            logger.Verify(l=>l.LogRequest(submitter.Object));
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
            string refreshToken = testObject.GetRefreshToken("blah", "blah");

            Assert.Equal("rtoken", refreshToken);
        }

        [Fact]
        public void GetOrders_SetsReqiestVerbToGet()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetOrders(DateTime.UtcNow, "token");

            submitter.VerifySet(r => r.Verb = HttpVerb.Get);
        }

        [Fact]
        public void GetOrders_SetsUriToOrdersEndPoint()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetOrders(DateTime.UtcNow, "token");

            submitter.VerifySet(r => r.Uri = new Uri("https://api.channeladvisor.com/v1/Orders"));
        }

        [Fact]
        public void GetOrders_SetsFilterVariable()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            var start = DateTime.UtcNow;

            testObject.GetOrders(start, "token");

            submitter.Verify(s => s.Variables.Add("$filter", $"CreatedDateUtc gt {start:yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'ff'Z'}"));
        }

        [Fact]
        public void GetOrders_SetsOrderByVariable()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetOrders(DateTime.UtcNow, "token");

            submitter.Verify(s => s.Variables.Add("$orderby", "CreatedDateUtc desc"));
        }

        [Fact]
        public void GetOrders_SetsExpandVariable()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetOrders(DateTime.UtcNow, "token");

            submitter.Verify(s => s.Variables.Add("$expand", "Fulfillments"));
        }

        [Fact]
        public void GetOrders_SetsCountVariable()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetOrders(DateTime.UtcNow, "token");

            submitter.Verify(s => s.Variables.Add("$count", "true"));
        }

        [Fact]
        public void GetOrders_SetsAccesstokenVariable()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetOrders(DateTime.UtcNow, "token");

            submitter.Verify(s => s.Variables.Add("access_token", "token"));
        }

        [Fact]
        public void GetAccessToken_UsesExpectedEndpoint()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetAccessToken("blah");
            submitter.VerifySet(s => s.Uri =
                It.Is<Uri>(u => u.ToString() == "https://api.channeladvisor.com/oauth2/token"));
        }

        [Fact]
        public void GetAccessToken_SetsVerbToPost()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetAccessToken("blah");
            submitter.VerifySet(s => s.Verb = HttpVerb.Post);
        }

        [Fact]
        public void GetAccessToken_SetsContentTypeToWwwForm()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetAccessToken("blah");
            submitter.VerifySet(s => s.ContentType = "application/x-www-form-urlencoded");
        }

        [Fact]
        public void GetAccessToken_SetsCorrectAuthorization()
        {
            var encryptionProvider = mock.Mock<IEncryptionProvider>();
            encryptionProvider.Setup(p => p.Decrypt("hij91GRVDQQP9SvJq7tKvrTVAyaqNeyG8AwzcuRHXg4=")).Returns("Preb8E42ckWZZpFHh6OV2w");
            mock.Mock<IEncryptionProviderFactory>()
                .Setup(f => f.CreateChannelAdvisorEncryptionProvider())
                .Returns(encryptionProvider);

            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetAccessToken("blah");

            submitter.Verify(s => s.Headers.Add("Authorization",
                "Basic d3g3NmRnempjd2xmeTFjazNuYjhva2U3cWwydWt2MDU6UHJlYjhFNDJja1daWnBGSGg2T1Yydw=="));
        }

        [Theory]
        [InlineData("grant_type", "refresh_token")]
        [InlineData("refresh_token", "blah")]
        public void GetAccessToken_VariablesSetCorrectly(string variableName, string value)
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetAccessToken("blah");

            submitter.Verify(s => s.Variables.Add(variableName, value));
        }

        [Fact]
        public void GetAccessToken_RequestIsLogged()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetAccessToken("blah");

            logger.Verify(l => l.LogRequest(submitter.Object));
        }

        [Fact]
        public void GetAccessToken_ResponseIsLogged()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetAccessToken("blah");

            logger.Verify(l => l.LogResponse(getTokenResult, "json"));
        }

        [Fact]
        public void GetAccessToken_ReturnsRefreshToken()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            string refreshToken = testObject.GetAccessToken("blah");

            Assert.Equal("atoken", refreshToken);
        }

        [Fact]
        public void GetProfiles_UsesProfilesEndpoint()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetProfiles("blah");
            submitter.VerifySet(s => s.Uri =
                It.Is<Uri>(u => u.ToString() == "https://api.channeladvisor.com/v1/Profiles"));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}