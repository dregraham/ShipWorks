using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using ShipWorks.Data.Model.EntityClasses;
using System.IO;
using System.Xml.Xsl;

namespace ShipWorks.Templates.Processing
{
    /// <summary>
    /// Custom XmlResolver for resolving relative template references
    /// </summary>
    public class TemplateXslImportResolver : XmlUrlResolver
    {
        TemplateTree templateTree;

        List<TemplateXslImport> xslImports = new List<TemplateXslImport>();

        /// <summary>
        /// Constructor
        /// </summary>
        public TemplateXslImportResolver(TemplateTree templateTree)
        {
            this.templateTree = templateTree;
        }

        /// <summary>
        /// Overridden to set the correct baseUri for resolution
        /// </summary>
        public override Uri ResolveUri(Uri baseUri, string relativeUriString)
        {
            Uri relativeUri;
            if (Uri.TryCreate(relativeUriString, UriKind.RelativeOrAbsolute, out relativeUri))
            {
                // Anything that is absolute is definitely not a reference to a template
                if (!relativeUri.IsAbsoluteUri)
                {
                    // See if we can find a matching template
                    TemplateEntity template = templateTree.FindTemplate(relativeUriString);
                    TemplateXslImport xslImport;

                    // The BaseURI will only match on the XSL that is directly owning this ImportResolver...
                    // everything else will havea BaseURI of an actual physical resource.
                    bool directImport = (baseUri != null && baseUri.OriginalString == TemplateXsl.XslBaseUri);

                    // Found it
                    if (template != null)
                    {
                        // Get its XSL reference
                        TemplateXsl templateXsl = templateTree.XslCache.FromTemplate(template);

                        // We need a path to it for the xsl engine to reference.
                        baseUri = null;
                        relativeUriString = templateXsl.XslImportUri;

                        // Create the import
                        xslImport = new TemplateXslImport(template.FullName, directImport, templateXsl);
                    }

                    // Didn't find it.  We still have to record our dependancy on it though, so that if it comes into existance, we'll pick it up.
                    else
                    {
                        xslImport = new TemplateXslImport(relativeUriString, directImport, null);
                    }

                    // Add this to the list of templates we have resolved
                    if (!Contains(xslImports, xslImport))
                    {
                        xslImports.Add(xslImport);
                    }
                }
            }

            return base.ResolveUri(baseUri, relativeUriString);
        }

        /// <summary>
        /// Determine if the import is already in the list
        /// </summary>
        private bool Contains(List<TemplateXslImport> xslImports, TemplateXslImport xslImport)
        {
            foreach (TemplateXslImport import in xslImports)
            {
                if (import.TemplateFullName == xslImport.TemplateFullName && import.TemplateXslVersion == xslImport.TemplateXslVersion)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// This list of referenced templates resolved by the resolver as the stylesheet was loaded.
        /// </summary>
        public IList<TemplateXslImport> XslImports
        {
            get { return xslImports; }
        }
    }
}
