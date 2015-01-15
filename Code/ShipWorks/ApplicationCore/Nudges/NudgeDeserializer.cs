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
        ///         <Name>The name</Name>
        ///         <NudgeType>0</NudgeType>
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
            // Grab the nudge ID and the type of nudge from the XML
            int nudgeID = GetNudgeID(nudgeElement);
            NudgeType nudgeType = GetNudgeType(nudgeElement);
            string nudgeName = GetNudgeName(nudgeElement);

            // Grab the content data
            Uri contentUri = GetContentUri(nudgeElement);
            Size contentDimensions = ContentSize(nudgeElement);

            // We now have everything that's needed for our nudge
            Nudge nudge = new Nudge(nudgeID, nudgeName, nudgeType, contentUri, contentDimensions);

            // Build up the list of nudge options defined in the XML and add them to the nudge
            IEnumerable<XElement> optionElements = nudgeElement.Descendants("Option");
            if (!optionElements.Any())
            {
                log.Error(string.Format("Missing nudge options in nudge xml: {0}", nudgeElement));
                throw new NudgeException("Invalid nudge options in nudge xml.");
            }

            List<NudgeOption> nudgeOptions = Options(nudge, nudgeElement.Descendants("Option"));
            nudgeOptions.ForEach(nudge.AddNudgeOption);

            return nudge;
        }

        /// <summary>
        /// Gets the nudge ID value from the XElement provided.
        /// </summary>
        private static int GetNudgeID(XElement nudgeElement)
        {
            int nudgeID;
            string value = GetValue(nudgeElement, "NudgeID");

            if (!int.TryParse(value, out nudgeID))
            {
                log.Error(string.Format("Invalid Nudge ID in nudge xml: {0}", nudgeElement));
                throw new NudgeException("Invalid Nudge ID in nudge xml.");
            }

            return nudgeID;
        }

        /// <summary>
        /// Gets the name of the nudge from the XElement provided.
        /// </summary>
        private static string GetNudgeName(XElement nudgeElement)
        {
            // Allow empty strings to be used for the name
            string name = string.Empty;

            
            IEnumerable<XElement> elements = nudgeElement.Descendants("Name");
            List<XElement> xElements = elements as List<XElement> ?? elements.ToList();

            if (xElements.Any() && !string.IsNullOrWhiteSpace(xElements.First().Value))
            {
                // Use the text in the "Name" node if provided
                name = xElements.First().Value.Trim();
            }

            return name;
        }

        /// <summary>
        /// Gets the type of the nudge from the XElement provided.
        /// </summary>
        private static NudgeType GetNudgeType(XElement nudgeElement)
        {   
            try
            {
                string value = GetValue(nudgeElement, "NudgeType");
                return (NudgeType)(int.Parse(value));
            }
            catch (InvalidOperationException ex)
            {
                log.Error(string.Format("Invalid nudge type in nudge xml: {0}", nudgeElement));
                throw new NudgeException("Invalid nudge type in nudge xml.", ex);
            }
        }

        /// <summary>
        /// Gets the content URI from the XElement provided.
        /// </summary>
        private static Uri GetContentUri(XElement nudgeElement)
        {
            try
            {
                string value = GetValue(nudgeElement, "ContentUri");
                return new Uri(value);
            }
            catch (UriFormatException ex)
            {
                log.Error(string.Format("Invalid nudge content uri in nudge xml: {0}", nudgeElement));
                throw new NudgeException("Invalid nudge content uri in nudge xml.", ex);
            }
        }

        /// <summary>
        /// Populates a list of nudge options
        /// </summary>
        private static List<NudgeOption> Options(Nudge nudge, IEnumerable<XElement> optionElements)
        {
            List<NudgeOption> nudgeOptions = new List<NudgeOption>();
            optionElements.ToList().ForEach(nudgeOption => nudgeOptions.Add(NudgeOptionDeserializer.Deserialize(nudge, nudgeOption)));

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
