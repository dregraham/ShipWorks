using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ActiproSoftware.SyntaxEditor.Addons.Xml;
using ActiproSoftware.SyntaxEditor;
using Interapptive.Shared.Utility;
using ActiproSoftware.SyntaxEditor.Addons.Dynamic;
using System.Diagnostics;

namespace ShipWorks.Templates.Controls.XslEditing
{
    /// <summary>
    /// Our custom acipro syntax language
    /// </summary>
    public class ShipWorksXslSyntaxLanguage : XmlSyntaxLanguage
    {        
        // Language shcema
        XmlSchemaResolver xmlSchema = new XmlSchemaResolver();

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipWorksXslSyntaxLanguage()
        {
            // Load the schema languages
            // xmlSchema.AddSchema(ResourceUtility.ReadString("ShipWorks.Templates.Controls.XslEditing.Schemas.xml.xsd"));
            // xmlSchema.AddSchema(ResourceUtility.ReadString("ShipWorks.Templates.Controls.XslEditing.Schemas.xhtml.xsd"));
            // xmlSchema.AddSchema(ResourceUtility.ReadString("ShipWorks.Templates.Controls.XslEditing.Schemas.xslt.xsd"));

            this.IntelliPromptQuickInfoEnabled = false;
            this.IntelliPromptMemberListEnabled = false;
        }

        /// <summary>
        /// Applies the language to the specified editor.  The editor reference is not saved, as the language
        /// can be applies to many editors.
        /// </summary>
        public void ApplyTo(SyntaxEditor syntaxEditor)
        {
            syntaxEditor.Document.Language = this;

            // Load the schema
            syntaxEditor.Document.LanguageData = xmlSchema;
        }
    }
}
