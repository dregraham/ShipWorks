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
    ///                 <Action>Acknowledge</Action>
    ///                 <Result>OKClicked</Result>
    ///             </Option>
    ///             <Option>
    ///                 <Index>1</Index>
    ///                 <Text>Close</Text>
    ///                 <Action>Acknowledge</Action>
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
            int index = GetIndex(nudgeOptionElement);
            string text = GetValue(nudgeOptionElement, "Text");
            string result = GetValue(nudgeOptionElement, "Result");
            string action = GetValue(nudgeOptionElement, "Action");

            return new NudgeOption(index, text, owner, action, result);
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
