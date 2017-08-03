using System;
using System.Net;
using Autofac.Extras.Moq;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using Moq;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Jet;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Jet
{
    public class JetRequestTest
    {
        private const string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IldscUZYb2E1WkxSQ0hldmxwa1BHOVdHS0JrMCJ9.eyJpc19zYW5kYm94X3VpZCI6InRydWUiLCJtZXJjaGFudF9pZCI6IjVkZWMzZTg2ZjI2MDQ4ZmNhYWM0MzY2MDdjMjI0NjkwIiwicGFydG5lcl90eXBlIjoiTWVyY2hhbnQiLCJzY29wZSI6Imlyb25tYW4tYXBpIiwiaXNzIjoiamV0LmNvbSIsImV4cCI6MTUwMTU2NTcyOSwibmJmIjoxNTAxNTI5NzI5fQ.0VcWiSuPRzjAWhNTTcC3gKJNGIEGsnrNEm2SpkzqmyFXIVAfI0WihLfo_SFama8wGE6WSBUARTmo3jj_-TWkk1lnwphZkQ6v4NiXbDfbcJlN0y3SMKDcM-gs8G30ch_SZAj_ZAGGooaeVkCMFy8Wjd7WkApo69jqAnDpn6cNRkq82y9-Fy_WrGGPOJGuAcCV4GWfleRuQhB-grKDrzV4taVfjaHqWBiAvEJ4knDBOizT4NVilFC7PEAYfj40yjyGEizDvpc36VpGKZGdE-3XdTUezilWCdmAtFRuvUOzl660avcICrx8mS497N202mtrk7huvd2YcDmgd3u1d4Bin3_pa73FM075-0Ih6pfhj1_PW9JAw7OxSN14h1LqUo14WVaCPbGq8rNujlyx5j1DoQSn9gFylWuEBhbQP7TqSPdJY_q926IEZNe-GVV2Mnq5BDtFuOauAjBMkjLpaFbAzziFVs1665FvNxtcFA9jggGVtFDO9qKxdxSG0fTtHj6mQZKdwyrRZj_-UioUvDuPNnzy-1Ia3ZQ0DrDr-eMIMnrzMoYmB48TnnOESYwE7bNtFCUSZqTJwwk-G85J8d70-I8mLHJ7rp6iMBaJz3EoVLd6wK4Eol3uM9TaSMil7Z27O8Kqh-gxkf8ddEXJQm5N962XR0A_uUodUieY2rhuFEE";

        private const string successfulTokenResponse =
            @"{
              ""id_token"": ""eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IldscUZYb2E1WkxSQ0hldmxwa1BHOVdHS0JrMCJ9.eyJpc19zYW5kYm94X3VpZCI6InRydWUiLCJtZXJjaGFudF9pZCI6IjVkZWMzZTg2ZjI2MDQ4ZmNhYWM0MzY2MDdjMjI0NjkwIiwicGFydG5lcl90eXBlIjoiTWVyY2hhbnQiLCJzY29wZSI6Imlyb25tYW4tYXBpIiwiaXNzIjoiamV0LmNvbSIsImV4cCI6MTUwMTU2NTcyOSwibmJmIjoxNTAxNTI5NzI5fQ.0VcWiSuPRzjAWhNTTcC3gKJNGIEGsnrNEm2SpkzqmyFXIVAfI0WihLfo_SFama8wGE6WSBUARTmo3jj_-TWkk1lnwphZkQ6v4NiXbDfbcJlN0y3SMKDcM-gs8G30ch_SZAj_ZAGGooaeVkCMFy8Wjd7WkApo69jqAnDpn6cNRkq82y9-Fy_WrGGPOJGuAcCV4GWfleRuQhB-grKDrzV4taVfjaHqWBiAvEJ4knDBOizT4NVilFC7PEAYfj40yjyGEizDvpc36VpGKZGdE-3XdTUezilWCdmAtFRuvUOzl660avcICrx8mS497N202mtrk7huvd2YcDmgd3u1d4Bin3_pa73FM075-0Ih6pfhj1_PW9JAw7OxSN14h1LqUo14WVaCPbGq8rNujlyx5j1DoQSn9gFylWuEBhbQP7TqSPdJY_q926IEZNe-GVV2Mnq5BDtFuOauAjBMkjLpaFbAzziFVs1665FvNxtcFA9jggGVtFDO9qKxdxSG0fTtHj6mQZKdwyrRZj_-UioUvDuPNnzy-1Ia3ZQ0DrDr-eMIMnrzMoYmB48TnnOESYwE7bNtFCUSZqTJwwk-G85J8d70-I8mLHJ7rp6iMBaJz3EoVLd6wK4Eol3uM9TaSMil7Z27O8Kqh-gxkf8ddEXJQm5N962XR0A_uUodUieY2rhuFEE"",
              ""token_type"": ""Bearer"",
              ""expires_on"": ""2017-08-01T05:35:29Z""
            }";

        private Mock<IHttpResponseReader> tokenResponseReader;

        private readonly AutoMock mock;

        public JetRequestTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void GetToken_LogsRequest()
        {
            var response = mock.CreateMock<IHttpResponseReader>();
            response.Setup(r => r.ReadResult())
                .Returns(successfulTokenResponse);

            var submitter = mock.CreateMock<IHttpRequestSubmitter>();
            submitter.Setup(s => s.GetResponse())
                .Returns(response.Object);

            mock.Mock<IHttpRequestSubmitterFactory>()
                .Setup(f => f.GetHttpTextPostRequestSubmitter(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(submitter.Object);

            Mock<IApiLogEntry> logEntry = CreateLogger();

            var testObject = mock.Create<JetRequest>();
            testObject.GetToken("a", "b");

            logEntry.Verify(e => e.LogRequest(submitter.Object), Times.Once());
        }

        [Fact]
        public void GetToken_LogsResponse_WhenWebRequestSucessful()
        {
            var response = mock.CreateMock<IHttpResponseReader>();
            response.Setup(r => r.ReadResult())
                .Returns(successfulTokenResponse);

            var submitter = mock.CreateMock<IHttpRequestSubmitter>();
            submitter.Setup(s => s.GetResponse())
                .Returns(response.Object);

            mock.Mock<IHttpRequestSubmitterFactory>()
                .Setup(f => f.GetHttpTextPostRequestSubmitter(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(submitter.Object);

            Mock<IApiLogEntry> logEntry = CreateLogger();

            var testObject = mock.Create<JetRequest>();
            testObject.GetToken("a", "b");

            logEntry.Verify(e => e.LogResponse(successfulTokenResponse, "json"), Times.Once());
        }

        [Fact]
        public void GetToken_LogsResponse_WhenWebRequestFails()
        {
            WebException exception = new WebException();

            var response = mock.CreateMock<IHttpResponseReader>();
            response.Setup(r => r.ReadResult())
                .Throws(exception);

            var submitter = mock.CreateMock<IHttpRequestSubmitter>();
            submitter.Setup(s => s.GetResponse())
                .Returns(response.Object);

            mock.Mock<IHttpRequestSubmitterFactory>()
                .Setup(f => f.GetHttpTextPostRequestSubmitter(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(submitter.Object);

            Mock<IApiLogEntry> logEntry = CreateLogger();

            var testObject = mock.Create<JetRequest>();
            testObject.GetToken("a", "b");

            logEntry.Verify(e => e.LogResponse(exception), Times.Once());
        }

        [Fact]
        public void ProcessRequest_AddsAuthHeader()
        {
            JetStoreEntity store = new JetStoreEntity {ApiUser = "username", Secret = "encrypted"};
            MockSuccessfulTokenRequest(store);
            Mock<IHttpRequestSubmitter> httpSubmitter = mock.Mock<IHttpRequestSubmitter>();
            JetRequest testObject = mock.Create<JetRequest>();

            testObject.ProcessRequest("test", store, httpSubmitter.Object);

            httpSubmitter.Verify(r => r.Headers.Add("Authorization", $"bearer { token}"));
        }

        [Fact]
        public void ProcessRequest_LogsRequest()
        {
            JetStoreEntity store = new JetStoreEntity { ApiUser = "username", Secret = "encrypted" };
            MockSuccessfulTokenRequest(store);
            Mock<IHttpRequestSubmitter> httpSubmitter = mock.Mock<IHttpRequestSubmitter>();
            JetRequest testObject = mock.Create<JetRequest>();

            testObject.ProcessRequest("test", store, httpSubmitter.Object);

            mock.Mock<IApiLogEntry>().Verify(l => l.LogRequest(httpSubmitter.Object));
        }

        [Fact]
        public void ProcessRequest_GetsResponseFromRequest()
        {
            JetStoreEntity store = new JetStoreEntity { ApiUser = "username", Secret = "encrypted" };
            MockSuccessfulTokenRequest(store);
            Mock<IHttpRequestSubmitter> httpSubmitter = mock.Mock<IHttpRequestSubmitter>();
            JetRequest testObject = mock.Create<JetRequest>();

            testObject.ProcessRequest("test", store, httpSubmitter.Object);

            httpSubmitter.Verify(s => s.GetResponse());
        }

        [Fact]
        public void ProcessRequest_ReadsResultFromResonseReader()
        {
            JetStoreEntity store = new JetStoreEntity { ApiUser = "username", Secret = "encrypted" };
            MockSuccessfulTokenRequest(store);

            Mock<IHttpResponseReader> responseReader = mock.Mock<IHttpResponseReader>();
            Mock<IHttpRequestSubmitter> httpSubmitter = mock.Mock<IHttpRequestSubmitter>();
            httpSubmitter.Setup(r => r.GetResponse()).Returns(responseReader);

            JetRequest testObject = mock.Create<JetRequest>();

            testObject.ProcessRequest("test", store, httpSubmitter.Object);

            responseReader.Verify(r => r.ReadResult());
        }

        [Fact]
        public void ProcessRequest_LogsResponse()
        {
            JetStoreEntity store = new JetStoreEntity { ApiUser = "username", Secret = "encrypted" };

            MockSuccessfulTokenRequest(store);

            WebException ex = new WebException("something went wrong");

            Mock<IHttpRequestSubmitter> httpSubmitter = mock.Mock<IHttpRequestSubmitter>();
            httpSubmitter.SetupSequence(h => h.GetResponse()).Returns(tokenResponseReader.Object)
                .Throws(ex);

            JetRequest testObject = mock.Create<JetRequest>();

            testObject.ProcessRequest("test", store, httpSubmitter.Object);

            mock.Mock<IApiLogEntry>().Verify(l => l.LogResponse(ex));
        }

        [Fact]
        public void ProcessRequest_ReturnsGenericResultError_WhenRequestThrowsException()
        {
            JetStoreEntity store = new JetStoreEntity { ApiUser = "username", Secret = "encrypted" };

            MockSuccessfulTokenRequest(store);

            Mock<IHttpRequestSubmitter> httpSubmitter = mock.Mock<IHttpRequestSubmitter>();
            httpSubmitter.SetupSequence(h => h.GetResponse()).Returns(tokenResponseReader.Object)
                .Throws(new WebException("something went wrong"));

            JetRequest testObject = mock.Create<JetRequest>();

            var result = testObject.ProcessRequest("test", store, httpSubmitter.Object);

            Assert.True(result.Failure);
            Assert.Equal("something went wrong", result.Message);
        }

		[Fact]
        public void Acknowledge_CreatesCorrectSubmitter()
        {
            var submitterFactory = mock.Mock<IHttpRequestSubmitterFactory>();
            JetOrderEntity order = new JetOrderEntity()
            {
                MerchantOrderId = "1",
                OrderItems =
                {
                    new JetOrderItemEntity()
                    {
                        JetOrderItemID = "2"
                    }
                }
            };

            var store = new JetStoreEntity() {ApiUser = "blah", Secret = "bleh"};
            MockSuccessfulTokenRequest(store);

            string expectedRequest =
                "{\"acknowledgement_status\":\"accepted\",\"order_items\":[{\"order_item_acknowledgement_status\":\"fulfillable\",\"order_item_id\":\"2\"}]}";

            JetRequest testObject = mock.Create<JetRequest>();

            testObject.Acknowledge(order, store, "path");

            submitterFactory.Verify(s => s.GetHttpTextPostRequestSubmitter(expectedRequest, "application/json"));
        }

        [Fact]
        public void Acknowledge_CreatesCorrectSubmitter_WhenOrderContainsMultipleItems()
        {
            var submitterFactory = mock.Mock<IHttpRequestSubmitterFactory>();
            JetOrderEntity order = new JetOrderEntity()
            {
                MerchantOrderId = "1",
                OrderItems =
                {
                    new JetOrderItemEntity()
                    {
                        JetOrderItemID = "2"
                    },
                    new JetOrderItemEntity()
                    {
                        JetOrderItemID = "3"
                    }
                }
            };

            var store = new JetStoreEntity() { ApiUser = "blah", Secret = "bleh" };
            MockSuccessfulTokenRequest(store);

            string expectedRequest =
                "{\"acknowledgement_status\":\"accepted\",\"order_items\":[{\"order_item_acknowledgement_status\":\"fulfillable\",\"order_item_id\":\"2\"},{\"order_item_acknowledgement_status\":\"fulfillable\",\"order_item_id\":\"3\"}]}";

            JetRequest testObject = mock.Create<JetRequest>();

            testObject.Acknowledge(order, store, "path");

            submitterFactory.Verify(s => s.GetHttpTextPostRequestSubmitter(expectedRequest, "application/json"));
        }

        private void MockSuccessfulTokenRequest(JetStoreEntity store)
        {
            Mock<IEncryptionProvider> encryptionProvider = mock.Mock<IEncryptionProvider>();
            encryptionProvider.Setup(e => e.Decrypt(store.Secret)).Returns("decrypted");

            Mock<IEncryptionProviderFactory> encryptionProviderFactory = mock.Mock<IEncryptionProviderFactory>();
            encryptionProviderFactory.Setup(e => e.CreateSecureTextEncryptionProvider(store.ApiUser))
                .Returns(encryptionProvider);

            tokenResponseReader = mock.Mock<IHttpResponseReader>();
            tokenResponseReader.Setup(r => r.ReadResult()).Returns(successfulTokenResponse);

            Mock<IHttpRequestSubmitter> submitter = mock.Mock<IHttpRequestSubmitter>();
            submitter.Setup(s => s.GetResponse()).Returns(tokenResponseReader);

            Mock<IHttpRequestSubmitterFactory> submitterFactory = mock.Mock<IHttpRequestSubmitterFactory>();
            submitterFactory.Setup(s => s.GetHttpTextPostRequestSubmitter(
                $"{{\"user\": \"{store.ApiUser}\",\"pass\":\"decrypted\"}}",
                "application/json")).Returns(submitter);
        }

        private Mock<IApiLogEntry> CreateLogger()
        {
            var logEntry = mock.CreateMock<IApiLogEntry>();

            var apiLogEntryFactory = mock.CreateMock<Func<ApiLogSource, string, IApiLogEntry>>();
            apiLogEntryFactory.Setup(f => f(ApiLogSource.Jet, "GetToken"))
                .Returns(logEntry.Object);
            mock.Provide(apiLogEntryFactory.Object);
            return logEntry;
        }
    }
}