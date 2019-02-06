using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Autofac.Extras.Moq;
using Interapptive.Shared.Net;
using Moq;
using Newtonsoft.Json;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Insurance.InsureShip;
using ShipWorks.Shipping.Insurance.InsureShip.Net;
using ShipWorks.Shipping.Insurance.InsureShip.Net.Void;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Tests.Shipping.Insurance.InsureShip.Net
{
    public class InsureShipWebClientTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly Mock<IHttpVariableRequestSubmitter> submitter;

        public InsureShipWebClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            var settings = mock.Mock<IInsureShipSettings>();
            settings.SetupGet(x => x.ApiUrl).Returns(new Uri("https://www.example.com"));

            submitter = mock.FromFactory<IHttpRequestSubmitterFactory>()
                .Mock(x => x.GetHttpVariableRequestSubmitter());
            submitter.Setup(x => x.AddHeader(AnyString, AnyString)).Returns(submitter);
            submitter.Setup(x => x.AddVariable(AnyString, AnyString)).Returns(submitter);
        }

        [Fact]
        public void Submit_ConfiguresRequestSubmitter()
        {
            var store = mock.Build<IStoreEntity>();

            var credentials = mock.FromFactory<IInsureShipCredentialRetriever>()
                .Mock(x => x.Get(store));
            credentials.SetupGet(x => x.ClientID).Returns("abc123");
            credentials.SetupGet(x => x.ApiKey).Returns("xyx987");
            var testObject = mock.Create<InsureShipWebClient>();

            testObject.Submit<InsureShipVoidPolicyResponse>("foo", store, new Dictionary<string, string> { { "foo", "bar" } });

            submitter.VerifySet(x => x.Uri = new Uri("https://www.example.com/foo"));
            submitter.Verify(x => x.AddHeader("Accept", "application/json"));
            submitter.Verify(x => x.AddVariable("client_id", "abc123"));
            submitter.Verify(x => x.AddVariable("api_key", "xyx987"));
            submitter.Verify(x => x.AllowHttpStatusCodes(new[] { HttpStatusCode.NoContent, (HttpStatusCode) 311, (HttpStatusCode) 312, HttpStatusCode.Unauthorized, HttpStatusCode.PaymentRequired, HttpStatusCode.Conflict, HttpStatusCode.PreconditionFailed, (HttpStatusCode) 419, HttpStatusCode.OK }));
        }

        [Fact]
        public void Submit_AddsPostData_ToRequestSubmitter()
        {
            var testObject = mock.Create<InsureShipWebClient>();

            testObject.Submit<InsureShipVoidPolicyResponse>("foo", mock.Build<IStoreEntity>(), new Dictionary<string, string> { { "foo", "bar" }, { "baz", "quux" } });

            submitter.Verify(x => x.AddVariable("foo", "bar"));
            submitter.Verify(x => x.AddVariable("baz", "quux"));
        }

        [Fact]
        public void Submit_LogsRequest()
        {
            var logEntry = mock.FromFactory<ILogEntryFactory>().Mock(x => x.GetLogEntry(ApiLogSource.InsureShip, "foo", LogActionType.Other));
            var testObject = mock.Create<InsureShipWebClient>();

            testObject.Submit<InsureShipVoidPolicyResponse>("foo", mock.Build<IStoreEntity>(), new Dictionary<string, string>());

            logEntry.Verify(x => x.LogRequest(submitter.Object));
        }

        [Fact]
        public void Submit_ReturnsDeserializedResponse_WhenRequestSucceeds()
        {
            var reader = mock.Mock<IHttpResponseReader>();
            reader.Setup(x => x.ReadResult()).Returns("{ \"status\": \"bar\" }");
            submitter.Setup(x => x.GetResponse()).Returns(reader);
            var testObject = mock.Create<InsureShipWebClient>();

            var result = testObject.Submit<InsureShipVoidPolicyResponse>(AnyString, mock.Build<IStoreEntity>(), new Dictionary<string, string>());

            result
                .Do(x => Assert.Equal("bar", x.Status))
                .ThrowOnFailure();
        }

        [Fact]
        public void Submit_LogsResponse_WhenRequestSucceeds()
        {
            var reader = mock.Mock<IHttpResponseReader>();
            reader.Setup(x => x.ReadResult()).Returns("{ \"status\": \"bar\" }");
            submitter.Setup(x => x.GetResponse()).Returns(reader);
            var logEntry = mock.FromFactory<ILogEntryFactory>().Mock(x => x.GetLogEntry(ApiLogSource.InsureShip, "foo", LogActionType.Other));
            var testObject = mock.Create<InsureShipWebClient>();

            testObject.Submit<InsureShipVoidPolicyResponse>("foo", mock.Build<IStoreEntity>(), new Dictionary<string, string>());

            logEntry.Verify(x => x.LogResponse(It.Is<string>(s => s.Contains("bar")), "log"));
        }

        [Fact]
        public void Submit_ReturnsFailure_WhenRequestSucceedsButWithBadData()
        {
            var reader = mock.Mock<IHttpResponseReader>();
            reader.Setup(x => x.ReadResult()).Returns("{");
            submitter.Setup(x => x.GetResponse()).Returns(reader);
            var logEntry = mock.FromFactory<ILogEntryFactory>().Mock(x => x.GetLogEntry(ApiLogSource.InsureShip, "foo", LogActionType.Other));
            var testObject = mock.Create<InsureShipWebClient>();

            var result = testObject.Submit<InsureShipVoidPolicyResponse>("foo", mock.Build<IStoreEntity>(), new Dictionary<string, string>());

            result
                .Do(x => Assert.True(false))
                .OnFailure(ex => Assert.IsAssignableFrom<JsonException>(ex));
        }

        [Fact]
        public void Submit_ReturnsFailure_WhenRequestSucceedsButHasNoData()
        {
            var logEntry = mock.FromFactory<ILogEntryFactory>().Mock(x => x.GetLogEntry(ApiLogSource.InsureShip, "foo", LogActionType.Other));
            var testObject = mock.Create<InsureShipWebClient>();

            var result = testObject.Submit<InsureShipVoidPolicyResponse>("foo", mock.Build<IStoreEntity>(), new Dictionary<string, string>());

            result
                .Do(x => Assert.True(false))
                .OnFailure(ex => Assert.IsAssignableFrom<ArgumentNullException>(ex));
        }

        [Fact]
        public void Submit_LogsResponse_WhenWebExceptionIsThrown()
        {
            var response = mock.Mock<HttpWebResponse>();
            response.Setup(x => x.GetResponseStream()).Returns(new MemoryStream(Encoding.UTF8.GetBytes("{ \"status\": \"Failure\" }")));

            submitter.Setup(x => x.GetResponse()).Throws(new WebException("Foo", null, WebExceptionStatus.ConnectFailure, response.Object));
            var logEntry = mock.FromFactory<ILogEntryFactory>().Mock(x => x.GetLogEntry(ApiLogSource.InsureShip, "foo", LogActionType.Other));
            var testObject = mock.Create<InsureShipWebClient>();

            testObject.Submit<InsureShipVoidPolicyResponse>("foo", mock.Build<IStoreEntity>(), new Dictionary<string, string>());

            logEntry.Verify(x => x.LogResponse(It.Is<string>(s => s.Contains("Failure")), "log"));
        }

        [Fact]
        public void Submit_ReturnsFailure_WhenWebExceptionIsThrown()
        {
            var exception = new WebException("Foo");

            submitter.Setup(x => x.GetResponse()).Throws(exception);
            var logEntry = mock.FromFactory<ILogEntryFactory>().Mock(x => x.GetLogEntry(ApiLogSource.InsureShip, "foo", LogActionType.Other));
            var testObject = mock.Create<InsureShipWebClient>();

            var result = testObject.Submit<InsureShipVoidPolicyResponse>("foo", mock.Build<IStoreEntity>(), new Dictionary<string, string>());

            result
                .Do(x => Assert.True(false))
                .OnFailure(ex => Assert.Same(exception, ex));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
