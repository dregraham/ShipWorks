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
        [InlineData("   zjami83rplavi5tzmygipk9kwqynovv8")]
        [InlineData("\"zjami83rplavi5tzmygipk9kwqynovv8\"")]
        [InlineData("       \" zjami83rplavi5tzmygipk9kwqynovv8 \"")]
        [InlineData("zjami83rplavi5tzmygipk9kwqynovv8")]
        [InlineData(" \"zjami83rplavi5tzmygipk9kwqynovv8\"")]
        [InlineData(" \"zjami83rplavi5tzmygipk9kwqynovv8")]
        [InlineData("\n\n\t\n\"\t    zjami83rplavi5tzmygipk9kwqynovv8\"")]
        [InlineData("    \"zjami83rplavi5tzmygipk9kwqynovv8")]
        public void TrimToken_RemovesWhiteSpaceAndDoubleQuotes(string token)
        {
            var result = testObject.TrimToken(token);
            Assert.Equal("zjami83rplavi5tzmygipk9kwqynovv8", result);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}