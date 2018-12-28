using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Templates
{
    /// <summary>
    /// Wrapper for static TemplateManager
    /// </summary>
    [Component]
    public class TemplateManagerWrapper : ITemplateManager
    {
        /// <summary>
        /// The global default live ReadOnly TemplateTree
        /// </summary>
        public TemplateTree Tree =>
            TemplateManager.Tree;
    }
}
