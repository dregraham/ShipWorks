using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using log4net;

namespace ShipWorks.ApplicationCore.Nudges
{
    /// <summary>
    /// Class to create NudgeOption objects from XML
    /// <Nudges>
    ///     <Nudge>
    ///         <NudgeID>12345</NudgeID>
    ///         <NudgeType>ShipWorksUpgrade</NudgeType>
    ///         <ContentUri>https://www.shipworks.com/blah</ContentUri>
    ///         <ContentDimensions>
    ///             <Width>1024</Width>
    ///             <Height>768</Height>
    ///         </ContentDimensions>
    ///         <Options>
    ///             <Option>
    ///                 <Index>0</Index>
    ///                 <Text>OK</Text>
    ///                 <Action>0</Action>
    ///                 <Result>OKClicked</Result>
    ///             </Option>
    ///             <Option>
    ///                 <Index>1</Index>
    ///                 <Text>Close ShipWorks</Text>
    ///                 <Action>1</Action>
    ///                 <Result>CloseClicked</Result>
    ///             </Option>
    ///         </Options>
    ///     </Nudge>
    /// </Nudges>
    /// </summary>
    public static class NudgeOptionDeserializer
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(NudgeOptionDeserializer));

        /// <summary>
        /// Create a NudgeOption from an XElement.
        /// </summary>
        public static NudgeOption Deserialize(Nudge owner, XElement nudgeOptionElement)
        {
            int optionId = GetNudgeOptionID(nudgeOptionElement);
            int index = GetIndex(nudgeOptionElement);
            string text = GetValue(nudgeOptionElement, "Text");
            NudgeOptionActionType actionType = GetActionType(nudgeOptionElement);

            return new NudgeOption(optionId, index, text, owner, actionType);
        }

        /// <summary>
        /// Gets the index of the nudge option from the XElement provided.
        /// </summary>
        private static int GetIndex(XElement nudgeOptionElement)
        {
            int index;
            string value = GetValue(nudgeOptionElement, "Index");
            if (!int.TryParse(value, out index))
            {
                log.Error(string.Format("Invalid Index in nudge option xml: {0}", nudgeOptionElement));
                throw new NudgeException("Invalid Index in nudge option xml.");
            }
            return index;
        }

        /// <summary>
        /// Gets the index of the nudge option from the XElement provided.
        /// </summary>
        private static int GetNudgeOptionID(XElement nudgeOptionElement)
        {
            int optionId;
            string value = GetValue(nudgeOptionElement, "OptionId");
            if (!int.TryParse(value, out optionId))
            {
                log.Error(string.Format("Invalid ID in nudge option xml: {0}", nudgeOptionElement));
                throw new NudgeException("Invalid ID in nudge option xml.");
            }
            return optionId;
        }

        /// <summary>
        /// Gets the type of the action for the XElement provided.
        /// </summary>
        private static NudgeOptionActionType GetActionType(XElement nudgeOptionElement)
        {
            string value = GetValue(nudgeOptionElement, "Action");
            int numericValue;

            if (!int.TryParse(value, out numericValue))
            {
                log.ErrorFormat("An invalid nudge action was provided: {0}", value);
                throw new NudgeException(string.Format("An error while parsing the action of a nudge option: {0}", nudgeOptionElement));
            }

            return (NudgeOptionActionType) numericValue;
        }

        /// <summary>
        /// Gets the string value of an element
        /// </summary>
        private static string GetValue(XElement nudgeOptionElement, string elementName)
        {
            IEnumerable<XElement> elements = nudgeOptionElement.Descendants(elementName);
            List<XElement> xElements = elements as List<XElement> ?? elements.ToList();
            if (!xElements.Any() || string.IsNullOrWhiteSpace(xElements.First().Value))
            {
                log.Error(string.Format("Invalid or missing '{0}' in nudge option xml: {1}", elementName, nudgeOptionElement));
                throw new NudgeException(string.Format("Invalid '{0}' in nudge option xml.", elementName));
            }

            return xElements.First().Value.Trim();
        }
    }
}
