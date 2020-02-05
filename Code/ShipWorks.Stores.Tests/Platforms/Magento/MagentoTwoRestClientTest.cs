using System;
using Autofac;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Magento;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Magento
{
    public class MagentoTwoRestClientTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly MagentoTwoRestClient testObject;

        public MagentoTwoRestClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            var store = new MagentoStoreEntity()
            {
                ModuleUrl = "https://www.url.com/",
                ModuleUsername = "dude",
                ModulePassword = "sweet",
                StoreTypeCode = StoreTypeCode.Magento
            };

            testObject = mock.Create<MagentoTwoRestClient>(TypedParameter.From(store));
        }

        [Theory]
        [InlineData("   test")]
        [InlineData("\"test\"")]
        [InlineData("       \" test \"")]
        [InlineData("test")]
        [InlineData(" \"test\"")]
        [InlineData(" \"test")]
        [InlineData("\n\n\t\n\"\t    test\"")]
        public void TrimToken_RemovesWhiteSpaceAndDoubleQuotes(string token)
        {
            var result = testObject.TrimToken(token);
            Assert.Equal("test", result);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}