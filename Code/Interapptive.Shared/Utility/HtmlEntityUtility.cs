using System;
using System.Web;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Utility functions for working with HTML entities
    /// </summary>
    public static class HtmlEntityUtility
    {
        /// <summary>
        /// Takes a string that may be HTML-encoded multiple times, and decodes the HTML
        /// entites while leaving any XML entities single-encoded
        /// </summary>
        public static string DecodeHtmlWithoutXml(string input)
        {
            // Escape the &amp; entity for multi-encoded strings, e.g. &amp;amp;trade;
            string escapedInput = input.Replace("&amp;", "78186DB848C1431091DDF887670571B9&");

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
            singleEncoded = singleEncoded.Replace("&lt;", "8D98F933E20D423889CE736EDC072AC4");
            singleEncoded = singleEncoded.Replace("&gt;", "5720AC1968854264B485AF56728A16CB");
            singleEncoded = singleEncoded.Replace("&apos;", "7A56CB5DBD364FA9884F3BC06FAA3256");
            singleEncoded = singleEncoded.Replace("&quot;", "102A2CE0BAD54F31B26F27EDFDCF80A0");

            singleEncoded = HttpUtility.HtmlDecode(singleEncoded);

            // After the final decode, any &amp; that was originally non-stacked will now be the 
            // replacement string with & at the end, so change it back to &amp;. A stacked &amp;
            // will now be the replacement string with the decoded entity afterwards, so just
            // remove the replacement string
            singleEncoded = singleEncoded.Replace("78186DB848C1431091DDF887670571B9&", "&amp;");
            singleEncoded = singleEncoded.Replace("78186DB848C1431091DDF887670571B9", "");

            // Replace the strings from above with the encoded XML entities
            singleEncoded = singleEncoded.Replace("8D98F933E20D423889CE736EDC072AC4", "&lt;");
            singleEncoded = singleEncoded.Replace("5720AC1968854264B485AF56728A16CB", "&gt;");
            singleEncoded = singleEncoded.Replace("7A56CB5DBD364FA9884F3BC06FAA3256", "&apos;");
            singleEncoded = singleEncoded.Replace("102A2CE0BAD54F31B26F27EDFDCF80A0", "&quot;");

            return singleEncoded;
        }
    }
}
