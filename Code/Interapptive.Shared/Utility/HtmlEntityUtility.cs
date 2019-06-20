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
            // Escape XML entities before decoding. Add an & after &amp; so multi-encoded
            // entities will still decode (e.g. &amp;amp;trade will become <ampReplacement>&amp;trade;)
            string escapedInput = input.Replace("&amp;", ampReplacement + "&");
            escapedInput = escapedInput.Replace("lt;", ltReplacement);
            escapedInput = escapedInput.Replace("gt;", gtReplacement);
            escapedInput = escapedInput.Replace("apos;", aposReplacement);
            escapedInput = escapedInput.Replace("quot;", quotReplacement);

            string toDecode = escapedInput;
            string lastDecoded = HttpUtility.HtmlDecode(escapedInput);

            // Continue as long as the already-decoded string can be decoded
            while (!lastDecoded.Equals(toDecode, StringComparison.OrdinalIgnoreCase))
            {
                // Save the previously decoded string and decode it again
                toDecode = lastDecoded;
                lastDecoded = HttpUtility.HtmlDecode(toDecode);
            }

            // After decoding, any multi-encoded XML entity will look like '<ampReplacement>&<entityReplacement>'
            // except the & symbol itself, which will now look like '<ampReplacement>&'. Just replacing
            // '<ampReplacement>&' with &amp; would result in multi-encoded entities having an extra
            // '&amp;' at the front of them, so instead we do a replace to make them just 
            // '<ampReplacement><entityReplacement>'
            lastDecoded = lastDecoded.Replace("&" + ltReplacement, ltReplacement);
            lastDecoded = lastDecoded.Replace("&" + gtReplacement, gtReplacement);
            lastDecoded = lastDecoded.Replace("&" + aposReplacement, aposReplacement);
            lastDecoded = lastDecoded.Replace("&" + quotReplacement, quotReplacement);

            // We can now replace '<ampReplacement>&' with '&amp;' so all & symbols are properly decoded.
            // We then need to remove any remaining '<ampReplacement>' strings so multi-encoded entites
            // are correct: '<ampReplacement><entityReplacement>' now becomes just '<entityReplacement>'
            lastDecoded = lastDecoded.Replace(ampReplacement + "&", "&amp;");
            lastDecoded = lastDecoded.Replace(ampReplacement, "");

            // Finally we can replace all remaining '<entityReplacement>' strings with their encoded entities
            lastDecoded = lastDecoded.Replace(ltReplacement, "&lt;");
            lastDecoded = lastDecoded.Replace(gtReplacement, "&gt;");
            lastDecoded = lastDecoded.Replace(aposReplacement, "&apos;");
            lastDecoded = lastDecoded.Replace(quotReplacement, "&quot;");

            return lastDecoded;
        }
    }
}
