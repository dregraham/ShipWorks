using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;
using System.Text.RegularExpressions;

namespace ShipWorks.Templates.Management.Skeletons
{
    /// <summary>
    /// Helps to create new blank templates when a user creates a new template.
    /// </summary>
    public static class TemplateSkeletons
    {
        static string genericHtml;
        static string genericText;
        static string genericXml;
        static string labelHtml;
        static string labelText;
        static string labelXml;
        static string snippet;

        /// <summary>
        /// Static constructor
        /// </summary>
        static TemplateSkeletons()
        {
            genericHtml = ResourceUtility.ReadString("ShipWorks.Templates.Management.Skeletons.genericHtml.xsl");
            genericText = ResourceUtility.ReadString("ShipWorks.Templates.Management.Skeletons.genericText.xsl");
            genericXml = ResourceUtility.ReadString("ShipWorks.Templates.Management.Skeletons.genericXml.xsl");
            labelHtml = ResourceUtility.ReadString("ShipWorks.Templates.Management.Skeletons.labelHtml.xsl");
            labelText = ResourceUtility.ReadString("ShipWorks.Templates.Management.Skeletons.labelText.xsl");
            labelXml = ResourceUtility.ReadString("ShipWorks.Templates.Management.Skeletons.labelXml.xsl");
            snippet = ResourceUtility.ReadString("ShipWorks.Templates.Management.Skeletons.snippet.xsl");
        }

        /// <summary>
        /// Create the initial content for a new template of the given type and format.
        /// </summary>
        public static string GetTemplateSkeleton(TemplateType type, TemplateOutputFormat format)
        {
            if (type == TemplateType.Label)
            {
                switch (format)
                {
                    case TemplateOutputFormat.Html: return labelHtml;
                    case TemplateOutputFormat.Text: return labelText;
                    case TemplateOutputFormat.Xml: return labelXml;
                }
            }

            switch (format)
            {
                case TemplateOutputFormat.Html: return genericHtml;
                case TemplateOutputFormat.Text: return genericText;
                case TemplateOutputFormat.Xml: return genericXml;
            }

            throw new InvalidOperationException(string.Format("Unhandled case creating blank template content {0} {1}", type, format));
        }

        /// <summary>
        /// Get the initial content for when the user creates a new snippet
        /// </summary>
        public static string GetSnippetSkeleton(string name)
        {
            // Cleanup the name so its valid for xsl:template
            name = Regex.Replace(name, @"[^\d\w\._-]", "_");

            return snippet.Replace("SNIPPET_NAME", name);
        }
    }
}
