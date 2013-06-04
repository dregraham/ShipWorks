using System;
using System.Collections.Generic;
using System.Text;

namespace ShipWorks.Templates.Processing
{
    /// <summary>
    /// Represents a reference to a template that was references via an xsl:import
    /// </summary>
    public class TemplateXslImport
    {
        string templateFullName;
        bool directImport;
        TemplateXsl templateXsl;

        /// <summary>
        /// Constructor
        /// </summary>
        public TemplateXslImport(string templateFullName, bool directImport, TemplateXsl templateXsl)
        {
            this.templateFullName = templateFullName;
            this.directImport = directImport;
            this.templateXsl = templateXsl;
        }

        /// <summary>
        /// The full path and name of the template that was imported
        /// </summary>
        public string TemplateFullName
        {
            get { return templateFullName; }
        }

        /// <summary>
        /// Indicates if this import represents an import that was directly specified in the owning TemplateXsl, or if was an import of a template that the owning
        /// template imported.
        /// </summary>
        public bool DirectImport
        {
            get { return directImport; }
        }

        /// <summary>
        /// The physical file on disk that is being used to do this import
        /// </summary>
        public string XslPhysicalFile
        {
            get { return (templateXsl != null) ? templateXsl.XslImportUri : null; }
        }

        /// <summary>
        /// The version identifier of the TemplateXsl object for the template that was imported.
        /// </summary>
        public Guid TemplateXslVersion
        {
            get { return (templateXsl != null) ? templateXsl.Version : Guid.Empty; }
        }

        /// <summary>
        /// Indicates if the XSL referenced by this import makes use of the TemplateOutput tag.
        /// </summary>
        public bool HasPartitions
        {
            get { return (templateXsl != null) ? templateXsl.HasPartitions : false; }
        }
    }
}
