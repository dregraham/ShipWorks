using ShipWorks.Stores.Platforms.Shopify;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Shopify
{
    public class ShopifyHelperTest
    {
        [Theory]
        [InlineData("null<br/>Note attribute: NA value<br/>Note attribute2: NA value2", new [] { "null", "Note attribute: NA value", "Note attribute2: NA value2" })]
        [InlineData("null<br/>Note attribute: NA value", new [] { "null", "Note attribute: NA value" })]
        [InlineData("abc<br/>Note attribute: NA value", new [] { "abc", "Note attribute: NA value" })]
        [InlineData("<br/>Note attribute: NA value", new [] { "", "Note attribute: NA value" })]
        [InlineData("abc<br/>", new [] { "abc", "" })]
        [InlineData("abc", new [] { "abc" })]
        [InlineData("<br/>", new [] { "", "" })]
        [InlineData("", new [] { "" })]
        public void ShopifyHelper_GetSplitNotes(string text, string[] output)
        {
            var result = ShopifyHelper.GetSplitNotes(text);

            Assert.Equal(output, result);
        }
    }
}
