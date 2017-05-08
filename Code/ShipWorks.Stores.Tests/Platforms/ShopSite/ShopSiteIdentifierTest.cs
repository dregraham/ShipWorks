using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.Security;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.ShopSite;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.ShopSite
{
    public class ShopSiteIdentifierTest : IDisposable
    {
        readonly AutoMock mock;

        public ShopSiteIdentifierTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Theory]
        [InlineData("Foo")]
        [InlineData("Bar")]
        public void Get_DelegatesToEncryptionProvider(string identifier)
        {
            var encryptionProvider = mock.FromFactory<IEncryptionProviderFactory>()
                .Mock(x => x.CreateShopSiteEncryptionProvider());
            var store = mock.CreateMock<IShopSiteStoreEntity>(x => x.SetupGet(y => y.Identifier).Returns(identifier)).Object;
            var testObject = mock.Create<ShopSiteIdentifier>();

            testObject.Get(store);

            encryptionProvider.Verify(x => x.Decrypt(identifier));
        }

        [Theory]
        [InlineData("Foo")]
        [InlineData("Bar")]
        public void Get_ReturnsDecryptedIdentifier(string identifier)
        {
            mock.FromFactory<IEncryptionProviderFactory>()
                .Mock(x => x.CreateShopSiteEncryptionProvider())
                .Setup(x => x.Decrypt(It.IsAny<string>()))
                .Returns(identifier);
            var testObject = mock.Create<ShopSiteIdentifier>();

            var result = testObject.Get(mock.Create<IShopSiteStoreEntity>());

            Assert.Equal(identifier, result);
        }

        [Fact]
        public void Set_DoesNotChangeIdentifier_WhenIdentifierIsNotEmpty()
        {
            var store = new ShopSiteStoreEntity { Identifier = "Foo" };
            var testObject = mock.Create<ShopSiteIdentifier>();

            var result = testObject.Set(store, "Bar");

            Assert.Equal("Foo", result.Identifier);
        }

        [Theory]
        [InlineData("http://www.foo.com", "http://www.foo.com")]
        [InlineData("http://beta.shopsite.com/cgi-bin/ss/db_xml.cgi", "http://beta.shopsite.com/cgi-bin/ss/")]
        [InlineData("http://BETA.shopsite.com/cgi-bin/ss/db_xml.cgi", "http://beta.shopsite.com/cgi-bin/ss/")]
        [InlineData("http://beta.shopsite.com/cgi-bin/ss/authorize.cgi", "http://beta.shopsite.com/cgi-bin/ss/")]
        [InlineData("http://BETA.shopsite.com/cgi-bin/ss/authorize.cgi", "http://beta.shopsite.com/cgi-bin/ss/")]
        [InlineData("http://www.foo.com/bar/", "http://www.foo.com/bar/")]
        public void Set_DelegatesToEncryptionProvider_WithCleansedIdentifier(string input, string cleansed)
        {
            var encryptionProvider = mock.FromFactory<IEncryptionProviderFactory>()
                   .Mock(x => x.CreateShopSiteEncryptionProvider());
            var store = new ShopSiteStoreEntity { ApiUrl = input };
            var testObject = mock.Create<ShopSiteIdentifier>();

            testObject.Set(store, "Bar");

            encryptionProvider.Verify(x => x.Encrypt(cleansed));
        }

        [Theory]
        [InlineData("Foo")]
        [InlineData("Bar")]
        public void Set_SetsEncryptedIdentifier_OnStore(string input)
        {
            mock.FromFactory<IEncryptionProviderFactory>()
                .Mock(x => x.CreateShopSiteEncryptionProvider())
                .Setup(x => x.Encrypt(It.IsAny<string>()))
                .Returns(input);
            var store = new ShopSiteStoreEntity { ApiUrl = "stuff" };
            var testObject = mock.Create<ShopSiteIdentifier>();

            var result = testObject.Set(store, "Bar");

            Assert.Equal(input, result.Identifier);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
