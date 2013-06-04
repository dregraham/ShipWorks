using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Templates.Processing
{
    /// <summary>
    /// Used to provide TemplateXsl instances from TemplateEntity instances.
    /// </summary>
    public static class TemplateXslProvider
    {
        /// <summary>
        /// Get the TemplateXsl object that is associated with the given template.
        /// </summary>
        public static TemplateXsl FromTemplate(TemplateEntity template)
        {
            TemplateTree tree = template.TemplateTree as TemplateTree;
            if (tree == null)
            {
                throw new InvalidOperationException("Cannot get TemplateXsl for a template not associated with a TemplateTree.");
            }

            return tree.XslCache.FromTemplate(template);
        }

        /// <summary>
        /// Get the TemplateXsl object for the given token text
        /// </summary>
        public static TemplateXsl FromToken(string tokenText)
        {
            // Tokens always use the live global tree
            return TemplateManager.Tree.XslCache.FromToken(tokenText);
        }
    }
}
