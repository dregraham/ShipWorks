using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Templates
{
    /// <summary>
    /// Wrapper for static TemplateManager
    /// </summary>
    public class TemplateManagerWrapper : ITemplateManager
    {
        /// <summary>
        /// The global default live ReadOnly TemplateTree
        /// </summary>
        public TemplateTree Tree =>
            TemplateManager.Tree;

        /// <summary>
        /// Fetch all of the pick list templates
        /// </summary>
        public IEnumerable<TemplateEntity> FetchPickListTemplates() => TemplateManager.FetchPickListTemplates();
    }
}
