using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ShipWorks.ApplicationCore;
using System.IO;

namespace ShipWorks.Templates.Processing
{
    /// <summary>
    /// Provides fast and simplistic access to certain tags of an HTML document.  Taylored specifically for template processing.
    /// </summary>
    public class TemplateHtmlDocument
    {
        string html;
        TemplateResultUsage usage;

        string docType = null;
        string htmlStartTag = null;

        string headStartTag = null;
        string headInnerHtml = null;

        string bodyStartTag = null;
        string bodyInnerHtml = null;

        string extraCss = null;

        static readonly string previewCutoffCss = @"
            <style>
                div#sw_html_cutoff { font-name: Tahoma; font-size: 10pt; font-style: italic; font-weight: bold; color: rgb(100, 100, 100); text-align: center; }
            </style>";

        /// <summary>
        /// Constructor
        /// </summary>
        public TemplateHtmlDocument(string html, TemplateResultUsage usage)
        {
            this.html = html;
            this.usage = usage;

            docType = FindStartTag("!DOCTYPE");
            htmlStartTag = FindStartTag("html");

            FindHeadTag();
            FindBodyTag();
        }

        /// <summary>
        /// Get the entire contents of the html document
        /// </summary>
        public string CompleteHtml
        {
            get
            {
                return
                    HtmlStartTag +
                    HeadCompleteTag +
                    BodyStartTag +
                    BodyInnerHtml +
                    "</body></html>";
            }
        }

        /// <summary>
        /// The full html start tag with attributes as it appears in the source document.
        /// </summary>
        public string HtmlStartTag
        {
            get
            {
                return (docType ?? "") + htmlStartTag;
            }
        }

        /// <summary>
        /// The full head tag with attributes and content as it appears in the source document.
        /// </summary>
        public string HeadCompleteTag
        {
            get
            {
                // We mark the extra content with "shipworks" so we can find it later.  This is used by the reprint mechanism.
                string moreCss = !string.IsNullOrEmpty(extraCss) ? "<style source='shipworks' >" + extraCss + "</style>" : "";

                return
                    headStartTag +
                    BaseCompleteTag +
                    ((usage == TemplateResultUsage.ShipWorksDisplay || usage == TemplateResultUsage.Copy) ? previewCutoffCss : "") + 
                    moreCss +
                    headInnerHtml + 
                    "</head>";
            }
        }

        /// <summary>
        /// The base tag to use in the head section.
        /// </summary>
        private string BaseCompleteTag
        {
            get 
            {
                return string.Format("<base href='{0}' />", DataPath.CurrentResources + "\\");
            }
        }

        /// <summary>
        /// Gets or sets any additional CSS content to apply.  Does not include the surrounding style tag.
        /// </summary>
        public string ExtraCss
        {
            get { return extraCss; }
            set { extraCss = value; }
        }

        /// <summary>
        /// The full body start tag with attributes as it appears in the source document.
        /// </summary>
        public string BodyStartTag
        {
            get
            {
                return bodyStartTag;
            }
        }

        /// <summary>
        /// The entire content of the source document between contained within the body tag
        /// </summary>
        public string BodyInnerHtml
        {
            get
            {
                return bodyInnerHtml;
            }
        }

        /// <summary>
        /// Find the head tags
        /// </summary>
        private void FindHeadTag()
        {
            string startTag;
            string innerHtml;

            if (FindFullTag("head", out startTag, out innerHtml, false))
            {
                headStartTag = startTag;
                headInnerHtml = innerHtml;
            }
            else
            {
                headStartTag = "<head>";
                headInnerHtml = "";
            }
        }

        /// <summary>
        /// Find the body tags
        /// </summary>
        private void FindBodyTag()
        {
            string startTag;
            string innerHtml;

            if (FindFullTag("body", out startTag, out innerHtml, true))
            {
                bodyStartTag = startTag;
                bodyInnerHtml = innerHtml;
            }
            else
            {
                bodyStartTag = "<body>";
                bodyInnerHtml = "";
            }
        }

        /// <summary>
        /// Find the full tag details
        /// </summary>
        private bool FindFullTag(string tag, out string startTag, out string innerHtml, bool searchFromEnd)
        {
            int startTagStartIndex = -1;
            int startTagEndIndex = -1;

            if (FindStartTagPosition(tag, out startTagStartIndex, out startTagEndIndex))
            {
                startTag = html.Substring(startTagStartIndex, 1 + startTagEndIndex - startTagStartIndex);

                int closeTagStartIndex;

                if (searchFromEnd)
                {
                    closeTagStartIndex = html.LastIndexOf("</" + tag, html.Length, StringComparison.OrdinalIgnoreCase);
                }
                else
                {
                    closeTagStartIndex = html.IndexOf("</" + tag, startTagEndIndex, StringComparison.OrdinalIgnoreCase);
                }

                if (closeTagStartIndex != -1)
                {
                    innerHtml = html.Substring(startTagEndIndex + 1, closeTagStartIndex - startTagEndIndex - 1);

                    return true;
                }
            }

            startTag = null;
            innerHtml = null;

            return false;
        }

        /// <summary>
        /// Find the start tag with the given name.  Returns null if not found.
        /// </summary>
        private string FindStartTag(string tag)
        {
            int startIndex;
            int endIndex;

            if (FindStartTagPosition(tag, out startIndex, out endIndex))
            {
                return html.Substring(startIndex, 1 + (endIndex - startIndex));
            }

            return null;
        }

        /// <summary>
        /// Find the start and end index of the given tag
        /// </summary>
        private bool FindStartTagPosition(string tag, out int startIndex, out int endIndex)
        {
            startIndex = -1;
            endIndex = -1;

            startIndex = html.IndexOf("<" + tag, 0, StringComparison.OrdinalIgnoreCase);
            if (startIndex == -1)
            {
                return false;
            }

            endIndex = FindTagCloseBracket(startIndex + 1);
            if (endIndex == -1)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Find the next closing bracket.  startIndex must be within an existing tag.
        /// </summary>
        private int FindTagCloseBracket(int startIndex)
        {
            bool inSingleQuote = false;
            bool inDoubleQuote = false;

            for (int i = startIndex; i < html.Length; i++)
            {
                if (!inSingleQuote && !inDoubleQuote)
                {
                    if (html[i] == '>')
                    {
                        return i;
                    }
                }

                if (html[i] == '\'')
                {
                    if (!inDoubleQuote)
                    {
                        inSingleQuote = !inSingleQuote;
                    }
                }

                if (html[i] == '"')
                {
                    if (!inSingleQuote)
                    {
                        inDoubleQuote = !inDoubleQuote;
                    }
                }
            }

            return -1;
        }
    }
}
