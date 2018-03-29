using System.Text;
using System.Xml;
using System.Xml.Linq;
using log4net;
using Microsoft.XmlDiffPatch;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Utility functions for working with XML
    /// </summary>
    public static class XmlUtility
    {
        static readonly ILog log = LogManager.GetLogger(typeof(XmlUtility));

        /// <summary>
        /// Compare the two bits of XML for equality given the specified options.
        /// </summary>
        public static bool CompareXml(string original, string proposed, XmlDiffOptions options)
        {
            if (original == null || proposed == null)
            {
                return original == proposed;
            }

            if (original.Length == 0 || proposed.Length == 0)
            {
                return original == proposed;
            }

            try
            {
                XmlDiff xmlDiff = new XmlDiff(options);

                XmlDocument originalDoc = new XmlDocument();
                originalDoc.LoadXml(original);

                XmlDocument proposedDoc = new XmlDocument();
                proposedDoc.LoadXml(proposed);

                return xmlDiff.Compare(originalDoc, proposedDoc);
            }
            catch (XmlException)
            {
                // Not actually xml, just compare as strings
                return original == proposed;
            }
        }

        /// <summary>
        /// Strip all invalid XML chars from the input using a "whitelist" approach.  This is b\c XmlDocument, XDocument, XmlReader, etc.
        /// all barf of there are things like 0x1A (tab) in an XML input document. Unfortunately XmlReaderSettings.CheckCharacters does not
        /// fix this problem.
        /// </summary>
        public static string StripInvalidXmlCharacters(string inputXml)
        {
            if (inputXml == null)
            {
                return null;
            }

            StringBuilder outputXml = new StringBuilder(inputXml.Length);

            foreach (char inputChar in inputXml)
            {
                if ((inputChar >= 0x0020 && inputChar <= 0xD7FF) ||
                    (inputChar >= 0xE000 && inputChar <= 0xFFFD) ||
                    (inputChar == 0x0009) ||
                    (inputChar == 0x000A) ||
                    (inputChar == 0x000D))
                {
                    outputXml.Append(inputChar);
                }
            }

            // We also have to strip out entity references that would be translated into the string equivalent of invalid characters
            for (int invalidChar = 0; invalidChar <= 0x1F; invalidChar++)
            {
                // These are allowed
                if (invalidChar == 0x09 || invalidChar == 0x0A || invalidChar == 0x0D)
                {
                    continue;
                }

                outputXml.Replace(string.Format("&#x{0:x};", invalidChar), "");
                outputXml.Replace(string.Format("&#x{0:X};", invalidChar), "");
            }

            return outputXml.ToString();
        }

        // Convert this XmlNode to an XElement
        public static XElement ToXElement(this XmlNode node)
        {
            if (node == null)
            {
                return null;
            }

            XDocument xDoc = new XDocument();
            using (XmlWriter xmlWriter = xDoc.CreateWriter())
            {
                node.WriteTo(xmlWriter);
            }
            return xDoc.Root;
        }
    }
}
