using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Templates
{
    /// <summary>
    /// Represents the TemplateManager
    /// </summary>
    public interface ITemplateManager
    {
        /// <summary>
        /// The global default live ReadOnly TemplateTree
        /// </summary>
        TemplateTree Tree { get; }

        /// <summary>
        /// Fetch all of the pick list templates
        /// </summary>
        IEnumerable<TemplateEntity> FetchPickListTemplates();
    }
}