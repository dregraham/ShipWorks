using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.UI.Controls.Html.Core;
using System.Text.RegularExpressions;
using System.Web;

namespace ShipWorks.UI.Controls.Html
{
    /// <summary>
    /// Html utilty functions
    /// </summary>
    public static class HtmlUtility
    {
        static string htmlTagRegex =
            " <                                      " +
            "     (                                  " +
            "        \"[^\"]*\"                      " +
            "        |                               " +
            "        '[^']*'                         " +
            "        |                               " +
            "        [^'\">]                         " +
            "     )*                                 " +
            " >                                      ";

        /// <summary>
        /// Generate a full html document with the given content centered in it.
        /// </summary>
        public static string GetCenteredHtml(string content)
        {
            return string.Format(
            @"<html><body align='center'>
                <table style='height: 100%; margin: 0 15% 0 5%;'><tr><td style='height: 100%; vertical-align: middle;'>
              {0}
              </td></tr></table>
              </body></html>", content);
        }

        /// <summary>
        /// Explicitly sets the width and height of all img tags, and the padding and margin
        /// of tables and td tags.  The primary reason for this
        /// is that Word and Outlook dont handle img tags without explicit widths very well.
        /// </summary>
        public static void SetExplicitImageSizes(HtmlControl htmlControl)
        {
            if (htmlControl == null)
            {
                throw new ArgumentNullException("htmlControl");
            }

            HtmlApi.IHTMLDocument3 document = (HtmlApi.IHTMLDocument3) htmlControl.HtmlDocument;
            HtmlApi.IHTMLElementCollection elements = document.getElementsByTagName("img");

            if (elements != null)
            {
                foreach (HtmlApi.IHTMLImgElement img in elements)
                {
#pragma warning disable S1656 // Variables should not be self-assigned
                    // Seems silly, but forces them to explicitly appear in the html
                    img.width = img.width;
                    img.height = img.height;
#pragma warning restore S1656 // Variables should not be self-assigned
                }
            }
        }

        /// <summary>
        /// Strips all HTML tags leaving just the plain text
        /// </summary>
        public static string GetPlainText(string html)
        {
            // Strip newlines arlready in it
            html = html.Replace("\n", "");
            html = html.Replace("\r", "");

            // Strip the rest of the whitespace
            html = Regex.Replace(html, @"\s+", " ");

            // Strip the <head> completely
            html = Regex.Replace(html, "<head>.*</head>", "", RegexOptions.IgnoreCase);

            // special char
            html = Regex.Replace(html, "&nbsp;", " ", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, "&nbsp", " ", RegexOptions.IgnoreCase);

            // <br> -> newline
            html = Regex.Replace(html, "<br>", "\n", RegexOptions.IgnoreCase);

            // <p> -> newline
            html = Regex.Replace(html, "<p>", "\n", RegexOptions.IgnoreCase);

            // <tr> -> newline
            html = Regex.Replace(html, "<tr", "\n<tr", RegexOptions.IgnoreCase);

            // <td> -> tab
            html = Regex.Replace(html, "<td", "    <td", RegexOptions.IgnoreCase);

            // Strip all tags
            html = Regex.Replace(html, htmlTagRegex, "", RegexOptions.IgnorePatternWhitespace);

            // Cleanup newlines and trim
            html = html.ToString().Replace("\n", "\r\n");
            html = html.Trim();

            // Replace entity references
            html = HttpUtility.HtmlDecode(html);

            return html;
        }
    }
}
