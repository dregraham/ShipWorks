using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Xsl;
using System.Xml;
using System.IO;
using ShipWorks.Stores.Platforms.GenericModule.LegacyAdapter;
using ShipWorks.Data.Import.Xml.Schema;

namespace ShipWorks.Stores.Platforms.GenericFile.Formats.Xml
{
    /// <summary>
    /// Utility functions for dealing with XML file stuff
    /// </summary>
    public static class GenericFileXmlUtility
    {
        /// <summary>
        /// Load the XslCompiledTransform object corresponding to the given content.  Returns null if the xslContent is null or empty.
        /// </summary>
        public static XslCompiledTransform LoadXslTransform(string xslContent)
        {
            if (string.IsNullOrWhiteSpace(xslContent))
            {
                return null;
            }

            try
            {
                using (StringReader reader = new StringReader(xslContent))
                {
                    // Create the XML reader
                    XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
                    xmlReaderSettings.DtdProcessing = DtdProcessing.Parse;

                    XmlReader xmlReader = XmlReader.Create(reader, xmlReaderSettings);

                    // XSL settings.  Enable scripts and the document() function.
                    XsltSettings xsltSettings = new XsltSettings();
                    xsltSettings.EnableDocumentFunction = true;
                    xsltSettings.EnableScript = true;

                    // Load the transform
                    XslCompiledTransform xslTransform = new XslCompiledTransform();
                    xslTransform.Load(xmlReader, xsltSettings, null);

                    return xslTransform;
                }
            }
            catch (XsltException ex)
            {
                string message = string.Format("{0}\n\nLine {1}, Position {2}", ex.Message, ex.LineNumber, ex.LinePosition);

                throw new GenericFileStoreException(message, ex);
            }
            catch (XmlException ex)
            {
                string message = string.Format("{0}\n\nLine {1}, Position {2}", ex.Message, ex.LineNumber, ex.LinePosition);

                throw new GenericFileStoreException(message, ex);
            }
        }

        /// <summary>
        /// Load the given XML file PATH and optionally tranform it with the specified XSL.  It is validated against the ShipWorks file schema before being returned.
        /// </summary>
        public static XmlDocument LoadAndValidateDocument(string xmlFilePath, string xslContent)
        {
            try
            {
                return LoadAndValidateDocument(File.ReadAllText(xmlFilePath), LoadXslTransform(xslContent));
            }
            catch (IOException ex)
            {
                throw new GenericFileStoreException("ShipWorks could not read the XML file: " + ex.Message, ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new GenericFileStoreException("ShipWorks could not read the XML file: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Load the given XML CONTENT and optionally tranform it with the specified XSL.  It is validated against the ShipWorks file schema before being returned.
        /// </summary>
        public static XmlDocument LoadAndValidateDocument(string xmlContent, XslCompiledTransform xslTransform)
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlContent);

                // If there's a transform apply it
                if (xslTransform != null)
                {
                    xmlDocument = TransformDocument(xmlDocument, xslTransform);
                }

                // Ensure the root element is ShipWorks otherwise schema validation won't be performed
                if (xmlDocument.DocumentElement.NamespaceURI.Length > 0 || xmlDocument.DocumentElement.LocalName != "ShipWorks")
                {
                    throw new GenericFileStoreException("The XML document does not have the required ShipWorks root element.");
                }

                List<string> errors = ShipWorksSchemaValidator.FindValidationErrors(xmlDocument, ShipWorksSchema.FileImport);

                if (errors.Count > 0)
                {
                    throw new GenericFileStoreException("There was an error validating the XML document against the ShipWorks schema:\n\n" + errors[0]);
                }

                return xmlDocument;
            }
            catch (XsltException ex)
            {
                throw new GenericFileStoreException("ShipWorks could not transform the XML file: " + ex.Message, ex);
            }
            catch (XmlException ex)
            {
                throw new GenericFileStoreException("ShipWorks could not read the XML file: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Transform the given document
        /// </summary>
        private static XmlDocument TransformDocument(XmlDocument xmlDocument, XslCompiledTransform xslTransform)
        {
            Encoding encoding = (Encoding) xslTransform.OutputSettings.Encoding.Clone();

            using (Stream targetStream = new MemoryStream())
            {
                // For whatever target stream - memory or file - we put it through the proper encoding so the user gets the correct preview results.
                using (StreamWriter textWriter = new StreamWriter(targetStream, encoding))
                {
                    // Without doing this if there were funky characters in the source document it would fail
                    XmlWriterSettings writerSettings = xslTransform.OutputSettings.Clone();
                    writerSettings.CheckCharacters = false;

                    XsltArgumentList args = new XsltArgumentList();
                    args.AddExtensionObject("http://www.interapptive.com/shipworks/extensions", new LegacyAdapterXslExtensions());

                    // When using a TextWriter derived class, it will use the encoding of the writer exactly as is.  When you pass a stream, it replaces
                    // the fallback mechanism with a CharEnitityReplacementFallback, which can throw an exception.
                    using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, writerSettings))
                    {
                        xslTransform.Transform(xmlDocument, args, xmlWriter);

                        // Reset the stream for reading - we are going to read the data right back out
                        targetStream.Flush();
                        targetStream.Position = 0;

                        using (StreamReader reader = new StreamReader(targetStream, encoding))
                        {
                            XmlDocument resultDocument = new XmlDocument();
                            resultDocument.Load(reader);

                            return resultDocument;
                        }
                    }
                }
            }
        }
    }
}
