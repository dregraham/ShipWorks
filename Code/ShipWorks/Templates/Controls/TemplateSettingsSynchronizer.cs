using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using System.Xml.XPath;
using System.Xml;
using Interapptive.Shared.Utility;

namespace ShipWorks.Templates.Controls
{
    /// <summary>
    /// Synchronizes settings between template XSL and its data row
    /// </summary>
    public static class TemplateSettingsSynchronizer
    {
        /// <summary>
        /// Update the database settings of the given template from its XSL
        /// </summary>
        public static void UpdateSettingsFromXsl(TemplateEntity template)
        {
            XPathNavigator xpath = GetXslXpath(template);

            string outputMethod = XPathUtility.Evaluate(xpath, "/*/xsl:output/@method", "html");
            template.OutputFormat = (int) GetOutputFormat(outputMethod);

            string encoding = XPathUtility.Evaluate(xpath, "/*/xsl:output/@encoding", "utf-8");
            template.OutputEncoding = encoding;
        }

        /// <summary>
        /// Update the XSL of the template from its settings.
        /// </summary>
        public static void UpdateXslFromSettings(TemplateEntity template)
        {
            XmlDocument xmlDocument = CreateXmlDocument(template);

            XmlElement xslOutputElement = GetElement(xmlDocument.DocumentElement, "xsl:output");
            bool changed = false;

            string outputMethod = EnumHelper.GetDescription((TemplateOutputFormat) template.OutputFormat).ToLowerInvariant();
            string encoding = template.OutputEncoding;

            // See if we need to set the method
            if (!xslOutputElement.HasAttribute("method") || xslOutputElement.Attributes["method"].Value != outputMethod)
            {
                xslOutputElement.SetAttribute("method", outputMethod);
                changed = true;
            }

            // See if we need to set the encoding
            if (!xslOutputElement.HasAttribute("encoding") || xslOutputElement.Attributes["encoding"].Value != encoding)
            {
                xslOutputElement.SetAttribute("encoding", encoding);
                changed = true;
            }

            // Save the new XSL
            if (changed)
            {
                xslOutputElement.IsEmpty = true;
                template.Xsl = xmlDocument.OuterXml;
            }
        }

        /// <summary>
        /// Get the output format enum value for the given xsl:output value
        /// </summary>
        private static TemplateOutputFormat GetOutputFormat(string outputMethod)
        {
            switch (outputMethod)
            {
                case "html":
                    return TemplateOutputFormat.Html;

                case "xml":
                    return TemplateOutputFormat.Xml;

                case "text":
                    return TemplateOutputFormat.Text;
            }

            return TemplateOutputFormat.Html;
        }

        /// <summary>
        /// Create an XmlDocument of the template XSL
        /// </summary>
        private static XmlDocument CreateXmlDocument(TemplateEntity template)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.PreserveWhitespace = true;
            xmlDocument.LoadXml(template.Xsl);

            return xmlDocument;
        }

        /// <summary>
        /// Get the XPathNavigator over the xsl of the template
        /// </summary>
        private static XPathNavigator GetXslXpath(TemplateEntity template)
        {
            XmlDocument xmlDocument = CreateXmlDocument(template);

            XPathNamespaceNavigator xpath = new XPathNamespaceNavigator(xmlDocument);
            xpath.Namespaces.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");

            return xpath;
        }

        /// <summary>
        /// Gets the XML node with given prefix and name that is a child of the parent.  If the node
        /// does not exist, it is automaticaly created.
        /// </summary>
        private static XmlElement GetElement(XmlElement parent, string fullName)
        {
            return GetElement(parent, fullName, "");
        }

        /// <summary>
        /// Gets the XML node with given prefix and name that is a child of the parent.  If the node
        /// does not exist, it is automaticaly created.
        /// </summary>
        private static XmlElement GetElement(XmlElement parent, string fullName, string criteria)
        {
            XmlDocument xmlDocument = parent.OwnerDocument;

            string prefix = fullName.Split(':')[0];
            string name = fullName.Split(':')[1];

            XmlNamespaceManager namespaces = new XmlNamespaceManager(new NameTable());
            namespaces.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform"); 

            // Get the node
            XmlElement xmlElement = parent.SelectSingleNode(prefix + ":" + name + criteria, namespaces) as XmlElement;

            // Create it if it did not exist
            if (xmlElement == null)
            {
                string indent = GetChildIndentAmount(parent);

                // Convert to the prefixes actually found in the document
                if (prefix == "xsl")
                {
                    prefix = xmlDocument.DocumentElement.GetPrefixOfNamespace("http://www.w3.org/1999/XSL/Transform");
                }

                // Add the new element, putting it on one line, and its close tag indented the same level on the next line
                xmlElement = (XmlElement) xmlDocument.CreateNode(XmlNodeType.Element, prefix, name, xmlDocument.DocumentElement.GetNamespaceOfPrefix(prefix));
                parent.PrependChild(xmlElement);
                xmlElement.InnerText = "\r\n" + indent;

                // Insert a newline inbetween this element and its parent
                XmlNode wsNode = xmlDocument.CreateNode(XmlNodeType.Whitespace, null, null);
                wsNode.Value = "\r\n" + indent;
                parent.PrependChild(wsNode);
            }

            return xmlElement;
        }

        /// <summary>
        /// Determine how far a child of the given element should be indented
        /// </summary>
        private static string GetChildIndentAmount(XmlElement parent)
        {
            foreach (XmlNode childNode in parent.ChildNodes)
            {
                if (childNode.NodeType == XmlNodeType.Element)
                {
                    return GetIndentAmount((XmlElement) childNode);
                }
            }

            return GetIndentAmount(parent) + "    ";
        }

        /// <summary>
        /// Determine the how far indented the given element is
        /// </summary>
        private static string GetIndentAmount(XmlElement element)
        {
            // Didnt find a child element with preceeding whitespace.  So indent from the parent node's current level
            if (element.PreviousSibling != null && element.PreviousSibling.NodeType == XmlNodeType.Whitespace)
            {
                string previousWhitespace = element.PreviousSibling.Value.Replace("\r", "");

                int newLineIndex = previousWhitespace.LastIndexOf("\n");
                if (newLineIndex != -1 && newLineIndex + 1 < previousWhitespace.Length)
                {
                    return previousWhitespace.Substring(newLineIndex + 1);
                }
            }

            // Just default to newline
            return "";
        }
    }
}
