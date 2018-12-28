using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates.Printing;

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

        /// <summary>
        /// Ensure the given template is configured
        /// </summary>
        public bool EnsureTemplateConfigured(IWin32Window owner, TemplateEntity template) =>
            TemplatePrinterSelectionDlg.EnsureConfigured(owner, template);

        /// <summary>
        /// fetch the default pick list template
        /// </summary>
        public TemplateEntity FetchDefaultPickListTemplate() =>
            TemplateManager.FetchDefaultPickListTemplate();
    }
}
