using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using System.IO;
using ShipWorks.Templates.Processing.TemplateXml;

namespace ShipWorks.Templates.Processing
{
    /// <summary>
    /// Holds a single result unit of processing a template.
    /// </summary>
    public class TemplateResult
    {
        // The final transformed output, if it was done in memory.
        string resultContent;

        // The file the result content was written to if not done in memory
        string resultFile;
        Encoding encoding;

        // The xpath instance used as the source document to the xsl transformation
        TemplateXPathNavigator xpathSource;

        /// <summary>
        /// Constructor
        /// </summary>
        public TemplateResult(TemplateXPathNavigator xpathSource, string resultContent)
        {
            this.resultContent = resultContent;
            this.xpathSource = xpathSource;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public TemplateResult(TemplateXPathNavigator xpathSource, string resultFile, Encoding encoding)
        {
            this.resultFile = resultFile;
            this.encoding = encoding;
            this.xpathSource = xpathSource;
        }

        /// <summary>
        /// Read the result of the transformation.  This operation may be expensive, depending
        /// on if the result was saved to disk or memory.
        /// </summary>
        public string ReadResult()
        {
            if (resultContent != null)
            {
                return resultContent;
            }
            else
            {
                return File.ReadAllText(resultFile, encoding);
            }
        }

        /// <summary>
        /// The xpath instance used as the source document to the xsl transformation
        /// </summary>
        public TemplateXPathNavigator XPathSource
        {
            get { return xpathSource; }
        }

        /// <summary>
        /// Indicates if the result content is loaded in memory.  If its not, its backed by a file.
        /// </summary>
        public bool InMemory
        {
            get { return resultContent != null; }
        }
    }
}
