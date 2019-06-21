using Interapptive.Shared.Utility;
using Xunit;

namespace Interapptive.Shared.Tests.Utility
{
    public class HtmlEntityUtilityTest
    {
        [Fact]
        public void DecodeHtmlWithoutXml_DecodesSingleEncodedHtml()
        {
            string testString = "<html><body>This String&trade; is &copy; and &reg;</body></html>";
            string expected = "<html><body>This String™ is © and ®</body></html>";
            string actual = HtmlEntityUtility.DecodeHtmlWithoutXml(testString);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DecodeHtmlWithoutXml_DecodesMultiEncodedHtml()
        {
            string testString = "<div>This String&amp;amp;amp;amp;amp;amp;trade; is &amp;amp;amp;amp;amp;copy;</div>";
            string expected = "<div>This String™ is ©</div>";
            string actual = HtmlEntityUtility.DecodeHtmlWithoutXml(testString);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DecodeHtmlWithoutXml_LeavesSingleEncodedXmlEncoded()
        {
            string testString = "<?xml version=\"1.0\" encoding=\"UTF - 8\" standalone=\"no\" ?> <root>by the &lt;&gt; Brother&apos;s &quot;Strings &amp; More&quot; Co.</root>";
            string expected = "<?xml version=\"1.0\" encoding=\"UTF - 8\" standalone=\"no\" ?> <root>by the &lt;&gt; Brother&apos;s &quot;Strings &amp; More&quot; Co.</root>";
            string actual = HtmlEntityUtility.DecodeHtmlWithoutXml(testString);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DecodeHtmlWithoutXml_LeavesMultiEncodedXmlSingleEncoded()
        {
            string testString = "<parentnode><node12>This &amp;amp;amp;amp;amp; That &amp;amp;amp;quot;</node12></parentnode>";
            string expected = "<parentnode><node12>This &amp; That &quot;</node12></parentnode>";
            string actual = HtmlEntityUtility.DecodeHtmlWithoutXml(testString);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DecodeHtmlWithoutXml_LeavesSingleXmlEncoded_WhenMultiEncodedHtml()
        {
            string testString = "<node attribute=\"test\">This &amp; That&amp;amp;amp;trade;</node>";
            string expected = "<node attribute=\"test\">This &amp; That™</node>";
            string actual = HtmlEntityUtility.DecodeHtmlWithoutXml(testString);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DecodeHtmlWithoutXml_LeavesMultiEncodedXmlSingleEncoded_WhenMultiEncodedHtml()
        {
            string testString = "<node x='y' z='q'>This &amp;amp;amp;amp; &amp;amp;lt; That&amp;amp;amp;amp;amp;amp;amp;amp;trade;</node>";
            string expected = "<node x='y' z='q'>This &amp; &lt; That™</node>";
            string actual = HtmlEntityUtility.DecodeHtmlWithoutXml(testString);

            Assert.Equal(expected, actual);
        }
    }
}
