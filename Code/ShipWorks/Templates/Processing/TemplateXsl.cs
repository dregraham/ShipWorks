using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Xsl;
using System.Xml;
using System.IO;
using ShipWorks.Data.Model.EntityClasses;
using System.Xml.XPath;
using System.Diagnostics;
using log4net;
using ShipWorks.Templates.Processing.TemplateXml;
using ShipWorks.ApplicationCore;
using System.Threading;
using System.Windows.Forms;
using System.Linq;
using Interapptive.Shared;

namespace ShipWorks.Templates.Processing
{
    /// <summary>
    /// The result of template or token compilation and caching.
    /// </summary>
    public class TemplateXsl
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(TemplateXsl));

        // Total number of transformations since ShipWorks started
        static long uniqueFileCounter = 0;

        // URI of the xsl extension
        readonly static string shipworksUri = "http://www.interapptive.com/shipworks";

        // The name and XSL we were loaded from
        string name;
        string xsl;

        // The transform loaded from the XSL
        XslCompiledTransform xslTransform;

        // If the xsl is not valid, this is the exception caught
        TemplateXslException compileException;

        // Every TemplateXsl object has a unique version identifier for dependancy change tracking.
        Guid version = Guid.NewGuid();

        // The path on disk we saved it to for referencing via xsl:import
        string xslImportUri;

        // Templates and tokens can reference other templates using xsl:import.  This is a list of all templates
        // referenced by this xsl.
        IList<TemplateXslImport> xslImports = new List<TemplateXslImport>();

        // Indicates if there is any use of the TemplatePartition tag within this template xsl.
        bool hasPartitions = false;

        // The length of the output of the last transformation using this xsl. Setting this to the huge value ensures
        // that the first time through everything goes to disk - which ensures that a gigantic result set won't just 
        // crash sw right away if the first time went in memory.
        long lastOutputLength = long.MaxValue;

        /// <summary>
        /// Create a new instance of a ShipWorks XSL document based on the given XSL
        /// </summary>
        public TemplateXsl(string name, string xsl)
        {
            this.name = name;
            this.xsl = xsl;

            compileException = new TemplateXslException("The template has not yet been compiled.", 0, 0, null, null);
        }

        /// <summary>
        /// Indicates if the xsl document is valid.  If it is valid Transform will be non-null and ready.  If
        /// it is not valid, ErrorException will be non-null.
        /// </summary>
        public bool IsValid
        {
            get { return xslTransform != null; }
        }

        /// <summary>
        /// The name given to this XSL instance.  For templates this will be the template FullName, but could also not be for a template - like in the case of Tokens.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// The XSL used to generated the document
        /// </summary>
        public string Xsl
        {
            get { return xsl; }
        }

        /// <summary>
        /// Unique to this TemplateXsl object.  This is used to track when dependant xsl:import'ed templates change.
        /// </summary>
        public Guid Version
        {
            get { return version; }
        }

        /// <summary>
        /// Indicates if this template, or any templates that it imports, makes use of the TemplatePartition tag.
        /// </summary>
        public bool HasPartitions
        {
            get
            {
                if (hasPartitions)
                {
                    return true;
                }

                if (xslImports != null)
                {
                    return xslImports.Any(i => i.HasPartitions);
                }

                return false;
            }
        }

        /// <summary>
        /// Get the encoding used for the target of the xsl transform
        /// </summary>
        public Encoding TargetEncoding
        {
            get
            {
                if (xslTransform != null)
                {
                    return (Encoding) xslTransform.OutputSettings.Encoding.Clone();
                }

                return Encoding.GetEncoding("utf-8");
            }
        }

        /// <summary>
        /// The compiled and loaded XslCompiledTransform.  Will be null of the XSL is not valid.
        /// </summary>
        [NDependIgnoreLongMethod]
        public TemplateResult Transform(TemplateXPathNavigator xmlSource)
        {
            if (!IsValid)
            {
                throw compileException;
            }

            //Create an XsltArgumentList.
            XsltArgumentList xslArguments = new XsltArgumentList();

            // Add our extension object
            xslArguments.AddExtensionObject(ShipWorksUri, new TemplateXslExtensions());

            Stopwatch sw = Stopwatch.StartNew();

            // The stream to which the content will be written.
            Stream targetStream = null;

            // The target file that will be used as the stream's backing store if not written to memory.
            string targetFile = null;

            try
            {
                bool outputToFile = lastOutputLength * xmlSource.Context.TotalContexts >= TemplateHelper.MaxMemoryForXslOutput;

                // If the total translation would be more than the max then we output to file instead of memory
                if (outputToFile)
                {
                    targetFile = GetNextTemplateResultTempFile();
                    targetStream = new FileStream(targetFile, FileMode.CreateNew, FileAccess.ReadWrite);
                }
                // Otherwise, do it in memory
                else
                {
                    targetStream = new MemoryStream();
                }

                Encoding encoding = (Encoding)xslTransform.OutputSettings.Encoding.Clone();

                // For whatever target stream - memory or file - we put it through the proper encoding so the user gets the correct preview results.
                using (StreamWriter textWriter = new StreamWriter(targetStream, encoding))
                {
                    // Without doing this if there were funky characters in the source document it would fail
                    XmlWriterSettings writerSettings = xslTransform.OutputSettings.Clone();
                    writerSettings.CheckCharacters = false;

                    // When using a TextWriter derived class, it will use the encoding of the writer exactly as is.  When you pass a stream, it replaces
                    // the fallback mechanism with a CharEnitityReplacementFallback, which can throw an exception.
                    using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, writerSettings))
                    {
                        xslTransform.Transform(xmlSource, xslArguments, xmlWriter);

                        // Remember how long for next time
                        targetStream.Flush();
                        lastOutputLength = targetStream.Length;

                        // Create the result
                        if (outputToFile)
                        {
                            return new TemplateResult(xmlSource, targetFile, encoding);
                        }
                        else
                        {
                            // Reset the stream for reading - we are going to read the data right back out
                            targetStream.Position = 0;

                            using (StreamReader reader = new StreamReader(targetStream, encoding))
                            {
                                return new TemplateResult(xmlSource, reader.ReadToEnd());
                            }
                        }
                    }
                }
            }
            catch (XsltException ex)
            {
                log.Error("Error in Transform.", ex);

                throw new TemplateXslException(ex.Message, ex.LineNumber, ex.LinePosition, this.DetermineErrorSource(ex.SourceUri), ex);
            }
            finally
            {
                xmlSource.Context.ProcessingComplete = true;

                log.DebugFormat("Transform: {0}", sw.Elapsed.TotalSeconds);

                if (targetStream != null)
                {
                    targetStream.Dispose();
                }
            }
        }

        /// <summary>
        /// Get the filename of the next temp template result file to cache to disk.
        /// </summary>
        public static string GetNextTemplateResultTempFile()
        {
            return Path.Combine(OutputFolder, string.Format("result{0}.out", Interlocked.Increment(ref uniqueFileCounter)));
        }

        /// <summary>
        /// The path on disk we the xsl content is saved, for the purposes of referencing via xsl:import
        /// </summary>
        public string XslImportUri
        {
            get
            {
                if (xslImportUri == null)
                {
                    xslImportUri = Path.Combine(OutputFolder, string.Format("source{0}.xsl", Interlocked.Increment(ref uniqueFileCounter)));

                    File.WriteAllText(xslImportUri, xsl);
                }

                return xslImportUri;
            }
        }

        /// <summary>
        /// Since we compile templates from memory, this is the BaseURI that is assigned (as opposed to when you compile from a physical file or web URL, that file or URL is the base)
        /// </summary>
        public static string XslBaseUri
        {
            get { return @"Z:\__shipworks\__templates\__baseuri\"; }
        }

        /// <summary>
        /// Templates and tokens can reference other templates using xsl:import.  This is a list of all templates
        /// referenced by this xsl.
        /// </summary>
        public IList<TemplateXslImport> XslImports
        {
            get { return xslImports; }
        }

        /// <summary>
        /// If the XslDocument is not valid, this is the exception that is the cause.
        /// </summary>
        public TemplateXslException CompileException
        {
            get { return compileException; }
        }

        /// <summary>
        /// Prepare the XslCompiledTransform using the given TemplateTree for xsl:import resolution.
        /// </summary>
        public void Compile(TemplateTree templateTree)
        {
            try
            {
                // Create the resolver for resolving referenced templates
                TemplateXslImportResolver xslResolver = new TemplateXslImportResolver(templateTree);

                try
                {
                    using (StringReader reader = new StringReader(xsl))
                    {
                        // Create the XML reader
                        XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
                        xmlReaderSettings.DtdProcessing = DtdProcessing.Parse;

                        XmlReader xmlReader = XmlReader.Create(reader, xmlReaderSettings, XslBaseUri);

                        // XSL settings.  Enable scripts and the document() function.
                        XsltSettings xsltSettings = new XsltSettings();
                        xsltSettings.EnableDocumentFunction = true;
                        xsltSettings.EnableScript = true;

                        // Load the transform
                        xslTransform = new XslCompiledTransform();
                        xslTransform.Load(xmlReader, xsltSettings, xslResolver);
                    }

                    hasPartitions = xsl.Contains("TemplatePartition");
                }
                finally
                {
                    // Save the templates that were referenced during resolution
                    xslImports = xslResolver.XslImports;
                }
            }
            catch (XsltException ex)
            {
                HandleCompileError(ex, ex.SourceUri, ex.LineNumber, ex.LinePosition);
            }
            catch (XmlException ex)
            {
                HandleCompileError(ex, ex.SourceUri, ex.LineNumber, ex.LinePosition);
            }
        }

        /// <summary>
        /// An error occurred during compilation of the XSL
        /// </summary>
        private void HandleCompileError(Exception ex, string sourceUri, int lineNumber, int linePosition)
        {
            xslTransform = null;

            string message = ex.Message;

            if (ex.InnerException != null)
            {
                message += " " + ex.InnerException.Message;
            }

            // When we are missing an import, it will say it couldnt find the full path.  Make sure it just shows the relative path
            message = message.Replace(OutputFolder, "");
            message = message.Replace(XslBaseUri, "");

            int lineInfo = message.IndexOf(" Line ");
            if (lineInfo != -1)
            {
                message = message.Substring(0, lineInfo);
            }

            if (message == @"Stylesheet 'System\Snippets' cannot directly or indirectly include or import itself.")
            {
                message = @"A snippet cannot import 'System\Snippets'. Directly import individual snippets instead.";
            }

            compileException = new TemplateXslException(message, lineNumber, linePosition, DetermineErrorSource(sourceUri), ex);
        }

        /// <summary>
        /// Determine the source template (or external XSL file) given the specified URI.
        /// </summary>
        private string DetermineErrorSource(string sourceUri)
        {
            if (string.IsNullOrEmpty(sourceUri))
            {
                return null;
            }

            if (!sourceUri.EndsWith("xsl", StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            Uri uri = null;
            if (Uri.TryCreate(sourceUri, UriKind.Absolute, out uri))
            {
                TemplateXslImport xslImport = xslImports.FirstOrDefault(i => i.XslPhysicalFile == uri.LocalPath);
                if (xslImport != null)
                {
                    // Could be our template name in a recursive import situation
                    if (xslImport.TemplateFullName != name)
                    {
                        return xslImport.TemplateFullName;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            if (uri != null && uri.IsFile)
            {
                return uri.LocalPath;
            }

            return sourceUri;
        }

        /// <summary>
        /// The full URI for the ShipWorks namespace
        /// </summary>
        public static string ShipWorksUri
        {
            get { return shipworksUri; }
        }

        /// <summary>
        /// The folder we output our temporary files to
        /// </summary>
        private static string OutputFolder
        {
            get
            {
                string path = Path.Combine(DataPath.ShipWorksTemp, @"Templates");

                Directory.CreateDirectory(path);

                // This is important for error message display and baseURI processing
                return path + @"\";
            }
        }
    }
}
