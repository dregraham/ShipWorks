using ShipWorks.Templates.Processing.TemplateXml;
using System.Collections.Generic;

namespace ShipWorks.Templates.Tokens
{
    /// <summary>
    /// Wrapper for the static TemplateTokenProcessor
    /// </summary>
    public class TemplateTokenProcessorWrapper : ITemplateTokenProcessor
    {
        /// <summary>
        /// Process the tokens in the token string for the given entity ID
        /// </summary>
        public string ProcessTokens(string tokenText, long entityID, bool stripNewLines = true)
        {
            return TemplateTokenProcessor.ProcessTokens(tokenText, entityID, stripNewLines);
        }

        /// <summary>
        /// Process tokens in the token string with the given list of entity IDs
        /// </summary>
        public string ProcessTokens(string tokenText, List<long> idList, bool stripNewLines = true)
        {
            return TemplateTokenProcessor.ProcessTokens(tokenText, idList, stripNewLines);
        }

        /// <summary>
        /// Process the tokens in the token string for the given TemplateXPathNavigator as input
        /// </summary>
        public string ProcessTokens(string tokenText, TemplateXPathNavigator templateXPath, bool stripNewLines = true)
        {
            return TemplateTokenProcessor.ProcessTokens(tokenText, templateXPath, stripNewLines);
        }
    }
}