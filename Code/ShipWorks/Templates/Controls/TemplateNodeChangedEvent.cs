using System;
using System.Collections.Generic;
using System.Text;

namespace ShipWorks.Templates.Controls
{
    public delegate void TemplateNodeChangedEventHandler(object sender, TemplateNodeChangedEventArgs e);

    /// <summary>
    /// EventArgs for the TemplateNodeChanged event
    /// </summary>
    public class TemplateNodeChangedEventArgs : EventArgs
    {
        TemplateTreeNode oldNode;
        TemplateTreeNode newNode;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public TemplateNodeChangedEventArgs(TemplateTreeNode oldNode, TemplateTreeNode newNode)
        {
            this.oldNode = oldNode;
            this.newNode = newNode;
        }

        /// <summary>
        /// The node that was selected
        /// </summary>
        public TemplateTreeNode OldNode
        {
            get { return oldNode; }
        }

        /// <summary>
        /// The node that is now selected
        /// </summary>
        public TemplateTreeNode NewNode
        {
            get { return newNode; }
        }
    }
}
