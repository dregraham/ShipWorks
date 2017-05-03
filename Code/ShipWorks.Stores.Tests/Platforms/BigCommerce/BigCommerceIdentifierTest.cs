using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.Security;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.BigCommerce;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.BigCommerce
{
    public class BigCommerceIdentifierTest : IDisposable
    {
        readonly AutoMock mock;

        public BigCommerceIdentifierTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Theory]
        [InlineData("Foo")]
        [InlineData("Bar")]
        public void Get_DelegatesToEncryptionProvider(string identifier)
        {
            var encryptionProvider = mock.FromFactory<IEncryptionProviderFactory>()
                .Mock(x => x.CreateBigCommerceEncryptionProvider());
            var store = mock.CreateMock<IBigCommerceStoreEntity>(x => x.SetupGet(y => y.Identifier).Returns(identifier)).Object;
            var testObject = mock.Create<BigCommerceIdentifier>();

            testObject.Get(store);

            encryptionProvider.Verify(x => x.Decrypt(identifier));
        }

        [Theory]
        [InlineData("Foo")]
        [InlineData("Bar")]
        public void Get_ReturnsDecryptedIdentifier(string identifier)
        {
            mock.FromFactory<IEncryptionProviderFactory>()
                .Mock(x => x.CreateBigCommerceEncryptionProvider())
                .Setup(x => x.Decrypt(It.IsAny<string>()))
                .Returns(identifier);
            var testObject = mock.Create<BigCommerceIdentifier>();

            var result = testObject.Get(mock.Create<IBigCommerceStoreEntity>());

            Assert.Equal(identifier, result);
        }

        [Fact]
        public void Set_DoesNotChangeIdentifier_WhenIdentifierIsNotEmpty()
        {
            var store = new BigCommerceStoreEntity { Identifier = "Foo" };
            var testObject = mock.Create<BigCommerceIdentifier>();

            var result = testObject.Set(store, "Bar");

            Assert.Equal("Foo", result.Identifier);
        }

        [Theory]
        [InlineData("http://www.foo.com", "http://www.foo.com")]
        [InlineData("http://www.foo.com/api/v2", "http://www.foo.com/api/")]
        [InlineData("http://www.foo.com/api/v2/", "http://www.foo.com/")]
        [InlineData("http://www.foo.com/admin/shipworks.php", "http://www.foo.com/")]
        [InlineData("http://www.foo.com/shipworks.php", "http://www.foo.com/")]
        [InlineData("http://www.foo.com/bar", "http://www.foo.com/")]
        [InlineData("http://www.foo.com/bar/", "http://www.foo.com/bar/")]
        public void Set_DelegatesToEncryptionProvider_WithCleansedIdentifier(string input, string cleansed)
        {
            var encryptionProvider = mock.FromFactory<IEncryptionProviderFactory>()
                   .Mock(x => x.CreateBigCommerceEncryptionProvider());
            var store = new BigCommerceStoreEntity { ApiUrl = input };
            var testObject = mock.Create<BigCommerceIdentifier>();

            testObject.Set(store, "Bar");

            encryptionProvider.Verify(x => x.Encrypt(cleansed));
        }

        [Theory]
        [InlineData("Foo")]
        [InlineData("Bar")]
        public void Set_SetsEncryptedIdentifier_OnStore(string input)
        {
            mock.FromFactory<IEncryptionProviderFactory>()
                .Mock(x => x.CreateBigCommerceEncryptionProvider())
                .Setup(x => x.Encrypt(It.IsAny<string>()))
                .Returns(input);
            var store = new BigCommerceStoreEntity { ApiUrl = "stuff" };
            var testObject = mock.Create<BigCommerceIdentifier>();

            var result = testObject.Set(store, "Bar");

            Assert.Equal(input, result.Identifier);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
