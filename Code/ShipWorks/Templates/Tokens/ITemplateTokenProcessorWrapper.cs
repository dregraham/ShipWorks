using System.Collections.Generic;
using ShipWorks.Templates.Processing.TemplateXml;

namespace ShipWorks.Templates.Tokens
{
    /// <summary>
    /// Interface that represents the TemplateTokenProcessor
    /// </summary>
    public interface ITemplateTokenProcessorWrapper
    {
        /// <summary>
        /// Process the tokens in the token string for the given TemplateXPathNavigator as input
        /// </summary>
        string ProcessTokens(string tokenText, TemplateXPathNavigator templateXPath, bool stripNewLines = true);

        /// <summary>
        /// Process the tokens in the token string for the given entity ID
        /// </summary>
        string ProcessTokens(string tokenText, long entityID, bool stripNewLines = true);

        /// <summary>
        /// Process tokens in the token string with the given list of entity IDs
        /// </summary>
        string ProcessTokens(string tokenText, List<long> idList, bool stripNewLines = true);
    }
}