using System;
using System.Text;
using System.Text.RegularExpressions;

namespace ShipWorks.Shipping.Carriers.Amazon.Api
{
    /// <summary>
    /// Class containing Mws related utilities
    /// </summary>
    public static class AmazonMwsUtility
    {
        /** Match one back-slash. */
        static readonly Regex backSlashPtn = new Regex(@"\\");

        /** Match one equal character. */
        private static readonly Regex equalPtn = new Regex(@"=");

        /** Match one forward-slash. */
        private static readonly Regex forwardSlashPtn = new Regex("/");

        /** Match one right parenthesis character. */
        private static readonly Regex lParenPtn = new Regex(@"\(");

        /** Match leading and/or trailing white spaces. */
        private static readonly Regex outerWhiteSpacesPtn = new Regex(@"\A(\s)+|(\s)+\z");

        /** Match one right parenthesis character. */
        private static readonly Regex rParenPtn = new Regex(@"\)");

        /** Match one semicolon character. */
        private static readonly Regex semicolonPtn = new Regex(";");

        /** Match one or more white spaces. */
        private static readonly Regex whiteSpacesPtn = new Regex("\\s+");

        /** Default character encoding. */
        static readonly Encoding defaultEncoding = Encoding.UTF8;

        /** Match a + character. */
        private static readonly Regex plusPtn = new Regex(@"\+");

        /** Match a ! character. */
        private static readonly Regex exlamationPoint = new Regex(@"\!");

        /** Match an asterisk character. */
        private static readonly Regex asteriskPtn = new Regex(@"\*");

        /** Match "%7E". */
        private static readonly Regex pct7EPtn = new Regex("%7[e|E]");

        /** Match "%7E". */
        private static readonly Regex pctSingleQuotePtn = new Regex("'");

        /** Match "%2F". */
        private static readonly Regex pct2FPtn = new Regex("%2[f|F]");

        /// <summary>
        /// Encode string for use with Amazon Mws Signature
        /// </summary>
        /// <param name="value"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string SignatureEncode(string value, bool path)
        {
            try
            {
                // HttpUtility.UrlEncode returns lower case values though it does not escape tilda
                // Therefore using EscapeDataString since it encodes to Utf-8 and also returns escaped values in upper case, i.e., %3A vs %3a for :
                // ARS only supports uppercase and RFC 3986 says it should be upper case.
                // Highly unlikely but should the default encoding ever change, this will need change
                value = Uri.EscapeDataString(value);
            }
            catch (NotSupportedException e)
            {
                throw new SystemException("Unsupported encoding", e);
            }
            value = ReplaceAll(value, asteriskPtn, "%2A");
            value = ReplaceAll(value, pct7EPtn, "~");
            value = ReplaceAll(value, pctSingleQuotePtn, "%27");
            value = ReplaceAll(value, exlamationPoint, "%21");
            value = ReplaceAll(value, lParenPtn, "%28");
            value = ReplaceAll(value, rParenPtn, "%29");
            if (path)
            {
                value = ReplaceAll(value, pct2FPtn, "/");
            }
            return value;
        }

        /// <summary>
        /// Replace a pattern in a string 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="p"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        private static string ReplaceAll(string s, Regex p, string r)
        {
            int n = s == null ? 0 : s.Length;
            if (n == 0)
            {
                return s;
            }
            Match m = p.Match(s);
            if (!m.Success)
            {
                return s;
            }
            StringBuilder buf = new StringBuilder(n + 12);
            int k = 0;
            do
            {
                buf.Append(s, k, m.Index - k);
                buf.Append(r);
                k = m.Index + m.Length;
            } while ((m = m.NextMatch()).Success);
            if (k < n)
            {
                buf.Append(s, k, n - k);
            }
            return buf.ToString();
        }
    }
}
