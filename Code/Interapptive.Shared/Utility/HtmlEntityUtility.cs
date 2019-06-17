using System;
using System.Web;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Utility functions for working with HTML entities
    /// </summary>
    public static class HtmlEntityUtility
    {
        // The following strings are used to replace XML entities to prevent them from being decoded
        // &amp;
        const string ampReplacement = "78186DB848C1431091DDF887670571B9";
        // &lt;
        const string ltReplacement = "8D98F933E20D423889CE736EDC072AC4";
        // &gt;
        const string gtReplacement = "5720AC1968854264B485AF56728A16CB";
        // &apos;
        const string aposReplacement = "7A56CB5DBD364FA9884F3BC06FAA3256";
        // &quot;
        const string quotReplacement = "102A2CE0BAD54F31B26F27EDFDCF80A0";

        /// <summary>
        /// Takes a string that may be HTML-encoded multiple times, and decodes the HTML
        /// entites while leaving any XML entities single-encoded
        /// </summary>
        public static string DecodeHtmlWithoutXml(string input)
        {
            // Add an & at the end of the &amp; replacement string so that multi-encoded
            // strings (e.g. &amp;amp;trade;) will still be properly decoded multiple times
            string escapedInput = input.Replace("&amp;", ampReplacement + "&");

            string singleEncoded = escapedInput;
            string toDecode = escapedInput;
            string lastDecoded = HttpUtility.HtmlDecode(escapedInput);

            // Continue as long as the already-decoded string can be decoded
            while (!lastDecoded.Equals(toDecode, StringComparison.OrdinalIgnoreCase))
            {
                // Save the encoded string in case it is only encoded once
                singleEncoded = toDecode;

                // Save the previously decoded string and decode it again
                toDecode = lastDecoded;
                lastDecoded = HttpUtility.HtmlDecode(toDecode);
            }

            // Replace encoded XML entities with strings that won't get decoded
            singleEncoded = singleEncoded.Replace("&lt;", ltReplacement);
            singleEncoded = singleEncoded.Replace("&gt;", gtReplacement);
            singleEncoded = singleEncoded.Replace("&apos;", aposReplacement);
            singleEncoded = singleEncoded.Replace("&quot;", quotReplacement);

            singleEncoded = HttpUtility.HtmlDecode(singleEncoded);

            // After the final decode, any &amp; that was originally non-stacked will now be the 
            // replacement string with & at the end, so change it back to &amp;. A stacked &amp;
            // will now be the replacement string with the decoded entity afterwards, so just
            // remove the replacement string
            singleEncoded = singleEncoded.Replace(ampReplacement + "&", "&amp;");
            singleEncoded = singleEncoded.Replace(ampReplacement, "");

            // Replace the strings from above with the encoded XML entities
            singleEncoded = singleEncoded.Replace(ltReplacement, "&lt;");
            singleEncoded = singleEncoded.Replace(gtReplacement, "&gt;");
            singleEncoded = singleEncoded.Replace(aposReplacement, "&apos;");
            singleEncoded = singleEncoded.Replace(quotReplacement, "&quot;");

            return singleEncoded;
        }
    }
}
