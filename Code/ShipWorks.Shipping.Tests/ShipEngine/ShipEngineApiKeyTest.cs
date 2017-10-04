using Autofac.Extras.Moq;
using Interapptive.Shared.Security;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Tests.Shared;
using System;
using Xunit;

namespace ShipWorks.Shipping.Tests.ShipEngine
{
    public class ShipEngineApiKeyTest : IDisposable
    {
        AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks();

        [Fact]
        public void Configure_SetsValueToShipEngineApiKey_FromSettings()
        {
            mock.Mock<IShippingSettings>()
                .Setup(s => s.Fetch())
                .Returns(new ShippingSettingsEntity() { ShipEngineApiKey = "apikey" });

            var testObject = mock.Create<ShipEngineApiKey>();
            testObject.Configure();

            Assert.Equal("apikey", testObject.Value);
        }

        [Fact]
        public void Configure_SetsValueToShipEngineApiKey_WhenShippingSettingsKeyBlank_FromPartnerClient()
        {
            mock.Mock<IShippingSettings>()
               .Setup(s => s.Fetch())
               .Returns(new ShippingSettingsEntity());

            mock.Mock<IShipEnginePartnerWebClient>()
                .Setup(c => c.GetApiKey(It.IsAny<string>(), It.IsAny<string>()))
                .Returns("newkey");

            var testObject = mock.Create<ShipEngineApiKey>();
            testObject.Configure();

            Assert.Equal("newkey", testObject.Value);
        }

        [Fact]
        public void Configure_UsesAccountNumberWhenGettingApiKey()
        {
            mock.Mock<IShippingSettings>()
               .Setup(s => s.Fetch())
               .Returns(new ShippingSettingsEntity());

            mock.Mock<IShipEnginePartnerWebClient>()
                .Setup(c => c.CreateNewAccount(It.IsAny<string>()))
                .Returns("accountId");

            var testObject = mock.Create<ShipEngineApiKey>();
            testObject.Configure();

            mock.Mock<IShipEnginePartnerWebClient>()
                .Verify(c => c.GetApiKey(It.IsAny<string>(), "accountId"), Times.Once());
        }

        [Fact]
        public void Configure_UsePartnerApiKeyWhenGettingApiKey()
        {
            mock.Mock<IShippingSettings>()
               .Setup(s => s.Fetch())
               .Returns(new ShippingSettingsEntity());

            var encryptionProvider = mock.FromFactory<IEncryptionProviderFactory>()
                .Mock(f => f.CreateSecureTextEncryptionProvider(It.IsAny<string>()));

            encryptionProvider
                .Setup(p => p.Decrypt(It.IsAny<string>()))
                .Returns("decrypted");

            var testObject = mock.Create<ShipEngineApiKey>();
            testObject.Configure();

            mock.Mock<IShipEnginePartnerWebClient>()
                .Verify(c => c.GetApiKey("decrypted", It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public void Configure_UsesPartnerIdWhenCreatingNewAccount()
        {
            mock.Mock<IShippingSettings>()
              .Setup(s => s.Fetch())
              .Returns(new ShippingSettingsEntity());

            var encryptionProvider = mock.FromFactory<IEncryptionProviderFactory>()
                .Mock(f => f.CreateSecureTextEncryptionProvider(It.IsAny<string>()));

            encryptionProvider
                .Setup(p => p.Decrypt(It.IsAny<string>()))
                .Returns("decrypted");

            var testObject = mock.Create<ShipEngineApiKey>();
            testObject.Configure();

            mock.Mock<IShipEnginePartnerWebClient>()
                .Verify(c => c.CreateNewAccount("decrypted"), Times.Once());
        }

        [Fact]
        public void Configure_DecryptsPartnerApiKey()
        {
            mock.Mock<IShippingSettings>()
              .Setup(s => s.Fetch())
              .Returns(new ShippingSettingsEntity());

            var encryptionProvider = mock.FromFactory<IEncryptionProviderFactory>()
                .Mock(f => f.CreateSecureTextEncryptionProvider(It.IsAny<string>()));

            var testObject = mock.Create<ShipEngineApiKey>();
            testObject.Configure();

            encryptionProvider.Verify(p => p.Decrypt("Auapk4J9PBSgT+Luq91kHHGNhTddMY2y0Ih7x0/7V5bjZ1FQE2yF7WyR7oR0e0DA"), Times.Once());
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
