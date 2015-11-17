using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates.Processing.TemplateXml;
using ShipWorks.Common.Threading;
using log4net;
using ShipWorks.Templates.Processing;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using System.Text.RegularExpressions;

namespace ShipWorks.Templates.Tokens
{
    /// <summary>
    /// Utilty class for processing and editing tokens.
    /// </summary>
    public static class TemplateTokenProcessor
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(TemplateTokenProcessor));

        static string tokenXslHeader = 
            @"<?xml version='1.0' encoding='utf-8'?>
              <xsl:stylesheet version='1.0' xmlns:xsl='http://www.w3.org/1999/XSL/Transform' xmlns:sw='http://www.interapptive.com/shipworks' extension-element-prefixes='sw'>
                  <xsl:output method='text' />
                  <xsl:template match='/'>";

        static string tokenXslFooter =
            @"    </xsl:template>
              </xsl:stylesheet>";

        /// <summary>
        /// The header XSL snippet that goes before the token when building the final XSL document to be processed.
        /// </summary>
        public static string TokenXslHeader
        {
            get { return tokenXslHeader; }
        }

        /// <summary>
        /// The footer XSL snippet that goes after the token when building the final XSL document to be processed.
        /// </summary>
        public static string TokenXslFooter
        {
            get { return tokenXslFooter; }
        }

        /// <summary>
        /// Process tokens in the token string with the given entity
        /// </summary>
        public static string ProcessTokens(string tokenText, long entityID, bool stripNewLines = true)
        {
            return ProcessTokens(tokenText, new List<long> { entityID }, stripNewLines);
        }

        /// <summary>
        /// Process the tokens in the token string for the given entity ID
        /// </summary>
        public static string ProcessTokens(string tokenText, List<long> idList, bool stripNewLines = true)
        {
            if (!HasTokens(tokenText))
            {
                return tokenText.Replace("{{", "{").Replace("}}", "}");
            }

            TemplateInput input = new TemplateInput(idList, idList, idList.Count > 0 ?
                TemplateContextTranslator.ResolveContextFromEntityType(EntityUtility.GetEntityType(idList[0])) :
                TemplateInputContext.Customer);

            TemplateTranslationContext context = new TemplateTranslationContext(null, input, new ProgressItem(""));

            TemplateXPathNavigator xpath = new TemplateXPathNavigator(context);

            return ProcessTokens(tokenText, xpath, stripNewLines);
        }

        /// <summary>
        /// Process the tokens in the token string for the given TemplateXPathNavigator as input
        /// </summary>
        public static string ProcessTokens(string tokenText, TemplateXPathNavigator templateXPath, bool stripNewLines = true)
        {
            if (!HasTokens(tokenText))
            {
                return tokenText.Replace("{{", "{").Replace("}}", "}");
            }

            try
            {
                // Geneate the XSL and process the result
                TemplateXsl templateXsl = TemplateXslProvider.FromToken(tokenText);
                TemplateResult result = templateXsl.Transform(templateXPath);

                // Read the result into a string
                StringBuilder sb = new StringBuilder(result.ReadResult());

                // And strip newlines if requested
                if (stripNewLines)
                {
                    // Return the result, with newlines stripped
                    sb.Replace("\r", "").Replace("\n", "");
                }

                return sb.ToString();
            }
            catch (TemplateException ex)
            {
                log.Error("ProcessTokens", ex);

                string converted = ConvertToXsl(tokenText);
                if (converted != tokenText)
                {
                    converted = "\n\nExpanded:\n" + converted;
                }
                else
                {
                    converted = "";
                }

                throw new TemplateTokenException($"There was a problem with one of your tokens:\n\nToken:\n{tokenText}{converted}\n\nError:\n{ex.Message}", ex);
            }
        }


        /// <summary>
        /// Indicates if the given text contains tokens that require processing
        /// </summary>
        public static bool HasTokens(string text)
        {
            return ConvertToXsl(text).Contains("<xsl:");
        }

        /// <summary>
        /// Convert the given tokenized text to real XSL, translating shortcuts. The XSL should not include the header or footer.
        /// </summary>
        public static string ConvertToXsl(string tokenText)
        {
            StringBuilder sb = new StringBuilder(tokenText);

            sb.Replace("{{", "__open_curly");
            sb.Replace("}}", "__close_curly");

            sb.Replace("{", "<xsl:value-of select=\"");
            sb.Replace("}", "\" />");

            sb.Replace("__open_curly", "{");
            sb.Replace("__close_curly", "}");

            string expanded = sb.ToString();

            // Now replace instances of & with &amp; unless its already an entity reference
            expanded = Regex.Replace(expanded, @"&(?!\w+;)", "&amp;");

            return expanded;
        }
    }
}
