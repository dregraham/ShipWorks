using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content.Editors.ValueEditors;
using ShipWorks.Templates;
using ShipWorks.Filters.Content.SqlGeneration;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Filters.Content.Conditions
{
    /// <summary>
    /// Condition based on needing a tempalte selection
    /// </summary>
    public abstract class TemplateCondition : Condition
    {
        long templateID = 0;

        /// <summary>
        /// The TemplateID that the object is being filtered against being emailed with
        /// </summary>
        public long TemplateID
        {
            get { return templateID; }
            set { templateID = value; }
        }

        /// <summary>
        /// Create the editor for the condition
        /// </summary>
        public override ValueEditor CreateEditor()
        {
            EnsureDefaultTemplateID();

            return new TemplateValueEditor(this);
        }

        /// <summary>
        /// Ensures that a default TemplateID has been selected.  If a TemplateID has been selected but the template has 
        /// been deleted, then nothing is done.
        /// </summary>
        protected void EnsureDefaultTemplateID()
        {
            if (TemplateID != 0)
            {
                return;
            }

            // Try to pick standard
            TemplateEntity standard = TemplateManager.Tree.FindTemplate(@"Invoices\Standard");
            if (standard != null)
            {
                TemplateID = standard.TemplateID;
                return;
            }

            // Get all the folders
            IList<TemplateFolderEntity> folders = TemplateManager.Tree.RootFolders;

            // If there is an invoice folder, use the first one
            TemplateFolderEntity invoices = folders.FirstOrDefault(f => f.Name == "Invoices");
            if (invoices != null && invoices.Templates.Count > 0)
            {
                TemplateID = invoices.Templates[0].TemplateID;
                return;
            }

            // If there are any folders with any templates, use the first
            TemplateFolderEntity any = folders.FirstOrDefault(f => f.Templates.Count > 0);
            if (any != null)
            {
                TemplateID = any.Templates[0].TemplateID;
            }
        }

        /// <summary>
        /// Generate a SQL statement for the given column
        /// </summary>
        protected string GenerateSql(string valueExpression, SqlGenerationContext context)
        {
            return string.Format("{0} = {1}",
                valueExpression,
                TemplateID);
        }
    }
}
