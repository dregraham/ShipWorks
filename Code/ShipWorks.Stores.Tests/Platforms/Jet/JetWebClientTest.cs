using System;
using System.Net;
using System.Windows.Forms;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Stores.Platforms.Jet;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Jet
{
    public class JetWebClientTest : IDisposable
    {
        private readonly AutoMock mock;

        //private const string successfulTokenResponse = 
        //    @"{
        //      ""id_token"": ""eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IldscUZYb2E1WkxSQ0hldmxwa1BHOVdHS0JrMCJ9.eyJpc19zYW5kYm94X3VpZCI6InRydWUiLCJtZXJjaGFudF9pZCI6IjVkZWMzZTg2ZjI2MDQ4ZmNhYWM0MzY2MDdjMjI0NjkwIiwicGFydG5lcl90eXBlIjoiTWVyY2hhbnQiLCJzY29wZSI6Imlyb25tYW4tYXBpIiwiaXNzIjoiamV0LmNvbSIsImV4cCI6MTUwMTU2NTcyOSwibmJmIjoxNTAxNTI5NzI5fQ.0VcWiSuPRzjAWhNTTcC3gKJNGIEGsnrNEm2SpkzqmyFXIVAfI0WihLfo_SFama8wGE6WSBUARTmo3jj_-TWkk1lnwphZkQ6v4NiXbDfbcJlN0y3SMKDcM-gs8G30ch_SZAj_ZAGGooaeVkCMFy8Wjd7WkApo69jqAnDpn6cNRkq82y9-Fy_WrGGPOJGuAcCV4GWfleRuQhB-grKDrzV4taVfjaHqWBiAvEJ4knDBOizT4NVilFC7PEAYfj40yjyGEizDvpc36VpGKZGdE-3XdTUezilWCdmAtFRuvUOzl660avcICrx8mS497N202mtrk7huvd2YcDmgd3u1d4Bin3_pa73FM075-0Ih6pfhj1_PW9JAw7OxSN14h1LqUo14WVaCPbGq8rNujlyx5j1DoQSn9gFylWuEBhbQP7TqSPdJY_q926IEZNe-GVV2Mnq5BDtFuOauAjBMkjLpaFbAzziFVs1665FvNxtcFA9jggGVtFDO9qKxdxSG0fTtHj6mQZKdwyrRZj_-UioUvDuPNnzy-1Ia3ZQ0DrDr-eMIMnrzMoYmB48TnnOESYwE7bNtFCUSZqTJwwk-G85J8d70-I8mLHJ7rp6iMBaJz3EoVLd6wK4Eol3uM9TaSMil7Z27O8Kqh-gxkf8ddEXJQm5N962XR0A_uUodUieY2rhuFEE"",
        //      ""token_type"": ""Bearer"",
        //      ""expires_on"": ""2017-08-01T05:35:29Z""
        //    }";

        public JetWebClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void GetToken_IsSuccessful_WhenTokenReturnedSuccessfully()
        {
            mock.Mock<IJetRequest>().Setup(r => r.GetToken(It.IsAny<string>(), It.IsAny<string>())).Returns(GenericResult.FromSuccess("your username"));

            JetWebClient testObject = mock.Create<JetWebClient>();
            GenericResult<string> result = testObject.GetToken("valid username", "correct password");

            Assert.True(result.Success);
        }

        [Fact]
        public void GetToken_Fails_WhenCredentialsAreIncorrect()
        {
            mock.Mock<IJetRequest>().Setup(r => r.GetToken(It.IsAny<string>(), It.IsAny<string>())).Returns(GenericResult.FromError<string>(new Exception("Whammy!")));

            JetWebClient testObject = mock.Create<JetWebClient>();
            GenericResult<string> result = testObject.GetToken("simulatedIncorrectUsername", "wrong password");

            Assert.False(result.Success);
        }

        //[Fact]
        //public void GetToken_LogsRequest()
        //{
        //    var response = mock.CreateMock<IHttpResponseReader>();
        //    response.Setup(r => r.ReadResult())
        //        .Returns(sucessfullTokenResponse);

        //    var submitter = mock.CreateMock<IHttpRequestSubmitter>();
        //    submitter.Setup(s => s.GetResponse())
        //        .Returns(response.Object);

        //    mock.Mock<IHttpRequestSubmitterFactory>()
        //        .Setup(f => f.GetHttpTextPostRequestSubmitter(It.IsAny<string>(), It.IsAny<string>()))
        //        .Returns(submitter.Object);

        //    Mock<IApiLogEntry> logEntry = CreateLogger();

        //    var testObject = mock.Create<JetWebClient>();
        //    testObject.GetToken("a", "b");

        //    logEntry.Verify(e => e.LogRequest(submitter.Object), Times.Once());
        //}

        //[Fact]
        //public void GetToken_LogsResponse_WhenWebRequestSucessful()
        //{
        //    var response = mock.CreateMock<IHttpResponseReader>();
        //    response.Setup(r => r.ReadResult())
        //        .Returns(sucessfullTokenResponse);

        //    var submitter = mock.CreateMock<IHttpRequestSubmitter>();
        //    submitter.Setup(s => s.GetResponse())
        //        .Returns(response.Object);

        //    mock.Mock<IHttpRequestSubmitterFactory>()
        //        .Setup(f => f.GetHttpTextPostRequestSubmitter(It.IsAny<string>(), It.IsAny<string>()))
        //        .Returns(submitter.Object);

        //    Mock<IApiLogEntry> logEntry = CreateLogger();

        //    var testObject = mock.Create<JetWebClient>();
        //    testObject.GetToken("a", "b");

        //    logEntry.Verify(e => e.LogResponse(sucessfullTokenResponse, "json"), Times.Once());
        //}

        //[Fact]
        //public void GetToken_LogsResponse_WhenWebRequestFails()
        //{
        //    WebException exception = new WebException();

        //    var response = mock.CreateMock<IHttpResponseReader>();
        //    response.Setup(r => r.ReadResult())
        //        .Throws(exception);

        //    var submitter = mock.CreateMock<IHttpRequestSubmitter>();
        //    submitter.Setup(s => s.GetResponse())
        //        .Returns(response.Object);

        //    mock.Mock<IHttpRequestSubmitterFactory>()
        //        .Setup(f => f.GetHttpTextPostRequestSubmitter(It.IsAny<string>(), It.IsAny<string>()))
        //        .Returns(submitter.Object);

        //    Mock<IApiLogEntry> logEntry = CreateLogger();

        //    var testObject = mock.Create<JetWebClient>();
        //    testObject.GetToken("a", "b");

        //    logEntry.Verify(e => e.LogResponse(exception), Times.Once());
        //}

        private Mock<IApiLogEntry> CreateLogger()
        {
            var logEntry = mock.CreateMock<IApiLogEntry>();

            var apiLogEntryFactory = mock.CreateMock<Func<ApiLogSource, string, IApiLogEntry>>();
            apiLogEntryFactory.Setup(f => f(ApiLogSource.Jet, "GetToken"))
                .Returns(logEntry.Object);
            mock.Provide(apiLogEntryFactory.Object);
            return logEntry;
        }

        public void Dispose()
        {
            mock.Dispose();
        }

    }
}