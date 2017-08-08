using System;
using System.Net;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Interapptive.Shared.Net;
using Moq;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common.Net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Jet;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Jet
{
    public class JetAuthenticatedRequestTest : IDisposable
    {
        private readonly AutoMock mock;

        public JetAuthenticatedRequestTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void ProcessRequest_ReturnsSuccessfulGenericResult_WhenRequestIsSuccessful()
        {
            mock.Mock<IJetTokenRepository>()
                .Setup(r => r.GetToken(It.IsAny<JetStoreEntity>()))
                .Returns(new JetToken("valid"));

            mock.Mock<IJsonRequest>()
                .Setup(r => r.ProcessRequest<string>("action", ApiLogSource.Jet, It.IsAny<IHttpRequestSubmitter>()))
                .Returns("success");

            var testObject = mock.Create<JetAuthenticatedRequest>();
            var result = testObject.ProcessRequest<string>("action", mock.CreateMock<IHttpRequestSubmitter>().Object,
                new JetStoreEntity());

            Assert.True(result.Success);
        }

        [Fact]
        public void ProcessRequest_ReturnsResponseForJsonRequest_WhenRequestIsSuccessful()
        {
            mock.Mock<IJetTokenRepository>()
                .Setup(r => r.GetToken(It.IsAny<JetStoreEntity>()))
                .Returns(new JetToken("valid"));

            mock.Mock<IJsonRequest>()
                .Setup(r => r.ProcessRequest<string>("action", ApiLogSource.Jet, It.IsAny<IHttpRequestSubmitter>()))
                .Returns("success");

            var testObject = mock.Create<JetAuthenticatedRequest>();
            var result = testObject.ProcessRequest<string>("action", mock.CreateMock<IHttpRequestSubmitter>().Object,
                new JetStoreEntity());

            Assert.Equal("success", result.Value);
        }

        [Fact]
        public void ProcessRequest_DelegatesToTokenAttachTo()
        {
            var jetToken = mock.CreateMock<IJetToken>();
            jetToken.SetupGet(t => t.IsValid).Returns(true);

            mock.Mock<IJetTokenRepository>()
                .Setup(r => r.GetToken(It.IsAny<JetStoreEntity>()))
                .Returns(jetToken.Object);
            
            mock.Mock<IJsonRequest>()
                .Setup(r => r.ProcessRequest<string>("action", ApiLogSource.Jet, It.IsAny<IHttpRequestSubmitter>()))
                .Returns("success");

            var testObject = mock.Create<JetAuthenticatedRequest>();
            var submitter = mock.CreateMock<IHttpRequestSubmitter>().Object;
            testObject.ProcessRequest<string>("action", submitter, new JetStoreEntity());

            jetToken.Verify(t=>t.AttachTo(submitter), Times.Once);
        }

        [Fact]
        public void ProcessRequest_GetsTokenFromStore()
        {
            var jetToken = mock.CreateMock<IJetToken>();
            jetToken.SetupGet(t => t.IsValid).Returns(true);

            mock.Mock<IJetTokenRepository>()
                .Setup(r => r.GetToken(It.IsAny<JetStoreEntity>()))
                .Returns(jetToken.Object);

            mock.Mock<IJsonRequest>()
                .Setup(r => r.ProcessRequest<string>("action", ApiLogSource.Jet, It.IsAny<IHttpRequestSubmitter>()))
                .Returns("success");

            var testObject = mock.Create<JetAuthenticatedRequest>();
            var submitter = mock.CreateMock<IHttpRequestSubmitter>().Object;
            var store = new JetStoreEntity();
            testObject.ProcessRequest<string>("action", submitter, store);

            mock.Mock<IJetTokenRepository>()
                .Verify(r => r.GetToken(store), Times.Once);
        }

        [Fact]
        public void ProcessRequest_ReturnsGenericErrorWithMessage_WhenRequestFails()
        {
            var jetToken = mock.CreateMock<IJetToken>();
            jetToken.SetupGet(t => t.IsValid).Returns(true);

            mock.Mock<IJetTokenRepository>()
                .Setup(r => r.GetToken(It.IsAny<JetStoreEntity>()))
                .Returns(jetToken.Object);

            var unauthorizedResponse = mock.CreateMock<HttpWebResponse>();
            unauthorizedResponse.Setup(r => r.StatusCode).Returns(HttpStatusCode.NoContent);

            var webException = new WebException("error message", null, WebExceptionStatus.CacheEntryNotFound, unauthorizedResponse.Object);
            mock.Mock<IJsonRequest>()
                .Setup(r => r.ProcessRequest<string>("action", ApiLogSource.Jet, It.IsAny<IHttpRequestSubmitter>()))
                .Throws(webException);

            var testObject = mock.Create<JetAuthenticatedRequest>();
            var submitter = mock.CreateMock<IHttpRequestSubmitter>().Object;
            var store = new JetStoreEntity();

            var response = testObject.ProcessRequest<string>("action", submitter, store);
            
            Assert.True(response.Failure);
            Assert.Equal("error message", response.Message);
        }

        [Fact]
        public void ProcessRequest_GetsTokenTwice_WhenTokenExpires()
        {
            var jetToken = mock.CreateMock<IJetToken>();
            jetToken.SetupGet(t => t.IsValid).Returns(true);

            mock.Mock<IJetTokenRepository>()
                .Setup(r => r.GetToken(It.IsAny<JetStoreEntity>()))
                .Returns(jetToken.Object);

            var unauthorizedResponse = mock.CreateMock<HttpWebResponse>();
            unauthorizedResponse.Setup(r => r.StatusCode).Returns(HttpStatusCode.Unauthorized);

            var webException = new WebException("error message", null, WebExceptionStatus.CacheEntryNotFound, unauthorizedResponse.Object);
            mock.Mock<IJsonRequest>()
                .Setup(r => r.ProcessRequest<string>("action", ApiLogSource.Jet, It.IsAny<IHttpRequestSubmitter>()))
                .Throws(webException);

            var testObject = mock.Create<JetAuthenticatedRequest>();
            var submitter = mock.CreateMock<IHttpRequestSubmitter>().Object;
            var store = new JetStoreEntity();

            testObject.ProcessRequest<string>("action", submitter, store);

            mock.Mock<IJetTokenRepository>()
                .Verify(r => r.GetToken(store), Times.Exactly(2));
        }

        [Fact]
        public void ProcessRequest_Removes_WhenTokenExpires()
        {
            var jetToken = mock.CreateMock<IJetToken>();
            jetToken.SetupGet(t => t.IsValid).Returns(true);

            mock.Mock<IJetTokenRepository>()
                .Setup(r => r.GetToken(It.IsAny<JetStoreEntity>()))
                .Returns(jetToken.Object);

            var unauthorizedResponse = mock.CreateMock<HttpWebResponse>();
            unauthorizedResponse.Setup(r => r.StatusCode).Returns(HttpStatusCode.Unauthorized);

            var webException = new WebException("error message", null, WebExceptionStatus.CacheEntryNotFound, unauthorizedResponse.Object);
            mock.Mock<IJsonRequest>()
                .Setup(r => r.ProcessRequest<string>("action", ApiLogSource.Jet, It.IsAny<IHttpRequestSubmitter>()))
                .Throws(webException);

            var testObject = mock.Create<JetAuthenticatedRequest>();
            var submitter = mock.CreateMock<IHttpRequestSubmitter>().Object;
            var store = new JetStoreEntity();

            testObject.ProcessRequest<string>("action", submitter, store);

            mock.Mock<IJetTokenRepository>()
                .Verify(r => r.RemoveToken(store), Times.Once);
        }

        [Fact]
        public void ProcessRequest_ReturnsFailure_WhenCannotGetGoodToken()
        {
            var jetToken = mock.CreateMock<IJetToken>();
            jetToken.SetupGet(t => t.IsValid).Returns(true);

            mock.Mock<IJetTokenRepository>()
                .Setup(r => r.GetToken(It.IsAny<JetStoreEntity>()))
                .Returns(jetToken.Object);

            var unauthorizedResponse = mock.CreateMock<HttpWebResponse>();
            unauthorizedResponse.Setup(r => r.StatusCode).Returns(HttpStatusCode.Unauthorized);

            var webException = new WebException("error message", null, WebExceptionStatus.CacheEntryNotFound, unauthorizedResponse.Object);
            mock.Mock<IJsonRequest>()
                .Setup(r => r.ProcessRequest<string>("action", ApiLogSource.Jet, It.IsAny<IHttpRequestSubmitter>()))
                .Throws(webException);

            var testObject = mock.Create<JetAuthenticatedRequest>();
            var submitter = mock.CreateMock<IHttpRequestSubmitter>().Object;
            var store = new JetStoreEntity();

            var processRequest = testObject.ProcessRequest<string>("action", submitter, store);

            Assert.True(processRequest.Failure);
            Assert.Equal("error message", processRequest.Message);
        }

        [Fact]
        public void ProcessRequest_ReturnsSuccess_WhenRefreshTokenRequiredAndIsSucessful()
        {
            var jetToken = mock.CreateMock<IJetToken>();
            jetToken.SetupGet(t => t.IsValid).Returns(true);

            mock.Mock<IJetTokenRepository>()
                .Setup(r => r.GetToken(It.IsAny<JetStoreEntity>()))
                .Returns(jetToken.Object);

            var unauthorizedResponse = mock.CreateMock<HttpWebResponse>();
            unauthorizedResponse.Setup(r => r.StatusCode).Returns(HttpStatusCode.Unauthorized);

            var webException = new WebException("error message", null, WebExceptionStatus.CacheEntryNotFound, unauthorizedResponse.Object);
            mock.Mock<IJsonRequest>()
                .SetupSequence(r => r.ProcessRequest<string>("action", ApiLogSource.Jet,
                    It.IsAny<IHttpRequestSubmitter>()))
                .Throws(webException)
                .Returns("success");

            var testObject = mock.Create<JetAuthenticatedRequest>();
            var submitter = mock.CreateMock<IHttpRequestSubmitter>().Object;
            var store = new JetStoreEntity();

            var processRequest = testObject.ProcessRequest<string>("action", submitter, store);

            // This line here is a sanity check to make sure that we attach twice. It is included in other
            // tests, but the point of the test is the method being called twice...
            jetToken.Verify(t=> t.AttachTo(submitter), Times.Exactly(2));

            Assert.True(processRequest.Success);
            Assert.Equal("success", processRequest.Value);
        }

        public void Dispose()
        {
            mock.Dispose();
        }

    }
}