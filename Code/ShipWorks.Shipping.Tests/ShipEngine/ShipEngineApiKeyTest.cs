﻿using Autofac.Extras.Moq;
using Interapptive.Shared.Security;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;
using ShipWorks.ShipEngine;
using ShipWorks.Tests.Shared;
using System;
using Xunit;
using System.Threading.Tasks;

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
        public void Configure_ValueIsBlank_WhenCreateNewAccountThrowsShipEngineException()
        {
            mock.Mock<IShippingSettings>()
               .Setup(s => s.Fetch())
               .Returns(new ShippingSettingsEntity());

            mock.Mock<IShipEnginePartnerWebClient>()
                .Setup(c => c.CreateNewAccount(It.IsAny<string>()))
                .Throws(new ShipEngineException("error"));

            var testObject = mock.Create<ShipEngineApiKey>();
            testObject.Configure();

            Assert.True(string.IsNullOrEmpty(testObject.Value));
        }
        
        [Fact]
        public void Configure_ValueIsBlank_WhenGetApiKeyThrowsShipEngineException()
        {
            mock.Mock<IShippingSettings>()
               .Setup(s => s.Fetch())
               .Returns(new ShippingSettingsEntity());

            mock.Mock<IShipEnginePartnerWebClient>()
                .Setup(c => c.GetApiKey(It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new ShipEngineException("error"));

            var testObject = mock.Create<ShipEngineApiKey>();
            testObject.Configure();

            Assert.True(string.IsNullOrEmpty(testObject.Value));
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

        [Fact]
        public async Task ConfigureAsync_SetsValueToShipEngineApiKey_FromSettings()
        {
            mock.Mock<IShippingSettings>()
                .Setup(s => s.Fetch())
                .Returns(new ShippingSettingsEntity() { ShipEngineApiKey = "apikey" });

            var testObject = mock.Create<ShipEngineApiKey>();
            await testObject.ConfigureAsync();

            Assert.Equal("apikey", testObject.Value);
        }

        [Fact]
        public async Task ConfigureAsync_SetsValueToShipEngineApiKey_WhenShippingSettingsKeyBlank_FromPartnerClient()
        {
            mock.Mock<IShippingSettings>()
               .Setup(s => s.Fetch())
               .Returns(new ShippingSettingsEntity());

            mock.Mock<IShipEnginePartnerWebClient>()
                .Setup(c => c.GetApiKeyAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync("newkey");

            var testObject = mock.Create<ShipEngineApiKey>();
            await testObject.ConfigureAsync();

            Assert.Equal("newkey", testObject.Value);
        }

        [Fact]
        public async Task ConfigureAsync_UsesAccountNumberWhenGettingApiKey()
        {
            mock.Mock<IShippingSettings>()
               .Setup(s => s.Fetch())
               .Returns(new ShippingSettingsEntity());

            mock.Mock<IShipEnginePartnerWebClient>()
                .Setup(c => c.CreateNewAccountAsync(It.IsAny<string>()))
                .ReturnsAsync("accountId");

            var testObject = mock.Create<ShipEngineApiKey>();
            await testObject.ConfigureAsync();

            mock.Mock<IShipEnginePartnerWebClient>()
                .Verify(c => c.GetApiKeyAsync(It.IsAny<string>(), "accountId"), Times.Once());
        }

        [Fact]
        public async Task ConfigureAsync_UsePartnerApiKeyWhenGettingApiKey()
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
            await testObject.ConfigureAsync();

            mock.Mock<IShipEnginePartnerWebClient>()
                .Verify(c => c.GetApiKeyAsync("decrypted", It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task ConfigureAsync_UsesPartnerIdWhenCreatingNewAccount()
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
            await testObject.ConfigureAsync();

            mock.Mock<IShipEnginePartnerWebClient>()
                .Verify(c => c.CreateNewAccountAsync("decrypted"), Times.Once());
        }

        [Fact]
        public async Task ConfigureAsync_ValueIsBlank_WhenCreateNewAccountThrowsShipEngineException()
        {
            mock.Mock<IShippingSettings>()
               .Setup(s => s.Fetch())
               .Returns(new ShippingSettingsEntity());

            mock.Mock<IShipEnginePartnerWebClient>()
                .Setup(c => c.CreateNewAccount(It.IsAny<string>()))
                .Throws(new ShipEngineException("error"));

            var testObject = mock.Create<ShipEngineApiKey>();
            await testObject.ConfigureAsync();

            Assert.True(string.IsNullOrEmpty(testObject.Value));
        }

        [Fact]
        public async Task ConfigureAsync_ValueIsBlank_WhenGetApiKeyThrowsShipEngineException()
        {
            mock.Mock<IShippingSettings>()
               .Setup(s => s.Fetch())
               .Returns(new ShippingSettingsEntity());

            mock.Mock<IShipEnginePartnerWebClient>()
                .Setup(c => c.GetApiKey(It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new ShipEngineException("error"));

            var testObject = mock.Create<ShipEngineApiKey>();
            await testObject.ConfigureAsync();

            Assert.True(string.IsNullOrEmpty(testObject.Value));
        }

        [Fact]
        public async Task ConfigureAsync_DecryptsPartnerApiKey()
        {
            mock.Mock<IShippingSettings>()
              .Setup(s => s.Fetch())
              .Returns(new ShippingSettingsEntity());

            var encryptionProvider = mock.FromFactory<IEncryptionProviderFactory>()
                .Mock(f => f.CreateSecureTextEncryptionProvider(It.IsAny<string>()));

            var testObject = mock.Create<ShipEngineApiKey>();
            await testObject.ConfigureAsync();

            encryptionProvider.Verify(p => p.Decrypt("Auapk4J9PBSgT+Luq91kHHGNhTddMY2y0Ih7x0/7V5bjZ1FQE2yF7WyR7oR0e0DA"), Times.Once());
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
