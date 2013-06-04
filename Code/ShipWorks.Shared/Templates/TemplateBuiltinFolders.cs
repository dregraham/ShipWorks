using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Templates
{
    /// <summary>
    /// Helps work with template folders that are builtin to ShipWorks
    /// </summary>
    public static class TemplateBuiltinFolders
    {
        /// <summary>
        /// Get the constant key value of the "System" template folder.
        /// </summary>
        public static long SystemFolderID
        {
            get { return -1025; }
        }

        /// <summary>
        /// Get the constant key value of the "Snippets" template folder;
        /// </summary>
        public static long SnippetsFolderID
        {
            get { return -2025; }
        }
    }
}
