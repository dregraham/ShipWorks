using Autofac.Extras.Moq;
using Interapptive.Shared.Security;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Tests.Shared;
using System;
using Xunit;
using System.Threading.Tasks;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Shipping.Tests.ShipEngine
{
    public class ShipEngineApiKeyTest : IDisposable
    {
        AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks();

        [Fact]
        public async Task Configure_SetsValueToShipEngineApiKey_FromSettings()
        {
            mock.Mock<IShippingSettings>()
                .Setup(s => s.Fetch())
                .Returns(new ShippingSettingsEntity() { ShipEngineApiKey = "apikey" });

            var testObject = mock.Create<ShipEngineApiKey>();
            await testObject.Configure();

            Assert.Equal("apikey", testObject.Value);
        }

        [Fact]
        public async Task Configure_SetsValueToShipEngineApiKey_WhenShippingSettingsKeyBlank_FromPartnerClient()
        {
            mock.Mock<IShippingSettings>()
               .Setup(s => s.Fetch())
               .Returns(new ShippingSettingsEntity());

            mock.Mock<IShipEnginePartnerWebClient>()
                .Setup(c => c.GetApiKey(AnyString, AnyString))
                .ReturnsAsync("newkey");

            var testObject = mock.Create<ShipEngineApiKey>();
            await testObject.Configure();

            Assert.Equal("newkey", testObject.Value);
        }

        [Fact]
        public async Task Configure_UsesAccountNumberWhenGettingApiKey()
        {
            mock.Mock<IShippingSettings>()
               .Setup(s => s.Fetch())
               .Returns(new ShippingSettingsEntity());

            mock.Mock<IShipEnginePartnerWebClient>()
                .Setup(c => c.CreateNewAccount(AnyString))
                .ReturnsAsync("accountId");

            var testObject = mock.Create<ShipEngineApiKey>();
            await testObject.Configure();

            mock.Mock<IShipEnginePartnerWebClient>()
                .Verify(c => c.GetApiKey(AnyString, "accountId"), Times.Once());
        }

        [Fact]
        public async Task Configure_UsePartnerApiKeyWhenGettingApiKey()
        {
            mock.Mock<IShippingSettings>()
               .Setup(s => s.Fetch())
               .Returns(new ShippingSettingsEntity());

            var encryptionProvider = mock.FromFactory<IEncryptionProviderFactory>()
                .Mock(f => f.CreateSecureTextEncryptionProvider(AnyString));

            encryptionProvider
                .Setup(p => p.Decrypt(AnyString))
                .Returns("decrypted");

            var testObject = mock.Create<ShipEngineApiKey>();
            await testObject.Configure();

            mock.Mock<IShipEnginePartnerWebClient>()
                .Verify(c => c.GetApiKey("decrypted", AnyString), Times.Once());
        }

        [Fact]
        public async Task Configure_UsesPartnerIdWhenCreatingNewAccount()
        {
            mock.Mock<IShippingSettings>()
              .Setup(s => s.Fetch())
              .Returns(new ShippingSettingsEntity());

            var encryptionProvider = mock.FromFactory<IEncryptionProviderFactory>()
                .Mock(f => f.CreateSecureTextEncryptionProvider(AnyString));

            encryptionProvider
                .Setup(p => p.Decrypt(AnyString))
                .Returns("decrypted");

            var testObject = mock.Create<ShipEngineApiKey>();
            await testObject.Configure();

            mock.Mock<IShipEnginePartnerWebClient>()
                .Verify(c => c.CreateNewAccount("decrypted"), Times.Once());
        }

        [Fact]
        public async Task Configure_ValueIsBlank_WhenCreateNewAccountThrowsShipEngineException()
        {
            mock.Mock<IShippingSettings>()
               .Setup(s => s.Fetch())
               .Returns(new ShippingSettingsEntity());

            mock.Mock<IShipEnginePartnerWebClient>()
                .Setup(c => c.CreateNewAccount(AnyString))
                .Throws(new ShipEngineException("error"));

            var testObject = mock.Create<ShipEngineApiKey>();
            await testObject.Configure();

            Assert.True(string.IsNullOrEmpty(testObject.Value));
        }

        [Fact]
        public async Task Configure_ValueIsBlank_WhenGetApiKeyThrowsShipEngineException()
        {
            mock.Mock<IShippingSettings>()
               .Setup(s => s.Fetch())
               .Returns(new ShippingSettingsEntity());

            mock.Mock<IShipEnginePartnerWebClient>()
                .Setup(c => c.GetApiKey(AnyString, AnyString))
                .Throws(new ShipEngineException("error"));

            var testObject = mock.Create<ShipEngineApiKey>();
            await testObject.Configure();

            Assert.True(string.IsNullOrEmpty(testObject.Value));
        }

        [Fact]
        public async Task Configure_DecryptsPartnerApiKey()
        {
            mock.Mock<IShippingSettings>()
              .Setup(s => s.Fetch())
              .Returns(new ShippingSettingsEntity());

            var encryptionProvider = mock.FromFactory<IEncryptionProviderFactory>()
                .Mock(f => f.CreateSecureTextEncryptionProvider(AnyString));

            var testObject = mock.Create<ShipEngineApiKey>();
            await testObject.Configure();

            encryptionProvider.Verify(p => p.Decrypt("Auapk4J9PBSgT+Luq91kHHGNhTddMY2y0Ih7x0/7V5bjZ1FQE2yF7WyR7oR0e0DA"), Times.Once());
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
