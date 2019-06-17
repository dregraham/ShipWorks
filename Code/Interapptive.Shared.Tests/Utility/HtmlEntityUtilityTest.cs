using Interapptive.Shared.Utility;
using Xunit;

namespace Interapptive.Shared.Tests.Utility
{
    public class HtmlEntityUtilityTest
    {
        [Fact]
        public void DecodeHtmlWithoutXml_DecodesSingleEncodedHtml()
        {
            string testString = "This String&trade; is &copy; and &reg;";
            string expected = "This String™ is © and ®";
            string actual = HtmlEntityUtility.DecodeHtmlWithoutXml(testString);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DecodeHtmlWithoutXml_DecodesMultiEncodedHtml()
        {
            string testString = "This String&amp;amp;amp;amp;amp;amp;trade; is &amp;amp;amp;amp;amp;copy;";
            string expected = "This String™ is ©";
            string actual = HtmlEntityUtility.DecodeHtmlWithoutXml(testString);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DecodeHtmlWithoutXml_LeavesSingleEncodedXmlEncoded()
        {
            string testString = "by the &lt;&gt; Brother&apos;s &quot;Strings &amp; More&quot; Co.";
            string expected = "by the &lt;&gt; Brother&apos;s &quot;Strings &amp; More&quot; Co.";
            string actual = HtmlEntityUtility.DecodeHtmlWithoutXml(testString);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DecodeHtmlWithoutXml_LeavesMultiEncodedXmlSingleEncoded()
        {
            string testString = "This &amp;amp;amp;amp;amp; That &amp;amp;amp;quot;";
            string expected = "This &amp; That \"";
            string actual = HtmlEntityUtility.DecodeHtmlWithoutXml(testString);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DecodeHtmlWithoutXml_LeavesSingleXmlEncoded_WhenMultiEncodedHtml()
        {
            string testString = "This &amp; That&amp;amp;amp;trade;";
            string expected = "This &amp; That™";
            string actual = HtmlEntityUtility.DecodeHtmlWithoutXml(testString);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DecodeHtmlWithoutXml_LeavesMultiEncodedXmlSingleEncoded_WhenMultiEncodedHtml()
        {
            string testString = "This &amp;amp;amp;amp; That&amp;amp;amp;amp;amp;amp;amp;amp;trade;";
            string expected = "This &amp; That™";
            string actual = HtmlEntityUtility.DecodeHtmlWithoutXml(testString);

            Assert.Equal(expected, actual);
        }
    }
}
