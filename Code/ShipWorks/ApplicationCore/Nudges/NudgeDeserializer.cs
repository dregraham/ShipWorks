using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Interapptive.Shared.Utility;
using log4net;

namespace ShipWorks.ApplicationCore.Nudges
{
    /// <summary>
    /// Populates a Nudge class from XML
    /// </summary>
    public static class NudgeDeserializer
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(NudgeDeserializer));

        /// <summary>
        /// Create a Nudge from an XML string.
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
        public static Nudge Deserialize(string xml)
        {
            XElement nudgeElement = XElement.Parse(xml);
            return Deserialize(nudgeElement);
        }

        /// <summary>
        /// Create a Nudge from an XElement.
        /// </summary>
        public static Nudge Deserialize(XElement nudgeElement)
        {
            int nudgeID;
            NudgeType nudgeType;
            Uri contentUri;
            List<NudgeOption> nudgeOptions = new List<NudgeOption>();
            Size contentDimensions;

            string value = GetValue(nudgeElement, "NudgeID");
            if (!int.TryParse(value, out nudgeID))
            {
                log.Error(string.Format("Invalid NudgeID in nudge xml: {0}", nudgeElement));
                throw new NudgeException("Invalid NudgeID in nudge xml.");
            }

            try
            {
                value = GetValue(nudgeElement, "NudgeType");
                nudgeType = EnumHelper.GetEnumByApiValue<NudgeType>(value);
            }
            catch (InvalidOperationException ex)
            {
                log.Error(string.Format("Invalid nudge type in nudge xml: {0}", nudgeElement));
                throw new NudgeException("Invalid nudge type in nudge xml.", ex);
            }

            try
            {
                value = GetValue(nudgeElement, "ContentUri");
                contentUri = new Uri(value);
            }
            catch (UriFormatException ex)
            {
                log.Error(string.Format("Invalid nudge content uri in nudge xml: {0}", nudgeElement));
                throw new NudgeException("Invalid nudge content uri in nudge xml.", ex);
            }

            contentDimensions = ContentSize(nudgeElement);

            nudgeOptions = Options(nudgeID, nudgeElement);

            return new Nudge(nudgeID, nudgeType, contentUri, nudgeOptions, contentDimensions);
        }

        /// <summary>
        /// Populates a list of nudge options
        /// </summary>
        private static List<NudgeOption> Options(int nudgeID, XElement nudgeElement)
        {
            List<NudgeOption> nudgeOptions = new List<NudgeOption>();
            IEnumerable<XElement> elements = nudgeElement.Descendants("Option");
            List<XElement> xElements = elements as List<XElement> ?? elements.ToList();

            if (!xElements.Any())
            {
                log.Error(string.Format("Missing nudge options in nudge xml: {0}", nudgeElement));
                throw new NudgeException("Invalid nudge options in nudge xml.");
            }

            xElements.ForEach(nudgeOption => nudgeOptions.Add(NudgeOptionDeserializer.Deserialize(nudgeID, nudgeOption)));

            return nudgeOptions;
        }

        /// <summary>
        /// Creates a Size based on nudge elements
        /// </summary>
        private static Size ContentSize(XElement nudgeElement)
        {
            Size contentDimensions = new Size();

            if (!nudgeElement.Descendants("ContentDimensions").Any())
            {
                log.Error(string.Format("Missing nudge content dimensions in nudge xml: {0}", nudgeElement));
                throw new NudgeException("Missing nudge content dimensions in nudge xml.");
            }

            XElement sizeElement = nudgeElement.Descendants("ContentDimensions").First();
            int sizeParam;
            IEnumerable<XElement> elements;

            elements = sizeElement.Descendants("Width");
            if (!elements.Any() || !int.TryParse(elements.First().Value, out sizeParam))
            {
                log.Error(string.Format("Invalid nudge content dimension width in nudge xml: {0}", nudgeElement));
                throw new NudgeException("Invalid nudge content dimension width in nudge xml.");
            }
            contentDimensions.Width = sizeParam;

            elements = sizeElement.Descendants("Height");
            if (!elements.Any() || !int.TryParse(elements.First().Value, out sizeParam))
            {
                log.Error(string.Format("Invalid nudge content dimension height in nudge xml: {0}", nudgeElement));
                throw new NudgeException("Invalid nudge content dimension height in nudge xml.");
            }
            contentDimensions.Height = sizeParam;

            return contentDimensions;
        }

        /// <summary>
        /// Gets the string value of an element
        /// </summary>
        private static string GetValue(XElement nudgeElement, string elementName)
        {
            IEnumerable<XElement> elements = nudgeElement.Descendants(elementName);
            List<XElement> xElements = elements as List<XElement> ?? elements.ToList();
            if (!xElements.Any() || string.IsNullOrWhiteSpace(xElements.First().Value))
            {
                log.Error(string.Format("Invalid or missing '{0}' in nudge xml: {1}", elementName, nudgeElement));
                throw new NudgeException(string.Format("Invalid '{0}' in nudge xml.", elementName));
            }

            return xElements.First().Value.Trim();
        }
    }
}
