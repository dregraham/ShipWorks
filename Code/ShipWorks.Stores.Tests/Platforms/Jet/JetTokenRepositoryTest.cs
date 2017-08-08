using System;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using Moq;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common.Net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Jet;
using ShipWorks.Stores.Platforms.Jet.DTO;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Jet
{
    public class JetTokenRepositoryTest : IDisposable
    {
        private readonly AutoMock mock;

        public JetTokenRepositoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void GetTokenWithUsernameAndPassword_ReturnsToken_WhenProcessRequestSuccessful()
        {
            var submitter = mock.FromFactory<IHttpRequestSubmitterFactory>()
                .Mock(f => f.GetHttpTextPostRequestSubmitter(It.IsAny<string>(), "application/json"));

            var jetTokenResponse = new JetTokenResponse()
            {
                Token = "the token"
            };

            mock.Mock<IJsonRequest>()
                .Setup(r => r.ProcessRequest<JetTokenResponse>("GetToken", ApiLogSource.Jet, submitter.Object))
                .Returns(jetTokenResponse);

            var tokenResponse = mock.Mock<IJetToken>();
            mock.MockFunc<string, IJetToken>(tokenResponse);

            var token = mock.Create<JetTokenRepository>().GetToken("username", "password");
            
            Assert.Equal(tokenResponse.Object, token);
        }

        [Fact]
        public void GetTokenWithUsernameAndPassword_CreatesHttpTextPostRequestSubmitter_WithCorrectUsernameAndPassword()
        {
            var submitter = mock.FromFactory<IHttpRequestSubmitterFactory>()
                .Mock(f => f.GetHttpTextPostRequestSubmitter(It.IsAny<string>(), "application/json"));

            var jetTokenResponse = new JetTokenResponse()
            {
                Token = "the token"
            };

            mock.Mock<IJsonRequest>()
                .Setup(r => r.ProcessRequest<JetTokenResponse>("GetToken", ApiLogSource.Jet, submitter.Object))
                .Returns(jetTokenResponse);

            var tokenResponse = mock.Mock<IJetToken>();
            mock.MockFunc<string, IJetToken>(tokenResponse);

            mock.Create<JetTokenRepository>().GetToken("username", "password");

            mock.Mock<IHttpRequestSubmitterFactory>().Verify(
                f => f.GetHttpTextPostRequestSubmitter("{\"user\": \"username\",\"pass\":\"password\"}",
                    "application/json"), Times.Once);
        }

        [Fact]
        public void GetTokenWithUsernameAndPassword_ProcessTokenIsCalled()
        {
            var submitter = mock.FromFactory<IHttpRequestSubmitterFactory>()
                .Mock(f => f.GetHttpTextPostRequestSubmitter(It.IsAny<string>(), "application/json"));

            var jetTokenResponse = new JetTokenResponse()
            {
                Token = "the token"
            };

            var request = mock.Mock<IJsonRequest>();
            request.Setup(r => r.ProcessRequest<JetTokenResponse>("GetToken", ApiLogSource.Jet, submitter.Object))
                .Returns(jetTokenResponse);

            var tokenResponse = mock.Mock<IJetToken>();
            mock.MockFunc<string, IJetToken>(tokenResponse);

            mock.Create<JetTokenRepository>().GetToken("username", "password");

            request.Verify(r => r.ProcessRequest<JetTokenResponse>("GetToken", ApiLogSource.Jet, submitter.Object), Times.Once);
        }

        [Fact]
        public void GetTokenWithUsernameAndPassword_ProcessTokenIsCalledOnce_WhenGetTokenCalledTwice()
        {
            var submitter = mock.FromFactory<IHttpRequestSubmitterFactory>()
                .Mock(f => f.GetHttpTextPostRequestSubmitter(It.IsAny<string>(), "application/json"));

            var jetTokenResponse = new JetTokenResponse()
            {
                Token = "the token"
            };

            var request = mock.Mock<IJsonRequest>();
            request.Setup(r => r.ProcessRequest<JetTokenResponse>("GetToken", ApiLogSource.Jet, submitter.Object))
                .Returns(jetTokenResponse);

            var tokenResponse = mock.Mock<IJetToken>();
            mock.MockFunc<string, IJetToken>(tokenResponse);

            var testObject = mock.Create<JetTokenRepository>();

            testObject.GetToken("username", "password");
            request.Verify(r => r.ProcessRequest<JetTokenResponse>("GetToken", ApiLogSource.Jet, submitter.Object), Times.Once);

            testObject.GetToken("username", "password");
            request.Verify(r => r.ProcessRequest<JetTokenResponse>("GetToken", ApiLogSource.Jet, submitter.Object), Times.Once);
        }

        [Fact]
        public void RemoveToken_ProcessRequestIsCalledTwice_WhenRemoveTokenCalledBetweenGetTokenCalls()
        {
            var submitter = mock.FromFactory<IHttpRequestSubmitterFactory>()
                .Mock(f => f.GetHttpTextPostRequestSubmitter(It.IsAny<string>(), "application/json"));
            
            var jetTokenResponse = new JetTokenResponse()
            {
                Token = "the token"
            };

            var request = mock.Mock<IJsonRequest>();
            request.Setup(r => r.ProcessRequest<JetTokenResponse>("GetToken", ApiLogSource.Jet, submitter.Object))
                .Returns(jetTokenResponse);

            var tokenResponse = mock.Mock<IJetToken>();
            mock.MockFunc<string, IJetToken>(tokenResponse);

            var testObject = mock.Create<JetTokenRepository>();

            testObject.GetToken("username", "password");

            request.Verify(r => r.ProcessRequest<JetTokenResponse>("GetToken", ApiLogSource.Jet, submitter.Object), Times.Once);

            testObject.RemoveToken(new JetStoreEntity {ApiUser = "username"});

            testObject.GetToken("username", "password");
            request.Verify(r => r.ProcessRequest<JetTokenResponse>("GetToken", ApiLogSource.Jet, submitter.Object), Times.Exactly(2));
        }

        [Fact]
        public void GetTokenWithStore_GetHttpTextPostRequest_WithDeobfuscatedPassword()
        {
            var submitter = mock.FromFactory<IHttpRequestSubmitterFactory>()
                .Mock(f => f.GetHttpTextPostRequestSubmitter(It.IsAny<string>(), "application/json"));
            
            var jetTokenResponse = new JetTokenResponse()
            {
                Token = "the token"
            };

            mock.Mock<IJsonRequest>()
                .Setup(r => r.ProcessRequest<JetTokenResponse>("GetToken", ApiLogSource.Jet, submitter.Object))
                .Returns(jetTokenResponse);

            var tokenResponse = mock.Mock<IJetToken>();
            mock.MockFunc<string, IJetToken>(tokenResponse);

            var jetStoreEntity = new JetStoreEntity()
            {
                ApiUser = "username",
                Secret = "secret"
            };

            var encryptionProvider = mock.CreateMock<IEncryptionProvider>();
            encryptionProvider.Setup(p => p.Decrypt(It.IsAny<string>()))
                .Returns("decrypted password");

            mock.Mock<IEncryptionProviderFactory>()
                .Setup(e => e.CreateSecureTextEncryptionProvider(It.IsAny<string>()))
                .Returns(encryptionProvider.Object);

            mock.Create<JetTokenRepository>().GetToken(jetStoreEntity);

            mock.Mock<IHttpRequestSubmitterFactory>().Verify(
                f => f.GetHttpTextPostRequestSubmitter("{\"user\": \"username\",\"pass\":\"decrypted password\"}",
                    "application/json"), Times.Once);
        }

        [Fact]
        public void GetTokenWithStore_GetHttpTextPostRequest_CreatesEncryptionProvider_WithCorrectSalt()
        {
            var submitter = mock.FromFactory<IHttpRequestSubmitterFactory>()
                .Mock(f => f.GetHttpTextPostRequestSubmitter(It.IsAny<string>(), "application/json"));

            var jetTokenResponse = new JetTokenResponse()
            {
                Token = "the token"
            };

            mock.Mock<IJsonRequest>()
                .Setup(r => r.ProcessRequest<JetTokenResponse>("GetToken", ApiLogSource.Jet, submitter.Object))
                .Returns(jetTokenResponse);

            var tokenResponse = mock.Mock<IJetToken>();
            mock.MockFunc<string, IJetToken>(tokenResponse);

            var jetStoreEntity = new JetStoreEntity()
            {
                ApiUser = "username",
                Secret = "secret"
            };

            var encryptionProvider = mock.CreateMock<IEncryptionProvider>();
            encryptionProvider.Setup(p => p.Decrypt(It.IsAny<string>()))
                .Returns("decrypted password");

            mock.Mock<IEncryptionProviderFactory>()
                .Setup(e => e.CreateSecureTextEncryptionProvider(It.IsAny<string>()))
                .Returns(encryptionProvider.Object);

            mock.Create<JetTokenRepository>().GetToken(jetStoreEntity);

            mock.Mock<IEncryptionProviderFactory>()
                .Verify(e => e.CreateSecureTextEncryptionProvider("username"), Times.Once);
        }

        [Fact]
        public void GetTokenWithStore_GetHttpTextPostRequest_DecryptsApiPassword()
        {
            var submitter = mock.FromFactory<IHttpRequestSubmitterFactory>()
                .Mock(f => f.GetHttpTextPostRequestSubmitter(It.IsAny<string>(), "application/json"));

            var jetTokenResponse = new JetTokenResponse()
            {
                Token = "the token"
            };

            mock.Mock<IJsonRequest>()
                .Setup(r => r.ProcessRequest<JetTokenResponse>("GetToken", ApiLogSource.Jet, submitter.Object))
                .Returns(jetTokenResponse);

            var tokenResponse = mock.Mock<IJetToken>();
            mock.MockFunc<string, IJetToken>(tokenResponse);

            var jetStoreEntity = new JetStoreEntity()
            {
                ApiUser = "username",
                Secret = "secret"
            };

            var encryptionProvider = mock.CreateMock<IEncryptionProvider>();
            encryptionProvider.Setup(p => p.Decrypt(It.IsAny<string>()))
                .Returns("decrypted password");

            mock.Mock<IEncryptionProviderFactory>()
                .Setup(e => e.CreateSecureTextEncryptionProvider(It.IsAny<string>()))
                .Returns(encryptionProvider.Object);

            mock.Create<JetTokenRepository>().GetToken(jetStoreEntity);

            encryptionProvider.Verify(p=>p.Decrypt("secret"), Times.Once);
        }

        public void Dispose()
        {
            mock.Dispose();
        }

    }
}