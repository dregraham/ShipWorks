using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Templates.Controls
{
    /// <summary>
    /// Controls how snippets will be shown in the template tree
    /// </summary>
    public enum TemplateTreeSnippetDisplayType
    {
        /// <summary>
        /// Snippets will not be displayed
        /// </summary>
        Hidden,

        /// <summary>
        /// Snippets will be displayed
        /// </summary>
        Visible,

        /// <summary>
        /// Only snippets will be displayed
        /// </summary>
        OnlySnippets
    }
}
