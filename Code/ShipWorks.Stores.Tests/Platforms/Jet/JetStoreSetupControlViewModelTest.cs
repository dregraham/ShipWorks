using Autofac.Extras.Moq;
using Interapptive.Shared.Security;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Jet;
using ShipWorks.Stores.UI.Platforms.Jet;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Jet
{
    public class JetStoreSetupControlViewModelTest
    {
        private readonly AutoMock mock;
        private readonly Mock<IEncryptionProviderFactory> encryptionProviderFactory;
        private readonly Mock<IEncryptionProvider> encryptionProvider;
        private readonly JetStoreEntity jetStore;
        private readonly JetStoreSetupControlViewModel testObject;

        public JetStoreSetupControlViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            encryptionProvider = mock.Mock<IEncryptionProvider>();
            encryptionProvider.Setup(e => e.Decrypt("EncryptedSecret")).Returns("DecryptedSecret");
            encryptionProvider.Setup(e => e.Encrypt("DecryptedSecret")).Returns("EncryptedSecret");

            encryptionProviderFactory = mock.Mock<IEncryptionProviderFactory>();

            encryptionProviderFactory.Setup(e => e.CreateSecureTextEncryptionProvider(It.IsAny<string>()))
                .Returns(encryptionProvider.Object);

            jetStore = new JetStoreEntity
            {
                ApiUser = "test User",
                Secret = "EncryptedSecret"
            };

            testObject = mock.Create<JetStoreSetupControlViewModel>();
        }

        [Fact]
        public void Load_SetsApiUserSecretFromStore()
        {
            testObject.Load(jetStore);

            Assert.Equal("test User", testObject.ApiUser);
            Assert.Equal("DecryptedSecret", testObject.Secret);
        }

        [Fact]
        public void Load_UsesUsernameAsSaltForEncryptionProviderFactory()
        {
            testObject.Load(jetStore);

            encryptionProviderFactory.Verify(e => e.CreateSecureTextEncryptionProvider("test User"));
        }

        [Fact]
        public void Load_DelegatesToEncryptionProviderWithSecret()
        {
            testObject.Load(jetStore);
            
            encryptionProvider.Verify(e => e.Decrypt("EncryptedSecret"));
        }

        [Fact]
        public void Save_DelegatesToWebClientForToken()
        {
            var webClient = mock.Mock<IJetWebClient>();
            testObject.ApiUser = "test User";
            testObject.Secret = "TestSecret";

            testObject.Save(jetStore);

            webClient.Verify(w => w.GetToken("test User", "TestSecret"));
        }

        [Fact]
        public void Save_ReturnsTrue_WhenGetTokenSucceeds()
        {
            mock.Mock<IJetWebClient>().Setup(w => w.GetToken("test User", "TestSecret"))
                .Returns(GenericResult.FromSuccess(""));

            testObject.ApiUser = "test User";
            testObject.Secret = "TestSecret";

            Assert.True(testObject.Save(jetStore));
        }

        [Fact]
        public void Save_DelegatesToMessageHelperWithError_WhenGetTokenFails()
        {
            var webClient = mock.Mock<IJetWebClient>();
            webClient.Setup(w => w.GetToken("test User", "TestSecret")).Returns(GenericResult.FromError<string>("Something went wrong"));

            testObject.ApiUser = "test User";
            testObject.Secret = "TestSecret";

            testObject.Save(jetStore);

            mock.Mock<IMessageHelper>().Verify(m => m.ShowError("Something went wrong"));
        }

        [Fact]
        public void Save_ReturnsFalse_WhenGetTokenFails()
        {
            var webClient = mock.Mock<IJetWebClient>();
            webClient.Setup(w => w.GetToken("test User", "TestSecret")).Returns(GenericResult.FromError<string>("Something went wrong"));

            testObject.ApiUser = "test User";
            testObject.Secret = "TestSecret";

            Assert.False(testObject.Save(jetStore));
        }

        [Fact]
        public void Save_SetsApiUser()
        {
            mock.Mock<IJetWebClient>().Setup(w => w.GetToken("test User", "DecryptedSecret"))
                .Returns(GenericResult.FromSuccess(""));

            testObject.ApiUser = "test User";
            testObject.Secret = "DecryptedSecret";

            JetStoreEntity newStore = new JetStoreEntity();

            testObject.Save(newStore);

            Assert.Equal("EncryptedSecret", newStore.Secret);
            Assert.Equal("test User", newStore.ApiUser);
        }
    }
}