using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using log4net;
using Quartz.Util;
using ShipWorks.ApplicationCore.Nudges.NudgeActions;

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
        public static NudgeOption Deserialize(XElement nudgeOptionElement)
        {
            int index;
            string text;
            INudgeAction nudgeAction;
            string result;

            string value = GetValue(nudgeOptionElement, "Index");
            if (!int.TryParse(value, out index))
            {
                log.Error(string.Format("Invalid Index in nudge option xml: {0}", nudgeOptionElement));
                throw new NudgeException("Invalid Index in nudge option xml.");
            }

            text = GetValue(nudgeOptionElement, "Text");
            result = GetValue(nudgeOptionElement, "Result");

            string nudgeActionName = GetValue(nudgeOptionElement, "Action");
            Type nudgeActionType = Assembly.GetExecutingAssembly().GetType(string.Format("ShipWorks.ApplicationCore.Nudges.NudgeActions.{0}", nudgeActionName));

            if (nudgeActionType == null)
            {
                log.Error(string.Format("Unable to get type '{0}'", nudgeActionName));
                throw new NudgeException(string.Format("Unable to get type '{0}'", nudgeActionName));
            }

            try
            {
                nudgeAction = (INudgeAction)Activator.CreateInstance(nudgeActionType);
            }
            catch (Exception)
            {
                throw new NudgeException(string.Format("Unable to create an instance of type '{0}'", nudgeActionName));
            }

            return new NudgeOption(index, text, nudgeAction, result);
        }

        /// <summary>
        /// Gets the string value of an element
        /// </summary>
        private static string GetValue(XElement nudgeOptionElement, string elementName)
        {
            IEnumerable<XElement> elements = nudgeOptionElement.Descendants(elementName);
            List<XElement> xElements = elements as List<XElement> ?? elements.ToList();
            if (!xElements.Any() || xElements.First().Value.IsNullOrWhiteSpace())
            {
                log.Error(string.Format("Invalid or missing '{0}' in nudge option xml: {1}", elementName, nudgeOptionElement));
                throw new NudgeException(string.Format("Invalid '{0}' in nudge option xml.", elementName));
            }

            return xElements.First().Value.Trim();
        }
    }
}
