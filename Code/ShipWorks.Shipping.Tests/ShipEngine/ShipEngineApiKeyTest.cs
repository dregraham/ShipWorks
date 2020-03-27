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
                .Setup(c => c.CreateNewAccount())
                .ReturnsAsync("newkey");

            var testObject = mock.Create<ShipEngineApiKey>();
            await testObject.Configure();

            Assert.Equal("newkey", testObject.Value);
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
                .Verify(c => c.CreateNewAccount(), Times.Once());
        }

        [Fact]
        public async Task Configure_ValueIsBlank_WhenCreateNewAccountThrowsShipEngineException()
        {
            mock.Mock<IShippingSettings>()
               .Setup(s => s.Fetch())
               .Returns(new ShippingSettingsEntity());

            mock.Mock<IShipEnginePartnerWebClient>()
                .Setup(c => c.CreateNewAccount())
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
                .Setup(c => c.CreateNewAccount())
                .Throws(new ShipEngineException("error"));

            var testObject = mock.Create<ShipEngineApiKey>();
            await testObject.Configure();

            Assert.True(string.IsNullOrEmpty(testObject.Value));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
