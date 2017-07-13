using System;
using System.Net;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Interapptive.Shared.Security;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.ChannelAdvisor;
using ShipWorks.Stores.UI.Platforms.ChannelAdvisor;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.ChannelAdvisor
{
    public class ChannelAdvisorAccountSettingsViewModelTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly Mock<IEncryptionProvider> encryptionProvider;

        public ChannelAdvisorAccountSettingsViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            encryptionProvider = mock.CreateMock<IEncryptionProvider>();

            mock.Mock<IEncryptionProviderFactory>()
                .Setup(m => m.CreateSecureTextEncryptionProvider(It.IsAny<string>()))
                .Returns(encryptionProvider.Object);
        }

        [Fact]
        public void Save_SavesEncryptedRefreshTokenToStore()
        {
            encryptionProvider.Setup(e => e.Encrypt(It.IsAny<string>())).Returns("encrypted");

            mock.Mock<IChannelAdvisorRestClient>()
                .Setup(c => c.GetRefreshToken(It.IsAny<string>(), It.IsAny<string>()))
                .Returns("refreshToken");
            var store = new ChannelAdvisorStoreEntity {StoreTypeCode = StoreTypeCode.ChannelAdvisor};

            var testObject = mock.Create<ChannelAdvisorAccountSettingsViewModel>();
            testObject.AccessCode = "accessCode";
            testObject.Save(store);

            Assert.Equal("encrypted", store.RefreshToken);
        }

        [Fact]
        public void Save_DelegatesToWebClientWithAccessCode()
        {
            encryptionProvider.Setup(e => e.Encrypt(It.IsAny<string>())).Returns("encrypted");

            mock.Mock<IChannelAdvisorRestClient>()
                .Setup(c => c.GetRefreshToken(It.IsAny<string>(), It.IsAny<string>()))
                .Returns("refreshToken");
            var store = new ChannelAdvisorStoreEntity { StoreTypeCode = StoreTypeCode.ChannelAdvisor };

            var testObject = mock.Create<ChannelAdvisorAccountSettingsViewModel>();
            testObject.AccessCode = "accessCode";
            testObject.Save(store);

            mock.Mock<IChannelAdvisorRestClient>().Verify(c=>c.GetRefreshToken("accessCode", It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void Save_DelegatesToWebClientWithRedirectUrl()
        {
            encryptionProvider.Setup(e => e.Encrypt(It.IsAny<string>())).Returns("encrypted");

            mock.Mock<IChannelAdvisorRestClient>()
                .Setup(c => c.GetRefreshToken(It.IsAny<string>(), It.IsAny<string>()))
                .Returns("refreshToken");
            var store = new ChannelAdvisorStoreEntity { StoreTypeCode = StoreTypeCode.ChannelAdvisor };

            var testObject = mock.Create<ChannelAdvisorAccountSettingsViewModel>();
            testObject.AccessCode = "accessCode";
            testObject.Save(store);

            mock.Mock<IChannelAdvisorRestClient>().Verify(c => c.GetRefreshToken(It.IsAny<string>(), WebUtility.UrlEncode("https://www.interapptive.com/channeladvisor/subscribe.php")), Times.Once);
        }

        [Fact]
        public void Save_EncryptsRefreshToken()
        {
            encryptionProvider.Setup(e => e.Encrypt(It.IsAny<string>())).Returns("encrypted");

            mock.Mock<IChannelAdvisorRestClient>()
                .Setup(c => c.GetRefreshToken(It.IsAny<string>(), It.IsAny<string>()))
                .Returns("refreshToken");
            var store = new ChannelAdvisorStoreEntity { StoreTypeCode = StoreTypeCode.ChannelAdvisor };

            var testObject = mock.Create<ChannelAdvisorAccountSettingsViewModel>();
            testObject.AccessCode = "accessCode";
            testObject.Save(store);

            encryptionProvider.Verify(e=>e.Encrypt("refreshToken"), Times.Exactly(1));
        }

        [Fact]
        public void Save_CallsGetRefreshTokenOnce_WhenSaveCalledTwice_AndAccessCodeIsTheSame()
        {
            encryptionProvider.Setup(e => e.Encrypt(It.IsAny<string>())).Returns("encrypted");

            mock.Mock<IChannelAdvisorRestClient>()
                .Setup(c => c.GetRefreshToken(It.IsAny<string>(), It.IsAny<string>()))
                .Returns("refreshToken");
            var store = new ChannelAdvisorStoreEntity { StoreTypeCode = StoreTypeCode.ChannelAdvisor };

            var testObject = mock.Create<ChannelAdvisorAccountSettingsViewModel>();
            testObject.AccessCode = "accessCode";
            testObject.Save(store);
            mock.Mock<IChannelAdvisorRestClient>().Verify(c => c.GetRefreshToken(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            testObject.Save(store);

            mock.Mock<IChannelAdvisorRestClient>().Verify(c => c.GetRefreshToken(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void Save_CallsGetRefreshTokenTwice_WhenSaveCalledTwice_AndAccessCodeChanged()
        {
            encryptionProvider.Setup(e => e.Encrypt(It.IsAny<string>())).Returns("encrypted");

            mock.Mock<IChannelAdvisorRestClient>()
                .Setup(c => c.GetRefreshToken(It.IsAny<string>(), It.IsAny<string>()))
                .Returns("refreshToken");
            var store = new ChannelAdvisorStoreEntity { StoreTypeCode = StoreTypeCode.ChannelAdvisor };

            var testObject = mock.Create<ChannelAdvisorAccountSettingsViewModel>();
            testObject.AccessCode = "accessCode";
            testObject.Save(store);
            mock.Mock<IChannelAdvisorRestClient>().Verify(c => c.GetRefreshToken(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            testObject.AccessCode = "accessCode2";
            testObject.Save(store);

            mock.Mock<IChannelAdvisorRestClient>().Verify(c => c.GetRefreshToken(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}