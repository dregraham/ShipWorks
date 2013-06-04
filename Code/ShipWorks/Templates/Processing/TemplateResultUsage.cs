using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Templates.Processing
{
    /// <summary>
    /// Represents how a formatted template result is going to be used
    /// </summary>
    public enum TemplateResultUsage
    {
        /// <summary>
        /// The result is intended to be displayed in the preview window of the TemplateManager or for other in-memory renderings.
        /// </summary>
        ShipWorksDisplay,

        /// <summary>
        /// The result is intended to be sent to the printer.
        /// </summary>
        Print,

        /// <summary>
        /// The result is intended to be displayed in the IE print preview window.
        /// </summary>
        PrintPreview,

        /// <summary>
        /// The result is intended to be saved.
        /// </summary>
        Save,

        /// <summary>
        /// The result is intended to be sent via email
        /// </summary>
        Email,

        /// <summary>
        /// The result is intended to be rendered as a bitmap by the copy mechanism
        /// </summary>
        Copy
    }
}
