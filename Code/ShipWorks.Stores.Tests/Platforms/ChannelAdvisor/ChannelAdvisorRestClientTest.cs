using System;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Interapptive.Shared.Net;
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
        private readonly Mock<IHttpResponseReader> responseReader;

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

            responseReader = mock.CreateMock<IHttpResponseReader>();
            submitter.Setup(s => s.GetResponse()).Returns(responseReader);
            
            responseReader.Setup(r => r.ReadResult()).Returns(getTokenResult);
        }

        [Fact]
        public void AuthorizeUrl_IsExpectedUrl()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            Assert.Equal(
                @"https://api.channeladvisor.com/oauth2/authorize?client_id=wx76dgzjcwlfy1ck3nb8oke7ql2ukv05&response_type=code&access_type=offline&scope=orders+inventory&approval_prompt=force&redirect_uri=https:%2F%2Fwww.interapptive.com%2Fchanneladvisor%2Fsubscribe.php",
                testObject.AuthorizeUrl.ToString());
        }

        [Fact]
        public void GetRefreshToken_UsesExpectedEndpoint()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetRefreshToken("blah");
            submitter.VerifySet(s => s.Uri =
                It.Is<Uri>(u => u.ToString() == "https://api.channeladvisor.com/oauth2/token"));
        }

        [Fact]
        public void GetRefreshToken_SetsVerbToPost()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetRefreshToken("blah");
            submitter.VerifySet(s => s.Verb = HttpVerb.Post);
        }

        [Fact]
        public void GetRefreshToken_SetsContentTypeToWwwForm()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetRefreshToken("blah");
            submitter.VerifySet(s => s.ContentType = "application/x-www-form-urlencoded");
        }

        [Fact]
        public void GetRefreshToken_SetsCorrectAuthorization()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetRefreshToken("blah");

            submitter.Verify(s => s.Headers.Add("Authorization", 
                "Basic d3g3NmRnempjd2xmeTFjazNuYjhva2U3cWwydWt2MDU6UHJlYjhFNDJja1daWnBGSGg2T1Yydw=="));
        }

        [Theory]
        [InlineData("grant_type", "authorization_code")]
        [InlineData("code", "blah")]
        public void GetRefreshToken_VariablesSetCorrectly(string variableName, string value)
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetRefreshToken("blah");

            submitter.Verify(s => s.Variables.Add(variableName, value));
        }

        [Fact]
        public void GetRefreshToken_RedirectUriSetCorrectly()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetRefreshToken("blah");

            submitter.Verify(s => s.Variables.Add(
                It.Is<HttpVariable>(v => v.Name == "redirect_uri" &&
                                         v.Value == "https%3A%2F%2Fwww.interapptive.com%2Fchanneladvisor%2Fsubscribe.php" &&
                                         !v.UrlEncode)));
        }

        [Fact]
        public void GetRefreshToken_RequestIsLogged()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetRefreshToken("blah");

            logger.Verify(l=>l.LogRequest(submitter.Object));
        }

        [Fact]
        public void GetRefreshToken_ResponseIsLogged()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            testObject.GetRefreshToken("blah");

            logger.Verify(l => l.LogResponse(getTokenResult, "json"));
        }

        [Fact]
        public void GetRefreshToken_ReturnsRefreshToken()
        {
            var testObject = mock.Create<ChannelAdvisorRestClient>();
            string refreshToken = testObject.GetRefreshToken("blah");

            Assert.Equal("rtoken", refreshToken);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}